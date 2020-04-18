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
		private readonly AssetProvider assetProvider;
		private readonly SDLSurfaceFactory surfaceFactory;
		private readonly WindowOptions windowOptions;
		private StaticGameObject scoreBar;
		private StaticGameObject energyBar;
		private TextGameObject scorePlayer1;
		private TextGameObject energyPlayer1;
		private TextGameObject scorePlayer2;
		private TextGameObject energyPlayer2;

		public UserIntefaceFactory(GameObjectFactory gameObjectFactory, AssetProvider assetProvider, SDLSurfaceFactory surfaceFactory, WindowOptions windowOptions)
		{
			this.gameObjectFactory = gameObjectFactory ?? throw new ArgumentNullException(nameof(gameObjectFactory));
			this.assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			this.surfaceFactory = surfaceFactory ?? throw new ArgumentNullException(nameof(surfaceFactory));
			this.windowOptions = windowOptions ?? throw new ArgumentNullException(nameof(windowOptions));
		}

		public GameUI CreateGameUI()
		{
			var scoreBarSurface = assetProvider.GetSurfaces().GetSurface("bars_score");

			var scaledScoreBarSurface = surfaceFactory.CreateResizedSurface(scoreBarSurface, windowOptions.Width / 6);

			var maxWidth = windowOptions.Width / 22;
			var energyBarX = windowOptions.Width - 5 - windowOptions.Width / 6;

			gameObjectFactory.CreateMap();
			gameObjectFactory.CreateScoreBar(-2, -8);
			gameObjectFactory.CreateLifeBar(energyBarX, -8);

			scorePlayer1 = gameObjectFactory.CreateLabel(scaledScoreBarSurface.Surface.w - 55, 18, maxWidth);
			energyPlayer1 = gameObjectFactory.CreateLabel(energyBarX, 20, maxWidth + 15);

			scorePlayer2 = gameObjectFactory.CreateLabel(scaledScoreBarSurface.Surface.w - 55, 38, maxWidth);
			energyPlayer2 = gameObjectFactory.CreateLabel(energyBarX, 40, maxWidth + 15);

			return new GameUI(scorePlayer1, scorePlayer2, energyPlayer1, energyPlayer2);
		}
	}
}
