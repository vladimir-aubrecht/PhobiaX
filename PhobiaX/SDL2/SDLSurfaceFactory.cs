using PhobiaX.SDL2.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.SDL2
{
	public class SDLSurfaceFactory
	{
		private readonly ISDL2 sdl2;

		public SDLSurfaceFactory(ISDL2 sdl2)
		{
			this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
		}

		public SDLSurface CreateSurface(int width, int height)
		{
			return CreateSurface(width, height, 32, SDLColor.Black, 0);
		}

		public SDLSurface CreateSurface(int width, int height, int depth, SDLColor maskColor, uint maskAlpha)
		{
			return new SDLSurface(this.sdl2, width, height, depth, maskColor, maskAlpha);
		}

		public SDLSurface CreateResizedSurface(SDLSurface originalSurface, int newWidth)
		{
			var ratio = (float)originalSurface.Surface.w / originalSurface.Surface.h;
			var resizedSurface = this.CreateSurface(newWidth, (int)(newWidth / ratio));
			resizedSurface.SetColorKey(SDLColor.Black);
			originalSurface.BlitScaled(resizedSurface);
			return resizedSurface;
		}

		public SDLSurface LoadSurface(string filePath)
		{
			var surface = sdl2.LoadBMP(filePath);

			return new SDLSurface(sdl2, surface);
		}

	}
}
