using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhobiaX.Actions;
using PhobiaX.Assets;
using PhobiaX.Game.GameLoops;
using PhobiaX.Game.GameObjects;
using PhobiaX.Game.UserInteface;
using PhobiaX.Game.UserInterface;
using PhobiaX.Graphics;
using PhobiaX.Physics;
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
        private readonly SDLEventProcessor eventProcessor;
        private readonly GameLoopFactory gameLoopFactory;
        private readonly EnemyFactory enemyFactory;
        private readonly Renderer renderer;
        private readonly CollissionObserver collissionObserver;
        private readonly PathFinder pathFinder;
        private EnemyManager enemyManager;
        private GameUI gameUI;
        private GameLoop gameLoop;

        public Program(SDLApplication application, SDLEventProcessor eventProcessor, GameUI gameUI, GameLoopFactory gameLoopFactory, EnemyFactory enemyFactory, Renderer renderer, CollissionObserver collissionObserver, PathFinder pathFinder)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
            this.eventProcessor = eventProcessor ?? throw new ArgumentNullException(nameof(eventProcessor));
            this.gameUI = gameUI ?? throw new ArgumentNullException(nameof(gameUI));
            this.gameLoopFactory = gameLoopFactory ?? throw new ArgumentNullException(nameof(gameLoopFactory));
            this.enemyFactory = enemyFactory ?? throw new ArgumentNullException(nameof(enemyFactory));
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.collissionObserver = collissionObserver ?? throw new ArgumentNullException(nameof(collissionObserver));
            this.pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
            Restart();
        }

        private void Restart()
        {
            gameLoop = gameLoopFactory.CreateGameLoop();

            gameLoop.ActionBinder.AssignKeysToGameAction(GameAction.Quit, false, SDL.SDL_Scancode.SDL_SCANCODE_Q);
            gameLoop.ActionBinder.AssignKeysToGameAction(GameAction.Restart, false, SDL.SDL_Scancode.SDL_SCANCODE_F2);
            gameLoop.ActionBinder.RegisterPressAction(GameAction.Quit, () => application.Quit());
            gameLoop.ActionBinder.RegisterPressAction(GameAction.Restart, () => Restart());

            collissionObserver.SetForObserving("players", new List<IGameObject> { gameLoop.GetPlayer1GameObject(), gameLoop.GetPlayer2GameObject() });

            this.enemyManager = new EnemyManager(enemyFactory, collissionObserver, pathFinder, gameLoop.GetPlayer1GameObject(), gameLoop.GetPlayer2GameObject());
            
            renderer.SetForRendering("ui", gameUI.GetGameObjects());
            renderer.SetForRendering("players", new List<IGameObject> { gameLoop.GetPlayer1GameObject(), gameLoop.GetPlayer2GameObject() });
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
            gameLoop.Evaluate();

            var player1 = gameLoop.GetPlayer1GameObject();
            var player2 = gameLoop.GetPlayer2GameObject();
            var map = gameUI.GetMapGameObject();

            enemyManager.SetDesiredAmountOfEnemies(gameLoop.GetDifficulty());

            var enemies = new List<IGameObject>(enemyManager.GetAllEnemies());
            renderer.SetForRendering("enemies", enemies);
            enemies.Add(player1);
            enemies.Add(player2);

            player1.EvaluateRockets(map, enemies);
            player2.EvaluateRockets(map, enemies);

            enemyManager.MoveEnemies();

            renderer.Render();

            application.Delay(renderingDelay);
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
            serviceCollection.AddSingleton<SDLEventProcessor>();
            serviceCollection.AddSingleton<SDLWindow>();
            serviceCollection.AddSingleton<SDLRenderer>();
            serviceCollection.AddSingleton<SDLSurfaceFactory>();
            serviceCollection.AddSingleton<SDLTextureFactory>();
            serviceCollection.AddSingleton<Renderer>();
            serviceCollection.AddSingleton<CollissionObserver>();
            serviceCollection.AddSingleton<UserIntefaceFactory>();
            serviceCollection.AddSingleton<GameLoopFactory>();
            serviceCollection.AddSingleton<EnemyFactory>();
            serviceCollection.AddSingleton<PathFinder>();
            serviceCollection.AddSingleton<GameUI>((sc) => sc.GetService<UserIntefaceFactory>().CreateGameUI());

            return serviceCollection.BuildServiceProvider();
        }
    }
}
