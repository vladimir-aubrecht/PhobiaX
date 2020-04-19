using PhobiaX.Graphics;
using PhobiaX.Physics;
using PhobiaX.SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhobiaX.Game.GameObjects
{
	public class MapGameObject : StaticGameObject
	{
		public MapGameObject(IRenderableObject renderableObject, ICollidableObject collidableObjet) : base(renderableObject, collidableObjet)
		{
		}
	}
}
