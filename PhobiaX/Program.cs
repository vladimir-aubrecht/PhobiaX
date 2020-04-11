using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhobiaX.Actions;
using PhobiaX.Assets;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;
using PhobiaX.SDL2.Wrappers;
using SDL2;

namespace PhobiaX
{
    class Program
    {
        private readonly SDLApplication application;
        private readonly SDLRenderer renderer;
        private readonly SDLEventProcessor eventProcessor;
        private readonly SDLKeyboardStates keyboardProcessor;
        private readonly AssetProvider assetProvider;
        private readonly ActionBinder actionBinder;
        private readonly EnemyManager enemyManager;
        private SDLSurface screenSurface;
        private GameObject hero1;
        private GameObject hero2;

        private IList<EffectGameObject> rocketsToDrop = new List<EffectGameObject>();
        private IList<EffectGameObject> rockets = new List<EffectGameObject>();

        private bool player1Fire = false;
        private bool player2Fire = false;


        public Program(SDLApplication application, SDLRenderer renderer, SDLEventProcessor eventProcessor, SDLKeyboardStates keyboardProcessor, AssetProvider assetProvider, ActionBinder actionBinder, WindowOptions windowOptions)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.eventProcessor = eventProcessor ?? throw new ArgumentNullException(nameof(eventProcessor));
            this.keyboardProcessor = keyboardProcessor ?? throw new ArgumentNullException(nameof(keyboardProcessor));
            this.assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
            this.actionBinder = actionBinder ?? throw new ArgumentNullException(nameof(actionBinder));

            screenSurface = renderer.CreateSurface(windowOptions.Width, windowOptions.Height);

            assetProvider.LoadSurfaces("AssetResources/UI");
            assetProvider.LoadSurfaces("AssetResources/Environments");
            assetProvider.LoadAnimations("AssetResources/Player", "neutral", 2, 65, 17);
            assetProvider.LoadAnimations("AssetResources/Aliens", "neutral", 0, 0, 255);
            assetProvider.LoadAnimations("AssetResources/Effects", "rocket", 2, 65, 17);

            var playerAnimatedSet = assetProvider.GetAnimatedSurfaces()["player"];
            hero1 = new GameObject(new AnimatedSet(playerAnimatedSet));
            hero2 = new GameObject(new AnimatedSet(playerAnimatedSet));

            hero1.X = windowOptions.Width / 3;
            hero2.X = 2 * windowOptions.Width / 3;
            hero1.Y = windowOptions.Height / 2;
            hero2.Y = windowOptions.Height / 2;

            this.enemyManager = new EnemyManager(assetProvider.GetAnimatedSurfaces()["aliens"], windowOptions, hero1, hero2);

            InitKeyboardController();
        }

        private void InitKeyboardController()
        {
            actionBinder.AssignKeysToGameAction(GameAction.Quit, false, SDL.SDL_Scancode.SDL_SCANCODE_Q);

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
            actionBinder.RegisterPressAction(GameAction.Player1Fire, () => player1Fire = true);

            actionBinder.RegisterPressAction(GameAction.Player2MoveForward, () => hero2.MoveForward());
            actionBinder.RegisterPressAction(GameAction.Player2MoveBackward, () => hero2.MoveBackward());
            actionBinder.RegisterPressAction(GameAction.Player2RotateLeft, () => hero2.TurnLeft());
            actionBinder.RegisterPressAction(GameAction.Player2RotateRight, () => hero2.TurnRight());
            actionBinder.RegisterPressAction(GameAction.Player2StopMoving, () => hero2.Stop());
            actionBinder.RegisterPressAction(GameAction.Player2Fire, () => player2Fire = true);

            actionBinder.RegisterPressAction(GameAction.Quit, () => application.Quit());
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
            var surfaces = assetProvider.GetSurfaces();
            var map = surfaces.GetSurface("environments_grass");

            keyboardProcessor.ScanKeys();

            map.BlitScaled(screenSurface, IntPtr.Zero);

            hero1.Draw(screenSurface);
            hero2.Draw(screenSurface);

            enemyManager.Draw(screenSurface);

            if (player1Fire)
            {
                var effectsAnimatedSet = assetProvider.GetAnimatedSurfaces()["effects"];
                var rocket = new EffectGameObject(new AnimatedSet(effectsAnimatedSet), hero1);
                rockets.Add(rocket);
                player1Fire = false;
            }

            if (player2Fire)
            {
                var effectsAnimatedSet = assetProvider.GetAnimatedSurfaces()["effects"];
                var rocket = new EffectGameObject(new AnimatedSet(effectsAnimatedSet), hero2);
                rockets.Add(rocket);
                player2Fire = false;
            }

            foreach (var rocket in rockets)
            {
                if (rocket.IsColliding(hero1) || rocket.IsColliding(hero2) || !rocket.IsColliding(0, 0, screenSurface))
                {
                    rocketsToDrop.Add(rocket);
                    continue;
                }

                rocket.MoveForward();
                rocket.Draw(screenSurface);
            }

            foreach (var rocket in rocketsToDrop)
            {
                rockets.Remove(rocket);
            }

            rocketsToDrop.Clear();

            renderer.Copy(screenSurface.SurfacePointer, IntPtr.Zero, IntPtr.Zero);

            renderer.Present();
            application.Delay(20);

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
            serviceCollection.AddSingleton((sc) => new WindowOptions { Title = "PhobiaX", X = 0, Y = 0, Width = 1024, Height = 768, WindowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL });
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

            return serviceCollection.BuildServiceProvider();
        }
    }
}
