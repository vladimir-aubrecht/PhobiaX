using PhobiaX.Assets;
using PhobiaX.Game.GameObjects;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;
using System;
using System.Collections.Generic;

namespace PhobiaX
{
	public class EnemyFactory
	{
        private readonly Random random = new Random();
        private readonly WindowOptions windowOptions;
        private readonly AssetProvider assetProvider;
        private readonly AnimatedCollection animatedSet;

        public EnemyFactory(WindowOptions windowOptions, AssetProvider assetProvider)
		{
            this.windowOptions = windowOptions ?? throw new ArgumentNullException(nameof(windowOptions));
            this.assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
            
            assetProvider.LoadAnimations("AssetResources/Aliens", "neutral", "death", false, new SDLColor(0, 0, 255), new SDLColor(18, 18, 242));
            this.animatedSet = assetProvider.GetAnimatedSurfaces()["aliens"];
        }

        public IList<EnemyGameObject> CreateEnemies(int amount, Func<EnemyGameObject, bool> isValidEnemyFunc)
        {
            var enemies = new List<EnemyGameObject>(amount);
            for (int i = 0; i < amount; i++)
            {
                var enemy = new EnemyGameObject(new AnimatedCollection(animatedSet));

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

                if (isValidEnemyFunc(enemy))
                {
                    i--;
                    continue;
                }

                enemies.Add(enemy);
            }

            return enemies;
        }
    }
}
