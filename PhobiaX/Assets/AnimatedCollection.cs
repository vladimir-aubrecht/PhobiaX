using System;
using System.Collections.Generic;
using System.Linq;
using PhobiaX.Assets.Models;
using PhobiaX.SDL2;

namespace PhobiaX.Assets
{
    public class AnimatedCollection : IDisposable
    {
        private readonly string collectionName;
        private readonly string defaultSetName;
        private readonly string finalSetName;
        private IDictionary<string, AnimatedSet> animations = new Dictionary<string, AnimatedSet>();
        private int frameIndex = 0;
        private bool useOnlyDefaultSetCollection;

        public bool IsFinalSetAnimation { get; }

        public AnimatedCollection(AnimatedCollection animatedSet)
        {
            this.collectionName = animatedSet.collectionName;
            this.defaultSetName = animatedSet.defaultSetName;
            this.finalSetName = animatedSet.finalSetName;
            this.animations = animatedSet.animations.ToDictionary(k => k.Key, i => new AnimatedSet(i.Value));
            this.frameIndex = animatedSet.frameIndex;
            this.useOnlyDefaultSetCollection = animatedSet.useOnlyDefaultSetCollection;
            this.IsFinalSetAnimation = animatedSet.IsFinalSetAnimation;
        }

        public AnimatedCollection(string collectionName, string defaultSetName, string finalSetName, bool isFinalSetAnimation) : this(collectionName, defaultSetName, finalSetName, false, isFinalSetAnimation)
        {
        }

        public AnimatedCollection(string collectionName, string defaultSetName, string finalSetName, bool useOnlyDefaultSetCollection, bool isFinalSetAnimation)
        {
            this.collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            this.defaultSetName = defaultSetName ?? throw new ArgumentNullException(nameof(defaultSetName));
            this.finalSetName = finalSetName ?? throw new ArgumentNullException(nameof(finalSetName));
            this.useOnlyDefaultSetCollection = useOnlyDefaultSetCollection;
            IsFinalSetAnimation = isFinalSetAnimation;
        }

        public void AddAnimation(string name, Metadata metadata, IList<SDLSurface> animation)
        {
            animations.Add(name, new AnimatedSet(name, metadata, animation));
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

            var currentSet = GetCurrentAnimatedSet();

            if (frameIndex >= currentSet.GetAnimationFrames().Count)
            {
                frameIndex = 0;
            }

            currentSet.SetFrameIndex(frameIndex);
        }

        public void PreviousFrame()
        {
            frameIndex--;

            var currentSet = GetCurrentAnimatedSet();

            if (frameIndex < 0)
            {
                frameIndex = currentSet.GetAnimationFrames().Count - 1;
            }

            currentSet.SetFrameIndex(frameIndex);
        }

        public AnimatedSet GetCurrentAnimatedSet()
        {
            var index = animations[defaultSetName].GetCurrentFrameIndex().ToString();

            if (animations.ContainsKey(index))
            {
                return animations[index];
            }

            return GetDefaultAnimatedSet();
        }

        public AnimatedSet GetDefaultAnimatedSet()
        {
            return animations[defaultSetName];
        }

        public AnimatedSet GetFinalAnimatedSet()
        {
            return animations[finalSetName];
        }

        public AnimatedSet GetAnimatedAsset(string name)
        {
            return animations[name];
        }
    }
}
