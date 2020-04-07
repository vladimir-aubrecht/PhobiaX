using System;
using System.Collections.Generic;
using System.Linq;
using PhobiaX.SDL2;
using SDL2;

namespace PhobiaX.Actions
{
    public class ActionBinder
    {
        private readonly SDLApplication application;
        private readonly SDLKeyboardStates keyboardStates;

        private Dictionary<GameAction, Action> gameActions = new Dictionary<GameAction, Action>();
        private Dictionary<SDL.SDL_Scancode, bool> pressedKeys = new Dictionary<SDL.SDL_Scancode, bool>();

        public ActionBinder(SDLApplication application, SDLKeyboardStates keyboardStates)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
            this.keyboardStates = keyboardStates ?? throw new ArgumentNullException(nameof(keyboardStates));
        }

        public void AssignKeysToGameAction(GameAction action, bool isKeyReleased, params SDL.SDL_Scancode[] scancodes)
        {
            if (scancodes.Length == 1)
            {
                keyboardStates.RegisterEvents(scancodes[0], isKeyReleased, () => Evaluate(action));
            }
            else if (scancodes.Length > 1)
            {
                foreach (var scancode in scancodes)
                {
                    pressedKeys.Add(scancode, true);

                    keyboardStates.RegisterEvents(scancode, !isKeyReleased, () =>
                    {
                        pressedKeys[scancode] = !isKeyReleased;
                    });

                    keyboardStates.RegisterEvents(scancode, isKeyReleased, () =>
                    {
                        pressedKeys[scancode] = isKeyReleased;

                        bool canExecute = true;
                        foreach (var lscancode in scancodes)
                        {
                            canExecute &= pressedKeys[lscancode] == isKeyReleased;
                        }

                        if (canExecute)
                        {
                            Console.WriteLine(action);
                            Evaluate(action);
                        }
                    });
                }
            }
        }

        public void RegisterPressAction(GameAction gameAction, Action actionMethod)
        {
            gameActions.Add(gameAction, actionMethod);
        }

        private void Evaluate(GameAction action)
        {
            if (gameActions.ContainsKey(action))
            {
                gameActions[action]();
            }
        }
    }
}
