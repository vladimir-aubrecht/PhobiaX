﻿using PhobiaX.Assets;
using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public class PlayerGameObject : AnimatedGameObject
	{
		private static int playerGameObjectCount = 0;

		public int PlayerNumber { get; } = playerGameObjectCount;
		public int Score { get; set; } = 0;
		public int Life { get; set; } = 100;

		public PlayerGameObject(AnimatedCollection playerAnimatedSet, GameObjectFactory gameObjectFactory) : base(playerAnimatedSet, false)
		{
			playerGameObjectCount++;
		}

		public override void Hit()
		{
			if (Life > 0)
			{
				Life--;
			}
			
			if (Life == 0)
			{
				base.Hit();
			}
		}
	}
}