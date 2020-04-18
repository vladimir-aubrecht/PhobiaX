using PhobiaX.Actions;
using PhobiaX.Game.UserInterface;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;
using PhobiaX.SDL2.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameLoops
{
	public class GameLoopFactory
	{
		private readonly ISDL2 sdl2;
		private readonly GameUI gameUI;
		private readonly GameObjectFactory gameObjectFactory;
		private readonly TimeThrottler timeThrottler;
		private readonly WindowOptions windowOptions;

		public GameLoopFactory(ISDL2 sdl2, GameUI gameUI, GameObjectFactory gameObjectFactory, TimeThrottler timeThrottler, WindowOptions windowOptions)
		{
			this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
			this.gameUI = gameUI ?? throw new ArgumentNullException(nameof(gameUI));
			this.gameObjectFactory = gameObjectFactory ?? throw new ArgumentNullException(nameof(gameObjectFactory));
			this.timeThrottler = timeThrottler ?? throw new ArgumentNullException(nameof(timeThrottler));
			this.windowOptions = windowOptions ?? throw new ArgumentNullException(nameof(windowOptions));
		}

		public GameLoop CreateGameLoop()
		{
			var keyboardStates = new SDLKeyboardStates(sdl2);
			var actionBinder = new ActionBinder(keyboardStates);

			var player1 = gameObjectFactory.CreatePlayer(windowOptions.Width / 3, windowOptions.Height / 2);
			var player2 = gameObjectFactory.CreatePlayer(2 * windowOptions.Width / 3, windowOptions.Height / 2);

			return new GameLoop(timeThrottler, gameObjectFactory, player1, player2, actionBinder, keyboardStates, gameUI);
		}
	}
}
