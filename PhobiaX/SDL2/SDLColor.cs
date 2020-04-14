using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.SDL2
{
	public struct SDLColor
	{
		public static readonly SDLColor Black = new SDLColor(0, 0, 0);

		public readonly byte Red;
		public readonly byte Green;
		public readonly byte Blue;

		public SDLColor(byte red, byte green, byte blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}
	}
}
