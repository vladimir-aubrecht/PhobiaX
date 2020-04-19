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

        private const int defaultAngleOffset = 90;

        private double angle = defaultAngleOffset;
        public int X 
        {
            get
            {
                return this.ColladableObject.X;
            }
            set
            {
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
                this.ColladableObject.Y = value; 
            } 
        }

        private int previousX = 0;
        private int previousY = 0;
        private double previousAngle = 0;
        private int previousFrameIndex = 0;

        public double Speed { get; set; } = 4;
        public IRenderableObject RenderableObject { get; }
        private AnimatedCollection AnimatedSet { get; }

        public SDLSurface CurrentSurface => AnimatedSet.GetCurrentAnimatedAsset().GetCurrentFrame();

        private bool isHit = false;

        private bool alwaysStopped;
        private readonly double minimalAngleStep = 1;
        private bool isStopped = true;
        public bool IsFinalAnimationFinished => isHit && (!AnimatedSet.IsFinalSetAnimation || AnimatedSet.GetCurrentAnimatedAsset().GetCurrentFrameIndex() == AnimatedSet.GetCurrentAnimatedAsset().GetAnimationFrames().Count - 1);


        public Action DestroyCallback { get; set; }
        public Action MoveCallback { get; set; }

        public double Angle
        {
            get { return angle; }
            set
            {
                angle = value;

                var index = AnimationFormulas.GetFrameIndexByAngle(Angle, defaultAngleOffset, AnimatedSet.GetDefaultAnimatedAsset().GetAnimationFrames().Count);
                AnimatedSet.GetDefaultAnimatedAsset().SetFrameIndex(index);
            }
        }

        public bool CanCollide => !isHit;

        public ICollidableObject ColladableObject { get; }

        public AnimatedGameObject(IRenderableObject renderableObject, ICollidableObject colladableObject, AnimatedCollection animatedSurfaceAssets) : this(renderableObject, colladableObject, animatedSurfaceAssets, false)
        {
        }

        public AnimatedGameObject(IRenderableObject renderableObject, ICollidableObject colladableObject, AnimatedCollection animatedSurfaceAssets, bool alwaysStopped)
        {
            RenderableObject = renderableObject ?? throw new ArgumentNullException(nameof(renderableObject));
            ColladableObject = colladableObject;
            this.AnimatedSet = animatedSurfaceAssets;
            this.alwaysStopped = alwaysStopped;
            minimalAngleStep = MathFormulas.CircleDegrees / animatedSurfaceAssets.GetDefaultAnimatedAsset().GetAnimationFrames().Count;
        }

        public void Stop()
        {
            isStopped = true;
            this.AnimatedSet.GetCurrentAnimatedAsset().ResetFrame();
        }

        public void TurnLeft()
        {
            Angle = MathFormulas.Modulo(Angle + minimalAngleStep, MathFormulas.CircleDegrees);
        }

        public void TurnRight()
        {
            Angle = MathFormulas.Modulo(Angle - minimalAngleStep, MathFormulas.CircleDegrees);
        }

        public void MoveForward()
        {
            if (isHit)
            {
                return;
            }

            var (x, y) = MathFormulas.GetIncrementByAngle(Speed, CalculateAngleFromFrameIndex());

            BackupCurrentPosition();

            X -= (int)(x);
            Y += (int)(y);

            if (!alwaysStopped)
            {
                AnimatedSet.GetCurrentAnimatedAsset().NextFrame();
                isStopped = false;
            }
        }

        public void MoveBackward()
        {
            if (isHit)
            {
                return;
            }

            var (x, y) = MathFormulas.GetIncrementByAngle(Speed, CalculateAngleFromFrameIndex());

            BackupCurrentPosition();

            X += (int)x;
            Y -= (int)y;

            if (!alwaysStopped)
            {
                AnimatedSet.GetCurrentAnimatedAsset().PreviousFrame();
                isStopped = false;
            }
        }

        private void BackupCurrentPosition()
        {
            previousX = X;
            previousY = Y;
            previousAngle = Angle;
            previousFrameIndex = AnimatedSet.GetCurrentAnimatedAsset().GetCurrentFrameIndex();
        }

        public void RollbackLastMove()
        {
            this.X = previousX;
            this.Y = previousY;
            this.Angle = previousAngle;
            this.AnimatedSet.GetCurrentAnimatedAsset().SetFrameIndex(previousFrameIndex);
        }

        public bool TryMoveTowards(IGameObject gameObject)
        {
            if (this.IsColliding(gameObject))
            {
                return false;
            }

            Angle = MathFormulas.GetAngleTowardsTarget(X, Y, gameObject.X, gameObject.Y);

            MoveForward();
            return true;
        }

        public bool IsColliding(IGameObject gameObject)
        {
            return this.ColladableObject.IsColliding(gameObject.ColladableObject);
        }

        public virtual void Draw(SDLSurface destination)
        {
            var animatedAsset = AnimatedSet.GetCurrentAnimatedAsset();

            if (isStopped)
            {
                animatedAsset = AnimatedSet.GetDefaultAnimatedAsset();
            }

            if (isHit)
            {
                animatedAsset = AnimatedSet.GetFinalAnimatedAsset();

                if (AnimatedSet.IsFinalSetAnimation)
                {
                    if (animatedAsset.GetCurrentFrameIndex() != animatedAsset.GetAnimationFrames().Count - 1)
                    {
                        animatedAsset.NextFrame();
                    }
                }
            }

            var objectSurface = animatedAsset.GetCurrentFrame();
            var surfaceRectangle = new SDL.SDL_Rect() { x = X, y = Y, w = objectSurface.Surface.w, h = objectSurface.Surface.h };
            objectSurface.BlitSurface(destination, ref surfaceRectangle);
        }

        private double CalculateAngleFromFrameIndex()
        {
            int amountOfAngles = AnimatedSet.GetDefaultAnimatedAsset().GetAnimationFrames().Count;
            var index = AnimationFormulas.GetFrameIndexByAngle(Angle, defaultAngleOffset, amountOfAngles);
            
            return AnimationFormulas.GetAngleByIndex(index, defaultAngleOffset, amountOfAngles);
        }

        public virtual void Hit()
        {
            isHit = true;
        }

        public override string ToString()
        {
            return $"Type: {this.GetType().Name} {nameof(Id)}: {Id} {nameof(CanCollide)}: {CanCollide}";
        }
    }
}
