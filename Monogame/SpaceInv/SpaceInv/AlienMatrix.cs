using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInv
{
    class AlienMatrix : Entity
    {
        // Matrix info
        const int rows = 5;
        const int cols = 11;
        private Alien[,] alienMatrix;
        int speed;
        int direction = 1;

        // Alien info
        const int width = 35;
        const int height = 25;
        Texture2D alienTx1;

        public int Speed { get => speed; set => speed = value; }

        public AlienMatrix(Texture2D alienTx1)
        {
            
            this.alienMatrix = new Alien[rows, cols];
            this.alienTx1 = alienTx1;
            this.Speed = 4;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    alienMatrix[i, j] = new Alien(new Vector2( (width * j * 1.3f), (height * i * 1.5f)), alienTx1, width, height);
                    EntityManager.Add(alienMatrix[i, j]);
                }
            }

        }
        public override void Update()
        {
            // List<Alien> deadAliens = new List<Alien>();
            foreach (Alien alien in alienMatrix)
            {
                alien.movePosition(Speed*direction, 0);

                //Don't bother if no missile on screen

                if (alien.Collision())
                {
                    alien.IsExpired = true;
                    //deadAliens.Add(alien);
                }
            }


            if (Right() > Game1.getScreenWidth() || Left() < -1)
            {
                // Reverse direction & move down
                direction *= -1;
                foreach (Alien alien in alienMatrix)
                {
                    alien.movePosition(0, height);
                }
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            foreach (Alien alien in alienMatrix)
            {
                if (!alien.IsExpired)
                    alien.Draw(_spriteBatch);

            }
            
        }

        private int Right()
        {
            // Returns X value for top Right corner of matrix.
            return alienMatrix[0, cols-1].Right();
        }

        private int Left()
        {
            // Returns X value for top left corner of matrix
            return alienMatrix[0, 0].Left();
        }

    }
}
