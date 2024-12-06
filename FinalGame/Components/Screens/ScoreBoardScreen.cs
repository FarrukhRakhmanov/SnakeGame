using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Components.Levels;
using System.Collections.Generic;
using System.Linq;
using SnakeGame.Components.Entity;

namespace SnakeGame.Components.Screens
{
    public class ScoreBoardScreen : GameScreen
    {
        private SpriteFont _font;
        private ScreenManager _screenManager;
        private ContentManager Content;
        private Menu _menu;
        private MouseState currentMouseState;
        private Vector2 mousePosition;
        public List<GameState> gameStates = new List<GameState>(); 
        public GameState gameState = new GameState();

        public ScoreBoardScreen(ScreenManager screenManager)
        {

            _screenManager = screenManager;
        }

        public override void LoadContent(ContentManager content)
        {
            gameStates = gameState.LoadGameStates()
                .OrderByDescending(gs => gs.Score) 
                .Take(10)
                .Where(gs => gs.Score > 0)
                .ToList();                         

            _font = content.Load<SpriteFont>("Fonts/File");
            Content = content;
            _menu = new Menu(_font, Color.DarkBlue);

            string backToMenuText = "Back to Main menu";

            _menu.AddMenuItem(new MenuItem(backToMenuText, new Rectangle(100, 100, 420, 50), BackToMainMenu, Color.DarkViolet, Color.Orange));
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
            int spacing = 200;
            int count = 1;
            gameStates.ForEach(state =>
            {
                spriteBatch.DrawString(_font, $"{count}. Play time:  {state.PlayDate.ToShortDateString()}  Score:  {state.Score} \n", new Vector2(150, spacing), Color.White);
                spacing += 65;
                count++;
            });

            spriteBatch.End();
        }

        public void BackToMainMenu()
        {
            _screenManager.ChangeScreen(new MenuScreen(_screenManager), Content);
        }
    }

}
