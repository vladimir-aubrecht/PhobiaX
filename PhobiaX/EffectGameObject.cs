using System;
using PhobiaX.Assets;
using PhobiaX.SDL2;

namespace PhobiaX
{
    public class EffectGameObject : GameObject
    {
        private readonly GameObject gameObject;

        public EffectGameObject(AnimatedSet animatedSet, GameObject gameObject) : base(animatedSet, true)
        {
            this.X = gameObject.X;
            this.Y = gameObject.Y;
            this.Angle = gameObject.Angle;
            this.gameObject = gameObject;
        }

        public override bool IsColliding(int x, int y, SDLSurface surface)
        {
            var animatedAsset = gameObject.AnimatedSet.GetCurrentAnimatedAsset();

            if (surface == animatedAsset.GetCurrentFrame() && x == gameObject.X && y == gameObject.Y)
            {
                return false;
            }

            return base.IsColliding(x, y, surface);
        }
    }
}
