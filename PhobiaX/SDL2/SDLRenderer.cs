using System;
using System.Runtime.InteropServices;
using PhobiaX.SDL2.Wrappers;
using SDL2;

namespace PhobiaX.SDL2
{
    public class SDLRenderer : IDisposable
    {
        private readonly ISDL2 sdl2;
        private readonly IntPtr renderer;

        public SDLRenderer(ISDL2 sdl2, IntPtr renderer)
        {
            this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
            this.renderer = renderer;
        }

        public SDLSurface LoadSurface(string filePath)
        {
            var surface = sdl2.LoadBMP(filePath);

            return new SDLSurface(sdl2, surface);
        }

        public SDLSurface CreateSurface(int width, int height)
        {
            return new SDLSurface(sdl2, width, height);
        }

        public IntPtr CreateTextureFromSurface(IntPtr surface)
        {
            return sdl2.CreateTextureFromSurface(renderer, surface);
        }

        public void Clear()
        {
            sdl2.RenderClear(renderer);
        }

        public void Copy(IntPtr surfacePointer, IntPtr sourceRectangle, IntPtr destinationRectangle)
        {
            var texture = sdl2.CreateTextureFromSurface(renderer, surfacePointer);
            sdl2.RenderCopy(renderer, texture, sourceRectangle, destinationRectangle);
            sdl2.DestroyTexture(texture);
        }

        public void Copy(IntPtr surfacePointer, ref SDL.SDL_Rect sourceRectangle, IntPtr destinationRectangle)
        {
            var texture = sdl2.CreateTextureFromSurface(renderer, surfacePointer);
            sdl2.RenderCopy(renderer, texture, ref sourceRectangle, destinationRectangle);
        }

        public void Copy(IntPtr surfacePointer, IntPtr sourceRectangle, ref SDL.SDL_Rect destinationRectangle)
        {
            var texture = sdl2.CreateTextureFromSurface(renderer, surfacePointer);
            sdl2.RenderCopy(renderer, texture, sourceRectangle, ref destinationRectangle);
        }

        public void Copy(IntPtr surfacePointer, ref SDL.SDL_Rect sourceRectangle, ref SDL.SDL_Rect destinationRectangle)
        {
            var texture = sdl2.CreateTextureFromSurface(renderer, surfacePointer);
            sdl2.RenderCopy(renderer, texture, ref sourceRectangle, ref destinationRectangle);
        }

        public void Present()
        {
            sdl2.RenderPresent(renderer);
        }

        public int SetColorKey(IntPtr surface, int flag, uint key)
        {
            return sdl2.SetColorKey(surface, flag, key);
        }

        public void Delay(uint miliseconds)
        {
            sdl2.Delay(miliseconds);
        }

        public void Dispose()
        {
            this.sdl2.DestroyRenderer(renderer);
        }
    }
}
