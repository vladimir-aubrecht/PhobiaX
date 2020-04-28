using System;
using Microsoft.Extensions.Logging;
using PhobiaX.SDL2.Options;
using PhobiaX.SDL2.Wrappers;
using SDL2;

namespace PhobiaX.SDL2
{
    public class SDLWindow : IDisposable
    {
        private readonly SDLApplication application;
        private readonly ILogger<SDLWindow> logger;
        public IntPtr Handle { get; }

        public SDLWindow(SDLApplication application, WindowOptions windowOptions) : this(application, windowOptions, null)
        {
        }

        public SDLWindow(SDLApplication application, WindowOptions windowOptions, ILogger<SDLWindow> logger)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
            this.logger = logger;
            
            Handle = application.CreateWindow(windowOptions);
        }

        public void Dispose()
        {
            logger?.LogDebug("Disposing");

            application.DestroyWindow(this);

            logger?.LogDebug("Disposed");
        }
    }
}
