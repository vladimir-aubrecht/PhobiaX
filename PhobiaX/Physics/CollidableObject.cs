using PhobiaX.Assets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Physics
{
	class CollidableObject : ICollidableObject
	{
		public CollissionMetadata Metadata { get; }

		public int X { get; set; }
		public int Y { get; set; }

		public int MiddleX => X + Width / 2;
		public int MiddleY => Y + Height / 2;

		public int Width { get; }
		public int Height { get; }

		public CollidableObject(int width, int height, CollissionMetadata collissionMetadata)
		{
			this.Width = width;
			this.Height = height;
			this.Metadata = collissionMetadata ?? new CollissionMetadata();
		}

		public bool IsColliding(ICollidableObject collidableObject)
		{
			if (collidableObject is null)
			{
				return false;
			}

			var x = X + Metadata.OffsetX;
			var y = Y + Metadata.OffsetY;
			var width = Width - 2 * Metadata.OffsetX;
			var height = Height - 2 * Metadata.OffsetY;

			var targetX = collidableObject.X + collidableObject.Metadata.OffsetX;
			var targetY = collidableObject.Y + collidableObject.Metadata.OffsetY;
			var targetWidth = collidableObject.Width - 2 * collidableObject.Metadata.OffsetX;
			var targetHeight = collidableObject.Height - 2 * collidableObject.Metadata.OffsetY;

			var isCollissionX = x + width >= targetX && x <= targetX + targetWidth;
			var isCollissionY = y + height >= targetY && y <= targetY + targetHeight;
			var isCollission = isCollissionX && isCollissionY;

			return isCollission;
		}
	}
}
