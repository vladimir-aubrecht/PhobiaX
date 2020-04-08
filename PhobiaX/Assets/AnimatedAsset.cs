using System;
using System.Collections.Generic;
using PhobiaX.SDL2;

namespace PhobiaX.Assets
{
    public class AnimatedAsset : IDisposable
    {
        private IList<SDLSurface> surfaces = new List<SDLSurface>();
        private int currentFrame = 0;

        public AnimatedAsset(IList<SDLSurface> surfaces)
        {
            this.surfaces = surfaces;
        }

        public void PreviousFrame()
        {
            currentFrame--;

            if (currentFrame < 0)
            {
                currentFrame = this.surfaces.Count - 1;
            }
        }

        public void NextFrame()
        {
            currentFrame++;

            if (currentFrame == this.surfaces.Count)
            {
                currentFrame = 0;
            }
        }

        public void ResetFrame()
        {
            currentFrame = 0;
        }

        public int GetCurrentFrameIndex()
        {
            return currentFrame;
        }

        public SDLSurface GetCurrentFrame()
        {
            return surfaces[currentFrame];
        }

        public IList<SDLSurface> GetAnimationFrames()
        {
            return surfaces;
        }

        public void Dispose()
        {
            foreach (var surface in surfaces)
            {
                surface.Dispose();
            }
        }
    }
}
