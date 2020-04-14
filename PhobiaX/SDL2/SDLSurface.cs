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

        internal SDLSurface(ISDL2 sdl2, int width, int height) : this(sdl2, width, height, 32, SDLColor.Black, 0)
        {
        }

        internal SDLSurface(ISDL2 sdl2, int width, int height, int depth, SDLColor maskColor, uint aMask) : this(sdl2, sdl2.CreateRGBSurface(0, width, height, depth, maskColor.Red, maskColor.Green, maskColor.Blue, aMask))
        {
        }

        internal SDLSurface(ISDL2 sdl2, IntPtr surfacePointer)
        {
            this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
            this.SurfacePointer = surfacePointer;

            Surface = Marshal.PtrToStructure<SDL.SDL_Surface>(surfacePointer);
        }

        public void SetColorKey(SDLColor color)
        {
            var keyColor = sdl2.MapRGB(this.Surface.format, color.Red, color.Green, color.Blue);
            sdl2.SetColorKey(this.SurfacePointer, (int)SDL.SDL_RLEACCEL | SDL.SDL_ENABLE, keyColor);
        }

        public int BlitScaled(SDLSurface dst)
        {
            return sdl2.BlitScaled(this.SurfacePointer, IntPtr.Zero, dst.SurfacePointer, IntPtr.Zero);
        }

        public int BlitSurface(SDLSurface dst)
        {
            var surfaceRectangle = new SDL.SDL_Rect() { x = 0, y = 0, w = dst.Surface.w, h = dst.Surface.h };
            return BlitSurface(dst, ref surfaceRectangle);
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
