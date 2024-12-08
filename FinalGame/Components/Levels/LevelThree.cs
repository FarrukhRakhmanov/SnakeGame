using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SnakeGame.Components.Screens;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using SnakeGame.Components.Entities;


namespace SnakeGame.Components.Levels
{
    public class LevelThree : Level
    {
        public ScreenManager _screenManager { get; set; }

        // Snake and food textures
        private Texture2D _snakeHead;
        private Texture2D _snakeBody;
        private Texture2D _poisonedApple;
        private Texture2D _apple;
        private Texture2D _life;
        private Texture2D _wall;
        private Texture2D _background;

        // Fonts
        private SpriteFont _font;
        private SpriteFont _countDownFont;

        // Random number generator
        private Random random = new Random();

        // Distance from the screen edge
        const int distanceFromTheScreenEdge = 2;

        //Game elements
        Game Game;
        public int snakeSize = 60;
        public MySnakeGame snakeGame;
        public List<Obstacle> obstacles = new List<Obstacle>();
        public int thickness = 60;


        // Sounds
        public Song backgroundSound;
        public Song startCountDownSound;
        public SoundEffect startCountDownSoundEffect;
        public SoundEffect gameFail;
        public SoundEffect yummy;
        public SoundEffect disgusting;

        // Game states
        public int targetElapsedTime = 160;
        private bool _isPaused = false;
        private bool _isPausedFlag = false;
        private bool _eatPoisonedFood = false;
        private bool _isCollided = false;
        private bool _isGameOver = false;
        private bool isPoisoned = false;
        private KeyboardState _previousKeyState;
        public int _currentScore;

        //Game state saving
        public GameState GameState;

        // Countdown fields
        private bool _isCountdownActive = true;
        private double _countdownTime = 3.0; // 3 seconds countdown

        public LevelThree(string levelName, int score, ScreenManager screenManager)
        {
            Name = levelName;
            TargetElapsedTime = TimeSpan.FromMilliseconds(targetElapsedTime);
            _screenManager = screenManager;
            snakeGame = new MySnakeGame(3);
            snakeGame.score = new Score();
            snakeGame.level = this;
            _currentScore = score;
            snakeGame.score.SetScore(_currentScore);
            GameState = new GameState();
        }

        public LevelThree(ScreenManager screenManager)
        {
            // Initialize the new game
            InitializeGameComponents();
            _screenManager = screenManager;
            _isPaused = false;
        }

        public void InitializeGameComponents()
        {
            obstacles = new List<Obstacle>();
            snakeGame = new MySnakeGame(3);
            snakeGame.score.SetScore(_currentScore);
            _currentScore = 0;

            int x = random.Next(0, ScreenWidth / snakeSize * 2) * snakeSize + snakeSize;
            int y = random.Next(0, ScreenHeight / snakeSize * 2) * snakeSize + snakeSize;

            int barSize = 60 * 6;
            int adjustedWidth = ScreenWidth - barSize;
            int adjustedHeight = ScreenHeight - (barSize + thickness);

           

            snakeGame.snake.Create();
            isPoisoned = false;

            for (int i = 0; i < 4; i++)
            {
                Food food = new Food();
                Food poisonedFood = new Food();

                food.Create(snakeGame.snake, poisonedFood, distanceFromTheScreenEdge, obstacles);
                snakeGame.food.Add(food);

                for (int j = 0; j < 2; j++)
                {
                    poisonedFood = new Food();
                    poisonedFood.Create(snakeGame.snake, food, distanceFromTheScreenEdge, obstacles);
                    snakeGame.poisonedFood.Add(poisonedFood);
                }
            }

        }

