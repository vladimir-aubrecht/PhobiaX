using PhobiaX.Assets;
using PhobiaX.Graphics;
using PhobiaX.Physics;
using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public class RocketGameObject : AnimatedGameObject
	{
		public IGameObject Owner { get; }

		public RocketGameObject(RenderablePeriodicAnimation renderablePeriodicAnimation, ICollidableObject collidableObject, AnimatedGameObject owner) : base(renderablePeriodicAnimation, collidableObject)
		{
			this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
			Speed = 12;

			this.X = owner.X;
			this.Y = owner.Y;
			this.RenderablePeriodicAnimation.Angle = owner.RenderablePeriodicAnimation.Angle;

		}
	}
}
