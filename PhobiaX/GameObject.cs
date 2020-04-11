using System;
using System.Runtime.Versioning;
using PhobiaX.Assets;
using PhobiaX.SDL2;
using SDL2;

namespace PhobiaX
{
    public class GameObject
    {
        private double angle = 90;
        private const double CircleDegrees = 360;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        private int speed = 5;
        public AnimatedSet AnimatedSet { get; }
        private readonly bool alwaysStopped;
        private readonly double minimalAngleStep = 1;
        private bool isStopped = true;

        public double Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                (double radians, int rotationFrameIndex) = CalculateMovement();

                AnimatedSet.GetDefaultAnimatedAsset().SetFrameIndex(rotationFrameIndex);
            }
        }

        public GameObject(AnimatedSet animatedSurfaceAssets) : this(animatedSurfaceAssets, false)
        {
        }

        public GameObject(AnimatedSet animatedSurfaceAssets, bool alwaysStopped)
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
            (double radians, int rotationFrameIndex) = CalculateMovement();

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
            (double radians, int rotationFrameIndex) = CalculateMovement();

            X += (int)(speed * Math.Cos(radians));
            Y -= (int)(speed * Math.Sin(radians));

            if (!alwaysStopped)
            {
                AnimatedSet.GetCurrentAnimatedAsset().PreviousFrame();
                isStopped = false;
            }
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

        public bool IsColliding(GameObject gameObject)
        {
            var animatedAsset = AnimatedSet.GetCurrentAnimatedAsset();
            return IsColliding(gameObject.X, gameObject.Y, gameObject.AnimatedSet.GetCurrentAnimatedAsset().GetCurrentFrame());
        }

        public void Draw(SDLSurface destination)
        {
            var animatedAsset = AnimatedSet.GetCurrentAnimatedAsset();

            if (isStopped)
            {
                animatedAsset = AnimatedSet.GetDefaultAnimatedAsset();
            }

            var objectSurface = animatedAsset.GetCurrentFrame();
            var letterRect = new SDL.SDL_Rect() { x = X, y = Y, w = objectSurface.Surface.w, h = objectSurface.Surface.h };
            //image.SetColorKey(48, 255, 0); //numbers
            objectSurface.SetColorKey(2, 65, 17);
            objectSurface.BlitSurface(destination, ref letterRect);
        }

        private (double radians, int rotationFrameIndex) CalculateMovement()
        {
            double animationAngle = Modulo(Angle - 90, CircleDegrees);
            int amountOfAngles = AnimatedSet.GetDefaultAnimatedAsset().GetAnimationFrames().Count;

            int rotationFrameIndex = (int)Modulo(Math.Ceiling(animationAngle * amountOfAngles / CircleDegrees), amountOfAngles);
            double radians = (Math.PI / 180) * ((CircleDegrees / amountOfAngles) * (Modulo(rotationFrameIndex - amountOfAngles / 4, amountOfAngles)));

            Console.WriteLine($"index: {rotationFrameIndex} from amount: {amountOfAngles} with angle: {Angle} and animationAngle: {animationAngle}");

            return (radians, rotationFrameIndex);
        }

        private static double Modulo(double x, double m)
        {
            return (x % m + m) % m;
        }
    }
}
