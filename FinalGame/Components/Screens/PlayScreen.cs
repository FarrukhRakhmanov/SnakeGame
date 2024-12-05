using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Components.Levels;

namespace SnakeGame.Components.Screens
{
    public class PlayScreen : GameScreen
    {
        Game _game;

        private Texture2D _sprites;
        private SpriteFont _font;
        private LevelManager _levelManager;
        private ScreenManager _screenManager;
        private int _currentLevelIndex;
        private int _currentScore;

        private bool _isPaused; // New: Track pause state
        private KeyboardState _previousKeyState; // New: Track previous keyboard state for debounce

        public PlayScreen(ScreenManager screenManager, int levelIndex, int score=0)
        {
            _game = screenManager._game;
            _screenManager = screenManager;
            _levelManager = new LevelManager();
            _currentLevelIndex = levelIndex;
            _currentScore = score;
        }

        public override void LoadContent(ContentManager content)
        {
            //_sprites = content.Load<Texture2D>("Images/");
            _font = content.Load<SpriteFont>("Fonts/File");

            Level level1 = new LevelOne("Level One", _currentScore, _screenManager);
            Level level2 = new LevelTwo("Level Two", _currentScore, _screenManager);
            Level level3 = new LevelThree("Level Three", _currentScore, _screenManager);

            level1.LoadContent(content);
            level2.LoadContent(content);
            level3.LoadContent(content);

            // Add levels to the manager
            _levelManager.AddLevel(level1);
            _levelManager.AddLevel(level2);
            _levelManager.AddLevel(level3);
            _levelManager.LoadLevel(_currentLevelIndex);
            _game.TargetElapsedTime = _levelManager.currentLevel.TargetElapsedTime;

        }

        public override void Update(GameTime gameTime)
        {
            _levelManager.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _levelManager.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
