using PhobiaX.Assets;
using PhobiaX.Graphics;
using PhobiaX.Physics;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public class EnemyGameObject : AnimatedGameObject
	{
        private readonly Random random = new Random();

        public EnemyGameObject(RenderablePeriodicAnimation renderablePeriodicAnimation, ICollidableObject collidableObject) : base(renderablePeriodicAnimation, collidableObject)
		{
			Speed = 3 * random.NextDouble() + 2;
        }
	}
}
