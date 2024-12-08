using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Components.Levels;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SnakeGame.Components.Entities;
using SnakeGame.Components.Entities;

namespace SnakeGame.Components.Screens
{
    public class AboutScreen : GameScreen
    {
        private SpriteFont _font;
        private ScreenManager _screenManager;
        private ContentManager Content;
        private Menu _menu;
        private MouseState currentMouseState;
        private Vector2 mousePosition;
        private Song hoverSound;

        public AboutScreen(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/File");
            Content = content;
            _menu = new Menu(_font, Color.DarkBlue);
            _menu.AddMenuItem(new MenuItem("Back to Main menu", new Rectangle(100, 150, 420, 50), BackToMainMenu, Color.DarkViolet, Color.Orange));
        }

        public override void Update(GameTime gameTime)
        {
            // Get the current mouse state
            currentMouseState = Mouse.GetState();
            mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

            bool bMenuClicked = _menu.Update(gameTime, mousePosition);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _menu.Draw(spriteBatch);
            spriteBatch.DrawString(_font, "Developed by: Farrukh Rakhmanov \n", new Vector2(150, 250), Color.White);
            spriteBatch.DrawString(_font, "Developed by: Valentine Ohalebo", new Vector2(150, 300), Color.White);
            spriteBatch.End();
        }

        public void BackToMainMenu()
        {
            _screenManager.ChangeScreen(new MenuScreen(_screenManager), Content);
        }
    }

}
