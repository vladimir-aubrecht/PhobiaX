using PhobiaX.Assets;
using PhobiaX.Graphics;
using PhobiaX.Physics;
using PhobiaX.SDL2.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public class EnemyGameObject : AnimatedGameObject
	{
        private readonly Random random = new Random();
        private readonly WindowOptions windowOptions;

        public EnemyGameObject(IRenderableObject renderableObject, ICollidableObject collidableObject, AnimatedCollection animatedSet, WindowOptions windowOptions) : base(renderableObject, collidableObject, animatedSet)
		{
			Speed = 3 * random.NextDouble() + 2;
            this.windowOptions = windowOptions ?? throw new ArgumentNullException(nameof(windowOptions));
        }

		public void FindRandomStartLocation()
		{

            var minX = -random.Next(150) - this.CurrentSurface.Surface.w;
            var minY = -random.Next(150) - this.CurrentSurface.Surface.h;
            var maxX = random.Next(windowOptions.Width);
            var maxY = random.Next(windowOptions.Height);

            var rnd = random.Next(100);
            if (rnd <= 25)
            {
                this.X = maxX;
                this.Y = minY;
            }
            else if (rnd > 25 && rnd <= 50)
            {
                this.X = minX;
                this.Y = maxY;
            }
            else if (rnd > 50 && rnd <= 75)
            {
                this.X = windowOptions.Width - minX;
                this.Y = maxY;
            }
            else
            {
                this.X = maxX;
                this.Y = windowOptions.Height - minY;
            }
        }

		public override void Hit()
		{
			base.Hit();
			DestroyCallback();
		}
	}
}
