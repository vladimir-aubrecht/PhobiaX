using System;
using System.Collections.Generic;
using PhobiaX.SDL2;

namespace PhobiaX.Assets
{
    public class AnimatedSurfaceAssets : IDisposable
    {
        private Dictionary<string, AnimatedAsset> animations = new Dictionary<string, AnimatedAsset>();

        public void AddAnimation(string name, IList<SDLSurface> animation)
        {
            animations.Add(name, new AnimatedAsset(animation));
        }

        public void Dispose()
        {
            foreach (var animation in animations)
            {
                animation.Value.Dispose();
            }
        }

        public void PreviousFrame(string name)
        {
            animations[name].PreviousFrame();
        }

        public void NextFrame(string name)
        {
            animations[name].NextFrame();
        }

        public void ResetFrame(string name)
        {
            animations[name].ResetFrame();
        }

        public int GetCurrentFrameIndex(string name)
        {
            return animations[name].GetCurrentFrameIndex();
        }

        public SDLSurface GetCurrentFrame(string name)
        {
            return animations[name].GetCurrentFrame();
        }

        public IList<SDLSurface> GetAnimationFrames(string name)
        {
            return animations[name].GetAnimationFrames();
        }
    }
}
