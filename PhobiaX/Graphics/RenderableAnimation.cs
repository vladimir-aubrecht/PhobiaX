using PhobiaX.Assets;
using PhobiaX.SDL2;
using SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Graphics
{
	public class RenderableAnimation : IRenderableObject
	{
		private readonly TimeSpan animationSpeed = TimeSpan.FromMilliseconds(30);
		private readonly TimeThrottler timeThrottler;
		private readonly AnimatedCollection animatedCollection;
		private AnimationSetType animationSetType = AnimationSetType.Default;

		public RenderableAnimation(TimeThrottler timeThrottler, AnimatedCollection animatedCollection)
		{
			this.timeThrottler = timeThrottler ?? throw new ArgumentNullException(nameof(timeThrottler));
			this.animatedCollection = animatedCollection ?? throw new ArgumentNullException(nameof(animatedCollection));
		}

		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; }

		public int Height { get; }

		public bool ShouldDestroy { get; private set; }

		public int RenderingPriority { get; set; }

		public void ChangeSet(AnimationSetType animationSetType)
		{
			this.animationSetType = animationSetType;
		}

		public void NextFrame()
		{
			timeThrottler.ExecuteThrottled(CreateThrottlingKey("frame switch"), animationSpeed, DateTimeOffset.MinValue, () => this.animatedCollection.NextFrame());
		}

		public void PreviousFrame()
		{
			timeThrottler.ExecuteThrottled(CreateThrottlingKey("frame switch"), animationSpeed, DateTimeOffset.MinValue, () => this.animatedCollection.PreviousFrame());
		}

		public void SetFrameIndex(int index)
		{
			AnimatedSet animatedSet = GetAnimationSet(this.animationSetType);
			animatedSet.SetFrameIndex(index);
		}

		public int GetFramesCount(AnimationSetType animationSetType)
		{
			AnimatedSet animatedSet = GetAnimationSet(animationSetType);
			return animatedSet.GetAnimationFrames().Count;
		}

		public void Draw(SDLSurface destination)
		{
			AnimatedSet animatedSet = GetAnimationSet(this.animationSetType);

			var objectSurface = animatedSet.GetCurrentFrame();
			var surfaceRectangle = new SDL.SDL_Rect() { x = this.X, y = this.Y, w = Width, h = Height };
			objectSurface.BlitSurface(destination, ref surfaceRectangle);

			if (animationSetType == AnimationSetType.Final)
			{
				if (animatedCollection.IsFinalSetAnimation && animatedSet.GetCurrentFrameIndex() < animatedSet.GetAnimationFrames().Count - 1)
				{
					timeThrottler.ExecuteThrottled(CreateThrottlingKey("final animation"), animationSpeed, DateTimeOffset.MinValue, () => animatedSet.NextFrame());
				}
				else
				{
					timeThrottler.ExecuteThrottled(CreateThrottlingKey("final surface"), TimeSpan.FromSeconds(2), DateTimeOffset.UtcNow, () => ShouldDestroy = true);
				}
			}
		}

		private string CreateThrottlingKey(string key)
		{
			return this.GetHashCode() + key;
		}

		private AnimatedSet GetAnimationSet(AnimationSetType animationSetType)
		{
			return animationSetType switch
			{
				AnimationSetType.Default => animatedCollection.GetDefaultAnimatedSet(),
				AnimationSetType.InProgress => animatedCollection.GetCurrentAnimatedSet(),
				AnimationSetType.Final => animatedCollection.GetFinalAnimatedSet()
			};
		}
	}
}
