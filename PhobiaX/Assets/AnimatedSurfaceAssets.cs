using System;
using System.Collections.Generic;
using PhobiaX.SDL2;

namespace PhobiaX.Assets
{
    public class AnimatedSurfaceAssets : IDisposable
    {
        private Dictionary<string, IList<SDLSurface>> animations = new Dictionary<string, IList<SDLSurface>>();
        private Dictionary<string, int> currentFrames = new Dictionary<string, int>();

        public void AddAnimation(string name, IList<SDLSurface> animation)
        {
            animations.Add(name, animation);
            currentFrames.Add(name, 0);
        }

        public void Dispose()
        {
            foreach (var animation in animations)
            {
                foreach (var surface in animation.Value)
                {
                    surface.Dispose();
                }
            }
        }

        public void PreviousFrame(string name)
        {
            currentFrames[name]--;

            if (currentFrames[name] < 0)
            {
                currentFrames[name] = animations[name].Count - 1;
            }
        }

        public void NextFrame(string name)
        {
            currentFrames[name]++;

            if (currentFrames[name] == animations[name].Count)
            {
                currentFrames[name] = 0;
            }
        }

        public void ResetFrame(string name)
        {
            currentFrames[name] = 0;
        }

        public int GetCurrentFrameIndex(string name)
        {
            return currentFrames[name];
        }

        public SDLSurface GetCurrentFrame(string name)
        {
            var index = currentFrames[name];
            return animations[name][index];
        }

        public IList<SDLSurface> GetAnimationFrames(string name)
        {
            return animations[name];
        }
    }
}
