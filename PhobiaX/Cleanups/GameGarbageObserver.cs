using PhobiaX.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Cleanups
{
	class GameGarbageObserver : ICleanable
	{
		private readonly TimeThrottler timeThrottler;
		private IList<ICleanable> cleanableObjects = new List<ICleanable>();
		protected IList<IGameObject> gameObjects = new List<IGameObject>();

		public void Observe(IGameObject gameObject)
		{
			this.gameObjects.Add(gameObject);
		}

		public virtual void Cleanup(IGameObject gameObject)
		{
			this.gameObjects.Remove(gameObject);
		}

		public GameGarbageObserver(TimeThrottler timeThrottler)
		{
			this.timeThrottler = timeThrottler ?? throw new ArgumentNullException(nameof(timeThrottler));
		}

		public void CleanupAll()
		{
			foreach (var cleanableObject in cleanableObjects)
			{
				cleanableObject.CleanupAll();
			}

			this.cleanableObjects.Clear();
			gameObjects.Clear();
		}

		public void AddCleanableObject(ICleanable cleanableObject)
		{
			this.cleanableObjects.Add(cleanableObject);
		}

		public void Evaluate()
		{
			timeThrottler.Execute(TimeSpan.FromSeconds(1), () =>
			{
				foreach (var obj in gameObjects)
				{
					foreach (var cleanableObject in cleanableObjects)
					{
						cleanableObject.Cleanup(obj);
					}
				}
			});
		}
	}
}
