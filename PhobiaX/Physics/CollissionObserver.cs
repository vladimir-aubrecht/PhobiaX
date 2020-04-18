using PhobiaX.Cleanups;
using PhobiaX.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Physics
{
	public class CollissionObserver : ICleanable
	{
		protected IList<IGameObject> gameObjects = new List<IGameObject>();
		protected IList<Action<IGameObject, IList<IGameObject>>> callbacks = new List<Action<IGameObject, IList<IGameObject>>>();

		public void OnCollissionCallback(Action<IGameObject, IList<IGameObject>> callback)
		{
			callbacks.Add(callback);
		}

		public void ObserveCollission(IGameObject gameObject)
		{
			this.gameObjects.Add(gameObject);
		}

		public virtual void Cleanup(IGameObject gameObject)
		{
			this.gameObjects.Remove(gameObject);
		}

		public virtual void CleanupAll()
		{
			gameObjects.Clear();
			callbacks.Clear();
		}

		public void Evaluate()
		{
			var collides = new Dictionary<IGameObject, IList<IGameObject>>();
			foreach (var gameObject in gameObjects)
			{
				var collidingObjects = FindCollidingObjects(gameObject);
				collides.Add(gameObject, collidingObjects);
			}

			foreach (var collide in collides)
			{
				TriggerCallback(collide.Key, collide.Value);
			}
		}

		public IList<IGameObject> FindCollidingObjects(IGameObject testedObject)
		{
			var output = new List<IGameObject>();
			
			foreach (var gameObject in gameObjects)
			{
				if (testedObject == gameObject)
				{
					continue;
				}

				if (testedObject.IsColliding(gameObject))
				{
					output.Add(gameObject);
				}
			}

			return output;
		}

		protected void TriggerCallback(IGameObject testedObject, IList<IGameObject> colliders)
		{
			foreach (var callback in callbacks)
			{
				callback(testedObject, colliders);
			}
		}

	}
}
