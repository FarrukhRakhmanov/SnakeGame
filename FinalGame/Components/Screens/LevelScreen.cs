using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Components.Levels;

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
        public int level;
        public int score;

        public LevelScreen(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/File");
            Content = content;
            string levelOneText = "Level 1";
            string levelTwoText = "Level 2";
            string levelThreeText = "Level 3";
            string backToMenuText = "Back to Main menu";
            _menu = new Menu(_font, Color.DarkBlue);
            _menu.AddMenuItem(new MenuItem(levelOneText, CalculateMenuItemRectangle(levelOneText, 100), ShowLevelOne, Color.DarkViolet, Color.Orange));
            _menu.AddMenuItem(new MenuItem(levelTwoText, CalculateMenuItemRectangle(levelTwoText, 50), ShowLevelTwo, Color.DarkViolet, Color.Orange));
            _menu.AddMenuItem(new MenuItem(levelThreeText, CalculateMenuItemRectangle(levelThreeText, 0), ShowLevelThree, Color.DarkViolet, Color.Orange));
            _menu.AddMenuItem(new MenuItem(backToMenuText, CalculateMenuItemRectangle(backToMenuText, -50), BackToMainMenu, Color.DarkViolet, Color.Orange));
        }

        public Vector2 CalculateMenuItemLength(string menuName)
        {
            return _font.MeasureString(menuName);
        }

        public Rectangle CalculateMenuItemRectangle(string menuName, int height)
        {
            Vector2 menuLength = CalculateMenuItemLength(menuName);
            return new Rectangle((ScreenWidth / 2) - ((int)menuLength.X / 2) - (int)menuLength.Y * 2, (ScreenHeight / 2) - ((int)menuLength.Y / 2) - height, (int)menuLength.X, (int)menuLength.Y);
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
            level = 0;
            score = 0;
            _screenManager.ChangeScreen(new PlayScreen(_screenManager, level, score), Content);
        }
        public void ShowLevelTwo()
        {
            level = 1;
            score = 0;
            _screenManager.ChangeScreen(new PlayScreen(_screenManager, level, score), Content);
        }
        public void ShowLevelThree()
        {
            level = 2;
            score = 0;
            _screenManager.ChangeScreen(new PlayScreen(_screenManager, level, score), Content);
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
