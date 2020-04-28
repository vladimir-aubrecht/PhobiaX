using System;
using System.Security.Cryptography.X509Certificates;
using SDL2;

namespace PhobiaX.SDL2.Options
{
    public class RendererOptions
    {
        public int Index { get; set; }
        public SDL.SDL_RendererFlags RendererFlags { get; set; }
    }
}
