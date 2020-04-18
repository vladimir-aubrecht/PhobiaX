using PhobiaX.Assets;
using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Graphics
{
	public class RenderableAnimation : IRenderableObject
	{
		private readonly AnimatedCollection animatedCollection;

		public RenderableAnimation(AnimatedCollection animatedCollection)
		{
			this.animatedCollection = animatedCollection ?? throw new ArgumentNullException(nameof(animatedCollection));
		}

		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; }

		public int Height { get; }

		public void Draw(SDLSurface destination)
		{

		}
	}
}
