using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SnakeGame.Components.Levels
{
    public class MenuItem
    {
        public string Text { get; set; }
        public Rectangle Bounds { get; set; }
        public Action OnClick { get; set; }

        public Color DefaultColor { get; set; }
        public Color HoverColor { get; set; }
        private bool isHovered;

        public MenuItem(string text, Rectangle bounds, Action onClick, Color defaultColor, Color hoverColor)
        {
            Text = text;
            Bounds = bounds;
            OnClick = onClick;
            DefaultColor = defaultColor;
            HoverColor = hoverColor;
        }


        public MenuItem(string text, Rectangle bounds, Action onClick)
        {
            Text = text;
            Bounds = bounds;
            OnClick = onClick;
        }

        public void Update(Vector2 mousePosition)
        {
            isHovered = Bounds.Contains(mousePosition);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            Color color = isHovered ? HoverColor : DefaultColor;

            spriteBatch.DrawString(font, Text, new Vector2(Bounds.X, Bounds.Y), color);
        }
    }
}
