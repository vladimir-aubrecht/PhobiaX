using PhobiaX.SDL2.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.SDL2
{
	public class SDLTexture : IDisposable
	{
		private readonly ISDL2 sdl2;
		private readonly SDLRenderer renderer;
		private IntPtr Handle { get; }

		internal SDLTexture(ISDL2 sdl2, SDLRenderer renderer, SDLSurface surface)
		{
			this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
			this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));

			this.Handle = sdl2.CreateTextureFromSurface(this.renderer.Handle, surface.Handle);
		}

		public void CopyToRenderer()
		{
			sdl2.RenderCopy(this.renderer.Handle, this.Handle, IntPtr.Zero, IntPtr.Zero);
		}

		public void Dispose()
		{
			sdl2.DestroyTexture(this.Handle);
		}
	}
}
