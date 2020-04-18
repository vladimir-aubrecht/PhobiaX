using PhobiaX.Assets;
using PhobiaX.SDL2;
using SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Graphics
{
	public class RenderableSurface : IRenderableObject
	{
		private readonly SDLSurface surface;

		public RenderableSurface(SDLSurface sDLSurface)
		{
			this.surface = sDLSurface ?? throw new ArgumentNullException(nameof(sDLSurface));
		}

		public int X { get; set; }

		public int Y { get; set; }

		public int Width => surface.Surface.w;

		public int Height => surface.Surface.h;

		public void Draw(SDLSurface destination)
		{
			var surfaceRectangle = new SDL.SDL_Rect() { x = X, y = Y, w = Width, h = Height };
			this.surface.BlitSurface(destination, ref surfaceRectangle);
		}
	}
}
