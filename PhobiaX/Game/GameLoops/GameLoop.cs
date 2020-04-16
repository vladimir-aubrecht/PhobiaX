using PhobiaX.Actions;
using PhobiaX.Game.GameObjects;
using PhobiaX.Game.UserInterface;
using PhobiaX.SDL2;
using SDL2;
using System;

namespace PhobiaX.Game.GameLoops
{
	public class GameLoop
	{
		private readonly PlayerGameObject player1GameObject;
		private readonly PlayerGameObject player2GameObject;
        private readonly GameUI gameUI;

        public ActionBinder ActionBinder { get; }
		private SDLKeyboardStates KeyboardStates { get; }

        public GameLoop(PlayerGameObject player1GameObject, PlayerGameObject player2GameObject, ActionBinder actionBinder, SDLKeyboardStates keyboardStates, GameUI gameUI)
		{
			this.player1GameObject = player1GameObject ?? throw new ArgumentNullException(nameof(player1GameObject));
			this.player2GameObject = player2GameObject ?? throw new ArgumentNullException(nameof(player2GameObject));
			this.ActionBinder = actionBinder ?? throw new ArgumentNullException(nameof(actionBinder));
			this.KeyboardStates = keyboardStates ?? throw new ArgumentNullException(nameof(keyboardStates));
            this.gameUI = gameUI ?? throw new ArgumentNullException(nameof(gameUI));

            InitKeyboardController();

        }

		public PlayerGameObject GetPlayer1GameObject()
		{
			return this.player1GameObject;
		}

		public PlayerGameObject GetPlayer2GameObject()
		{
			return this.player2GameObject;
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

            this.ActionBinder.RegisterPressAction(GameAction.Player1MoveForward, () => player1GameObject.MoveForward());
            this.ActionBinder.RegisterPressAction(GameAction.Player1MoveBackward, () => player1GameObject.MoveBackward());
            this.ActionBinder.RegisterPressAction(GameAction.Player1RotateLeft, () => player1GameObject.TurnLeft());
            this.ActionBinder.RegisterPressAction(GameAction.Player1RotateRight, () => player1GameObject.TurnRight());
            this.ActionBinder.RegisterPressAction(GameAction.Player1StopMoving, () => player1GameObject.Stop());
            this.ActionBinder.RegisterPressAction(GameAction.Player1Fire, () => player1GameObject.Shoot());

            this.ActionBinder.RegisterPressAction(GameAction.Player2MoveForward, () => player2GameObject.MoveForward());
            this.ActionBinder.RegisterPressAction(GameAction.Player2MoveBackward, () => player2GameObject.MoveBackward());
            this.ActionBinder.RegisterPressAction(GameAction.Player2RotateLeft, () => player2GameObject.TurnLeft());
            this.ActionBinder.RegisterPressAction(GameAction.Player2RotateRight, () => player2GameObject.TurnRight());
            this.ActionBinder.RegisterPressAction(GameAction.Player2StopMoving, () => player2GameObject.Stop());
            this.ActionBinder.RegisterPressAction(GameAction.Player2Fire, () => player2GameObject.Shoot());
        }

        public int GetDifficulty()
        {
            var totalScore = (player1GameObject.Score + player2GameObject.Score);
            return 8 + totalScore / 10;
        }

        public void Evaluate()
        {
            this.KeyboardStates.ScanKeys();

            gameUI.SetPlayer1Score(player1GameObject.Score);
            gameUI.SetPlayer2Score(player2GameObject.Score);
            gameUI.SetPlayer1Life(player1GameObject.Life);
            gameUI.SetPlayer2Life(player2GameObject.Life);
        }

    }
}
