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

        public SDLSurface LoadSurface(string filePath)
        {
            var surface = sdl2.LoadBMP(filePath);

            return new SDLSurface(sdl2, surface);
        }

        public SDLSurface CreateSurface(int width, int height)
        {
            return new SDLSurface(sdl2, width, height);
        }

        public SDLSurface CreateResizedSurface(SDLSurface originalSurface, int newWidth)
        {
            var ratio = (float)originalSurface.Surface.w / originalSurface.Surface.h;
            var resizedSurface = this.CreateSurface(newWidth, (int)(newWidth / ratio));
            resizedSurface.SetColorKey(0, 0, 0);
            originalSurface.BlitScaled(resizedSurface, IntPtr.Zero);
            return resizedSurface;
        }

        public void Copy(IntPtr surfacePointer, IntPtr sourceRectangle, IntPtr destinationRectangle)
        {
            var texture = sdl2.CreateTextureFromSurface(Handle, surfacePointer);
            sdl2.RenderCopy(Handle, texture, sourceRectangle, destinationRectangle);
            sdl2.DestroyTexture(texture);
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
