using System;
using System.Collections.Generic;
using PhobiaX.SDL2;

namespace PhobiaX.Assets
{
    public class AnimatedAsset : IDisposable
    {
        private readonly string setName;
        private IList<SDLSurface> surfaces = new List<SDLSurface>();
        private int currentFrame = 0;

        public AnimatedAsset(AnimatedAsset animatedAsset)
        {
            this.setName = animatedAsset.setName;
            this.surfaces = animatedAsset.surfaces;
            this.currentFrame = animatedAsset.currentFrame;
        }

        public AnimatedAsset(string setName, IList<SDLSurface> surfaces)
        {
            this.setName = setName ?? throw new ArgumentNullException(nameof(setName));
            this.surfaces = surfaces ?? throw new ArgumentNullException(nameof(surfaces));
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

        public void SetFrameIndex(int index)
        {
            currentFrame = index;
        }

        public SDLSurface GetCurrentFrame()
        {
            return surfaces[currentFrame];
        }

        public IList<SDLSurface> GetAnimationFrames()
        {
            return surfaces;
        }

        public string GetSetName()
        {
            return setName;
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
