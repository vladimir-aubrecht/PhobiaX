using PhobiaX.Assets;
using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public class PlayerGameObject : AnimatedGameObject
	{
		private readonly AnimatedCollection effectsAnimatedSet;

		private IList<EffectGameObject> rocketsToDrop = new List<EffectGameObject>();
		private IList<EffectGameObject> rockets = new List<EffectGameObject>();
		private DateTimeOffset lastShoot = DateTimeOffset.MinValue;
		public int Score { get; private set; } = 0;
		public int Life { get; private set; } = 100;

		public PlayerGameObject(AnimatedCollection playerAnimatedSet, AnimatedCollection effectsAnimatedSet) : base(playerAnimatedSet, false, 0)
		{
			this.effectsAnimatedSet = effectsAnimatedSet ?? throw new ArgumentNullException(nameof(effectsAnimatedSet));
		}

		public void Shoot()
		{
			if (!CanBeHit)
			{
				return;
			}
			
			var rocket = new EffectGameObject(new AnimatedCollection(effectsAnimatedSet), this);
			rocket.Speed = 12;

			if ((DateTimeOffset.UtcNow - lastShoot).TotalMilliseconds > 300)
			{
				rockets.Add(rocket);
				lastShoot = DateTimeOffset.UtcNow;
			}
		}

		public void EvaluateRockets(IGameObject map, IList<IGameObject> gameObjects)
		{
			foreach (var rocket in rockets)
			{
				if (!rocket.IsColliding(map))
				{
					rocketsToDrop.Add(rocket);
				}

				foreach (var gameObject in gameObjects)
				{
					if (gameObject.CanBeHit && rocket.CanBeHit && rocket.IsColliding(gameObject))
					{
						if (!(gameObject is PlayerGameObject))
						{
							Score++;
						}
						
						rocket.Hit();
						gameObject.Hit();
						rocketsToDrop.Add(rocket);
					}
				}
			}

			foreach (var rocket in rocketsToDrop)
			{
				if (rocket.IsFinalAnimationFinished)
				{
					rockets.Remove(rocket);
				}
			}

			rocketsToDrop.Clear();
		}

		public override void Draw(SDLSurface destination)
		{
			base.Draw(destination);

			foreach (var rocket in rockets)
			{
				rocket.MoveForward();
				rocket.Draw(destination);
			}
		}

		public override void Hit()
		{
			if (Life > 0)
			{
				Life--;
			}
			
			if (Life == 0)
			{
				base.Hit();
			}
		}
	}
}
