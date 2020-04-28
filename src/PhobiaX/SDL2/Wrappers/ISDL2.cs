using System;
using SDL2;
using static SDL2.SDL;

namespace PhobiaX.SDL2.Wrappers
{
    public interface ISDL2
    {
        IntPtr CreateRenderer(IntPtr window, int index, SDL.SDL_RendererFlags rendererFlags);
        IntPtr CreateWindow(string title, int x, int y, int width, int height, SDL.SDL_WindowFlags windowFlags);
        SDL_AudioSpec LoadWAV(string file, ref SDL_AudioSpec spec, out IntPtr audio_buf, out uint audio_len);
        int OpenAudio(ref SDL_AudioSpec desired, out SDL_AudioSpec obtained);
        void PauseAudio(int pause_on);
        void CloseAudio();
        void FreeWAV(IntPtr audio_buf);
        void DestroyRenderer(IntPtr renderer);
        void DestroyTexture(IntPtr texture);
        void DestroyWindow(IntPtr window);
        void FreeSurface(IntPtr surface);
        void Init(uint flags);
        int PollEvent(out SDL.SDL_Event @event);
        void Quit();
        void RenderClear(IntPtr renderer);
        void RenderCopy(IntPtr renderer, IntPtr texture, IntPtr sourceRectangle, IntPtr destinationRectangle);
        void RenderCopy(IntPtr renderer, IntPtr texture, ref SDL.SDL_Rect sourceRectangle, IntPtr destinationRectangle);
        void RenderCopy(IntPtr renderer, IntPtr texture, IntPtr sourceRectangle, ref SDL.SDL_Rect destinationRectangle);
        void RenderCopy(IntPtr renderer, IntPtr texture, ref SDL.SDL_Rect sourceRectangle, ref SDL.SDL_Rect destinationRectangle);
        void RenderPresent(IntPtr renderer);
        int SetColorKey(IntPtr surface, int flag, uint key);
        IntPtr LoadBMP(string file);
        IntPtr CreateTextureFromSurface(IntPtr renderer, IntPtr surface);
        void Delay(uint miliseconds);
        uint MapRGB(IntPtr format, byte r, byte g, byte b);
        int BlitSurface(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect);
        int BlitSurface(IntPtr src, IntPtr srcrect, IntPtr dst, ref SDL_Rect dstrect);
        int BlitScaled(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect);
        int BlitSurface(IntPtr src, ref SDL_Rect srcrect, IntPtr dst, ref SDL_Rect dstrect);
        void GetRGB(uint pixel, IntPtr format, out byte r, out byte g, out byte b);
        IntPtr CreateRGBSurface(uint flags, int width, int height, int depth, uint Rmask, uint Gmask, uint Bmask, uint Amask);
        int GL_SetSwapInterval(int interval);
        IntPtr GetKeyboardState(out int numkeys);
    }
}