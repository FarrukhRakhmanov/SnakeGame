using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SnakeGame.Components.Screens;
using System.Threading.Tasks;
using System.Linq;
using System;
using SnakeGame.Components.Entities;

namespace SnakeGame.Components.Levels
{
    public class LevelOne : Level
    {
        public ScreenManager _screenManager { get; set; }

        // Snake and food textures
        private Texture2D _snakeHead;
        private Texture2D _snakeBody;
        private Texture2D _poisonedApple;
        private Texture2D _apple;
        private Texture2D _life;
        private SpriteFont _font;
        private Texture2D _wall;
        private SpriteFont _countDownFont;
        private Texture2D _background;
        const int distanceFromTheScreenEdge = 3;
        
        Game Game;

        // Game elements

        public int snakeSize = 60;
        public MySnakeGame snakeGame;

        public Song backgroundSound;
        public Song startCountDownSound;
        public SoundEffect startCountDownSoundEffect;
        public SoundEffect gameFail;
        public SoundEffect yummy;
        public SoundEffect disgusting;

        public int targetElapsedTime = 180;
        private bool _isPaused = false;
        private bool _isPausedFlag = false;
        private bool _eatPoisonedFood = false;
        private bool _isCollided = false;
        private bool _isGameOver = false;
        private bool isPoisoned = false;
        private bool _hasWon = false;
        private KeyboardState _previousKeyState;

        // Countdown fields
        private bool _isCountdownActive = true;
        private double _countdownTime = 3.0; 
        public int _currentScore = 0;
        public Score Score;

        //Game state saving
        public GameState GameState;

        public LevelOne(string levelName, int score, ScreenManager screenManager)
        {
            Name = levelName;
            TargetElapsedTime = TimeSpan.FromMilliseconds(targetElapsedTime);
            _screenManager = screenManager;
            snakeGame = new MySnakeGame(5);
            snakeGame.level = this;
            _currentScore = score;
            snakeGame.score.SetScore(_currentScore);
            GameState = new GameState();
        }

        public LevelOne(ScreenManager screenManager)
        {
            // Initialize the new game
            InitializeGameComponents();
            _screenManager = screenManager;
            _isPaused = false;
        }

