using PhobiaX.Assets;
using PhobiaX.Game.GameObjects;
using PhobiaX.Graphics;
using PhobiaX.Physics;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;
using System;
using System.Collections.Generic;

namespace PhobiaX
{
	public class GameObjectFactory
	{
        private readonly WindowOptions windowOptions;
        private readonly SDLSurfaceFactory surfaceFactory;
        private readonly TimeThrottler timeThrottler;
        private readonly AnimatedCollection enemyAnimatedSet;
        private readonly AnimatedCollection playerAnimatedSet;
        private readonly AnimatedCollection effectsAnimatedSet;
        private readonly SDLSurface mapSurface;
        private readonly SDLSurface scoreSurface;
        private readonly SDLSurface lifeSurface;
        private readonly SurfaceAssets symbolSurfaceAssets;
        private readonly double angleOfFirstFrame = 90;

        private IList<Action<IGameObject>> callbacks = new List<Action<IGameObject>>();

        public GameObjectFactory(WindowOptions windowOptions, AssetProvider assetProvider, SDLSurfaceFactory surfaceFactory, TimeThrottler timeThrottler)
		{
            this.windowOptions = windowOptions ?? throw new ArgumentNullException(nameof(windowOptions));
            this.surfaceFactory = surfaceFactory ?? throw new ArgumentNullException(nameof(surfaceFactory));
            this.timeThrottler = timeThrottler ?? throw new ArgumentNullException(nameof(timeThrottler));
            _ = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));

            assetProvider.LoadSurfaces("AssetResources/Environments", new SDLColor(255, 255, 255));
            assetProvider.LoadAnimations("AssetResources/Player", "neutral", "death", false, new SDLColor(2, 65, 17), new SDLColor(2, 66, 17), new SDLColor(2, 66, 18));
            assetProvider.LoadAnimations("AssetResources/Effects", "rocket", "explosion", true, new SDLColor(2, 65, 17), new SDLColor(2, 66, 17), new SDLColor(2, 66, 18), new SDLColor(0, 112, 5), new SDLColor(0, 111, 5), new SDLColor(16, 107, 7), new SDLColor(5, 110, 6), new SDLColor(4, 110, 5), new SDLColor(21, 105, 7));
            assetProvider.LoadAnimations("AssetResources/Aliens", "neutral", "death", false, new SDLColor(0, 0, 255), new SDLColor(18, 18, 242));
            assetProvider.LoadSurfaces("AssetResources/UI/Symbols", new SDLColor(48, 255, 0), new SDLColor(49, 255, 0));
            assetProvider.LoadSurfaces("AssetResources/UI/Bars", new SDLColor(255, 255, 255));

            this.symbolSurfaceAssets = assetProvider.GetSurfaces();
            this.mapSurface = assetProvider.GetSurfaces().GetSurface("environments_grass");
            this.enemyAnimatedSet = assetProvider.GetAnimatedSurfaces()["aliens"];
            this.playerAnimatedSet = assetProvider.GetAnimatedSurfaces()["player"];
            this.effectsAnimatedSet = assetProvider.GetAnimatedSurfaces()["effects"];

            scoreSurface = assetProvider.GetSurfaces().GetSurface("bars_score");
            lifeSurface = assetProvider.GetSurfaces().GetSurface("bars_energy");

        }

        private void TriggerCallback(IGameObject gameObject)
        {
            foreach (var callback in this.callbacks)
            {
                callback(gameObject);
            }
        }

        public void ClearCallbacks()
        {
            callbacks.Clear();
        }

        public void OnCreateCallback(Action<IGameObject> callback)
        {
            this.callbacks.Add(callback);
        }

        public StaticGameObject CreateScoreBar(int x, int y)
        {
            var scaledScoreBarSurface = surfaceFactory.CreateResizedSurface(scoreSurface, windowOptions.Width / 6);
            var scoreBar = new StaticGameObject(new RenderableSurface(scaledScoreBarSurface) { X = x, Y = y }, null);

            TriggerCallback(scoreBar);

            return scoreBar;
        }

        public StaticGameObject CreateLifeBar(int x, int y)
        {
            var scaledLifeBarSurface = surfaceFactory.CreateResizedSurface(lifeSurface, windowOptions.Width / 6);
            
            var lifeBar = new StaticGameObject(new RenderableSurface(scaledLifeBarSurface) { X = x, Y = y }, null);

            TriggerCallback(lifeBar);

            return lifeBar;
        }

        public TextGameObject CreateLabel(int x, int y, int maxWidth)
        {
            var symbolMap = CreateSymbolMap(symbolSurfaceAssets);
            var obj = new TextGameObject(x, y, symbolMap, surfaceFactory, maxWidth);

            TriggerCallback(obj);

            return obj;
        }

        public MapGameObject CreateMap()
        {
            var scaledMapSurface = surfaceFactory.CreateResizedSurface(mapSurface, windowOptions.Width);
            var map = new MapGameObject(new RenderableSurface(scaledMapSurface), new CollidableObject(scaledMapSurface.Surface.w, scaledMapSurface.Surface.h));
            TriggerCallback(map);

            return map;
        }

        public RocketGameObject CreateRocket(AnimatedGameObject owner)
        {
            var surface = effectsAnimatedSet.GetDefaultAnimatedSet().GetCurrentFrame().Surface;
            var renderableObject = new RenderablePeriodicAnimation(new RenderableAnimation(this.timeThrottler, new AnimatedCollection(effectsAnimatedSet)), angleOfFirstFrame, true);
            var gameObject = new RocketGameObject(renderableObject, new CollidableObject(surface.w, surface.h), owner);
            TriggerCallback(gameObject);

            return gameObject;
        }

        public PlayerGameObject CreatePlayer(int x, int y)
        {
            var surface = playerAnimatedSet.GetDefaultAnimatedSet().GetCurrentFrame().Surface;
            var renderableObject = new RenderablePeriodicAnimation(new RenderableAnimation(this.timeThrottler, new AnimatedCollection(playerAnimatedSet)), angleOfFirstFrame, false);
            var gameObject = new PlayerGameObject(renderableObject, new CollidableObject(surface.w, surface.h)) { X = x, Y = y };
            TriggerCallback(gameObject);

            return gameObject;
        }

        public EnemyGameObject CreateEnemy()
        {
            var surface = playerAnimatedSet.GetDefaultAnimatedSet().GetCurrentFrame().Surface;
            var renderableObject = new RenderablePeriodicAnimation(new RenderableAnimation(this.timeThrottler, new AnimatedCollection(enemyAnimatedSet)), angleOfFirstFrame, false);
            var enemy = new EnemyGameObject(renderableObject, new CollidableObject(surface.w, surface.h));

            TriggerCallback(enemy);

            return enemy;
        }

        public IList<EnemyGameObject> CreateEnemies(int amount)
        {
            var enemies = new List<EnemyGameObject>(amount);
            for (int i = 0; i < amount; i++)
            {
                var enemy = CreateEnemy();
                enemies.Add(enemy);
            }

            return enemies;
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
