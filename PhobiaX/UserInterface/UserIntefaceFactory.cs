using PhobiaX.Assets;
using PhobiaX.GameObjects;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;
using PhobiaX.UserInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.UserInteface
{
	public class UserIntefaceFactory
	{
		private readonly AssetProvider assetProvider;
		private readonly SDLSurfaceFactory surfaceFactory;
		private readonly WindowOptions windowOptions;
		private StaticGameObject map;
		private StaticGameObject scoreBar;
		private StaticGameObject energyBar;
		private TextGameObject scorePlayer1;
		private TextGameObject energyPlayer1;
		private TextGameObject scorePlayer2;
		private TextGameObject energyPlayer2;
		private IDictionary<char, SDLSurface> symbolMap;
		private bool areGameUIAssetsLoaded;

		public UserIntefaceFactory(AssetProvider assetProvider, SDLSurfaceFactory surfaceFactory, WindowOptions windowOptions)
		{
			this.assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
			this.surfaceFactory = surfaceFactory ?? throw new ArgumentNullException(nameof(surfaceFactory));
			this.windowOptions = windowOptions ?? throw new ArgumentNullException(nameof(windowOptions));
		}

		public GameUI CreateGameUI()
		{
			if (!areGameUIAssetsLoaded)
			{
				assetProvider.LoadSurfaces("AssetResources/UI/Bars", new SDLColor(255, 255, 255));
				assetProvider.LoadSurfaces("AssetResources/UI/Symbols", new SDLColor(48, 255, 0), new SDLColor(49, 255, 0));
				assetProvider.LoadSurfaces("AssetResources/Environments", new SDLColor(255, 255, 255));
				areGameUIAssetsLoaded = true;
			}

			var mapSurface = assetProvider.GetSurfaces().GetSurface("environments_grass");
			var scoreBarSurface = assetProvider.GetSurfaces().GetSurface("bars_score");
			var energyBarSurface = assetProvider.GetSurfaces().GetSurface("bars_energy");

			var scaledMapSurface = surfaceFactory.CreateResizedSurface(mapSurface, windowOptions.Width);
			var scaledScoreBarSurface = surfaceFactory.CreateResizedSurface(scoreBarSurface, windowOptions.Width / 6);
			var scaledEnergyBarSurface = surfaceFactory.CreateResizedSurface(energyBarSurface, windowOptions.Width / 6);

			symbolMap = CreateSymbolMap(assetProvider.GetSurfaces());

			var maxWidth = windowOptions.Width / 22;
			var energyBarX = windowOptions.Width - 5 - windowOptions.Width / 6;

			map = new StaticGameObject(0, 0, scaledMapSurface);
			scoreBar = new StaticGameObject(-2, -8, scaledScoreBarSurface);
			energyBar = new StaticGameObject(energyBarX, -8, scaledEnergyBarSurface);

			scorePlayer1 = new TextGameObject(scaledScoreBarSurface.Surface.w - 55, 18, symbolMap, surfaceFactory, maxWidth);
			energyPlayer1 = new TextGameObject(energyBarX, 20, symbolMap, surfaceFactory, maxWidth + 15);

			scorePlayer2 = new TextGameObject(scaledScoreBarSurface.Surface.w - 55, 38, symbolMap, surfaceFactory, maxWidth);
			energyPlayer2 = new TextGameObject(energyBarX, 40, symbolMap, surfaceFactory, maxWidth + 15);

			return new GameUI(map, scorePlayer1, scorePlayer2, energyPlayer1, energyPlayer2, scoreBar, energyBar);
		}

		private static Dictionary<char, SDLSurface> CreateSymbolMap(SurfaceAssets surfaceAssets)
		{
			string symbols = "abcdefghijklmnopqrstuvwxyz0123456789";
			var symbolMap = new Dictionary<char, SDLSurface>();

			foreach (var symbol in symbols)
			{
				var surface = surfaceAssets.GetSurface($"symbols_{symbol}");
				symbolMap.Add(symbol, surface);
			}

			return symbolMap;
		}

	}
}
