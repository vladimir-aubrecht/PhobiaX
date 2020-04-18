using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public interface IGameObject
	{
		Guid Id { get; }

		int X { get; }

		int Y { get; }

		double Angle { get; }

		public bool CanCollide { get; }

		SDLSurface CurrentSurface { get; }

		bool IsColliding(IGameObject gameObject);

		public void Hit();

		void Draw(SDLSurface destination);
	}
}
