using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInv
{
    class Missile : Entity
    {
        private Rectangle missile;
        private int speed;
        private Tank parent;
        bool colision;
        private const int width = 5;
        private const int height = 20;

        public Missile(Vector2 position, Texture2D image, Tank parent)
        {
            this.speed = 5;
            this.image = image;
            this.Position = position;
            this.Colision = false;
            this.parent = parent;
            IsExpired = false;
            missile = new Rectangle((int)position.X, (int)position.Y, Width, height);
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(image, missile, color);
        }
        public override void Update()
        {
            if (missile.Y > 0)
            {
                position.Y -= speed;
                missile.Y = (int)Position.Y;
            }
            else
            {            
                IsExpired = true;
                setShotStatus(false);
            }
            
        }
        public Vector2 Position { get => position; set => position = value; }
        public bool Colision { get => colision; set => colision = value; }

        public void setShotStatus(bool status)
        {
            parent.setShotStatus(status);
        }
        public static int Width => width;

        public int getPosX()
        {
            return missile.X;
        }
        public int getPosY()
        {
            return missile.Y;
        }


    }
}
