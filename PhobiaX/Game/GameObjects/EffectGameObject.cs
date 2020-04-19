using System;
using PhobiaX.Assets;
using PhobiaX.Graphics;
using PhobiaX.Physics;
using PhobiaX.SDL2;

namespace PhobiaX.Game.GameObjects
{
    public class EffectGameObject : AnimatedGameObject
    {
        public IGameObject Owner { get; }

        public EffectGameObject(IRenderableObject renderableObject, ICollidableObject collidableObject, AnimatedCollection animatedSet, IGameObject owner) : base(renderableObject, collidableObject, animatedSet, true)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));

            this.X = owner.X;
            this.Y = owner.Y;
            this.Angle = owner.Angle;
        }
    }
}
