using PhobiaX.SDL2;
using SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public class StaticGameObject : IGameObject
	{
		public int X { get; }
		public int Y { get; }
		public SDLSurface CurrentSurface { get; }

		public bool CanBeHit { get; } = false;

		public StaticGameObject(int x, int y, SDLSurface surface)
		{
			X = x;
			Y = y;
			this.CurrentSurface = surface ?? throw new ArgumentNullException(nameof(surface));
		}

		public void Draw(SDLSurface destination)
		{
			var surfaceRectangle = new SDL.SDL_Rect() { x = X, y = Y, w = CurrentSurface.Surface.w, h = CurrentSurface.Surface.h };
			this.CurrentSurface.BlitSurface(destination, ref surfaceRectangle);
		}

		public bool IsColliding(IGameObject gameObject)
		{
			var isXCollission = gameObject.X + gameObject.CurrentSurface.Surface.w >= X && gameObject.X <= X + CurrentSurface.Surface.w;
			var isYCollission = gameObject.Y + gameObject.CurrentSurface.Surface.h >= Y && gameObject.Y <= Y + CurrentSurface.Surface.h;

			return isXCollission && isYCollission;
		}

		public void Hit()
		{
			
		}
	}
}
