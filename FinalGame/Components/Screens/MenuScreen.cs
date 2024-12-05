using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Components.Levels;
using Microsoft.Xna.Framework.Media;

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
            string selectLevelText = "Select a Level";
            string helpText = "Help";
            string aboutText = "About";
            string scoreBoardText = "Score Board";
            string exitText = "Exit";
            

            _menu = new Menu(_font, Color.DarkBlue);
            _menu.AddMenuItem(new MenuItem(selectLevelText, CalculateMenuItemRectangle(selectLevelText, 100), ShowLevelMenu, Color.DarkViolet, Color.Orange));
            _menu.AddMenuItem(new MenuItem(helpText, CalculateMenuItemRectangle(exitText, 50), ShowHelpMenu, Color.DarkViolet, Color.Orange));
            _menu.AddMenuItem(new MenuItem(aboutText, CalculateMenuItemRectangle(aboutText, 0), ShowAboutMenu, Color.DarkViolet, Color.Orange));
            _menu.AddMenuItem(new MenuItem(scoreBoardText, CalculateMenuItemRectangle(scoreBoardText, -50), ShowScoreBoardScreen, Color.DarkViolet, Color.Orange));
            _menu.AddMenuItem(new MenuItem(exitText, CalculateMenuItemRectangle(exitText, -100), Exit, Color.DarkViolet, Color.Orange));
        }

        public Vector2 CalculateMenuItemLength(string menuName)
        {
            return _font.MeasureString(menuName);
        }

        public Rectangle CalculateMenuItemRectangle(string menuName, int height)
        {
            Vector2 menuLength = CalculateMenuItemLength(menuName);
            return new Rectangle((ScreenWidth / 2) - ((int)menuLength.X / 2)-(int)menuLength.Y*2, (ScreenHeight / 2) - ((int)menuLength.Y / 2)-height, (int)menuLength.X, (int)menuLength.Y);
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

            // Calculate centered rectangle
            int imageWidth = (int)(ScreenWidth*0.4);  // Replace with your image width
            int imageHeight = (int)(ScreenWidth * 0.4); // Replace with your image height
            Rectangle destinationRectangle = new Rectangle(
                ScreenWidth / 2 - imageWidth / 8-imageWidth/2,  // Center horizontally
                ScreenHeight / 2 - imageHeight /9-imageHeight/2, // Center vertically
                imageWidth,
                imageHeight
            );

            // Draw the image centered
            spriteBatch.Draw(_background, destinationRectangle, Color.White);
                        
            //spriteBatch.Draw(_background, new Rectangle(ScreenWidth / 2 - 450, ScreenHeight / 2 - 450, 800, 800), Color.White);
            
            _menu.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void ShowLevelMenu()
        {
            _screenManager.ChangeScreen(new LevelScreen(_screenManager), Content);
        }
        public void ShowHelpMenu()
        {
            _screenManager.ChangeScreen(new HelpScreen(_screenManager), Content);

        }
        public void ShowAboutMenu()
        {
            _screenManager.ChangeScreen(new AboutScreen(_screenManager), Content);
            
        }
        public void ShowScoreBoardScreen()
        {
            
            _screenManager.ChangeScreen(new ScoreBoardScreen(_screenManager), Content);

        }

        public void Exit()
        {
            _screenManager.Exit();
        }
    }
}