        public void InitializeGameComponents()
        {
            // Create snake
            snakeGame = new MySnakeGame(5);
            snakeGame.snake.Create();


            isPoisoned = false;

            // Create food
            for (int i = 0; i < 3; i++)
            {
                Food food = new Food();
                Food poisonedFood = new Food();

                poisonedFood.Create(snakeGame.snake, food, distanceFromTheScreenEdge);
                snakeGame.poisonedFood.Add(poisonedFood);

                for (int j = 0; j < 2; j++)
                {
                    food = new Food();
                    food.Create(snakeGame.snake, poisonedFood, distanceFromTheScreenEdge);
                    snakeGame.food.Add(food);
                }
            }
        }
        public override void LoadContent(ContentManager content)
        {
            Content = content;

            try
            {
                // Load snake textures
                _snakeHead = content.Load<Texture2D>("Images/snake_green_head");
                _snakeBody = content.Load<Texture2D>("Images/snake_green_body");
                _life = content.Load<Texture2D>("Images/snake_green_head");
                _background = content.Load<Texture2D>("Images/grass");

                // Load food textures
                _apple = content.Load<Texture2D>("Images/apple");
                _poisonedApple = content.Load<Texture2D>("Images/poisoned-apple");

                // Load wall texture
                _wall = content.Load<Texture2D>("Images/wall_block");

                // Load font
                _font = content.Load<SpriteFont>("Fonts/File");

                // Load countdown font
                _countDownFont = content.Load<SpriteFont>("Fonts/bigFont");

                // Load sounds
                backgroundSound = content.Load<Song>("Audio/maringa-conga-246609");
                yummy = content.Load<SoundEffect>("Audio/gulp");
                gameFail = content.Load<SoundEffect>("Audio/crash");
                disgusting = content.Load<SoundEffect>("Audio/e-oh");
                startCountDownSoundEffect = content.Load<SoundEffect>("Audio/game-countdown");

                // Initialize new game
                InitializeGameComponents();
                MediaPlayer.Stop();
                PlaySoundEffectForDuration(startCountDownSoundEffect, TimeSpan.FromSeconds(4));
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_isCountdownActive)
            {
                _countdownTime -= gameTime.ElapsedGameTime.TotalSeconds;

                if (_countdownTime <= 0)
                {
                    _isCountdownActive = false;
                }
                return;
            }

            var keyState = Keyboard.GetState();

            // Disable pause functionality if the game is over
            if (!_isCollided && !_eatPoisonedFood)
            {
                // Toggle pause with 'P' key (debounced to avoid rapid toggles)
                if (keyState.IsKeyDown(Keys.P) && !_previousKeyState.IsKeyDown(Keys.P))
                {
                    _isPaused = !_isPaused;
                    if (_isPaused)
                    {
                        _isPausedFlag = true;
                    }
                    else
                    {
                        _isPausedFlag = false;
                    }
                }
            }

            _previousKeyState = keyState; // Update previous key state

            // Skip the update logic if the game is paused
            if (_isPaused && _isPausedFlag)
            {
                return;
            }

            // Skip further updates if the game is over
            if (_eatPoisonedFood || _isCollided)
            {
                return;
            }

            // Continue with the original update logic for movement and food checks

            if (keyState.IsKeyDown(Keys.Up) && snakeGame.snake.GetDirection() != Direction.DOWN)
            {
                snakeGame.snake.SetDirection(Direction.UP);
            }
            else if (keyState.IsKeyDown(Keys.Down) && snakeGame.snake.GetDirection() != Direction.UP)
            {
                snakeGame.snake.SetDirection(Direction.DOWN);
            }
            else if (keyState.IsKeyDown(Keys.Left) && snakeGame.snake.GetDirection() != Direction.RIGHT)
            {
                snakeGame.snake.SetDirection(Direction.LEFT);
            }
            else if (keyState.IsKeyDown(Keys.Right) && snakeGame.snake.GetDirection() != Direction.LEFT)
            {
                snakeGame.snake.SetDirection(Direction.RIGHT);
            }

            // Check if the snake eats the food
            for (int i = 0; i < snakeGame.food.Count; i++)
            {
                if (snakeGame.food[i].WasEated(snakeGame.snake.GetHead()))
                    if (snakeGame.food[i].WasEated(snakeGame.snake.GetHead()))
                    {
                        snakeGame.food[i].Create(snakeGame.snake, snakeGame.poisonedFood.FirstOrDefault(), distanceFromTheScreenEdge);
                        yummy.Play(1.0f, 0.0f, 0.0f);
                        snakeGame.snake.Grow();
                        snakeGame.score.IncreaseScore();
                        GameState.Score = snakeGame.score.GetScore();
                        GameState.PlayDate = DateTime.Now;
                        isPoisoned = false;
                    }
            }


            // Check if the snake eats the poisoned food
            for (int i = 0; i < snakeGame.poisonedFood.Count; i++)
            {
                if (snakeGame.poisonedFood[i].WasEated(snakeGame.snake.GetHead()))
                {
                    isPoisoned = true;
                    snakeGame.poisonedFood[i].Create(snakeGame.snake, snakeGame.food.FirstOrDefault(), distanceFromTheScreenEdge);
                    disgusting.Play(1.0f, 0.0f, 0.0f);
                    snakeGame.life--;
                    snakeGame.score.DecreaseScore();

                    break;
                }
            }
            if (snakeGame.life < 1)
            {
                GameState.SaveGameState(GameState.Score);

                _isGameOver = true;
                if (_isGameOver)
                {
                    return;
                }
            }

            if (snakeGame.snake.Collided())
            {
                GameState.SaveGameState(GameState.Score);

                _isCollided = true;
                gameFail.Play(1.0f, 0.0f, 0.0f);
                return;
            }

            if (snakeGame.score.GetScore() == 10)
            {
                GameState.SaveGameState(GameState.Score);
                _hasWon = true;
                return;
            }

            snakeGame.snake.Move();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
            if (_isCountdownActive)
            {
                DrawCountdown(spriteBatch);
                return;
            }

            var keyState = Keyboard.GetState();

            if (_isPaused)
            {
                PauseGame(spriteBatch);
            }
            else if (_eatPoisonedFood)
            {
                CollisionHandling(spriteBatch, "Poisoned");
            }
            else if (_isCollided)
            {
                CollisionHandling(spriteBatch, "Collision");
            }
            else if (_isGameOver)
            {
                spriteBatch.DrawString(_font, "Game Over!", new Vector2(ScreenWidth / 2 - 150, ScreenHeight / 2 - 100), Color.Red);
                PlayAgain(spriteBatch);
                _isGameOver = false;
            }
            else if (_hasWon)
            {
                _screenManager.ChangeScreen(new PlayScreen(_screenManager, 1, snakeGame.score.GetScore()), Content);
            }
            else
            {
                // Set rotation for snake
                float rotation = 0f;

                // Draw poisoned snake
                if (isPoisoned)
                {
                    DrawSnake(spriteBatch, rotation, Color.Yellow);
                }
                else
                {
                    DrawSnake(spriteBatch, rotation, Color.White);
                }

                // Draw food
                DrawFood(spriteBatch);

                // Draw frame
                DrawFrame(spriteBatch, _wall, 60);

                string levelInfo = "Level 1";
                string scoreInfo = "Score: " + snakeGame.score.GetScore().ToString();
                string lifeInfo = "Life: ";
              
                // Draw level and score bar                
                spriteBatch.DrawString(_font, levelInfo, new Vector2((ScreenWidth / 10) * 2, 10), Color.DarkViolet);
                spriteBatch.DrawString(_font, scoreInfo, new Vector2((ScreenWidth / 10) * 4, 10), Color.DarkViolet);
                spriteBatch.DrawString(_font, lifeInfo, new Vector2(((ScreenWidth / 10) * 6), 10), Color.DarkViolet);
                int space = 150;
                for (int i = 0; i < snakeGame.life; i++)
                {
                    spriteBatch.Draw(_life, new Rectangle(((ScreenWidth / 10) * 6) + space, 10, 40, 40), Color.White);
                    space += 50;
                }

            }
            spriteBatch.End();
        }
        private void DrawCountdown(SpriteBatch spriteBatch)
        {
            string countdownText = Math.Ceiling(_countdownTime).ToString();
            string levelInfo = "Level 1";

            Vector2 textSize = _countDownFont.MeasureString(countdownText);
            Vector2 level = _countDownFont.MeasureString(levelInfo);

            Vector2 positionInfo = new Vector2((ScreenWidth - level.X) / 2, (ScreenHeight - level.Y) / 2 - level.Y);
            Vector2 positionCount = new Vector2((ScreenWidth - textSize.X) / 2, (ScreenHeight - textSize.Y) / 2);

            spriteBatch.DrawString(_countDownFont, levelInfo, positionInfo, Color.DarkViolet);
            spriteBatch.DrawString(_countDownFont, countdownText, positionCount, Color.White);
            spriteBatch.End();
        }

