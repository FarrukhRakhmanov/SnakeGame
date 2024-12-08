using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SnakeGame.Components.Entities
{
    public class Menu
    {
        private List<MenuItem> menuItems;
        private SpriteFont font;
        private Color fontColor;

        public Menu(SpriteFont font, Color color)
        {
            this.font = font;
            fontColor = color;
            menuItems = new List<MenuItem>();
        }

        public void AddMenuItem(MenuItem item)
        {
            menuItems.Add(item);
        }

        public bool Update(GameTime gameTime, Vector2 mousePosition)
        {
            MouseState mouseState = Mouse.GetState();

            foreach (var item in menuItems)
            {
                item.Update(mousePosition);
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 position = new Vector2(mouseState.X, mouseState.Y);
                return Update(position);
            }

            return false;
        }

        public bool Update(Vector2 position)
        {
            foreach (var item in menuItems)
            {
                if (item.Bounds.Contains(position))
                {
                    item.OnClick?.Invoke();
                    return true;
                }
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (var item in menuItems)
            {
                item.Draw(spriteBatch, font);
            }

        }
    }
}
