﻿using System;
using PhobiaX.Assets;
using PhobiaX.SDL2;

namespace PhobiaX.Game.GameObjects
{
    public class EffectGameObject : AnimatedGameObject
    {
        public IGameObject Owner { get; }

        public EffectGameObject(AnimatedCollection animatedSet, IGameObject owner) : base(animatedSet, true)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));

            this.X = owner.X;
            this.Y = owner.Y;
            this.Angle = owner.Angle;
        }

        public override bool IsColliding(int x, int y, SDLSurface surface)
        {
            if (surface == Owner.CurrentSurface && x == Owner.X && y == Owner.Y)
            {
                return false;
            }

            return base.IsColliding(x, y, surface);
        }
    }
}