using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PhobiaX.SDL2.Wrappers;
using SDL2;
using static SDL2.SDL;

namespace PhobiaX.SDL2
{
    public class SDLKeyboardStates
    {
        private int numkeys = 0;
        private IntPtr keysBuffer;
        private byte[] keysCurr = new byte[(int)SDL_Scancode.SDL_NUM_SCANCODES];
        private byte[] keysPrev = new byte[(int)SDL_Scancode.SDL_NUM_SCANCODES];

        private Dictionary<SDL_Scancode, Action> eventsToActionsMap = new Dictionary<SDL_Scancode, Action>();
        private Dictionary<SDL_Scancode, Action> eventsToActionsWhenNotPressedMap = new Dictionary<SDL_Scancode, Action>();

        public SDLKeyboardStates(ISDL2 sdl2)
        {
            _ = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
            keysBuffer = sdl2.GetKeyboardState(out numkeys);
        }

        public void Clear()
        {
            eventsToActionsMap.Clear();
            eventsToActionsWhenNotPressedMap.Clear();
        }

        public void RegisterEvents(SDL_Scancode scanCode, bool isReleased, Action eventMethod)
        {
            if (!isReleased)
            {
                if (eventsToActionsMap.TryGetValue(scanCode, out var actionMethod))
                {
                    eventsToActionsMap[scanCode] = () => { actionMethod(); eventMethod(); };
                }
                else
                {
                    eventsToActionsMap.Add(scanCode, eventMethod);
                }
            }
            else
            {
                if (eventsToActionsWhenNotPressedMap.TryGetValue(scanCode, out var actionMethod))
                {
                    eventsToActionsWhenNotPressedMap[scanCode] = () => { actionMethod(); eventMethod(); };
                }
                else
                {
                    eventsToActionsWhenNotPressedMap.Add(scanCode, eventMethod);
                }
            }
        }

        public void ScanKeys()
        {
            var tmp = keysPrev;
            keysPrev = keysCurr;
            keysCurr = tmp;

            Marshal.Copy(keysBuffer, keysCurr, 0, numkeys);

            for (int i = 0; i < keysCurr.Length; i++)
            {
                var map = eventsToActionsWhenNotPressedMap;

                if (keysCurr[i] == 1)
                {
                    map = eventsToActionsMap;
                }

                var keyCode = (SDL_Scancode)i;

                if (map.TryGetValue(keyCode, out Action eventMethod))
                {
                    eventMethod();
                }
            }
        }
    }
}
