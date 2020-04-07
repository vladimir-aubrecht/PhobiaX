using System;
using PhobiaX.SDL2.Wrappers;
using SDL2;

namespace PhobiaX.SDL2
{
    public class SDLWindow : IDisposable
    {
        private readonly ISDL2 sdl2;
        private readonly IntPtr windowIntPtr;

        public SDLWindow(ISDL2 sdl2, IntPtr windowIntPtr)
        {
            this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
            this.windowIntPtr = windowIntPtr;
        }

        public SDLRenderer CreateRenderer(int index, SDL.SDL_RendererFlags rendererFlags)
        {
            var renderer = sdl2.CreateRenderer(windowIntPtr, index, rendererFlags);
            return new SDLRenderer(sdl2, renderer);
        }

        public void Dispose()
        {
            sdl2.DestroyWindow(windowIntPtr);
        }
    }
}
