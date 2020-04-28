using System;
using System.Collections.Generic;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Wrappers;
using SDL2;

namespace PhobiaX.SDL2
{
    public class SDLEventProcessor
    {
        private readonly ISDL2 sdl2;
        private Dictionary<SDL.SDL_EventType, Action<SDL.SDL_Event>> eventsToActionsMap = new Dictionary<SDL.SDL_EventType, Action<SDL.SDL_Event>>();

        public SDLEventProcessor(ISDL2 sdl2)
        {
            this.sdl2 = sdl2 ?? throw new ArgumentNullException(nameof(sdl2));
        }

        public void RegisterEvents(SDL.SDL_EventType eventType, Action<SDL.SDL_Event> eventMethod)
        {
            eventsToActionsMap.Add(eventType, eventMethod);
        }

        public void Evaluate()
        {
            SDL.SDL_Event e;
            while (sdl2.PollEvent(out e) != 0)
            {
                if (eventsToActionsMap.TryGetValue(e.type, out Action<SDL.SDL_Event> eventMethod))
                {
                    eventMethod(e);
                }
            }
        }
    }
}