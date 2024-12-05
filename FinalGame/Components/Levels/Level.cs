using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using SnakeGame.Entity;

namespace SnakeGame.Components.Levels
{
    public abstract class Level
    {
        public static int ScreenWidth = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.9);
        public static int ScreenHeight = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.9);
        public string Name { get; set; }
        public TimeSpan TargetElapsedTime { get; set; }
        public Texture2D Background { get; set; }

        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public ContentManager Content { get; set; }
        public Score Score { get; set; }
    }
}
