using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInv
{
    static class Input
    {
        private static KeyboardState keyboardState, lastKeyboardState;


        public static void Update()
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

		public static bool WasKeyPressed(Keys key)
		{
			return lastKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);
		}
		public static Vector2 GetMovementDirection()
		{
			// Tank does not move in Y, but used a vector for future compatability. 
			Vector2 direction = new Vector2(0,0);
			if (keyboardState.IsKeyDown(Keys.Left))
				direction.X -= 1;
			if (keyboardState.IsKeyDown(Keys.Right))
				direction.X += 1;

			return direction;
		}
	}
}
