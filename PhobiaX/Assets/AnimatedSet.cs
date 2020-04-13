using System;
using System.Collections.Generic;
using System.Linq;
using PhobiaX.SDL2;

namespace PhobiaX.Assets
{
    public class AnimatedSet : IDisposable
    {
        private readonly string collectionName;
        private readonly string defaultSetName;
        private readonly string finalSetName;
        private Dictionary<string, AnimatedAsset> animations = new Dictionary<string, AnimatedAsset>();
        private int frameIndex = 0;
        private bool useOnlyDefaultSetCollection;

        public AnimatedSet(AnimatedSet animatedSet)
        {
            this.collectionName = animatedSet.collectionName;
            this.defaultSetName = animatedSet.defaultSetName;
            this.finalSetName = animatedSet.finalSetName;
            this.animations = animatedSet.animations.ToDictionary(k => k.Key, i => new AnimatedAsset(i.Value));
            this.frameIndex = animatedSet.frameIndex;
            this.useOnlyDefaultSetCollection = animatedSet.useOnlyDefaultSetCollection;
        }

        public AnimatedSet(string collectionName, string defaultSetName, string finalSetName) : this(collectionName, defaultSetName, finalSetName, false)
        {
        }

        public AnimatedSet(string collectionName, string defaultSetName, string finalSetName, bool useOnlyDefaultSetCollection)
        {
            this.collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            this.defaultSetName = defaultSetName ?? throw new ArgumentNullException(nameof(defaultSetName));
            this.finalSetName = finalSetName ?? throw new ArgumentNullException(nameof(finalSetName));
            this.useOnlyDefaultSetCollection = useOnlyDefaultSetCollection;
        }

        public void AddAnimation(string name, IList<SDLSurface> animation)
        {
            animations.Add(name, new AnimatedAsset(name, animation));
        }

        public void Dispose()
        {
            foreach (var animation in animations)
            {
                animation.Value.Dispose();
            }
        }

        public void NextFrame()
        {
            frameIndex++;

            var currentSet = GetCurrentAnimatedAsset();

            if (frameIndex >= currentSet.GetAnimationFrames().Count)
            {
                frameIndex = 0;
            }

            currentSet.SetFrameIndex(frameIndex);
        }

        public void PreviousFrame()
        {
            frameIndex--;

            var currentSet = GetCurrentAnimatedAsset();

            if (frameIndex < 0)
            {
                frameIndex = currentSet.GetAnimationFrames().Count - 1;
            }

            currentSet.SetFrameIndex(frameIndex);
        }

        public AnimatedAsset GetCurrentAnimatedAsset()
        {
            var index = animations[defaultSetName].GetCurrentFrameIndex().ToString();

            if (animations.ContainsKey(index))
            {
                return animations[index];
            }

            return GetDefaultAnimatedAsset();
        }

        public AnimatedAsset GetDefaultAnimatedAsset()
        {
            return animations[defaultSetName];
        }

        public AnimatedAsset GetFinalAnimatedAsset()
        {
            return animations[finalSetName];
        }

        public AnimatedAsset GetAnimatedAsset(string name)
        {
            return animations[name];
        }
    }
}
