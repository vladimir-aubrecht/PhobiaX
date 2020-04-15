using PhobiaX.Actions;
using PhobiaX.Assets;
using PhobiaX.GameObjects;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;
using PhobiaX.SDL2.Wrappers;
using PhobiaX.UserInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.GameLoops
{
	public class GameLoopFactory
	{
		private readonly AssetProvider assetProvider;
		private readonly ISDL2 sdl2;
		private readonly GameUI gameUI;
		private readonly WindowOptions windowOptions;
		private bool areGameLoopAssetsLoaded;

		public GameLoopFactory(AssetProvider assetProvider, ISDL2 sdl2, GameUI gameUI, WindowOptions windowOptions)
		{
			this.assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
			this.gameUI = gameUI ?? throw new ArgumentNullException(nameof(gameUI));
			this.windowOptions = windowOptions ?? throw new ArgumentNullException(nameof(windowOptions));
		}

		public GameLoop CreateGameLoop()
		{
			if (!areGameLoopAssetsLoaded)
			{
				assetProvider.LoadAnimations("AssetResources/Player", "neutral", "death", false, new SDLColor(2, 65, 17), new SDLColor(2, 66, 17), new SDLColor(2, 66, 18));
				assetProvider.LoadAnimations("AssetResources/Aliens", "neutral", "death", false, new SDLColor(0, 0, 255), new SDLColor(18, 18, 242));
				assetProvider.LoadAnimations("AssetResources/Effects", "rocket", "explosion", true, new SDLColor(2, 65, 17), new SDLColor(2, 66, 17), new SDLColor(2, 66, 18), new SDLColor(0, 112, 5), new SDLColor(0, 111, 5), new SDLColor(16, 107, 7), new SDLColor(5, 110, 6), new SDLColor(4, 110, 5), new SDLColor(21, 105, 7));
				areGameLoopAssetsLoaded = true;
			}

			var keyboardStates = new SDLKeyboardStates(sdl2);
			var actionBinder = new ActionBinder(keyboardStates);

			var playerAnimatedSet = assetProvider.GetAnimatedSurfaces()["player"];
			var effectsAnimatedSet = assetProvider.GetAnimatedSurfaces()["effects"];

			var player1 = new PlayerGameObject(new AnimatedSet(playerAnimatedSet), new AnimatedSet(effectsAnimatedSet));
			var player2 = new PlayerGameObject(new AnimatedSet(playerAnimatedSet), new AnimatedSet(effectsAnimatedSet));

			player1.X = windowOptions.Width / 3;
			player2.X = 2 * windowOptions.Width / 3;
			player1.Y = windowOptions.Height / 2;
			player2.Y = windowOptions.Height / 2;

			return new GameLoop(player1, player2, actionBinder, keyboardStates, gameUI);
		}
	}
}
