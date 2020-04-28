using PhobiaX.Assets;
using PhobiaX.Game.GameObjects;
using PhobiaX.Game.UserInterface;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.UserInteface
{
	public class UserIntefaceFactory
	{
		private readonly GameObjectFactory gameObjectFactory;
		private readonly WindowOptions windowOptions;
		
		public UserIntefaceFactory(GameObjectFactory gameObjectFactory, WindowOptions windowOptions)
		{
			this.gameObjectFactory = gameObjectFactory ?? throw new ArgumentNullException(nameof(gameObjectFactory));
			this.windowOptions = windowOptions ?? throw new ArgumentNullException(nameof(windowOptions));
		}

		public ScoreUI CreateScoreUI(int initialScore, int initialLife)
		{
			var maxWidth = windowOptions.Width / 22;
			var energyBarX = windowOptions.Width - 5 - windowOptions.Width / 6;

			gameObjectFactory.CreateMap();
			gameObjectFactory.CreateScoreBar(-2, -8);
			gameObjectFactory.CreateLifeBar(energyBarX, -8);

			var scorePlayer1 = gameObjectFactory.CreateLabel(windowOptions.Width / 6 - 55, 18, maxWidth);
			var scorePlayer2 = gameObjectFactory.CreateLabel(windowOptions.Width / 6 - 55, 38, maxWidth);

			var energyPlayer1 = gameObjectFactory.CreateLabel(energyBarX, 20, maxWidth + 15);
			var energyPlayer2 = gameObjectFactory.CreateLabel(energyBarX, 40, maxWidth + 15);

			var scoreUI = new ScoreUI(new List<TextGameObject> { scorePlayer1, scorePlayer2 }, new List<TextGameObject> { energyPlayer1, energyPlayer2 });
			scoreUI.SetPlayerLife(0, initialLife);
			scoreUI.SetPlayerScore(0, initialScore);
			scoreUI.SetPlayerLife(1, initialLife);
			scoreUI.SetPlayerScore(1, initialScore);

			return scoreUI;
		}
	}
}
