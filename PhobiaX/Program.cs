﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhobiaX.Actions;
using PhobiaX.Assets;
using PhobiaX.GameObjects;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;
using PhobiaX.SDL2.Wrappers;
using SDL2;

namespace PhobiaX
{
    class Program
    {
        private readonly TimeSpan renderingDelay = TimeSpan.FromMilliseconds(20);
        private readonly SDLApplication application;
        private readonly SDLRenderer renderer;
        private readonly SDLEventProcessor eventProcessor;
        private readonly SDLKeyboardStates keyboardProcessor;
        private readonly AssetProvider assetProvider;
        private readonly ActionBinder actionBinder;
        private readonly SDLTextureFactory textureFactory;
        private readonly WindowOptions windowOptions;
        private EnemyManager enemyManager;
        private SDLSurface screenSurface;
        private PlayerGameObject hero1;
        private PlayerGameObject hero2;
        private StaticGameObject map;
        private StaticGameObject scoreBar;
        private StaticGameObject energyBar;
        private TextGameObject scorePlayer1;
        private TextGameObject energyPlayer1;
        private TextGameObject scorePlayer2;
        private TextGameObject energyPlayer2;
        private IDictionary<char, SDLSurface> symbolMap;
        private AnimatedSet playerAnimatedSet;
        private AnimatedSet effectsAnimatedSet;

        public Program(SDLApplication application, SDLRenderer renderer, SDLEventProcessor eventProcessor, SDLKeyboardStates keyboardProcessor, AssetProvider assetProvider, ActionBinder actionBinder, SDLSurfaceFactory surfaceFactory, SDLTextureFactory textureFactory, WindowOptions windowOptions)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.eventProcessor = eventProcessor ?? throw new ArgumentNullException(nameof(eventProcessor));
            this.keyboardProcessor = keyboardProcessor ?? throw new ArgumentNullException(nameof(keyboardProcessor));
            this.assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
            this.actionBinder = actionBinder ?? throw new ArgumentNullException(nameof(actionBinder));
            this.textureFactory = textureFactory ?? throw new ArgumentNullException(nameof(textureFactory));
            this.windowOptions = windowOptions;

            screenSurface = surfaceFactory.CreateSurface(windowOptions.Width, windowOptions.Height);

            assetProvider.LoadSurfaces("AssetResources/UI/Bars", new SDLColor(255, 255, 255));
            assetProvider.LoadSurfaces("AssetResources/UI/Symbols", new SDLColor(48, 255, 0), new SDLColor(49, 255, 0));
            assetProvider.LoadSurfaces("AssetResources/Environments", new SDLColor(255, 255, 255));
            assetProvider.LoadAnimations("AssetResources/Player", "neutral", "death", false, new SDLColor(2, 65, 17), new SDLColor(2, 66, 17), new SDLColor(2, 66, 18));
            assetProvider.LoadAnimations("AssetResources/Aliens", "neutral", "death", false, new SDLColor(0, 0, 255), new SDLColor(18, 18, 242));
            assetProvider.LoadAnimations("AssetResources/Effects", "rocket", "explosion", true, new SDLColor(2, 65, 17), new SDLColor(2, 66, 17), new SDLColor(2, 66, 18), new SDLColor(0, 112, 5), new SDLColor(0, 111, 5), new SDLColor(16, 107, 7), new SDLColor(5, 110, 6), new SDLColor(4, 110, 5), new SDLColor(21, 105, 7));

            playerAnimatedSet = assetProvider.GetAnimatedSurfaces()["player"];
            effectsAnimatedSet = assetProvider.GetAnimatedSurfaces()["effects"];
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

            InitGameObjects(windowOptions, symbolMap, playerAnimatedSet, effectsAnimatedSet);

            InitKeyboardController();
        }

        private void Restart()
        {
            InitGameObjects(windowOptions, symbolMap, playerAnimatedSet, effectsAnimatedSet);
        }

