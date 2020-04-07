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
        private readonly AnimatedSurfaceAssets animatedSurfaceAssets;
        private string framename = "player_neutral";
        private string neutralFramename = "player_neutral";
        private readonly int minimalAngleStep = 1;

        public GameObject(AnimatedSurfaceAssets animatedSurfaceAssets)
        {
            this.animatedSurfaceAssets = animatedSurfaceAssets;
            minimalAngleStep = CircleDegrees / animatedSurfaceAssets.GetAnimationFrames(neutralFramename).Count;
        }

        public void MoveToPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Stop()
        {
            framename = neutralFramename;
        }

        public void TurnLeft()
        {
            angle += Modulo(minimalAngleStep, CircleDegrees);

            animatedSurfaceAssets.NextFrame(neutralFramename);
            framename = neutralFramename.Replace("_neutral", "_" + animatedSurfaceAssets.GetCurrentFrameIndex(neutralFramename).ToString());
        }

        public void TurnRight()
        {
            angle -= Modulo(minimalAngleStep, CircleDegrees);

            animatedSurfaceAssets.PreviousFrame(neutralFramename);
            framename = neutralFramename.Replace("_neutral", "_" + animatedSurfaceAssets.GetCurrentFrameIndex(neutralFramename).ToString());
        }

        public void MoveForward()
        {
            (double radians, int frameIndex) = CalculateMovement();

            x -= (int)(speed * Math.Cos(radians));
            y += (int)(speed * Math.Sin(radians));

            framename = neutralFramename.Replace("_neutral", "_" + frameIndex.ToString());
            animatedSurfaceAssets.PreviousFrame(framename);
        }

        public void MoveBackward()
        {
            (double radians, int frameIndex) = CalculateMovement();

            x += (int)(speed * Math.Cos(radians));
            y -= (int)(speed * Math.Sin(radians));

            framename = neutralFramename.Replace("_neutral", "_" + frameIndex.ToString());
            animatedSurfaceAssets.NextFrame(framename);
        }

        public void Draw(SDLSurface destination)
        {
            var objectSurface = animatedSurfaceAssets.GetCurrentFrame(framename);
            var letterRect = new SDL.SDL_Rect() { x = x, y = y, w = objectSurface.Surface.w, h = objectSurface.Surface.h };
            //image.SetColorKey(48, 255, 0); //numbers
            objectSurface.SetColorKey(2, 65, 17);
            objectSurface.BlitSurface(destination, ref letterRect);
        }

        private (double radians, int frameIndex) CalculateMovement()
        {
            double animationAngle = Modulo(angle - 90, CircleDegrees);
            int amountOfAngles = animatedSurfaceAssets.GetAnimationFrames(neutralFramename).Count;

            int frameIndex = Modulo((int)Math.Ceiling(animationAngle * amountOfAngles / CircleDegrees), amountOfAngles);
            double radians = (Math.PI / 180) * ((CircleDegrees / amountOfAngles) * (Modulo(frameIndex - amountOfAngles / 4, amountOfAngles)));

            Console.WriteLine($"index: {frameIndex} from amount: {amountOfAngles} with angle: {angle} and animationAngle: {animationAngle}");

            return (radians, frameIndex);
        }

        private static int Modulo(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
