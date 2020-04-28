using PhobiaX.Actions;
using PhobiaX.Ai;
using PhobiaX.Game.GameObjects;
using PhobiaX.Game.UserInterface;
using PhobiaX.Physics;
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
		private readonly GameObjectFactory gameObjectFactory;
		private readonly TimeThrottler timeThrottler;
		private readonly CollissionObserver collissionObserver;
		private readonly EnemyAiObserver enemyAiObserver;
		private readonly WindowOptions windowOptions;

		public GameLoopFactory(ISDL2 sdl2, GameObjectFactory gameObjectFactory, TimeThrottler timeThrottler, CollissionObserver collissionObserver, EnemyAiObserver enemyAiObserver, WindowOptions windowOptions)
		{
			this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
			this.gameObjectFactory = gameObjectFactory ?? throw new ArgumentNullException(nameof(gameObjectFactory));
			this.timeThrottler = timeThrottler ?? throw new ArgumentNullException(nameof(timeThrottler));
			this.collissionObserver = collissionObserver ?? throw new ArgumentNullException(nameof(collissionObserver));
			this.enemyAiObserver = enemyAiObserver ?? throw new ArgumentNullException(nameof(enemyAiObserver));
			this.windowOptions = windowOptions ?? throw new ArgumentNullException(nameof(windowOptions));
		}

		public GameLoop CreateGameLoop()
		{
			var keyboardStates = new SDLKeyboardStates(sdl2);
			var actionBinder = new ActionBinder(keyboardStates);

			var player1 = gameObjectFactory.CreatePlayer(windowOptions.Width / 3, windowOptions.Height / 2);
			var player2 = gameObjectFactory.CreatePlayer(2 * windowOptions.Width / 3, windowOptions.Height / 2);

			return new GameLoop(timeThrottler, gameObjectFactory, new List<PlayerGameObject> { player1, player2 }, actionBinder, keyboardStates, collissionObserver, enemyAiObserver);
		}
	}
}
