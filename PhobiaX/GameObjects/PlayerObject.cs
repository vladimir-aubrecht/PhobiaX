using PhobiaX.Assets;
using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.GameObjects
{
	public class PlayerObject : AnimatedGameObject
	{
		private readonly AnimatedSet effectsAnimatedSet;

		private IList<EffectGameObject> rocketsToDrop = new List<EffectGameObject>();
		private IList<EffectGameObject> rockets = new List<EffectGameObject>();
		private DateTimeOffset lastShoot = DateTimeOffset.MinValue;

		public PlayerObject(AnimatedSet playerAnimatedSet, AnimatedSet effectsAnimatedSet) : base(playerAnimatedSet, false)
		{
			this.effectsAnimatedSet = effectsAnimatedSet ?? throw new ArgumentNullException(nameof(effectsAnimatedSet));
		}

		public void Shoot()
		{
			var rocket = new EffectGameObject(new AnimatedSet(effectsAnimatedSet), this);

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
					if (gameObject.CanBeHit && rocket.IsColliding(gameObject))
					{
						gameObject.Hit();
						rocketsToDrop.Add(rocket);
					}
				}
			}

			foreach (var rocket in rocketsToDrop)
			{
				rockets.Remove(rocket);
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
	}
}
