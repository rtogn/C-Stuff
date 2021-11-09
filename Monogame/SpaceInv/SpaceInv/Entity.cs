using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInv
{
    abstract class Entity
    {
        protected Texture2D image;
        protected Color color = Color.White;

        public Vector2 position;
        public bool IsExpired;
        public Vector2 Size
        {
            get
            {
                return image == null ? Vector2.Zero : new Vector2(image.Width, image.Height);
            }
        }

        public abstract void Update();
        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(image, position, color);
        }
    }
}
