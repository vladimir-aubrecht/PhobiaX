using PhobiaX.SDL2.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.SDL2
{
	public class SDLTextureFactory
	{
		private readonly ISDL2 sdl2;
		private readonly SDLRenderer renderer;

		public SDLTextureFactory(ISDL2 sdl2, SDLRenderer renderer)
		{
			this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
			this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
		}

		public SDLTexture CreateTexture(SDLSurface surface)
		{
			return new SDLTexture(sdl2, renderer, surface);
		}
	}
}
