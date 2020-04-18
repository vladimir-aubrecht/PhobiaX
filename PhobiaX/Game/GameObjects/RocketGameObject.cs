using PhobiaX.Assets;
using PhobiaX.Graphics;
using PhobiaX.Physics;
using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public class RocketGameObject : EffectGameObject
	{
		public Action DestroyCallback { get; set; }

		public RocketGameObject(IRenderableObject renderableObject, ICollidableObject collidableObject, AnimatedCollection animatedSet, IGameObject owner) : base(renderableObject, collidableObject, animatedSet, owner)
		{
			Speed = 12;
		}

		public override void Draw(SDLSurface destination)
		{
			if (CanCollide)
			{
				this.MoveForward();
			}

			base.Draw(destination);
		}

		public override void Hit()
		{
			base.Hit();
			DestroyCallback();
		}
	}
}
