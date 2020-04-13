using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.GameObjects
{
	public interface IGameObject
	{
		public int X { get; }

		public int Y { get; }

		SDLSurface CurrentSurface { get; }

		bool IsColliding(IGameObject gameObject);

		void Draw(SDLSurface destination);
	}
}
