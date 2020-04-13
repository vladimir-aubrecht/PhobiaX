using PhobiaX.Assets;
using PhobiaX.SDL2;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhobiaX.GameObjects
{
	public class TextGameObject : IGameObject
	{
		private string text;
		private readonly IDictionary<char, SDLSurface> symbolSurfaces;
		private readonly SDLRenderer renderer;
		private readonly int maxWidth;

		public int X { get; }

		public int Y { get; }

		public bool CanBeHit { get; } = false;

		public TextGameObject(int x, int y, IDictionary<char, SDLSurface> symbolSurfaces, SDLRenderer renderer, int maxWidth)
		{
			X = x;
			Y = y;
			this.symbolSurfaces = symbolSurfaces ?? throw new ArgumentNullException(nameof(symbolSurfaces));
			this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
			this.maxWidth = maxWidth;
			SetText("");
		}

		public SDLSurface CurrentSurface { get; private set; }

		public void Draw(SDLSurface destination)
		{
			var surfaceRectangle = new SDL.SDL_Rect() { x = X, y = Y, w = CurrentSurface.Surface.w, h = CurrentSurface.Surface.h };
			this.CurrentSurface.BlitSurface(destination, ref surfaceRectangle);
		}

		public void Hit() { }

		public bool IsColliding(IGameObject gameObject) => false;

		public void SetText(string text)
		{
			text = text.ToLower();
			this.text = text;

			int width = 0;
			int height = 0;
			foreach (var symbol in text)
			{
				if (symbol == ' ')
				{
					width += symbolSurfaces.First().Value.Surface.w;
					continue;
				}

				var surface = symbolSurfaces[symbol];
				width += surface.Surface.w;
				height = Math.Max(height, surface.Surface.h);
			}

			var textSurface = renderer.CreateSurface(width, height);

			int x = 0;
			foreach (var symbol in text)
			{
				if (symbol == ' ')
				{
					x += symbolSurfaces.First().Value.Surface.w;
					continue;
				}

				var surface = symbolSurfaces[symbol];
				var surfaceRectangle = new SDL.SDL_Rect() { x = x, y = 0, w = surface.Surface.w, h = surface.Surface.h };
				surface.BlitSurface(textSurface, ref surfaceRectangle);
				x += surface.Surface.w;
			}

			CurrentSurface = renderer.CreateResizedSurface(textSurface, maxWidth);
		}
	}
}
