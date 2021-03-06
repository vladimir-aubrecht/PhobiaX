﻿using PhobiaX.Cleanups;
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
		private List<IGameObject> gameObjects;

		public Action<IGameObject> DestroyCallback { get; set; }

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
			gameObjects.Sort((a, b) => a.RenderableObject.RenderingPriority < b.RenderableObject.RenderingPriority ? -1 : 1);

			foreach (var gameObject in gameObjects)
			{
				if (gameObject.RenderableObject?.ShouldDestroy ?? false)
				{
					this.DestroyCallback?.Invoke(gameObject);
				}

				gameObject.RenderableObject?.Draw(screenSurface);
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
