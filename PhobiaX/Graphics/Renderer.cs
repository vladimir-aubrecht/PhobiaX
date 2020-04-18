using PhobiaX.Cleanups;
using PhobiaX.Game.GameObjects;
using PhobiaX.SDL2;
using PhobiaX.SDL2.Options;
using System;
using System.Collections.Generic;

namespace PhobiaX.Graphics
{
	public class Renderer : IDisposable, ICleanable
	{
		private readonly SDLRenderer renderer;
		private readonly SDLTextureFactory textureFactory;
		private SDLSurface screenSurface;
		private IList<IGameObject> gameObjects;

		public Renderer(SDLRenderer renderer, SDLTextureFactory textureFactory, SDLSurfaceFactory surfaceFactory, WindowOptions windowOptions)
		{
			this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
			this.textureFactory = textureFactory ?? throw new ArgumentNullException(nameof(textureFactory));
			screenSurface = surfaceFactory.CreateSurface(windowOptions.Width, windowOptions.Height);

			gameObjects = new List<IGameObject>();
		}

		public void Add(IGameObject gameObject)
		{
			this.gameObjects.Add(gameObject);
		}

		public void Dispose()
		{
			this.CleanupAll();
			this.screenSurface.Dispose();
		}

		public void Evaluate()
		{
			foreach (var gameObject in gameObjects)
			{
				gameObject.Draw(screenSurface);
			}

			using (var texture = textureFactory.CreateTexture(screenSurface))
			{
				texture.CopyToRenderer();
				renderer.Present();
			}
		}

		public void Cleanup(IGameObject gameObject)
		{
			this.gameObjects.Remove(gameObject);
		}

		public void CleanupAll()
		{
			this.gameObjects.Clear();
		}
	}
}
