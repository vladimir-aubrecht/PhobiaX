using System;
using System.Runtime.Versioning;
using PhobiaX.Assets;
using PhobiaX.SDL2;
using SDL2;

namespace PhobiaX
{
    public class GameObject
    {
        private int angle = 90;
        private const int CircleDegrees = 360;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        private int speed = 5;
        private readonly AnimatedSet animatedSurfaceAssets;
        private readonly bool alwaysStopped;
        private readonly int minimalAngleStep = 1;
        private bool isStopped = true;

        public int Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                (double radians, int rotationFrameIndex) = CalculateMovement();

                animatedSurfaceAssets.GetDefaultAnimatedAsset().SetFrameIndex(rotationFrameIndex);
            }
        }

        public GameObject(AnimatedSet animatedSurfaceAssets) : this(animatedSurfaceAssets, false)
        {
        }

        public GameObject(AnimatedSet animatedSurfaceAssets, bool alwaysStopped)
        {
            this.animatedSurfaceAssets = animatedSurfaceAssets;
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
                animatedSurfaceAssets.GetCurrentAnimatedAsset().NextFrame();
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
                animatedSurfaceAssets.GetCurrentAnimatedAsset().PreviousFrame();
                isStopped = false;
            }
        }

        public bool IsColliding(int x, int y, SDLSurface surface)
        {
            var width = surface.Surface.w;
            var height = surface.Surface.h;

            var isCollissionX = X + width >= x && X <= x + width;
            var isCollissionY = Y + height >= y && Y <= y + height;
            var isCollission = isCollissionX && isCollissionY;

            return isCollission;
        }

        public bool IsColliding(GameObject gameObject)
        {
            var animatedAsset = animatedSurfaceAssets.GetCurrentAnimatedAsset();
            return IsColliding(gameObject.X, gameObject.Y, animatedAsset.GetCurrentFrame());
        }

        public void Draw(SDLSurface destination)
        {
            var animatedAsset = animatedSurfaceAssets.GetCurrentAnimatedAsset();

            if (isStopped)
            {
                animatedAsset = animatedSurfaceAssets.GetDefaultAnimatedAsset();
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
            int amountOfAngles = animatedSurfaceAssets.GetDefaultAnimatedAsset().GetAnimationFrames().Count;

            int rotationFrameIndex = Modulo((int)Math.Ceiling(animationAngle * amountOfAngles / CircleDegrees), amountOfAngles);
            double radians = (Math.PI / 180) * ((CircleDegrees / amountOfAngles) * (Modulo(rotationFrameIndex - amountOfAngles / 4, amountOfAngles)));

            Console.WriteLine($"index: {rotationFrameIndex} from amount: {amountOfAngles} with angle: {Angle} and animationAngle: {animationAngle}");

            return (radians, rotationFrameIndex);
        }

        private static int Modulo(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
