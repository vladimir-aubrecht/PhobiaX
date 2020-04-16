using System;
using System.Collections.Generic;
using System.Linq;
using PhobiaX.Game.GameObjects;
using PhobiaX.Physics;

namespace PhobiaX
{
    public class EnemyManager
    {
        private readonly EnemyFactory enemyFactory;
        private readonly CollissionObserver collissionObserver;
        private readonly PathFinder pathFinder;
        private readonly AnimatedGameObject[] targets;
        private readonly IList<EnemyGameObject> enemies = new List<EnemyGameObject>();
        private readonly Random random = new Random();
        private DateTimeOffset lastEnemyCleanup = DateTimeOffset.UtcNow;
        private int amountOfEnemies = 5;

        public EnemyManager(EnemyFactory enemyFactory, CollissionObserver collissionObserver, PathFinder pathFinder, params AnimatedGameObject[] targets)
        {
            this.enemyFactory = enemyFactory ?? throw new ArgumentNullException(nameof(enemyFactory));
            this.collissionObserver = collissionObserver ?? throw new ArgumentNullException(nameof(collissionObserver));
            this.pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
            this.targets = targets ?? throw new ArgumentNullException(nameof(targets));

            this.enemies = enemyFactory.CreateEnemies(amountOfEnemies, collissionObserver.IsObjectColliding);

            collissionObserver.SetForObserving("enemies", this.enemies.Cast<IGameObject>().ToList());
        }

        public void SetDesiredAmountOfEnemies(int amountOfEnemies)
        {
            this.amountOfEnemies = amountOfEnemies;
        }

        public IList<IGameObject> GetAllEnemies()
        {
            return enemies.Cast<IGameObject>().ToList();
        }

        public void MoveToClosestTarget(int probabilityOfNoMove, int percentageOfEnemiesWhichMustMove)
        {
            int movesInTurn = 0;

            foreach (var enemy in enemies)
            {
                var noMoveRandom = random.Next(100);
                if (noMoveRandom < probabilityOfNoMove && movesInTurn > enemies.Count * (double)percentageOfEnemiesWhichMustMove / 100)
                {
                    continue;
                }

                var closesestTarget = pathFinder.FindClosestTarget(enemy, targets.Where(i => i.CanBeHit).Cast<IGameObject>().ToList());

                if (closesestTarget == null)
                {
                    return;
                }

                movesInTurn++;

                var originalX = enemy.X;
                var originalY = enemy.Y;
                var originalAngle = enemy.Angle;

                if (!enemy.TryMoveTowards(closesestTarget))
                {
                    closesestTarget.Hit();
                }

                
                if (collissionObserver.IsObjectColliding(enemy))
                {
                    enemy.RollbackLastMove();
                }
            }
        }

        public void MoveEnemies()
        {
            MoveToClosestTarget(70, 20);

            var enemiesToDrop = new List<EnemyGameObject>();

            foreach (var enemy in enemies)
            {
                if (enemy.IsFinalAnimationFinished)
                {
                    enemiesToDrop.Add(enemy);
                }
            }

            var currentAmountOfEnemies = enemies.Count - enemiesToDrop.Count;
            if (currentAmountOfEnemies < this.amountOfEnemies)
            {
                var newEnemies = enemyFactory.CreateEnemies(this.amountOfEnemies - currentAmountOfEnemies, collissionObserver.IsObjectColliding);
                
                foreach (var enemy in newEnemies)
                {
                    enemies.Add(enemy);
                }

                collissionObserver.SetForObserving("enemies", this.enemies.Cast<IGameObject>().ToList());
            }


            if ((DateTimeOffset.UtcNow - lastEnemyCleanup).TotalMilliseconds > 6000)
            {
                foreach (var enemy in enemiesToDrop)
                {
                    enemies.Remove(enemy);
                }

                enemiesToDrop.Clear();

                collissionObserver.SetForObserving("enemies", this.enemies.Cast<IGameObject>().ToList());

                lastEnemyCleanup = DateTimeOffset.UtcNow;
            }
        }
    }
}
