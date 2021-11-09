using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInv
{
    class Tank : Entity
    {
        private Rectangle tankBase;
        private Rectangle turret;
        private int width;
        private int height;
        private int speed = 3;
        int screenWidth;
        int turretWidth;
        bool hasShot;


        public Tank(Vector2 position, int width, int height, Texture2D image, int screenWidth)
        {
            this.width = width;
            this.height = height;
            this.image = image;
            this.Position = position;
            this.screenWidth = screenWidth;
            this.hasShot = false;
            tankBase = new Rectangle((int)position.X, (int)position.Y, width, height);
            turretWidth = tankBase.Width / 6;
            float turretHeight = tankBase.Height / 1.5f;
            turret = new Rectangle(tankBase.Center.X - turretWidth / 2, tankBase.Y - (int)turretHeight, turretWidth, (int)turretHeight);
        }

        public Vector2 Position { get => position; set => position = value; }
        
        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(image, tankBase, color);
            _spriteBatch.Draw(image, turret, color);
        }


        public override void Update()
        {
            moveTank((int)Input.GetMovementDirection().X * speed);

            // Bounds collision 
            if (tankBase.Left < 0)
            {
                setPosition(0);
            }
            if (tankBase.Right > screenWidth)
            {
                setPosition(screenWidth - tankBase.Width);
            }

            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space) && !hasShot)
            {
                Shoot();
            }
        }

        public void moveTank(int speed)
        {
            tankBase.X += speed;
            turret.X += speed;
        }

        public void setPosition(int xCord)
        {
            tankBase.X = xCord;
            CenterTurret();
        }

        public void Shoot()
        {
            hasShot = true;
            EntityManager.Add(new Missile(new Vector2(turret.Center.X - (float)Missile.Width/2, turret.Y), image, this));
        }

        public void setShotStatus(bool setShot)
        {
            this.hasShot = setShot;
        }

        private void CenterTurret()
        {
            turret.X = tankBase.Center.X - turretWidth / 2;
        }
    }
}
