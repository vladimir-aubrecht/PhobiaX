using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.GameObjects
{
	public class StaticGameObject : IGameObject
	{
		public int X { get; }
		public int Y { get; }
		public SDLSurface CurrentSurface { get; }

		public StaticGameObject(int x, int y, SDLSurface surface)
		{
			X = x;
			Y = y;
			this.CurrentSurface = surface ?? throw new ArgumentNullException(nameof(surface));
		}

		public void Draw(SDLSurface destination)
		{
			this.CurrentSurface.BlitScaled(destination, IntPtr.Zero);
		}

		public bool IsColliding(IGameObject gameObject)
		{
			var isXCollission = gameObject.X + gameObject.CurrentSurface.Surface.w >= X && gameObject.X <= X + CurrentSurface.Surface.w;
			var isYCollission = gameObject.Y + gameObject.CurrentSurface.Surface.h >= Y && gameObject.Y <= Y + CurrentSurface.Surface.h;

			return isXCollission && isYCollission;
		}
	}
}
