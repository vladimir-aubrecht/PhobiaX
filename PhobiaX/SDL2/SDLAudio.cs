using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.SDL2
{
	public class SDLAudio
	{
		private readonly SDLApplication application;

		public SDLAudio(SDLApplication application)
		{
			this.application = application ?? throw new ArgumentNullException(nameof(application));
			//sdl2.Init(SDL.SDL_INIT_AUDIO);
		}
	}
}
