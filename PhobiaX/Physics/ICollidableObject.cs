namespace PhobiaX.Physics
{
	public interface ICollidableObject
	{
		int X { get; set; }
		int Y { get; set; }

		int Width { get; }

		int Height { get; }

		bool IsColliding(ICollidableObject collidableObject);
	}
}