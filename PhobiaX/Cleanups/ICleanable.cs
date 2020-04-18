using PhobiaX.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Cleanups
{
	public interface ICleanable
	{
		void Cleanup(IGameObject gameObject);
		void CleanupAll();
	}
}
