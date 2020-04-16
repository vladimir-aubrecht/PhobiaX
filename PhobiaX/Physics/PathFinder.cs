using PhobiaX.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhobiaX.Physics
{
	public class PathFinder
	{
		public PathFinder()
		{

		}

        public IGameObject FindClosestTarget(IGameObject objectSearchingPath, IList<IGameObject> targets)
        {
            var closestDistance = double.MaxValue;
            var closesestTarget = targets.First();

            foreach (var target in targets)
            {
                var xDiff = target.X - objectSearchingPath.X;
                var yDiff = target.Y - objectSearchingPath.Y;

                var distance = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closesestTarget = target;
                }
            }

            return closesestTarget;
        }
    }
}
