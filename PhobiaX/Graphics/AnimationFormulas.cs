using PhobiaX.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Graphics
{
	public static class AnimationFormulas
	{
		public static int GetFrameIndexByAngle(double currentAngle, double angleOfFirstFrame, int framesCountInAnimation)
		{
			double animationAngle = MathFormulas.Modulo(currentAngle - angleOfFirstFrame, MathFormulas.CircleDegrees);
			return (int)MathFormulas.Modulo(Math.Ceiling(animationAngle * framesCountInAnimation / MathFormulas.CircleDegrees), framesCountInAnimation);
		}

		public static double GetAngleByIndex(int frameIndex, double angleOfFirstFrame, int framesCountInAnimation)
		{
			var angle = (MathFormulas.CircleDegrees / framesCountInAnimation) * frameIndex;
			return MathFormulas.Modulo(angle - angleOfFirstFrame, MathFormulas.CircleDegrees);
		}
	}
}
