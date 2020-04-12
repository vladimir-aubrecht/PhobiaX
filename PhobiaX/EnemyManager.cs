using System;
using System.Collections.Generic;
using PhobiaX.Assets;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;

namespace PhobiaX
{
    public class EnemyManager
    {
        private readonly AnimatedSet animatedSet;
        private readonly WindowOptions windowOptions;
        private readonly GameObject[] targets;
        private readonly IList<GameObject> enemies = new List<GameObject>();
        private readonly Random random = new Random();

        public EnemyManager(AnimatedSet animatedSet, WindowOptions windowOptions, params GameObject[] targets)
        {
            this.animatedSet = animatedSet ?? throw new ArgumentNullException(nameof(animatedSet));
            this.windowOptions = windowOptions;

            this.targets = targets;

            GenerateEnemies(20);
        }

        private void GenerateEnemies(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var enemy = new GameObject(new AnimatedSet(animatedSet));

                var x = random.Next(windowOptions.Width);
                var y = random.Next(windowOptions.Height);

                var rnd = random.Next(100);
                if (rnd <= 25)
                {
                    enemy.X = x;
                }
                else if (rnd > 25 && rnd <= 50)
                {
                    enemy.Y = y;
                }
                else if (rnd > 50 && rnd <= 75)
                {
                    enemy.X = windowOptions.Width - 50;
                    enemy.Y = y;
                }
                else
                {
                    enemy.X = x;
                    enemy.Y = windowOptions.Height - 50;
                }

                if (HasEnemyCollissionWithRestOfEnemies(enemy))
                {
                    i--;
                    continue;
                }

                enemies.Add(enemy);
            }
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

                var closesestTarget = FindClosestTarget(enemy);

                movesInTurn++;

                var originalX = enemy.X;
                var originalY = enemy.Y;
                var originalAngle = enemy.Angle;

                enemy.MoveTowards(closesestTarget);

                if (HasEnemyCollissionWithRestOfEnemies(enemy))
                {
                    enemy.X = originalX;
                    enemy.Y = originalY;
                    enemy.Angle = originalAngle;
                    enemy.AnimatedSet.GetCurrentAnimatedAsset().PreviousFrame();
                }
            }

            movesInTurn = 0;
        }

        private bool HasEnemyCollissionWithRestOfEnemies(GameObject testedEnemy)
        {
            var isColliding = false;
            foreach (var enemy in enemies)
            {
                if (testedEnemy == enemy)
                {
                    continue;
                }

                isColliding |= testedEnemy.IsColliding(enemy);
            }

            return isColliding;
        }

        private GameObject FindClosestTarget(GameObject enemy)
        {
            var closestDistance = double.MaxValue;
            var closesestTarget = targets[random.Next(targets.Length)];

            foreach (var target in targets)
            {
                var xDiff = target.X - enemy.X;
                var yDiff = target.Y - enemy.Y;

                var distance = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closesestTarget = target;
                }
            }

            return closesestTarget;
        }

        public void Draw(SDLSurface surface)
        {
            MoveToClosestTarget(70, 20);

            foreach (var enemy in enemies)
            {
                enemy.Draw(surface);
            }
        }
    }
}
