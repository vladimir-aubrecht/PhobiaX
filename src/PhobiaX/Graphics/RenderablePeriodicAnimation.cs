using PhobiaX.Assets;
using PhobiaX.Physics;
using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Graphics
{
	public class RenderablePeriodicAnimation : IRenderableObject
	{
		private readonly RenderableAnimation renderableAnimation;
		private readonly double angleOfFirstFrame;
		private readonly bool isAnimationDisabled;
		private double angle = 0;

		public int X { get => this.renderableAnimation.X; set { this.renderableAnimation.X = value; } }
		public int Y { get => this.renderableAnimation.Y; set { this.renderableAnimation.Y = value; } }

		public int Width => this.renderableAnimation.Width;

		public int Height => this.renderableAnimation.Height;

		public bool ShouldDestroy => this.renderableAnimation.ShouldDestroy;

		public double Angle
		{
			get
			{
				int amountOfAngles = this.renderableAnimation.GetFramesCount(AnimationSetType.Default);
				var index = AnimationFormulas.GetFrameIndexByAngle(angle, angleOfFirstFrame, amountOfAngles);
				angle = AnimationFormulas.GetAngleByIndex(index, angleOfFirstFrame, amountOfAngles);

				return angle;
			}
			set
			{
				angle = value;
				var index = AnimationFormulas.GetFrameIndexByAngle(angle, angleOfFirstFrame, this.renderableAnimation.GetFramesCount(AnimationSetType.Default));
				this.renderableAnimation.ChangeSet(AnimationSetType.Default);
				this.renderableAnimation.SetFrameIndex(index);
			}
		}

		public int RenderingPriority { get; set; }

		public void NextAngle()
		{
			Angle = MathFormulas.Modulo(Angle + GetFrameAngleSize(), MathFormulas.CircleDegrees);
		}

		public void PreviousAngle()
		{
			Angle = MathFormulas.Modulo(Angle - GetFrameAngleSize(), MathFormulas.CircleDegrees);
		}

		public void ChangeSet(AnimationSetType animationSetType)
		{
			this.renderableAnimation.ChangeSet(animationSetType);
		}

		public void NextFrame()
		{
			if (!isAnimationDisabled)
			{
				this.renderableAnimation.ChangeSet(AnimationSetType.InProgress);
				this.renderableAnimation.NextFrame();
			}
		}

		public void PreviousFrame()
		{
			if (!isAnimationDisabled)
			{
				this.renderableAnimation.ChangeSet(AnimationSetType.InProgress);
				this.renderableAnimation.PreviousFrame();
			}
		}

		public RenderablePeriodicAnimation(RenderableAnimation renderableAnimation, double angleOfFirstFrame, bool isAnimationDisabled)
		{
			this.renderableAnimation = renderableAnimation ?? throw new ArgumentNullException(nameof(renderableAnimation));
			this.angleOfFirstFrame = angleOfFirstFrame;
			this.isAnimationDisabled = isAnimationDisabled;
			this.angle = angleOfFirstFrame;
		}

		public void Draw(SDLSurface destination)
		{
			this.renderableAnimation.Draw(destination);
		}

		private double GetFrameAngleSize()
		{
			return AnimationFormulas.GetAngleStep(this.renderableAnimation.GetFramesCount(AnimationSetType.Default));
		}

	}
}