        private void DrawFood(SpriteBatch spriteBatch)
        {
            // Draw food
            for (int i = 0; i < snakeGame.food.Count; i++)
            {
                spriteBatch.Draw(_apple, snakeGame.food[i].GetPosition(), snakeGame.food[i].GetSpritePosition(), Color.White);
            }

            // Draw poisoned food
            for (int i = 0; i < snakeGame.poisonedFood.Count; i++)
            {
                spriteBatch.Draw(_poisonedApple, snakeGame.poisonedFood[i].GetPosition(), snakeGame.poisonedFood[i].GetSpritePosition(), Color.White);
            }
        }

        private void DrawSnake(SpriteBatch spriteBatch, float rotation, Color color)
        {
            // Draw snake head
            spriteBatch.Draw(_snakeHead,
                new Vector2(snakeGame.snake.GetHead().X + snakeSize / 2, snakeGame.snake.GetHead().Y + snakeSize / 2),
                new Rectangle(0, 0, snakeSize, snakeSize),
                color,
                rotation,
                new Vector2(snakeSize / 2, snakeSize / 2),
                1.0f,
                SpriteEffects.None,
                0f);

            // Draw snake body
            for (int i = 1; i < snakeGame.snake.GetBody().Count; i++)
            {
                spriteBatch.Draw(_snakeBody,
                    new Vector2(snakeGame.snake.GetBody()[i].xPosition + snakeSize / 2, snakeGame.snake.GetBody()[i].yPosition + snakeSize / 2),
                    new Rectangle(0, 0, snakeSize, snakeSize),
                    color,
                    rotation,
                    new Vector2(snakeSize / 2, snakeSize / 2),
                    1.0f,
                    SpriteEffects.None,
                    0f);
            }

        }

        private void PauseGame(SpriteBatch spriteBatch)
        {
            var keyState = Keyboard.GetState();
            var pausedText = "Game Paused";
            var resumeText = "Press <P> to continue";
            var backToMainMenu = "Press <M> for main menu";
            var textSize = _font.MeasureString(pausedText);

            // Display "Game Paused" text when paused
            spriteBatch.DrawString(_font, pausedText, new Vector2(ScreenWidth / 2 - 150, ScreenHeight / 2 - 100), Color.Red);

            // Display "Press Space to resume game" text when paused
            spriteBatch.DrawString(_font, resumeText, new Vector2(ScreenWidth / 2 - 230, ScreenHeight / 2 - 50), Color.Red);

            DrawBackToMainMenu(spriteBatch, backToMainMenu);

            // Go back to main menu with 'M' key
            if (_isPaused && keyState.IsKeyDown(Keys.M))
            {
                if (_screenManager != null && Content != null)
                {
                    _screenManager.ChangeScreen(new MenuScreen(_screenManager), Content);
                }
            }
        }

