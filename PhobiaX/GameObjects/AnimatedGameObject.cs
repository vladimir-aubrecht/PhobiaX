using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;
using System.Transactions;
using PhobiaX.Assets;
using PhobiaX.SDL2;
using SDL2;

namespace PhobiaX.GameObjects
{
    public class AnimatedGameObject : IGameObject
    {
        private const int defaultAngleOffset = 90;
        private const double CircleDegrees = 360;

        private double angle = defaultAngleOffset;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        private int previousX = 0;
        private int previousY = 0;
        private double previousAngle = 0;
        private int previousFrameIndex = 0;

        private int speed = 5;
        private AnimatedSet AnimatedSet { get; }

        public SDLSurface CurrentSurface => AnimatedSet.GetCurrentAnimatedAsset().GetCurrentFrame();

        private bool isHit = false;

        private bool alwaysStopped;
        private readonly double minimalAngleStep = 1;
        private bool isStopped = true;

        public double Angle
        {
            get { return angle; }
            set
            {
                angle = value;

                AnimatedSet.GetDefaultAnimatedAsset().SetFrameIndex(CalculateFrameIndexFromCurrentAngle());
            }
        }

        public bool CanBeHit => !isHit;

        public AnimatedGameObject(AnimatedSet animatedSurfaceAssets) : this(animatedSurfaceAssets, false)
        {
        }

        public AnimatedGameObject(AnimatedSet animatedSurfaceAssets, bool alwaysStopped)
        {
            this.AnimatedSet = animatedSurfaceAssets;
            this.alwaysStopped = alwaysStopped;
            minimalAngleStep = CircleDegrees / animatedSurfaceAssets.GetDefaultAnimatedAsset().GetAnimationFrames().Count;
        }

        public void Stop()
        {
            isStopped = true;
        }

        public void TurnLeft()
        {
            Angle = Modulo(Angle + minimalAngleStep, CircleDegrees);
        }

        public void TurnRight()
        {
            Angle = Modulo(Angle - minimalAngleStep, CircleDegrees);
        }

        public void MoveForward()
        {
            if (isHit)
            {
                return;
            }

            var radians = CalculateNewAngleInRadiansFromFrameIndex();

            BackupCurrentPosition();

            X -= (int)(speed * Math.Cos(radians));
            Y += (int)(speed * Math.Sin(radians));

            if (!alwaysStopped)
            {
                AnimatedSet.GetCurrentAnimatedAsset().NextFrame();
                isStopped = false;
            }
        }

        public void MoveBackward()
        {
            if (isHit)
            {
                return;
            }

            var radians = CalculateNewAngleInRadiansFromFrameIndex();

            BackupCurrentPosition();

            X += (int)(speed * Math.Cos(radians));
            Y -= (int)(speed * Math.Sin(radians));

            if (!alwaysStopped)
            {
                AnimatedSet.GetCurrentAnimatedAsset().PreviousFrame();
                isStopped = false;
            }
        }

        private void BackupCurrentPosition()
        {
            previousX = X;
            previousY = Y;
            previousAngle = Angle;
            previousFrameIndex = AnimatedSet.GetCurrentAnimatedAsset().GetCurrentFrameIndex();
        }

        public void RollbackLastMove()
        {
            this.X = previousX;
            this.Y = previousY;
            this.Angle = previousAngle;
            this.AnimatedSet.GetCurrentAnimatedAsset().SetFrameIndex(previousFrameIndex);
        }

        public void MoveTowards(AnimatedGameObject gameObject)
        {
            if (this.IsColliding(gameObject))
            {
                this.AnimatedSet.GetCurrentAnimatedAsset().ResetFrame();
                return;
            }

            Angle = CalculateAngleTowardsGameObject(this.X, this.Y, gameObject);

            MoveForward();
        }

        public virtual bool IsColliding(int x, int y, SDLSurface surface)
        {
            var thisFrame = this.AnimatedSet.GetCurrentAnimatedAsset().GetCurrentFrame();

            var surfaceWidth = surface.Surface.w;
            var surfaceHeight = surface.Surface.h;

            var isCollissionX = X + thisFrame.Surface.w >= x && X <= x + surfaceWidth;
            var isCollissionY = Y + thisFrame.Surface.h >= y && Y <= y + surfaceHeight;
            var isCollission = isCollissionX && isCollissionY;

            return isCollission;
        }

        public bool IsColliding(IGameObject gameObject)
        {
            return IsColliding(gameObject.X, gameObject.Y, gameObject.CurrentSurface);
        }

        public virtual void Draw(SDLSurface destination)
        {
            var animatedAsset = AnimatedSet.GetCurrentAnimatedAsset();

            if (isStopped)
            {
                animatedAsset = AnimatedSet.GetDefaultAnimatedAsset();
            }

            if (isHit)
            {
                animatedAsset = AnimatedSet.GetFinalAnimatedAsset();
            }

            var objectSurface = animatedAsset.GetCurrentFrame();
            var surfaceRectangle = new SDL.SDL_Rect() { x = X, y = Y, w = objectSurface.Surface.w, h = objectSurface.Surface.h };
            //image.SetColorKey(48, 255, 0); //numbers
            objectSurface.BlitSurface(destination, ref surfaceRectangle);
        }

        private static double CalculateAngleTowardsGameObject(int x, int y, AnimatedGameObject gameObject)
        {
            double xDiff = gameObject.X - x;
            double yDiff = gameObject.Y - y;
            double xDistance = Math.Abs(xDiff);
            double yDistance = Math.Abs(yDiff);

            var angle = Modulo(Math.Atan(yDistance / xDistance) * 180 / Math.PI, CircleDegrees);

            // Left Down
            if (xDiff < 0 && yDiff > 0)
            {
                return 180 + angle;
            }
            // Right Down
            else if (xDiff > 0 && yDiff > 0)
            {
                return 360 - angle;
            }
            // Left Up
            else if (xDiff < 0 && yDiff < 0)
            {
                return 180 - angle;
            }
            // Right Up
            else if (xDiff > 0 && yDiff < 0)
            {
                return angle;
            }

            return 0;
        }

        private int CalculateFrameIndexFromCurrentAngle()
        {
            double animationAngle = Modulo(Angle - defaultAngleOffset, CircleDegrees);
            int amountOfAngles = AnimatedSet.GetDefaultAnimatedAsset().GetAnimationFrames().Count;

            return (int)Modulo(Math.Ceiling(animationAngle * amountOfAngles / CircleDegrees), amountOfAngles);
        }

        private double CalculateNewAngleInRadiansFromFrameIndex()
        {
            var rotationFrameIndex = CalculateFrameIndexFromCurrentAngle();
            int amountOfAngles = AnimatedSet.GetDefaultAnimatedAsset().GetAnimationFrames().Count;

            return (Math.PI / 180) * ((CircleDegrees / amountOfAngles) * (Modulo(rotationFrameIndex - amountOfAngles / 4, amountOfAngles)));
        }

        private static double Modulo(double x, double m)
        {
            return (x % m + m) % m;
        }

        public virtual void Hit()
        {
            isHit = true;
        }
    }
}
