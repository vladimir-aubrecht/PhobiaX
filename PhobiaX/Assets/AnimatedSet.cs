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
        private Dictionary<string, AnimatedAsset> animations = new Dictionary<string, AnimatedAsset>();
        private int frameIndex = 0;
        private bool useOnlyDefaultSetCollection;

        public AnimatedSet(AnimatedSet animatedSet)
        {
            this.collectionName = animatedSet.collectionName;
            this.defaultSetName = animatedSet.defaultSetName;
            this.animations = animatedSet.animations.ToDictionary(k => k.Key, i => new AnimatedAsset(i.Value));
            this.frameIndex = animatedSet.frameIndex;
            this.useOnlyDefaultSetCollection = animatedSet.useOnlyDefaultSetCollection;
        }

        public AnimatedSet(string collectionName, string defaultSetName) : this(collectionName, defaultSetName, false)
        {
        }

        public AnimatedSet(string collectionName, string defaultSetName, bool useOnlyDefaultSetCollection)
        {
            this.collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            this.defaultSetName = defaultSetName ?? throw new ArgumentNullException(nameof(defaultSetName));
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

        public AnimatedAsset GetAnimatedAsset(string name)
        {
            return animations[name];
        }
    }
}
