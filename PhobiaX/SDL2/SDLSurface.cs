using System;
using System.Runtime.InteropServices;
using PhobiaX.SDL2.Wrappers;
using SDL2;
using static SDL2.SDL;

namespace PhobiaX.SDL2
{
    public class SDLSurface : IDisposable
    {
        private readonly ISDL2 sdl2;

        public SDL.SDL_Surface Surface { get; }

        public IntPtr SurfacePointer { get; }

        public SDLSurface(ISDL2 sdl2, int width, int height) : this(sdl2, width, height, 32, 0, 0, 0, 0)
        {
        }

        public SDLSurface(ISDL2 sdl2, int width, int height, int depth) : this(sdl2, width, height, depth, 0, 0, 0, 0)
        {
        }

        public SDLSurface(ISDL2 sdl2, int width, int height, int depth, uint rMask, uint gMask, uint bMask, uint aMask) : this(sdl2, sdl2.CreateRGBSurface(0, width, height, depth, rMask, gMask, bMask, aMask))
        {
        }

        public SDLSurface(ISDL2 sdl2, IntPtr surfacePointer)
        {
            this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
            this.SurfacePointer = surfacePointer;

            Surface = Marshal.PtrToStructure<SDL.SDL_Surface>(surfacePointer);
        }

        public void SetColorKey(byte r, byte g, byte b)
        {
            var keyColor = sdl2.MapRGB(this.Surface.format, r, g, b);
            sdl2.SetColorKey(this.SurfacePointer, (int)SDL.SDL_RLEACCEL | SDL.SDL_ENABLE, keyColor);
        }

        public int BlitScaled(SDLSurface dst, IntPtr dstrect)
        {
            return sdl2.BlitScaled(this.SurfacePointer, IntPtr.Zero, dst.SurfacePointer, dstrect);
        }

        public int BlitSurface(SDLSurface dst, ref SDL_Rect dstrect)
        {
            return sdl2.BlitSurface(this.SurfacePointer, IntPtr.Zero, dst.SurfacePointer, ref dstrect);
        }

        public void Dispose()
        {
            sdl2.FreeSurface(SurfacePointer);
        }
    }
}
