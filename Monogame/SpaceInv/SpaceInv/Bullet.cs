using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace SpaceInv
{
    class Bullet : Entity
    {
        public Bullet(Vector2 position, Texture2D image)
        {
            this.position = position;
            this.image = image;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

    }
}