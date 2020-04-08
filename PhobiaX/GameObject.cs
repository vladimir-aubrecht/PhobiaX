using System;
using System.Runtime.Versioning;
using PhobiaX.Assets;
using PhobiaX.SDL2;
using SDL2;

namespace PhobiaX
{
    public class GameObject
    {
        private const int CircleDegrees = 360;
        private int x = 0;
        private int y = 0;
        private int angle = 90;
        private int speed = 5;
        private readonly AnimatedSet animatedSurfaceAssets;
        private readonly int minimalAngleStep = 1;
        private bool isStopped = true;

        public GameObject(AnimatedSet animatedSurfaceAssets)
        {
            this.animatedSurfaceAssets = animatedSurfaceAssets;
            minimalAngleStep = CircleDegrees / animatedSurfaceAssets.GetDefaultAnimatedAsset().GetAnimationFrames().Count;
        }

        public void MoveToPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Stop()
        {
            isStopped = true;
        }

        public void TurnLeft()
        {
            angle = Modulo(angle + minimalAngleStep, CircleDegrees);

            (double radians, int rotationFrameIndex) = CalculateMovement();

            animatedSurfaceAssets.GetDefaultAnimatedAsset().SetFrameIndex(rotationFrameIndex);
        }

        public void TurnRight()
        {
            angle = Modulo(angle - minimalAngleStep, CircleDegrees);

            (double radians, int rotationFrameIndex) = CalculateMovement();

            animatedSurfaceAssets.GetDefaultAnimatedAsset().SetFrameIndex(rotationFrameIndex);
        }

        public void MoveForward()
        {
            (double radians, int rotationFrameIndex) = CalculateMovement();

            x -= (int)(speed * Math.Cos(radians));
            y += (int)(speed * Math.Sin(radians));

            animatedSurfaceAssets.GetCurrentAnimatedAsset().NextFrame();
            isStopped = false;
        }

        public void MoveBackward()
        {
            (double radians, int rotationFrameIndex) = CalculateMovement();

            x += (int)(speed * Math.Cos(radians));
            y -= (int)(speed * Math.Sin(radians));

            animatedSurfaceAssets.GetCurrentAnimatedAsset().PreviousFrame();
            isStopped = false;
        }

        public void Draw(SDLSurface destination)
        {
            var animatedAsset = animatedSurfaceAssets.GetCurrentAnimatedAsset();

            if (isStopped)
            {
                animatedAsset = animatedSurfaceAssets.GetDefaultAnimatedAsset();
            }

            var objectSurface = animatedAsset.GetCurrentFrame();
            var letterRect = new SDL.SDL_Rect() { x = x, y = y, w = objectSurface.Surface.w, h = objectSurface.Surface.h };
            //image.SetColorKey(48, 255, 0); //numbers
            objectSurface.SetColorKey(2, 65, 17);
            objectSurface.BlitSurface(destination, ref letterRect);
        }

        private (double radians, int rotationFrameIndex) CalculateMovement()
        {
            double animationAngle = Modulo(angle - 90, CircleDegrees);
            int amountOfAngles = animatedSurfaceAssets.GetDefaultAnimatedAsset().GetAnimationFrames().Count;

            int rotationFrameIndex = Modulo((int)Math.Ceiling(animationAngle * amountOfAngles / CircleDegrees), amountOfAngles);
            double radians = (Math.PI / 180) * ((CircleDegrees / amountOfAngles) * (Modulo(rotationFrameIndex - amountOfAngles / 4, amountOfAngles)));

            Console.WriteLine($"index: {rotationFrameIndex} from amount: {amountOfAngles} with angle: {angle} and animationAngle: {animationAngle}");

            return (radians, rotationFrameIndex);
        }

        private static int Modulo(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
