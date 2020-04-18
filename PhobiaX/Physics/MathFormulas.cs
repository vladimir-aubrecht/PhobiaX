using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Physics
{
	public static class MathFormulas
	{
        private const double CircleDegrees = 360;

        public static double CalculateAngleTowardsGameObject(int x, int y, int targetX, int targetY)
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
