using System;
using System.Collections.Generic;
using PhobiaX.SDL2;

namespace PhobiaX.Assets
{
    public class SurfaceAssets : IDisposable
    {
        private Dictionary<string, SDLSurface> surfaces = new Dictionary<string, SDLSurface>();

        public void AddTexture(string name, SDLSurface surface)
        {
            surfaces.Add(name, surface);
        }

        public void Dispose()
        {
            foreach (var surface in surfaces)
            {
                surface.Value.Dispose();
            }
        }

        public SDLSurface GetSurface(string name)
        {
            return surfaces[name];
        }
    }
}
