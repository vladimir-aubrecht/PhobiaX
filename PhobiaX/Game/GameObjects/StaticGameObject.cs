using PhobiaX.Graphics;
using PhobiaX.Physics;
using PhobiaX.SDL2;
using SDL2;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public class StaticGameObject : IGameObject
	{
		public Guid Id { get; } = Guid.NewGuid();
		public IRenderableObject RenderableObject { get; }
		public ICollidableObject ColladableObject { get; }
		public int X { get; }
		public int Y { get; }
		public double Angle { get; }
		public SDLSurface CurrentSurface { get; }

		public bool CanCollide { get; } = false;

		public StaticGameObject(IRenderableObject renderableObject, ICollidableObject colladableObject)
		{
			RenderableObject = renderableObject;
			ColladableObject = colladableObject;
		}

		public void Draw(SDLSurface destination)
		{
			this.RenderableObject.Draw(destination);
		}

		public bool IsColliding(IGameObject gameObject)
		{
			return this.ColladableObject?.IsColliding(gameObject.ColladableObject) ?? false;
		}

		public void Hit()
		{
			
		}
	}
}
