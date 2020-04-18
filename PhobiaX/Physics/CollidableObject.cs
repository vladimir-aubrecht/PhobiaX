using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Physics
{
	class CollidableObject : ICollidableObject
	{
		public int X { get; set; }
		public int Y { get; set; }

		public int Width { get; }
		public int Height { get; }

		public CollidableObject(int width, int height)
		{
			this.Width = width;
			this.Height = height;
		}

		public bool IsColliding(ICollidableObject collidableObject)
		{
			if (collidableObject is null)
			{
				return false;
			}

			var isCollissionX = X + Width >= collidableObject.X && X <= collidableObject.X + collidableObject.Width;
			var isCollissionY = Y + Height >= collidableObject.Y && Y <= collidableObject.Y + collidableObject.Height;
			var isCollission = isCollissionX && isCollissionY;

			return isCollission;
		}
	}
}
