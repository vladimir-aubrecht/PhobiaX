using PhobiaX.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public class EnemyGameObject : AnimatedGameObject
	{
		public EnemyGameObject(AnimatedCollection animatedSet) : base(animatedSet)
		{
			Speed = 1;
		}
    }
}
