using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Physics
{
	public static class MathFormulas
	{
        private static Random random = new Random();
        public const double CircleDegrees = 360;

        public static (double xIncrement, double yIncrement) GetIncrementByAngle(double speed, double angle)
        {
            var radians = ToRadians(angle);
            return (speed * Math.Cos(radians), speed * Math.Sin(radians));
        }

        public static double GetAngleTowardsTarget(int x, int y, int targetX, int targetY)
        {
            double xDiff = targetX - x;
            double yDiff = targetY - y;
            double xDistance = Math.Abs(xDiff);
            double yDistance = Math.Abs(yDiff);

            var angle = Modulo(ToDegress(Math.Atan(yDistance / xDistance)), CircleDegrees);

            // Left Down
            if (xDiff < 0 && yDiff >= 0)
            {
                return 180 + angle;
            }
            // Right Down
            else if (xDiff > 0 && yDiff > 0)
            {
                return 360 - angle;
            }
            // Left Up
            else if (xDiff < 0 && yDiff <= 0)
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

        public static (int x, int y) GetRandomLocationAroundRectangle(int objectWidth, int objectHeight, int rectangleWidth, int rectangleHeight)
        {
            var minX = -random.Next(150) - objectWidth;
            var minY = -random.Next(150) - objectHeight;
            var maxX = random.Next(rectangleWidth);
            var maxY = random.Next(rectangleHeight);

            var rnd = random.Next(100);
            if (rnd <= 25)
            {
                return (maxX, maxY);
            }
            else if (rnd > 25 && rnd <= 50)
            {
                return (minX, maxY);
            }
            else if (rnd > 50 && rnd <= 75)
            {
                return (rectangleWidth - minX, maxY);
            }
            else
            {
                return (maxX, rectangleHeight - minY);
            }
        }

        public static double ToRadians(double degress)
        {
            return degress * Math.PI / 180;
        }

        public static double ToDegress(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double Modulo(double x, double m)
        {
            return (x % m + m) % m;
        }

    }
}
