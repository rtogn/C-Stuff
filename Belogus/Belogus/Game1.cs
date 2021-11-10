using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Globalization;

namespace Space_Pong
{

    public class Game1 : Game
    {
        // Base objects
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        // Fast random gen. from MonoGame.Extended.FastRandom. Call with r.Next()
        public int seed = DateTime.Now.Second;
        public static readonly FastRandom random = new FastRandom(DateTime.Now.Second +1);
      
        // Background objects
        private Texture2D background;
        private Texture2D earth;

        // Base texture for objects
        private Texture2D pongBaseColor;

        // Game objects and base sizes.
        const int paddleHeight = 55;
        const int paddleWidth = 15;
        int paddleSpeed = 8;
        Color paddleColor = Color.White;
        int colorFlashTimer = 0;
        private Rectangle paddleL;
        private Rectangle paddleR;
        
        const int mideLineWidth = 3;
        private Rectangle midLine;
        
        const int ballSize = 15;
        int ballSpeed = 5;
        Vector2 ballDir;
        private Rectangle ball;

        private SpriteFont font;
        private int scoreL = 0;
        private int scoreR = 0;
        private int debugVal;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            random.Next();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            pongBaseColor = new Texture2D(GraphicsDevice, 1, 1);
            pongBaseColor.SetData<Color>(new Color[] { Color.White });
            
            midLine = new Rectangle(getScreenMidX(), 0, mideLineWidth, getScreenHeight());
            ballDir = InitBallDir();
            ball = new Rectangle(BallOrigin(), new Point(ballSize, ballSize));
            paddleL = new Rectangle(paddleWidth +50, getScreenMidY() - geObjHeightMid(paddleHeight), paddleWidth, paddleHeight);
            paddleR = new Rectangle(getScreenWidth() - paddleWidth*2 -50, getScreenMidY() - geObjHeightMid(paddleHeight), paddleWidth, paddleHeight);

            background = Content.Load<Texture2D>("stars");
            earth = Content.Load<Texture2D>("earth");
            font = Content.Load<SpriteFont>("File");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // Update logic below

            // The Keyboard class has a method that returns the current state of the keyboard whenever it is called.
            // The current state of the keyboard is stored in a KeyboardState object. In this line of code, we are calling our current state state.
            KeyboardState state = Keyboard.GetState();
            if (colorFlashTimer == 0) paddleColor = Color.White;
            if (colorFlashTimer > 0) colorFlashTimer--;

            if (state.IsKeyDown(Keys.Up) && paddleL.Y > 0)
            {
                paddleL.Y -= paddleSpeed;
                paddleR.Y += paddleSpeed;
            }
            if (state.IsKeyDown(Keys.Down) && (paddleL.Y < (getScreenHeight() - paddleL.Height)))
            {
                paddleL.Y += paddleSpeed;
                paddleR.Y -= paddleSpeed;
            }
            if (state.IsKeyDown(Keys.R))
            {
                ResetBall();
            }

            debugVal = ballSpeed;
            MoveBall(ballDir);
            CheckBallPosition();
            UpdateScore();
            //score++;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
            _spriteBatch.Draw(earth, new Vector2(475, 240), Color.White);
            _spriteBatch.DrawString(font, "Score: " + scoreL, new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(font, "Score: " + scoreR, new Vector2(getScreenWidth() - 125, 10), Color.White);
            _spriteBatch.DrawString(font, "Debug: " + debugVal, new Vector2(getScreenWidth() / 2), Color.White);

            _spriteBatch.Draw(pongBaseColor, midLine, Color.White);
            _spriteBatch.Draw(pongBaseColor, ball, Color.Red);
            _spriteBatch.Draw(pongBaseColor, paddleL, paddleColor);
            _spriteBatch.Draw(pongBaseColor, paddleR, paddleColor);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
        public void MoveBall(Vector2 direction)
        {
            // Update ball method. References Vector2 for direciton. 
            ball.X += (int)direction.X;
            ball.Y += (int)direction.Y;
        }
        public Point BallOrigin()
        {
            // Gets point for centering the ball on the court. 
            return new Point(getScreenMidX() - geObjHeightMid(ballSize), getScreenMidY() - geObjHeightMid(ballSize));
        }

        public Vector2 InitBallDir()
        {
            // Initialize ball direction to a random side by multiplying speed by - 1.pos = right, negative = left.
            if (random.Next(0, 1) == 1)
            {
                return new Vector2(ballSpeed, 0);
            }
            else
            {
                return new Vector2(ballSpeed * -1, 0);
            }
            //return new Vector2(ballSpeed, 0);
        }

        public void ResetBall()
        {
            // Reset ball center court and with new rand. direction.
            ball.X = BallOrigin().X;
            ball.Y = BallOrigin().Y;
            ballDir = InitBallDir();
        }

        public void UpdateScore()
        {
            int center = ball.X;
            if (center < -10)
            {
                scoreR++;
                paddleL.Height++;
                paddleR.Height--;
                paddleColor = Color.Red;
                colorFlashTimer = 8;
                ResetBall();
            }
            else if (center > getScreenWidth() + 5)
            {
                scoreL++;
                paddleR.Height++;
                paddleL.Height--;
                paddleColor = Color.Red;
                colorFlashTimer = 8;
                ResetBall();
            }
        }

        public void CheckBallPosition()
        {
            CeilingColision();
            PaddleColision();
        }

        public void PaddleColision()
        {
            
            if ( paddleL.Intersects(ball))
            {
                // Set ball pos to front of paddle
                ball.X = paddleL.Location.X + ballSize;
                ballDir.X *= -1;
                if (ballDir.Y == 0)
                    ballDir.Y = ballDir.X;
            }
            if (paddleR.Intersects(ball))
            {
                ball.X = paddleR.Location.X - ballSize;
                ballDir.X *= -1;
                if (ballDir.Y == 0)
                    ballDir.Y = ballDir.X;
            }
            /*
            if ( (ball.Left <= paddleL.Right && ball.Left >= paddleL.Left) && ball.Center.Y >= paddleL.Top && ball.Center.Y <= paddleL.Bottom)
            {
                    ballDir.X *= -1;
                    if (ballDir.Y == 0)
                        ballDir.Y = ballDir.X;
            }
            else if((ball.Right >= paddleR.Left && ball.Right <= paddleR.Right) && ball.Center.Y >= paddleR.Top && ball.Center.Y <= paddleR.Bottom)
            {
                ballDir.X *= -1;
                if (ballDir.Y == 0)
                    ballDir.Y = ballDir.X;
            }
            */
        }
        public void CeilingColision()
        {
            if (ball.Bottom > getScreenHeight() || ball.Y < 0)
            {
                ballDir.Y *= -1;
            }
        }
        public int getScreenHeight()
        {
            return _graphics.PreferredBackBufferHeight;
        }
        public int getScreenWidth()
        {
            return _graphics.PreferredBackBufferWidth;
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
    }
}
