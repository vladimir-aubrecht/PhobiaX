using PhobiaX.Assets.Models;

namespace PhobiaX.Physics
{
	public interface ICollidableObject
	{
		int X { get; set; }
		int Y { get; set; }

		int MiddleX { get; }
		int MiddleY { get; }

		int Width { get; }

		int Height { get; }

		CollissionMetadata Metadata { get; }

		bool IsColliding(ICollidableObject collidableObject);
	}
}