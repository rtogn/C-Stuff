using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Diagnostics;
using System.Globalization;

namespace SpaceInv
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        // Fast random gen. from MonoGame.Extended.FastRandom. Call with r.Next()
        public int seed = DateTime.Now.Second;
        public static readonly FastRandom random = new FastRandom(DateTime.Now.Second + 1);

        private Texture2D baseColor;
        private Tank player;
        private Rectangle bounds;

        static int screenHeight;
        static int screenWidth;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            screenHeight = _graphics.PreferredBackBufferHeight;
            screenWidth = _graphics.PreferredBackBufferWidth;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice); 

            baseColor = new Texture2D(GraphicsDevice, 1, 1);
            baseColor.SetData<Color>(new Color[] { Color.GreenYellow });
            
            int tankH = 15;
            int tankW = 50;
            player = new Tank(new Vector2(getScreenWidth() / 2 - tankW / 2, getScreenHeight() - (tankH + 25)), tankW, tankH, baseColor, getScreenWidth());
            EntityManager.Add(player);

            Texture2D alien1 = recolorSpriteMonocolor(Content.Load<Texture2D>("in"), Color.GreenYellow);            
            AlienMatrix AM = new AlienMatrix(alien1);
            EntityManager.Add(AM);

            bounds = new Rectangle(0, 0, getScreenWidth(), getScreenHeight());
            //Rectangle(int x, int y, int width, int height);
            //missile = new Missile(new Vector2(turret.Center.X, turret.Y), 10, 20, baseColor);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.Update();
            EntityManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Draw base background.
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            {
                //_spriteBatch.Draw(baseColor, bounds, Color.White);
                EntityManager.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static int getScreenHeight()
        {
            return screenHeight;
        }
        public static int getScreenWidth()
        {
            return screenWidth;
        }
        public int getScreenMidY()
        {
            return getScreenHeight() / 2;
        }
        public int getScreenMidX()
        {
            return getScreenWidth() / 2;
        }

        public int geObjHeightMid(int objectHeight)
        {
            return objectHeight / 2;
        }

        public Texture2D recolorSpriteMonocolor(Texture2D sprite, Color color)
        {
            // Declare color array to the size of passed sprite and fill it with the data from the sprite
            Color[] data = new Color[sprite.Width * sprite.Height];
            sprite.GetData(data);

            // For every item in data that is not 100% transparant, replace with desired color.  
            for (int i = 0; i < data.Length; i++)
            {
                // If not transparancy pixel, change the color. 
                if (data[i].A > 0)
                {
                    data[i] = color;
                }              
            }
            // Set the sprite to this data.
            sprite.SetData<Color>(data);
            return sprite;
        }
    }
}
