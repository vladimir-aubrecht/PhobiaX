using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhobiaX.Actions;
using PhobiaX.Ai;
using PhobiaX.Assets;
using PhobiaX.Cleanups;
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
        private readonly GameGarbageObserver gameGarbageObserver;
        private readonly GameLoopFactory gameLoopFactory;
        private readonly EnemyAiObserver enemyAiObserver;
        private readonly GameObjectFactory gameObjectFactory;
        private readonly Renderer renderer;
        private readonly CollissionObserver collissionObserver;
        private GameUI gameUI;
        private GameLoop gameLoop;

        public Program(SDLApplication application, SDLEventProcessor eventProcessor, GameGarbageObserver gameGarbageObserver, GameUI gameUI, GameLoopFactory gameLoopFactory, EnemyAiObserver enemyAiObserver, GameObjectFactory gameObjectFactory, Renderer renderer, CollissionObserver collissionObserver)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
            this.eventProcessor = eventProcessor ?? throw new ArgumentNullException(nameof(eventProcessor));
            this.gameGarbageObserver = gameGarbageObserver ?? throw new ArgumentNullException(nameof(gameGarbageObserver));
            this.gameUI = gameUI ?? throw new ArgumentNullException(nameof(gameUI));
            this.gameLoopFactory = gameLoopFactory ?? throw new ArgumentNullException(nameof(gameLoopFactory));
            this.enemyAiObserver = enemyAiObserver ?? throw new ArgumentNullException(nameof(enemyAiObserver));
            this.gameObjectFactory = gameObjectFactory ?? throw new ArgumentNullException(nameof(gameObjectFactory));
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.collissionObserver = collissionObserver ?? throw new ArgumentNullException(nameof(collissionObserver));

            Restart();
        }

        private void Restart()
        {
            gameObjectFactory.ClearCallbacks();

            gameGarbageObserver.CleanupAll();
            gameGarbageObserver.AddCleanableObject(renderer);
            gameGarbageObserver.AddCleanableObject(collissionObserver);
            gameGarbageObserver.AddCleanableObject(enemyAiObserver);

            enemyAiObserver.EnemyDiedCallback((gameObject) => gameGarbageObserver.Observe(gameObject));

            foreach (var obj in gameUI.GetGameObjects())
            {
                renderer.Add(obj);
            }

            gameObjectFactory.OnCreateCallback((gameObject) => {
                collissionObserver.Observe(gameObject);
                renderer.Add(gameObject);

                if (gameObject is EnemyGameObject)
                {
                    enemyAiObserver.Observe(gameObject as EnemyGameObject);
                }

                if (gameObject is PlayerGameObject)
                {
                    enemyAiObserver.AddTarget(gameObject);
                }
            });
            
            gameLoop = gameLoopFactory.CreateGameLoop();

            gameLoop.ActionBinder.AssignKeysToGameAction(GameAction.Quit, false, SDL.SDL_Scancode.SDL_SCANCODE_Q);
            gameLoop.ActionBinder.AssignKeysToGameAction(GameAction.Restart, false, SDL.SDL_Scancode.SDL_SCANCODE_F2);
            gameLoop.ActionBinder.RegisterPressAction(GameAction.Quit, () => application.Quit());
            gameLoop.ActionBinder.RegisterPressAction(GameAction.Restart, () => Restart());

            collissionObserver.OnObserveCallback((gameObject, colliders) => {

                colliders = colliders.Where(i => i.CanCollide).ToList();
                if (!gameObject.CanCollide || !colliders.Any())
                {
                    return;
                }

                if (gameObject is PlayerGameObject && colliders.OfType<EnemyGameObject>().Any())
                {
                    gameObject.Hit();
                }
                else if (gameObject is EnemyGameObject && colliders.OfType<RocketGameObject>().Any())
                {
                    gameObject.Hit();

                    foreach (var rocket in colliders.OfType<RocketGameObject>())
                    {
                        var castedRocket = rocket as RocketGameObject;
                        (castedRocket.Owner as PlayerGameObject).Score++;
                    }
                }

                if (gameObject is EnemyGameObject)
                {
                    (gameObject as EnemyGameObject).Stop();
                }

                if (gameObject is RocketGameObject)
                {
                    gameObject.Hit();
                }
            });
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
            enemyAiObserver.SetAmountOfEnemies(gameLoop.GetDifficulty());

            gameLoop.Evaluate();
            eventProcessor.Evaluate();
            collissionObserver.Evaluate();
            enemyAiObserver.Evaluate();
            renderer.Evaluate();
            gameGarbageObserver.Evaluate();

            application.Delay(renderingDelay);
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
            serviceCollection.AddSingleton<GameObjectFactory>();
            serviceCollection.AddSingleton<PathFinder>();
            serviceCollection.AddSingleton<TimeThrottler>();
            serviceCollection.AddSingleton<EnemyAiObserver>();
            serviceCollection.AddSingleton<GameGarbageObserver>();
            serviceCollection.AddSingleton<GameUI>((sc) => sc.GetService<UserIntefaceFactory>().CreateGameUI());

            return serviceCollection.BuildServiceProvider();
        }
    }
}
