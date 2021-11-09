using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;

namespace SpaceInv
{
    class Alien : Entity
    {

        int width;
        int height;
        Rectangle hitBox;
        public Alien(Vector2 position, Texture2D image, int width, int height)
        {
            this.position = position;
            this.image = image;
            this.width = width;
            this.height = height;
            hitBox = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public override void Update()
        {
            //throw new NotImplementedException();
        }

        public bool Collision()
        {
            if (EntityManager.Missiles.Count > 0 && hitBox.Contains(EntityManager.Missiles.Peek().position))
            {
                Missile m = EntityManager.Missiles.Dequeue();
                m.IsExpired = true;
                m.setShotStatus(false);
                this.IsExpired = true;
                this.setPosition(-50, -50);
                return true;
            }
            return false;
        }

        public void setPosition(int x, int y)
        {
            hitBox.X = x;
            hitBox.Y = y;
        }

        public void movePosition(int x, int y)
        {
            hitBox.X += x;
            hitBox.Y += y;
        }

        public int Left()
        {
            // Top left X position
            return hitBox.Left;
        }

        public int Right()
        {
            return hitBox.Right;
        }
        public override void Draw(SpriteBatch _spriteBatch)
        {
             _spriteBatch.Draw(image, hitBox, Color.White);
        }

    }
}
