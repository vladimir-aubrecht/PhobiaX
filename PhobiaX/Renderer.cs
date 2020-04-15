using PhobiaX.GameObjects;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX
{
	public class Renderer : IDisposable
	{
		private readonly SDLRenderer renderer;
		private readonly SDLTextureFactory textureFactory;
		private SDLSurface screenSurface;
		private IDictionary<string, IList<IGameObject>> gameObjects;

		public Renderer(SDLRenderer renderer, SDLTextureFactory textureFactory, SDLSurfaceFactory surfaceFactory, WindowOptions windowOptions)
		{
			this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
			this.textureFactory = textureFactory ?? throw new ArgumentNullException(nameof(textureFactory));
			screenSurface = surfaceFactory.CreateSurface(windowOptions.Width, windowOptions.Height);

			gameObjects = new Dictionary<string, IList<IGameObject>>();
		}

		public void SetForRendering(string name, IList<IGameObject> gameObjects)
		{
			if (!this.gameObjects.TryAdd(name, gameObjects))
			{
				this.gameObjects[name] = gameObjects;
			}
		}

		public void Dispose()
		{
			this.screenSurface.Dispose();
		}

		public void Render()
		{
			foreach (var gameObjects in this.gameObjects.Values)
			{
				foreach (var gameObject in gameObjects)
				{
					gameObject.Draw(screenSurface);
				}
			}

			using (var texture = textureFactory.CreateTexture(screenSurface))
			{
				texture.CopyToRenderer();
				renderer.Present();
			}
		}
	}
}
