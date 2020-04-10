using System;
using SDL2;

namespace PhobiaX.SDL2.Options
{
    public class WindowOptions
    {
        public string Title { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public SDL.SDL_WindowFlags WindowFlags { get; set; }
    }
}
