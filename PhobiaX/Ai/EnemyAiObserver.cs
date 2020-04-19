using PhobiaX.Cleanups;
using PhobiaX.Game.GameObjects;
using PhobiaX.Physics;
using PhobiaX.SDL2.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhobiaX.Ai
{
	public class EnemyAiObserver : ICleanable
	{
		private readonly Random random = new Random();
		private readonly CollissionObserver collissionObserver;
		private readonly PathFinder pathFinder;
		private readonly GameObjectFactory gameObjectFactory;
		private readonly WindowOptions windowOptions;
		private IList<EnemyGameObject> enemies = new List<EnemyGameObject>();
		private IList<IGameObject> targets = new List<IGameObject>();
		private int desiredAmountOfEnemies = 4;

		public EnemyAiObserver(CollissionObserver collissionObserver, PathFinder pathFinder, GameObjectFactory gameObjectFactory, WindowOptions windowOptions)
		{
			this.collissionObserver = collissionObserver ?? throw new ArgumentNullException(nameof(collissionObserver));
			this.pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
			this.gameObjectFactory = gameObjectFactory ?? throw new ArgumentNullException(nameof(gameObjectFactory));
			this.windowOptions = windowOptions ?? throw new ArgumentNullException(nameof(windowOptions));
		}

		public void SetAmountOfEnemies(int amountOfEnemies)
		{
			this.desiredAmountOfEnemies = amountOfEnemies;
		}

		public void AddTarget(IGameObject target)
		{
			targets.Add(target);
		}

		public void Observe(EnemyGameObject enemy)
		{
			this.enemies.Add(enemy);
		}

		public void Evaluate()
		{
			RegenerateEnemies(desiredAmountOfEnemies);
			MoveToClosestTarget(70, 20);
		}

		private void MoveToClosestTarget(int probabilityOfNoMove, int percentageOfEnemiesWhichMustMove)
		{
			int movesInTurn = 0;

			foreach (var enemy in enemies)
			{
				var noMoveRandom = random.Next(100);
				if (noMoveRandom < probabilityOfNoMove && movesInTurn > enemies.Count * (double)percentageOfEnemiesWhichMustMove / 100)
				{
					continue;
				}

				if (enemy.ColladableObject != null && TryMoveEnemyToClosestTarget(enemy))
				{
					movesInTurn++;
				}
			}
		}

		private bool TryMoveEnemyToClosestTarget(EnemyGameObject enemy)
		{
			var validTargets = targets.Where(i => i.ColladableObject != null).Cast<IGameObject>().ToList();

			if (!validTargets.Any())
			{
				return false;
			}

			var closesestTarget = pathFinder.FindClosestTarget(enemy, validTargets);

			if (closesestTarget == null)
			{
				return false;
			}

			if (enemy.ColladableObject?.IsColliding(closesestTarget.ColladableObject) ?? false)
			{
				return false;
			}

			enemy.RenderablePeriodicAnimation.Angle = 
				MathFormulas.GetAngleTowardsTarget(enemy.X, enemy.Y, closesestTarget.ColladableObject.MiddleX, closesestTarget.ColladableObject.MiddleY);

			enemy.MoveForward();

			return true;
		}

		private void RegenerateEnemies(int enemiesCount)
		{
			if (enemiesCount > this.enemies.Count)
			{
				foreach (var enemy in gameObjectFactory.CreateEnemies(enemiesCount - this.enemies.Count + 1))
				{
					do
					{
						if (enemy.ColladableObject == null)
						{
							continue;
						}

						(enemy.X, enemy.Y) = MathFormulas.GetRandomLocationAroundRectangle(
							enemy.ColladableObject.Width,
							enemy.ColladableObject.Height,
							this.windowOptions.Width,
							this.windowOptions.Height);

					} while (collissionObserver.FindCollidingObjects(enemy).Where(i => i.ColladableObject != null).Count() > 0);
				}
			}
		}

		public void Cleanup(IGameObject gameObject)
		{
			if (gameObject is EnemyGameObject)
			{
				this.enemies.Remove(gameObject as EnemyGameObject);
			}

			this.targets.Remove(gameObject);
		}

		public void CleanupAll()
		{
			this.enemies.Clear();
			this.targets.Clear();
		}
	}
}
