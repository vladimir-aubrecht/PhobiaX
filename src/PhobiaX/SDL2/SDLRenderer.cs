using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using PhobiaX.SDL2.Options;
using PhobiaX.SDL2.Wrappers;
using SDL2;

namespace PhobiaX.SDL2
{
    public class SDLRenderer : IDisposable
    {
        private readonly ISDL2 sdl2;
        public IntPtr Handle { get; }
        private readonly SDLApplication application;
        private readonly ILogger<SDLRenderer> logger;

        public SDLRenderer(ISDL2 sdl2, SDLApplication application, SDLWindow window, RendererOptions rendererOptions) : this(sdl2, application, window, rendererOptions, null)
        {
        }

        public SDLRenderer(ISDL2 sdl2, SDLApplication application, SDLWindow window, RendererOptions rendererOptions, ILogger<SDLRenderer> logger)
        {
            this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
            this.application = application ?? throw new ArgumentNullException(nameof(application));
            this.logger = logger;
            this.Handle = this.application.CreateRenderer(window, rendererOptions);
        }

        public void Present()
        {
            sdl2.RenderPresent(Handle);
        }

        public void Dispose()
        {
            this.application.DestroyRenderer(this);
        }
    }
}
