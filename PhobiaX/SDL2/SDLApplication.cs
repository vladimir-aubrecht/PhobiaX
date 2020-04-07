using System;
using Microsoft.Extensions.Logging;
using PhobiaX.SDL2.Wrappers;
using SDL2;

namespace PhobiaX.SDL2
{
    public class SDLApplication : IDisposable
    {
        private readonly ISDL2 sdl2;
        private readonly ILogger<SDLApplication> logger;

        public bool ShouldQuit { get; private set; }

        public SDLApplication(ISDL2 sdl2) : this(sdl2, null)
        {
        }

        public SDLApplication(ISDL2 sdl2, ILogger<SDLApplication> logger)
        {
            this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
            this.logger = logger;
            sdl2.Init();

            logger?.LogDebug("Application started");
        }

        public SDLWindow CreateWindow(string title, int x, int y, int width, int height, SDL.SDL_WindowFlags windowFlags)
        {
            var window = sdl2.CreateWindow(title, x, y, width, height, windowFlags);
            var sdlWindow = new SDLWindow(sdl2, window);

            logger?.LogDebug("Window created");

            return sdlWindow;
        }

        public void Quit()
        {
            ShouldQuit = true;
            logger?.LogDebug("Waiting for quit ...");
        }

        public void Dispose()
        {
            sdl2.Quit();
            logger?.LogDebug("Disposing ...");
        }
    }
}
