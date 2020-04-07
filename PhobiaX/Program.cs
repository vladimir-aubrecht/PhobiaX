using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhobiaX.Actions;
using PhobiaX.SDL2;
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
        private GameObject hero;

        public Program(SDLApplication application, SDLRenderer renderer, SDLEventProcessor eventProcessor, SDLKeyboardStates keyboardProcessor, AssetProvider assetProvider, ActionBinder actionBinder)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.eventProcessor = eventProcessor ?? throw new ArgumentNullException(nameof(eventProcessor));
            this.keyboardProcessor = keyboardProcessor ?? throw new ArgumentNullException(nameof(keyboardProcessor));
            this.assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
            this.actionBinder = actionBinder ?? throw new ArgumentNullException(nameof(actionBinder));

            assetProvider.LoadAssets("AssetResources");

            hero = new GameObject(assetProvider.GetAnimatedSurfaces());

            InitKeyboardController();
        }

        private void InitKeyboardController()
        {
            actionBinder.AssignKeysToGameAction(GameAction.Quit, false, SDL.SDL_Scancode.SDL_SCANCODE_Q);
            actionBinder.AssignKeysToGameAction(GameAction.RotateLeft, false, SDL.SDL_Scancode.SDL_SCANCODE_LEFT);
            actionBinder.AssignKeysToGameAction(GameAction.RotateRight, false, SDL.SDL_Scancode.SDL_SCANCODE_RIGHT);
            actionBinder.AssignKeysToGameAction(GameAction.MoveForward, false, SDL.SDL_Scancode.SDL_SCANCODE_UP);
            actionBinder.AssignKeysToGameAction(GameAction.MoveBackward, false, SDL.SDL_Scancode.SDL_SCANCODE_DOWN);
            actionBinder.AssignKeysToGameAction(GameAction.StopMoving, true, SDL.SDL_Scancode.SDL_SCANCODE_UP, SDL.SDL_Scancode.SDL_SCANCODE_DOWN);

            actionBinder.RegisterPressAction(GameAction.MoveForward, () => hero.MoveForward());
            actionBinder.RegisterPressAction(GameAction.MoveBackward, () => hero.MoveBackward());
            actionBinder.RegisterPressAction(GameAction.RotateLeft, () => hero.TurnLeft());
            actionBinder.RegisterPressAction(GameAction.RotateRight, () => hero.TurnRight());
            actionBinder.RegisterPressAction(GameAction.StopMoving, () => hero.Stop());
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

            renderer.Clear();

            var finalSurface = renderer.CreateSurface(1024, 768);
            map.BlitScaled(finalSurface, IntPtr.Zero);

            hero.Draw(finalSurface);

            renderer.Copy(finalSurface.SurfacePointer, IntPtr.Zero, IntPtr.Zero);

            renderer.Present();
            renderer.Delay(20);

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

            serviceCollection.AddSingleton<Program>();
            serviceCollection.AddSingleton<ISDL2, SDL2Wrapper>();
            serviceCollection.AddSingleton<ISDL2Image, SDL2ImageWrapper>();
            serviceCollection.AddSingleton<SDLApplication>();
            serviceCollection.AddSingleton<AssetProvider>();
            serviceCollection.AddSingleton<SDLKeyboardStates>();
            serviceCollection.AddSingleton<SDLEventProcessor>();
            serviceCollection.AddSingleton<ActionBinder>();

            serviceCollection.AddSingleton((sc) => sc.GetService<SDLApplication>().CreateWindow("PhobiaX", 0, 0, 1024, 768, SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL));
            serviceCollection.AddSingleton((sc) => sc.GetService<SDLWindow>().CreateRenderer(-1, SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC | SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED));

            return serviceCollection.BuildServiceProvider();
        }
    }
}
