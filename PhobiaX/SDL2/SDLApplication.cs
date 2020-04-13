using System;
using Microsoft.Extensions.Logging;
using PhobiaX.SDL2.Options;
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

            logger?.LogDebug("Started");
        }

        public IntPtr CreateWindow(WindowOptions windowOptions)
        {
            logger?.LogDebug("Creating window");

            var window = sdl2.CreateWindow(windowOptions.Title, windowOptions.X, windowOptions.Y, windowOptions.Width, windowOptions.Height, windowOptions.WindowFlags);

            logger?.LogDebug("Window created");

            return window;
        }

        public IntPtr CreateRenderer(SDLWindow window, RendererOptions rendererOptions)
        {
            logger?.LogDebug("Creating renderer");
            var renderer = sdl2.CreateRenderer(window.Handle, rendererOptions.Index, rendererOptions.RendererFlags);
            logger?.LogDebug("Renderer created");

            return renderer;
        }

        public void DestroyWindow(SDLWindow window)
        {
            logger?.LogDebug("Destroying window");
            sdl2.DestroyWindow(window.Handle);
            logger?.LogDebug("Window destroyed");
        }

        public void DestroyRenderer(SDLRenderer renderer)
        {
            logger?.LogDebug("Destroying window");
            sdl2.DestroyRenderer(renderer.Handle);
            logger?.LogDebug("Window destroyed");
        }

        public void Quit()
        {
            ShouldQuit = true;
            logger?.LogDebug("Waiting for quit ...");
        }

        public void Dispose()
        {
            logger?.LogDebug("Disposing");
            sdl2.Quit();
            logger?.LogDebug("Disposed");
        }

        public void Delay(uint miliseconds)
        {
            sdl2.Delay(miliseconds);
        }
    }
}
