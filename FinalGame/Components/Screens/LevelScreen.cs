using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Components.Levels;
using SnakeGame.Components.Screens;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace SnakeGame.Components.Screens
{
    public class LevelScreen : GameScreen
    {
        private SpriteFont _font;
        public ScreenManager _screenManager { get; set; }
        private ContentManager Content;
        Menu _menu;
        MouseState currentMouseState;
        Vector2 mousePosition;

        public LevelScreen(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/File");
            Content = content;

            _menu = new Menu(_font, Color.DarkBlue);
            _menu.AddMenuItem(new MenuItem("Level 1", new Rectangle(ScreenWidth / 2 - 100, ScreenHeight / 2 - 200, 200, 50), ShowLevelOne, Color.White, Color.Orange));
            _menu.AddMenuItem(new MenuItem("Level 2", new Rectangle(ScreenWidth / 2 - 100, ScreenHeight / 2 - 125, 200, 50), ShowLevelTwo, Color.White, Color.Orange));
            _menu.AddMenuItem(new MenuItem("Level 3", new Rectangle(ScreenWidth / 2 - 100, ScreenHeight / 2 - 50, 200, 50), ShowLevelThree, Color.White, Color.Orange));
            _menu.AddMenuItem(new MenuItem("Back to Main menu", new Rectangle(ScreenWidth / 2 - 230, ScreenHeight / 2 + 25, 450, 50), BackToMainMenu, Color.White, Color.Orange));
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
            spriteBatch.End();
        }

        public void ShowLevelOne()
        {
            _screenManager.ChangeScreen(new PlayScreen(_screenManager, 0), Content);
        }
        public void ShowLevelTwo()
        {
            _screenManager.ChangeScreen(new PlayScreen(_screenManager, 1), Content);
        }
        public void ShowLevelThree()
        {
            _screenManager.ChangeScreen(new PlayScreen(_screenManager, 2), Content);
        }
        public void BackToMainMenu()
        {
            _screenManager.ChangeScreen(new MenuScreen(_screenManager), Content);
        }
        public void Exit()
        {
            _screenManager.Exit();
        }
    }
}
