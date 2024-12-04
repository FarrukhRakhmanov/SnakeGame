using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Components.Levels;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace SnakeGame.Components.Screens
{
    public class MenuScreen : GameScreen
    {
        private SpriteFont _font;
        private ScreenManager _screenManager;
        private ContentManager Content;
        private MouseState currentMouseState;
        private Vector2 mousePosition;
        private Texture2D _background;
        private Song backgroundSound;

        Menu _menu;

        public MenuScreen(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/File");
            _background = content.Load<Texture2D>("Images/snakeBackground");

            backgroundSound = content.Load<Song>("Audio/maringa-conga-246609");

            Content = content;
            
            MediaPlayer.Play(backgroundSound);

            _menu = new Menu(_font, Color.DarkBlue);
            _menu.AddMenuItem(new MenuItem("Select a Level", new Rectangle(ScreenWidth / 2 - 200, ScreenHeight / 2 - 125, 300, 50), ShowLevelMenu, Color.Black, Color.Orange));
            _menu.AddMenuItem(new MenuItem("Help", new Rectangle(ScreenWidth / 2 - 110, ScreenHeight / 2 - 50, 150, 50), ShowHelpMenu, Color.Black, Color.Orange));
            _menu.AddMenuItem(new MenuItem("About", new Rectangle(ScreenWidth / 2 - 125, ScreenHeight / 2 + 25, 150, 50), ShowAboutMenu, Color.Black, Color.Orange));
            _menu.AddMenuItem(new MenuItem("Exit", new Rectangle(ScreenWidth / 2 - 110, ScreenHeight / 2 + 100, 150, 50), Exit, Color.Black, Color.Orange));
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
            spriteBatch.Draw(_background, new Rectangle(ScreenWidth / 2 - 450, ScreenHeight / 2 - 450, 800, 800), Color.White);
            _menu.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void ShowLevelMenu()
        {
            //MediaPlayer.Stop();
            _screenManager.ChangeScreen(new LevelScreen(_screenManager), Content);
        }
        public void ShowHelpMenu()
        {
            //MediaPlayer.Stop();
            _screenManager.ChangeScreen(new HelpScreen(_screenManager), Content);

        }
        public void ShowAboutMenu()
        {
            //MediaPlayer.Stop();
            _screenManager.ChangeScreen(new AboutScreen(_screenManager), Content);
            
        }
        public void Exit()
        {
            //MediaPlayer.Stop();
            _screenManager.Exit();

        }
    }
}