        private void InitGameObjects(WindowOptions windowOptions, IDictionary<char, SDLSurface> symbolMap, AnimatedSet playerAnimatedSet, AnimatedSet effectsAnimatedSet)
        {
            hero1 = new PlayerGameObject(new AnimatedSet(playerAnimatedSet), new AnimatedSet(effectsAnimatedSet));
            hero2 = new PlayerGameObject(new AnimatedSet(playerAnimatedSet), new AnimatedSet(effectsAnimatedSet));

            hero1.X = windowOptions.Width / 3;
            hero2.X = 2 * windowOptions.Width / 3;
            hero1.Y = windowOptions.Height / 2;
            hero2.Y = windowOptions.Height / 2;

            this.enemyManager = new EnemyManager(assetProvider.GetAnimatedSurfaces()["aliens"], windowOptions, hero1, hero2);
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

        private void InitKeyboardController()
        {
            actionBinder.AssignKeysToGameAction(GameAction.Quit, false, SDL.SDL_Scancode.SDL_SCANCODE_Q);
            actionBinder.AssignKeysToGameAction(GameAction.Restart, false, SDL.SDL_Scancode.SDL_SCANCODE_F2);

            actionBinder.AssignKeysToGameAction(GameAction.Player1RotateLeft, false, SDL.SDL_Scancode.SDL_SCANCODE_LEFT);
            actionBinder.AssignKeysToGameAction(GameAction.Player1RotateRight, false, SDL.SDL_Scancode.SDL_SCANCODE_RIGHT);
            actionBinder.AssignKeysToGameAction(GameAction.Player1MoveForward, false, SDL.SDL_Scancode.SDL_SCANCODE_UP);
            actionBinder.AssignKeysToGameAction(GameAction.Player1MoveBackward, false, SDL.SDL_Scancode.SDL_SCANCODE_DOWN);
            actionBinder.AssignKeysToGameAction(GameAction.Player1Fire, false, SDL.SDL_Scancode.SDL_SCANCODE_RALT);
            actionBinder.AssignKeysToGameAction(GameAction.Player1StopMoving, true, SDL.SDL_Scancode.SDL_SCANCODE_UP, SDL.SDL_Scancode.SDL_SCANCODE_DOWN);

            actionBinder.AssignKeysToGameAction(GameAction.Player2RotateLeft, false, SDL.SDL_Scancode.SDL_SCANCODE_A);
            actionBinder.AssignKeysToGameAction(GameAction.Player2RotateRight, false, SDL.SDL_Scancode.SDL_SCANCODE_D);
            actionBinder.AssignKeysToGameAction(GameAction.Player2MoveForward, false, SDL.SDL_Scancode.SDL_SCANCODE_W);
            actionBinder.AssignKeysToGameAction(GameAction.Player2MoveBackward, false, SDL.SDL_Scancode.SDL_SCANCODE_S);
            actionBinder.AssignKeysToGameAction(GameAction.Player2Fire, false, SDL.SDL_Scancode.SDL_SCANCODE_LALT);
            actionBinder.AssignKeysToGameAction(GameAction.Player2StopMoving, true, SDL.SDL_Scancode.SDL_SCANCODE_W, SDL.SDL_Scancode.SDL_SCANCODE_S);

            actionBinder.RegisterPressAction(GameAction.Player1MoveForward, () => hero1.MoveForward());
            actionBinder.RegisterPressAction(GameAction.Player1MoveBackward, () => hero1.MoveBackward());
            actionBinder.RegisterPressAction(GameAction.Player1RotateLeft, () => hero1.TurnLeft());
            actionBinder.RegisterPressAction(GameAction.Player1RotateRight, () => hero1.TurnRight());
            actionBinder.RegisterPressAction(GameAction.Player1StopMoving, () => hero1.Stop());
            actionBinder.RegisterPressAction(GameAction.Player1Fire, () => hero1.Shoot());

            actionBinder.RegisterPressAction(GameAction.Player2MoveForward, () => hero2.MoveForward());
            actionBinder.RegisterPressAction(GameAction.Player2MoveBackward, () => hero2.MoveBackward());
            actionBinder.RegisterPressAction(GameAction.Player2RotateLeft, () => hero2.TurnLeft());
            actionBinder.RegisterPressAction(GameAction.Player2RotateRight, () => hero2.TurnRight());
            actionBinder.RegisterPressAction(GameAction.Player2StopMoving, () => hero2.Stop());
            actionBinder.RegisterPressAction(GameAction.Player2Fire, () => hero2.Shoot());

            actionBinder.RegisterPressAction(GameAction.Quit, () => application.Quit());
            actionBinder.RegisterPressAction(GameAction.Restart, () => Restart());
        }

        public void StartGameLoop()
        {
            while (!application.ShouldQuit)
            {
                DoGameLoop();
            }
        }

        private void DoGameLoop()
        {
            keyboardProcessor.ScanKeys();

            var totalScore = (hero1.Score + hero2.Score);
            enemyManager.SetDesiredAmountOfEnemies(8 + totalScore / 10);

            var enemies = new List<IGameObject>(enemyManager.GetAllEnemies());
            enemies.Add(hero1);
            enemies.Add(hero2);

            scorePlayer1.SetText($"P1 {hero1.Score}");
            energyPlayer1.SetText($"P1 {hero1.Life}");

            scorePlayer2.SetText($"P2 {hero2.Score}");
            energyPlayer2.SetText($"P2 {hero2.Life}");

            hero1.EvaluateRockets(map, enemies);
            hero2.EvaluateRockets(map, enemies);

            enemyManager.MoveEnemies();

            map.Draw(screenSurface);
            enemyManager.Draw(screenSurface);
            hero1.Draw(screenSurface);
            hero2.Draw(screenSurface);

            scoreBar.Draw(screenSurface);
            energyBar.Draw(screenSurface);
            
            scorePlayer1.Draw(screenSurface);
            energyPlayer1.Draw(screenSurface);
            scorePlayer2.Draw(screenSurface);
            energyPlayer2.Draw(screenSurface);

            using (var texture = textureFactory.CreateTexture(screenSurface))
            {
                texture.CopyToRenderer();
                renderer.Present();
                application.Delay(renderingDelay);
            }

            eventProcessor.EvaluateEvents();
        }

        static void Main(string[] args)
        {
            using (var serviceProvider = InitServiceProvider())
            {
                var program = serviceProvider.GetService<Program>();
                program.StartGameLoop();
            }
        }

        private static ServiceProvider InitServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging((builder) => builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
            serviceCollection.AddSingleton((sc) => new WindowOptions { Title = "PhobiaX", X = 0, Y = 0, Width = 1024, Height = 768, WindowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI });
            serviceCollection.AddSingleton((sc) => new RendererOptions { Index = -1, RendererFlags = SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC | SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED });

            serviceCollection.AddSingleton<Program>();
            serviceCollection.AddSingleton<ISDL2, SDL2Wrapper>();
            serviceCollection.AddSingleton<ISDL2Image, SDL2ImageWrapper>();
            serviceCollection.AddSingleton<SDLApplication>();
            serviceCollection.AddSingleton<AssetProvider>();
            serviceCollection.AddSingleton<SDLKeyboardStates>();
            serviceCollection.AddSingleton<SDLEventProcessor>();
            serviceCollection.AddSingleton<ActionBinder>();
            serviceCollection.AddSingleton<SDLWindow>();
            serviceCollection.AddSingleton<SDLRenderer>();
            serviceCollection.AddSingleton<SDLSurfaceFactory>();
            serviceCollection.AddSingleton<SDLTextureFactory>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
