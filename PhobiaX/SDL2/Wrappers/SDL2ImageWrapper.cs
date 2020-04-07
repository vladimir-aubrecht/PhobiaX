using System;
using Microsoft.Extensions.Logging;
using SDL2;

namespace PhobiaX.SDL2.Wrappers
{
    public class SDL2ImageWrapper : ISDL2Image
    {
        private readonly ILogger<SDL2ImageWrapper> logger;

        public SDL2ImageWrapper(ILogger<SDL2ImageWrapper> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IntPtr LoadTexture(IntPtr rendererIntPtr, string filePath)
        {
            var texture = SDL_image.IMG_LoadTexture(rendererIntPtr, filePath);

            if (texture == IntPtr.Zero)
            {
                CreateError(nameof(LoadTexture));
            }

            return texture;
        }

        private void CreateError(string methodName)
        {
            var error = $"Unable to {methodName}. SDL. Error: {SDL.SDL_GetError()}";
            this.logger.LogError(error);
            throw new Exception(error);
        }
    }
}
