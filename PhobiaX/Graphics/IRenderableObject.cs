using PhobiaX.SDL2;

namespace PhobiaX.Graphics
{
	public interface IRenderableObject
	{
		int X { get; }
		int Y { get; }
		int Width { get; }
		int Height { get; }

		void Draw(SDLSurface destination);
	}
}