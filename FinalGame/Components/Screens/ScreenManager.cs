using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SnakeGame.Components.Screens
{

    public class ScreenManager
    {
        private GameScreen _currentScreen;
        public Game _game;

        public ScreenManager(Game game)
        {
            _game = game;
        }
        public void ChangeScreen(GameScreen newScreen, ContentManager content)
        {
            _currentScreen = newScreen;
            _currentScreen.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            _currentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScreen.Draw(spriteBatch);
        }
        public void Exit()
        {
            _currentScreen = null;
            _game.Exit();
        }
    }
}
