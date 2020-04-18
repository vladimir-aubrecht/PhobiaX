using PhobiaX.Actions;
using PhobiaX.Ai;
using PhobiaX.Game.GameObjects;
using PhobiaX.Game.UserInterface;
using PhobiaX.Physics;
using PhobiaX.SDL2;
using SDL2;
using System;
using System.Collections.Generic;

namespace PhobiaX.Game.GameLoops
{
	public class GameLoop
	{
        private readonly TimeThrottler timeThrottler;
        private readonly GameObjectFactory gameObjectFactory;
        private readonly IList<PlayerGameObject> playerGameObjects;
        private readonly CollissionObserver collissionObserver;
        private readonly EnemyAiObserver enemyAiObserver;

        public ActionBinder ActionBinder { get; }
		private SDLKeyboardStates KeyboardStates { get; }

        public GameLoop(TimeThrottler timeThrottler, GameObjectFactory gameObjectFactory, IList<PlayerGameObject> playerGameObjects, ActionBinder actionBinder, SDLKeyboardStates keyboardStates, CollissionObserver collissionObserver, EnemyAiObserver enemyAiObserver)
		{
            this.timeThrottler = timeThrottler ?? throw new ArgumentNullException(nameof(timeThrottler));
            this.gameObjectFactory = gameObjectFactory ?? throw new ArgumentNullException(nameof(gameObjectFactory));
            this.playerGameObjects = playerGameObjects ?? throw new ArgumentNullException(nameof(playerGameObjects));
			this.ActionBinder = actionBinder ?? throw new ArgumentNullException(nameof(actionBinder));
			this.KeyboardStates = keyboardStates ?? throw new ArgumentNullException(nameof(keyboardStates));
            this.collissionObserver = collissionObserver ?? throw new ArgumentNullException(nameof(collissionObserver));
            this.enemyAiObserver = enemyAiObserver ?? throw new ArgumentNullException(nameof(enemyAiObserver));
            InitKeyboardController();
        }

        private void InitKeyboardController()
        {
            this.ActionBinder.AssignKeysToGameAction(GameAction.Player1RotateLeft, false, SDL.SDL_Scancode.SDL_SCANCODE_LEFT);
            this.ActionBinder.AssignKeysToGameAction(GameAction.Player1RotateRight, false, SDL.SDL_Scancode.SDL_SCANCODE_RIGHT);
            this.ActionBinder.AssignKeysToGameAction(GameAction.Player1MoveForward, false, SDL.SDL_Scancode.SDL_SCANCODE_UP);
            this.ActionBinder.AssignKeysToGameAction(GameAction.Player1MoveBackward, false, SDL.SDL_Scancode.SDL_SCANCODE_DOWN);
            this.ActionBinder.AssignKeysToGameAction(GameAction.Player1Fire, false, SDL.SDL_Scancode.SDL_SCANCODE_RALT);
            this.ActionBinder.AssignKeysToGameAction(GameAction.Player1StopMoving, true, SDL.SDL_Scancode.SDL_SCANCODE_UP, SDL.SDL_Scancode.SDL_SCANCODE_DOWN);

            this.ActionBinder.AssignKeysToGameAction(GameAction.Player2RotateLeft, false, SDL.SDL_Scancode.SDL_SCANCODE_A);
            this.ActionBinder.AssignKeysToGameAction(GameAction.Player2RotateRight, false, SDL.SDL_Scancode.SDL_SCANCODE_D);
            this.ActionBinder.AssignKeysToGameAction(GameAction.Player2MoveForward, false, SDL.SDL_Scancode.SDL_SCANCODE_W);
            this.ActionBinder.AssignKeysToGameAction(GameAction.Player2MoveBackward, false, SDL.SDL_Scancode.SDL_SCANCODE_S);
            this.ActionBinder.AssignKeysToGameAction(GameAction.Player2Fire, false, SDL.SDL_Scancode.SDL_SCANCODE_LALT);
            this.ActionBinder.AssignKeysToGameAction(GameAction.Player2StopMoving, true, SDL.SDL_Scancode.SDL_SCANCODE_W, SDL.SDL_Scancode.SDL_SCANCODE_S);

            this.ActionBinder.RegisterPressAction(GameAction.Player1MoveForward, () => playerGameObjects[0].MoveForward());
            this.ActionBinder.RegisterPressAction(GameAction.Player1MoveBackward, () => playerGameObjects[0].MoveBackward());
            this.ActionBinder.RegisterPressAction(GameAction.Player1RotateLeft, () => playerGameObjects[0].TurnLeft());
            this.ActionBinder.RegisterPressAction(GameAction.Player1RotateRight, () => playerGameObjects[0].TurnRight());
            this.ActionBinder.RegisterPressAction(GameAction.Player1StopMoving, () => playerGameObjects[0].Stop());
            this.ActionBinder.RegisterPressAction(GameAction.Player1Fire, () => timeThrottler.Execute(TimeSpan.FromMilliseconds(400), () => gameObjectFactory.CreateRocket(playerGameObjects[0])));

            this.ActionBinder.RegisterPressAction(GameAction.Player2MoveForward, () => playerGameObjects[1].MoveForward());
            this.ActionBinder.RegisterPressAction(GameAction.Player2MoveBackward, () => playerGameObjects[1].MoveBackward());
            this.ActionBinder.RegisterPressAction(GameAction.Player2RotateLeft, () => playerGameObjects[1].TurnLeft());
            this.ActionBinder.RegisterPressAction(GameAction.Player2RotateRight, () => playerGameObjects[1].TurnRight());
            this.ActionBinder.RegisterPressAction(GameAction.Player2StopMoving, () => playerGameObjects[1].Stop());
            this.ActionBinder.RegisterPressAction(GameAction.Player2Fire, () => timeThrottler.Execute(TimeSpan.FromMilliseconds(400), () => gameObjectFactory.CreateRocket(playerGameObjects[1])));
        }

        private int GetDifficulty()
        {
            var totalScore = 0;
            foreach (var player in playerGameObjects)
            {
                totalScore += player.Score;
            }
            return 8 + totalScore / 10;
        }

        public void Evaluate()
        {
            //enemyAiObserver.SetAmountOfEnemies(this.GetDifficulty());

            this.KeyboardStates.ScanKeys();
            this.collissionObserver.Evaluate();
            this.enemyAiObserver.Evaluate();
        }

    }
}
