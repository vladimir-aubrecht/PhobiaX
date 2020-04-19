using PhobiaX.Graphics;
using PhobiaX.Physics;
using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public interface IGameObject
	{
		Guid Id { get; }

		int X { get; }

		int Y { get; }

		IRenderableObject RenderableObject { get; set; }
		ICollidableObject ColladableObject { get; set; }
	}
}
