using System;
using System.Diagnostics.Contracts;
using Microsoft.Extensions.Logging;
using SDL2;
using static SDL2.SDL;

namespace PhobiaX.SDL2.Wrappers
{
    public class SDL2Wrapper : ISDL2
    {
        private readonly ILogger<SDL2Wrapper> logger;

        public SDL2Wrapper()
        {

        }

        public SDL2Wrapper(ILogger<SDL2Wrapper> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Init()
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                CreateError(nameof(Init));
            }
        }

        public IntPtr CreateWindow(string title, int x, int y, int width, int height, SDL.SDL_WindowFlags windowFlags)
        {
            var window = SDL.SDL_CreateWindow(title, x, y, width, height, windowFlags);

            if (window == IntPtr.Zero)
            {
                CreateError(nameof(CreateWindow));
            }

            return window;
        }

        public IntPtr CreateRenderer(IntPtr window, int index, SDL.SDL_RendererFlags rendererFlags)
        {
            var renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            if (renderer == IntPtr.Zero)
            {
                CreateError(nameof(CreateRenderer));
            }

            return renderer;
        }

        public void RenderClear(IntPtr renderer)
        {
            SDL.SDL_RenderClear(renderer);
        }

        public void RenderCopy(IntPtr renderer, IntPtr texture, IntPtr sourceRectangle, IntPtr destinationRectangle)
        {
            SDL.SDL_RenderCopy(renderer, texture, sourceRectangle, destinationRectangle);
        }

        public void RenderCopy(IntPtr renderer, IntPtr texture, ref SDL.SDL_Rect sourceRectangle, IntPtr destinationRectangle)
        {
            SDL.SDL_RenderCopy(renderer, texture, ref sourceRectangle, destinationRectangle);
        }

        public void RenderCopy(IntPtr renderer, IntPtr texture, IntPtr sourceRectangle, ref SDL.SDL_Rect destinationRectangle)
        {
            SDL.SDL_RenderCopy(renderer, texture, sourceRectangle, ref destinationRectangle);
        }

        public void RenderCopy(IntPtr renderer, IntPtr texture, ref SDL.SDL_Rect sourceRectangle, ref SDL.SDL_Rect destinationRectangle)
        {
            SDL.SDL_RenderCopy(renderer, texture, ref sourceRectangle, ref destinationRectangle);
        }

        public void RenderPresent(IntPtr renderer)
        {
            SDL.SDL_RenderPresent(renderer);
        }

        public int PollEvent(out SDL.SDL_Event @event)
        {
            return SDL.SDL_PollEvent(out @event);
        }

        public void DestroyTexture(IntPtr texture)
        {
            SDL.SDL_DestroyTexture(texture);
        }

        public void DestroyRenderer(IntPtr renderer)
        {
            SDL.SDL_DestroyRenderer(renderer);
        }

        public void DestroyWindow(IntPtr window)
        {
            SDL.SDL_DestroyWindow(window);
        }

        public void FreeSurface(IntPtr surface)
        {
            SDL.SDL_FreeSurface(surface);
        }

        public IntPtr CreateTextureFromSurface(IntPtr renderer, IntPtr surface)
        {
            return SDL.SDL_CreateTextureFromSurface(renderer, surface);
        }

        public void Quit()
        {
            SDL.SDL_Quit();
        }

        public void Delay(uint miliseconds)
        {
            SDL.SDL_Delay(miliseconds);
        }

        public IntPtr LoadBMP(string file)
        {
            return SDL.SDL_LoadBMP(file);
        }

        private void CreateError(string methodName)
        {
            var error = $"Unable to {methodName}. SDL. Error: {SDL.SDL_GetError()}";
            this.logger?.LogError(error);
            throw new Exception(error);
        }

        public int SetColorKey(IntPtr surface, int flag, uint key)
        {
            return SDL.SDL_SetColorKey(surface, flag, key);
        }

        public uint MapRGB(IntPtr format, byte r, byte g, byte b)
        {
            return SDL.SDL_MapRGB(format, r, g, b);
        }

        public int BlitSurface(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect)
        {
            return SDL.SDL_BlitSurface(src, srcrect, dst, dstrect);
        }

        public int BlitSurface(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, ref SDL_Rect dstrect)
        {
            return SDL.SDL_BlitSurface(src, ref srcrect, dst, ref dstrect);
        }

        public int BlitSurface(IntPtr src, IntPtr srcrect, IntPtr dst, ref SDL_Rect dstrect)
        {
            return SDL.SDL_BlitSurface(src, srcrect, dst, ref dstrect);
        }

        public int BlitScaled(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect)
        {
            return SDL.SDL_BlitScaled(src, srcrect, dst, dstrect);
        }

        public IntPtr CreateRGBSurface(uint flags, int width, int height, int depth, uint Rmask, uint Gmask, uint Bmask, uint Amask)
        {
            return SDL.SDL_CreateRGBSurface(flags, width, height, depth, Rmask, Gmask, Bmask, Amask);
        }

        public void GetRGB(uint pixel, IntPtr format, out byte r, out byte g, out byte b)
        {
            SDL.SDL_GetRGB(pixel, format, out r, out g, out b);
        }

        public int GL_SetSwapInterval(int interval)
        {
            return SDL.SDL_GL_SetSwapInterval(interval);
        }

        public IntPtr GetKeyboardState(out int numkeys)
        {
            return SDL.SDL_GetKeyboardState(out numkeys);
        }
    }
}
