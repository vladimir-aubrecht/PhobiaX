﻿using PhobiaX.SDL2;
using System;

namespace PhobiaX.Graphics
{
	public interface IRenderableObject
	{
		int X { get; set; }
		int Y { get; set; }
		int Width { get; }
		int Height { get; }
		bool ShouldDestroy { get; }

		int RenderingPriority { get; set; }

		void Draw(SDLSurface destination);
	}
}