using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
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
        private int movesInTurn = 0;

        public EnemyManager(AnimatedSet animatedSet, WindowOptions windowOptions, params GameObject[] targets)
        {
            this.animatedSet = animatedSet ?? throw new ArgumentNullException(nameof(animatedSet));
            this.windowOptions = windowOptions;

            this.targets = targets;

            GenerateEnemies(5);
        }

        private void GenerateEnemies(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var enemy = new GameObject(animatedSet);

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

                enemies.Add(enemy);
            }
        }

        public void MoveToClosestTarget()
        {
            int probabilityOfTargetSwitch = 25;
            int probabilityOfNoMove = 70;

            var noMoveRandom = random.Next(100);

            if (noMoveRandom < probabilityOfNoMove && movesInTurn > enemies.Count * 0.75)
            {
                return;
            }

            foreach (var enemy in enemies)
            {
                var closestDistance = double.MaxValue;
                var closesestTarget = targets[random.Next(targets.Length)];

                var targetSwitchRandom = random.Next(100);

                foreach (var target in targets)
                {
                    var xDiff = target.X - enemy.X;
                    var yDiff = target.Y - enemy.Y;

                    var distance = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

                    if (distance < closestDistance && targetSwitchRandom < probabilityOfTargetSwitch)
                    {
                        closestDistance = distance;
                        closesestTarget = target;
                    }
                }

                movesInTurn++;
                enemy.MoveTowards(closesestTarget);
            }
        }

        public void Draw(SDLSurface surface)
        {
            MoveToClosestTarget();

            movesInTurn = 0;

            foreach (var enemy in enemies)
            {
                enemy.Draw(surface);
            }
        }
    }
}
