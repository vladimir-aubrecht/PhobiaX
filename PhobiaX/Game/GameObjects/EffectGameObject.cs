using System;
using PhobiaX.Assets;
using PhobiaX.SDL2;

namespace PhobiaX.Game.GameObjects
{
    public class EffectGameObject : AnimatedGameObject
    {
        private readonly AnimatedGameObject gameObject;

        public EffectGameObject(AnimatedCollection animatedSet, AnimatedGameObject gameObject) : base(animatedSet, true, 0)
        {
            this.gameObject = gameObject ?? throw new ArgumentNullException(nameof(gameObject));

            this.X = gameObject.X;
            this.Y = gameObject.Y;
            this.Angle = gameObject.Angle;
        }

        public override bool IsColliding(int x, int y, SDLSurface surface)
        {
            if (surface == gameObject.CurrentSurface && x == gameObject.X && y == gameObject.Y)
            {
                return false;
            }

            return base.IsColliding(x, y, surface);
        }
    }
}