        private void DrawBackToMainMenu(SpriteBatch spriteBatch, string text)
        {
            // Display "Press M for main menu" text when paused
            spriteBatch.DrawString(_font, text, new Vector2(ScreenWidth / 2 - 300, ScreenHeight / 2), Color.DarkViolet);
        }

        private void CollisionHandling(SpriteBatch spriteBatch, string collisionOrPoisoned)
        {
            var backToMainMenu = "Press <M> for main menu";
            var outputText = "You failed!";

            if (_isCollided || _eatPoisonedFood)
            {
                switch (collisionOrPoisoned)
                {
                    case "Poisoned":
                        outputText = "Poisoned!";
                        break;
                    case "Collision":
                        outputText = "Collided!";
                        break;
                }

                var textSize = _font.MeasureString(outputText);
                spriteBatch.DrawString(_font, outputText, new Vector2(ScreenWidth / 2 - 150, ScreenHeight / 2 - 100), Color.Red);
                PlayAgain(spriteBatch);
                DrawBackToMainMenu(spriteBatch, backToMainMenu);

            }

            var keyState = Keyboard.GetState();

            // Go back to main menu with 'M' key
            if (_eatPoisonedFood | _isCollided && keyState.IsKeyDown(Keys.M))
            {
                _eatPoisonedFood = false;
                _isCollided = false;

                if (_screenManager != null && Content != null)
                {
                    _screenManager.ChangeScreen(new MenuScreen(_screenManager), Content);
                }
            }
        }

        private void PlayAgain(SpriteBatch spriteBatch)
        {
            var keyState = Keyboard.GetState();
            var navigationText = "Press <Space> to play again";
            spriteBatch.DrawString(_font, navigationText, new Vector2(ScreenWidth / 2 - 300, ScreenHeight / 2 - 50), Color.DarkViolet);

            if (keyState.IsKeyDown(Keys.Space))
            {
                InitializeGameComponents(); // Reset game elements
                _eatPoisonedFood = false;
                _isCollided = false;
                _isCountdownActive = true; // Activate countdown
                _hasWon = false;
                _countdownTime = 3.0; // Reset countdown time
                PlaySoundEffectForDuration(startCountDownSoundEffect, TimeSpan.FromSeconds(2));
            }
        }

        private async void PlaySoundEffectForDuration(SoundEffect soundEffect, TimeSpan duration)
        {
            var instance = soundEffect.CreateInstance();
            instance.Play();
            await Task.Delay(duration);
            instance.Stop();
        }

        private void DrawFrame(SpriteBatch spriteBatch, Texture2D spriteTexture, int spriteSize)
        {
            // Calculate the number of sprites needed for each border
            int horizontalSprites = ScreenWidth / spriteSize;
            int verticalSprites = ScreenHeight / spriteSize;

            // Top border 
            for (int i = 0; i < horizontalSprites; i++)
            {
                spriteBatch.Draw(
                    spriteTexture,
                    new Vector2(i * spriteSize + spriteSize / 2, spriteSize / 2), // Center of the sprite
                    null,
                    Color.White,
                     -MathHelper.PiOver2,
                    new Vector2(spriteSize / 2, spriteSize / 2), // Origin at the center
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }

            // Bottom border 
            for (int i = 0; i < horizontalSprites; i++)
            {
                spriteBatch.Draw(
                    spriteTexture,
                    new Vector2(i * spriteSize + spriteSize / 2, ScreenHeight - spriteSize / 2), // Center of the sprite
                    null,
                    Color.White,
                    -MathHelper.PiOver2, // 180 degrees rotation
                    new Vector2(spriteSize / 2, spriteSize / 2),
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }

            // Left border 
            for (int i = 0; i < verticalSprites; i++)
            {
                spriteBatch.Draw(
                    spriteTexture,
                    new Vector2(spriteSize / 2, i * spriteSize + spriteSize / 2),
                    null,
                    Color.White,
                    -MathHelper.PiOver2, // -90 degrees rotation
                    new Vector2(spriteSize / 2, spriteSize / 2),
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }

            // Right border
            for (int i = 0; i < verticalSprites; i++)
            {
                spriteBatch.Draw(
                    spriteTexture,
                    new Vector2(ScreenWidth - spriteSize / 2, i * spriteSize + spriteSize / 2),
                    null,
                    Color.White,
                    MathHelper.PiOver2, // 90 degrees rotation
                    new Vector2(spriteSize / 2, spriteSize / 2),
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }
        }


    }
}
