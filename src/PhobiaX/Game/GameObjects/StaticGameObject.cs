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
		public IRenderableObject RenderableObject { get; set; }
		public ICollidableObject ColladableObject { get; set; }
		public int X { get; }
		public int Y { get; }

		public StaticGameObject(IRenderableObject renderableObject, ICollidableObject colladableObject)
		{
			RenderableObject = renderableObject;
			ColladableObject = colladableObject;
		}
	}
}
