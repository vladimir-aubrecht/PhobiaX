using PhobiaX.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Physics
{
	public class CollissionObserver
	{
		private IDictionary<string, IList<IGameObject>> categorizedGameObjects = new Dictionary<string, IList<IGameObject>>();

		public CollissionObserver()
		{

		}

		public void SetForObserving(string name, IList<IGameObject> gameObjects)
		{
			if (!this.categorizedGameObjects.TryAdd(name, gameObjects))
			{
				this.categorizedGameObjects[name] = gameObjects;
			}
		}

		public bool IsObjectColliding(IGameObject testedObject)
		{
			var isColliding = false;

			foreach (var gameObjects in categorizedGameObjects)
			{
				foreach (var gameObject in gameObjects.Value)
				{
					// TODO: Removal of CanBeHit and replacing it with unregistration?
					if (testedObject == gameObject || !gameObject.CanBeHit)
					{
						continue;
					}

					isColliding |= testedObject.IsColliding(gameObject);
				}
			}

			return isColliding;
		}

	}
}
