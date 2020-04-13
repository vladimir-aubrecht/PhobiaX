using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.SDL2
{
	public struct SDLColor
	{
		public byte Red;
		public byte Green;
		public byte Blue;

		public SDLColor(byte red, byte green, byte blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}
	}
}