        public override void LoadContent(ContentManager content)
        {
            try
            {
                Content = content;

                _snakeHead = content.Load<Texture2D>("Images/snake_green_head");
                _snakeBody = content.Load<Texture2D>("Images/snake_green_body");
                _life = content.Load<Texture2D>("Images/snake_green_head");
                _background = content.Load<Texture2D>("Images/grass");
                _wall = content.Load<Texture2D>("Images/wall_block");
                _apple = content.Load<Texture2D>("Images/apple");
                _poisonedApple = content.Load<Texture2D>("Images/poisoned-apple");
                _font = content.Load<SpriteFont>("Fonts/File");
                _countDownFont = content.Load<SpriteFont>("Fonts/bigFont");
                backgroundSound = content.Load<Song>("Audio/maringa-conga-246609");

                yummy = content.Load<SoundEffect>("Audio/gulp");
                gameFail = content.Load<SoundEffect>("Audio/crash");
                disgusting = content.Load<SoundEffect>("Audio/e-oh");


                //startCountDownSound = content.Load<Song>("Audio/game-countdown");
                startCountDownSoundEffect = content.Load<SoundEffect>("Audio/game-countdown");

                Content = content;

                // Initialize new game
                InitializeGameComponents();
                MediaPlayer.Stop();
                PlaySoundEffectForDuration(startCountDownSoundEffect, TimeSpan.FromSeconds(4));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
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
                {
                    snakeGame.food[i].Create(snakeGame.snake, snakeGame.poisonedFood[i], distanceFromTheScreenEdge);
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
                _isGameOver = true;
                if (_isGameOver)
                {
                    GameState.SaveGameState(GameState.Score);
                    return;
                }
            }

            // Check if the snake collides with the obstacle
            if (snakeGame.snake.Collided(obstacles))
            {
                GameState.SaveGameState(GameState.Score);


                _isCollided = true;
                gameFail.Play(1.0f, 0.0f, 0.0f);
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
            else
            {

                // Draw snake
                if (isPoisoned)
                {
                    DrawSnake(spriteBatch, Color.Yellow);
                }
                else
                {
                    DrawSnake(spriteBatch, Color.White);
                }

                // Draw food
                DrawFood(spriteBatch);

                // Draw frame
                DrawFrame(spriteBatch, _wall, 60);

                // Draw bars
                DrawBars(spriteBatch);

                string levelInfo = "Level 3";
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
            string levelInfo = "Level 3";

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

        private void DrawSnake(SpriteBatch spriteBatch, Color color)
        {
            // Draw snake head
            spriteBatch.Draw(_snakeHead,
                new Vector2(snakeGame.snake.GetHead().X + snakeSize / 2, snakeGame.snake.GetHead().Y + snakeSize / 2),
                new Rectangle(0, 0, snakeSize, snakeSize),
                color,
                0f,
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
                    0f,
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
            spriteBatch.DrawString(_font, resumeText, new Vector2(ScreenWidth / 2 - 230, ScreenHeight / 2 - 50), Color.DarkViolet);

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
            spriteBatch.DrawString(_font, navigationText, new Vector2(ScreenWidth / 2 - 300, ScreenHeight / 2 - 50), Color.Red);

            if (keyState.IsKeyDown(Keys.Space))
            {
                InitializeGameComponents(); // Reset game elements
                _eatPoisonedFood = false;
                _isCollided = false;
                _isCountdownActive = true; // Activate countdown
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

        private void DrawBars(SpriteBatch spriteBatch)
        {            
            // Calculate the number of sprites needed for each border
            int horizontalSprites = ScreenWidth / 60;
            int verticalSprites = ScreenHeight / 60;

            // Top left bar
            int topLeftY = 60;
            int topLeftX = 6*60;
            for (int i = 0; i < 5; i++)
            {
                Obstacle obstacle = new Obstacle(new Rectangle(topLeftX, topLeftY, 60, 60));
                spriteBatch.Draw(
                    _wall,
                    obstacle.Position, // Rectangle for each sprite
                    Color.White
                );
                topLeftY += 60;
                obstacles.Add(obstacle);
            }

            // Top horizontal bar
            int topHorizontal = 60*9;
            int topVertical = 6 * 60;
            for (int i = 0; i < horizontalSprites; i++)
            {
                Obstacle obstacle = new Obstacle(new Rectangle(topHorizontal, topVertical, 60, 60));
                spriteBatch.Draw(
                    _wall,
                    obstacle.Position, // Rectangle for each sprite
                    Color.White
                );
                topHorizontal += 60;
                obstacles.Add(obstacle);
            }

            // Bottom horizontal bar
            int bottomHorizontal = 60;
            int bottomVertical = 60 * 11;
            for (int i = 0; i < verticalSprites; i++)
            {
                Obstacle obstacle = new Obstacle(new Rectangle(bottomHorizontal, bottomVertical, 60, 60));
                spriteBatch.Draw(
                    _wall,
                    obstacle.Position, // Rectangle for each sprite
                    Color.White
                );
                bottomHorizontal += 60;
                obstacles.Add(obstacle);
                if (bottomHorizontal >= ScreenWidth - 60)
                {
                    break;
                }
            }

            // Vertical right bar
            int rightVerticalBar = 60*11;
            int rightHorizontalBar = 60 * 20;
            for (int i = 0; i < verticalSprites; i++)
            {
                Obstacle obstacle = new Obstacle(new Rectangle(rightHorizontalBar, rightVerticalBar, 60, 60));
                spriteBatch.Draw(
                    _wall,
                    obstacle.Position, // Rectangle for each sprite
                    Color.White
                );
                rightVerticalBar += 60;
                obstacles.Add(obstacle);
                if (rightVerticalBar >= ScreenHeight - 60)
                {
                    break;
                }
            }            
        }

    }
}
