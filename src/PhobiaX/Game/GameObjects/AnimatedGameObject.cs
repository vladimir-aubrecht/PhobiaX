using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Transactions;
using PhobiaX.Assets;
using PhobiaX.Graphics;
using PhobiaX.Physics;
using PhobiaX.SDL2;
using SDL2;

namespace PhobiaX.Game.GameObjects
{
    public class AnimatedGameObject : IGameObject
    {
        public Guid Id { get; } = Guid.NewGuid();

        public int X 
        {
            get
            {
                return this.ColladableObject.X;
            }
            set
            {
                this.RenderableObject.X = value;
                this.ColladableObject.X = value;
            }
        }

        public int Y 
        { 
            get 
            { 
                return this.ColladableObject.Y; 
            } 
            set 
            {
                this.RenderableObject.Y = value;
                this.ColladableObject.Y = value; 
            } 
        }

        public double Speed { get; set; } = 4;
        public IRenderableObject RenderableObject { get; set; }
        public RenderablePeriodicAnimation RenderablePeriodicAnimation { get { return RenderableObject as RenderablePeriodicAnimation; } }

        public ICollidableObject ColladableObject { get; set; }

        public AnimatedGameObject(RenderablePeriodicAnimation renderablePeriodicAnimation, ICollidableObject colladableObject)
        {
            RenderableObject = renderablePeriodicAnimation ?? throw new ArgumentNullException(nameof(renderablePeriodicAnimation));
            ColladableObject = colladableObject;
        }

        public void Stop()
        {
            if (this.ColladableObject == null)
            {
                return;
            }

            this.RenderablePeriodicAnimation.ChangeSet(AnimationSetType.Default);
        }

        public void TurnLeft()
        {
            if (this.ColladableObject == null)
            {
                return;
            }

            RenderablePeriodicAnimation.NextAngle();
        }

        public void TurnRight()
        {
            if (this.ColladableObject == null)
            {
                return;
            }

            RenderablePeriodicAnimation.PreviousAngle();
        }

        public void MoveForward()
        {
            if (this.ColladableObject == null)
            {
                return;
            }

            var (x, y) = MathFormulas.GetIncrementByAngle(Speed, this.RenderablePeriodicAnimation.Angle);

            X += (int)(x);
            Y -= (int)(y);

            this.RenderablePeriodicAnimation.NextFrame();
        }

        public void MoveBackward()
        {
            if (this.ColladableObject == null)
            {
                return;
            }

            var (x, y) = MathFormulas.GetIncrementByAngle(Speed, this.RenderablePeriodicAnimation.Angle);

            X -= (int)x;
            Y += (int)y;

            this.RenderablePeriodicAnimation.PreviousFrame();
        }

        public virtual void Hit()
        {
            this.RenderablePeriodicAnimation.ChangeSet(AnimationSetType.Final);
            this.ColladableObject = null;
        }

        public override string ToString()
        {
            return $"Type: {this.GetType().Name} {nameof(Id)}: {Id} {nameof(ColladableObject)}: {ColladableObject}";
        }
    }
}
