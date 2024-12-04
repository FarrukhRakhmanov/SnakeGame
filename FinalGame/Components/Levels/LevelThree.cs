using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using SnakeGame.Components.Screens;
using SnakeGame.Entity;
using Microsoft.Xna.Framework.Media;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using System.Collections.Generic;

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

        // Fonts
        private SpriteFont _font;
        private SpriteFont _countDownFont;

        // Obstacles
        private List<Obstacle> obstacles = new List<Obstacle>();

        // Random number generator
        private Random random = new Random();

        // Distance from the screen edge
        const int distanceFromTheScreenEdge = 2;

        //Game elements
        Game Game;
        public int snakeSize = 60;
        public MySnakeGame snakeGame;

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
        private bool _hasWon = false;
        private KeyboardState _previousKeyState;

        // Countdown fields
        private bool _isCountdownActive = true;
        private double _countdownTime = 3.0; // 3 seconds countdown

        public LevelThree(string levelName, ScreenManager screenManager)
        {
            Name = levelName;
            TargetElapsedTime = TimeSpan.FromMilliseconds(targetElapsedTime);
            _screenManager = screenManager;
            snakeGame = new MySnakeGame(4);
            snakeGame.level = this;
        }

        public LevelThree()
        {
            obstacles = new List<Obstacle>();
        }

        public LevelThree(ScreenManager screenManager)
        {
            // Initialize the new game
            InitializeGameComponents();
            _screenManager = screenManager;
            _isPaused = false;
            obstacles = new List<Obstacle>();
        }

        public void InitializeGameComponents()
        {
            snakeGame = new MySnakeGame(4);
            snakeGame.snake.Create();
            isPoisoned = false;
            obstacles = new List<Obstacle>();

            // Generate random obstacles
            for (int i = 0; i < 5; i++)
            {
                int x = random.Next(0, ScreenWidth / snakeSize) * snakeSize + snakeSize;
                int y = random.Next(0, ScreenHeight / snakeSize) * snakeSize + snakeSize;
                obstacles.Add(new Obstacle(new Rectangle(x, y, snakeSize, snakeSize), Color.Green));
            }

            for (int i = 0; i < 5; i++)
            {
                Food food = new Food();
                Food poisonedFood = new Food();

                food.Create(snakeGame.snake, poisonedFood, obstacles[i], distanceFromTheScreenEdge);
                snakeGame.food.Add(food);

                for (int j = 0; j < 3; j++)
                {
                    poisonedFood = new Food();
                    poisonedFood.Create(snakeGame.snake, food, obstacles[i], distanceFromTheScreenEdge);
                    snakeGame.poisonedFood.Add(poisonedFood);
                }
            }

        }

        public override void LoadContent(ContentManager content)
        {
            Content = content;

            _snakeHead = content.Load<Texture2D>("Images/snakeHead");
            _snakeBody = content.Load<Texture2D>("Images/snakeBody");
            _life = content.Load<Texture2D>("Images/snakeLife");
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

        public override async void Update(GameTime gameTime)
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
                    return;
                }
            }

            // Check if the snake collides with the obstacles
            foreach (var obstacle in obstacles)
            {
                if (snakeGame.snake.Collided(obstacle))
                {
                    _isCollided = true;
                    gameFail.Play(1.0f, 0.0f, 0.0f);
                    return;
                }
            }

            if (snakeGame.score.GetScore() == 15)
            {
                _hasWon = true;
                return;
            }

            snakeGame.snake.Move();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw frame
            DrawFrame(spriteBatch, Color.LightBlue, ScreenWidth);

            if (_isCountdownActive)
            {
                DrawCountdown(spriteBatch);
                return;
            }

            // Draw obstacles
            foreach (var obstacle in obstacles)
            {
                spriteBatch.Draw(CreatePixelTexture(spriteBatch.GraphicsDevice, obstacle.Color), obstacle.Position, obstacle.Color);
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
                spriteBatch.DrawString(_font, "You won!", new Vector2(ScreenWidth / 2 - 150, ScreenHeight / 2 - 100), Color.Red);
                PlayAgain(spriteBatch);
            }
            else
            {
                // Set rotation for snake
                float rotation = 0f;

                // Draw snake
                //DrawSnake(spriteBatch, rotation);

                // Draw poisoned snake
                if (isPoisoned)
                {
                    DrawSnake(spriteBatch, rotation, Color.Red);
                }
                else
                {
                    DrawSnake(spriteBatch, rotation, Color.White);
                }

                // Draw food
                DrawFood(spriteBatch);

                // Draw frame
                DrawFrame(spriteBatch, Color.Blue, 60);

                string levelInfo = "Level 3";
                string scoreInfo = "Score: " + snakeGame.score.GetScore().ToString();
                string lifeInfo = "Life: ";
                // Draw level and score bar                
                spriteBatch.DrawString(_font, levelInfo, new Vector2((ScreenWidth / 10) * 2, 10), Color.White);
                spriteBatch.DrawString(_font, scoreInfo, new Vector2((ScreenWidth / 10) * 4, 10), Color.White);
                spriteBatch.DrawString(_font, lifeInfo, new Vector2(((ScreenWidth / 10) * 6), 10), Color.White);
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
            Vector2 textSize = _countDownFont.MeasureString(countdownText);
            Vector2 position = new Vector2((ScreenWidth - textSize.X) / 2, (ScreenHeight - textSize.Y) / 2);

            spriteBatch.DrawString(_countDownFont, countdownText, position, Color.Green);
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
            // Draw the game elements
            switch (snakeGame.snake.GetDirection())
            {
                case Direction.UP:
                    rotation = MathHelper.ToRadians(270);
                    break;
                case Direction.DOWN:
                    rotation = MathHelper.ToRadians(90);
                    break;
                case Direction.LEFT:
                    rotation = MathHelper.ToRadians(180);
                    break;
                case Direction.RIGHT:
                    rotation = MathHelper.ToRadians(0);
                    break;
            }

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
                float bodyRotation = 0f;
                switch (snakeGame.snake.GetBody()[i].direction)
                {
                    case Direction.UP:
                        bodyRotation = MathHelper.ToRadians(270);
                        break;
                    case Direction.DOWN:
                        bodyRotation = MathHelper.ToRadians(90);
                        break;
                    case Direction.LEFT:
                        bodyRotation = MathHelper.ToRadians(180);
                        break;
                    case Direction.RIGHT:
                        bodyRotation = MathHelper.ToRadians(0);
                        break;
                }

                spriteBatch.Draw(_snakeBody,
                    new Vector2(snakeGame.snake.GetBody()[i].xPosition + snakeSize / 2, snakeGame.snake.GetBody()[i].yPosition + snakeSize / 2),
                    new Rectangle(0, 0, snakeSize, snakeSize),
                    color,
                    bodyRotation,
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
            spriteBatch.DrawString(_font, text, new Vector2(ScreenWidth / 2 - 300, ScreenHeight / 2), Color.Red);
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

        private Texture2D CreatePixelTexture(GraphicsDevice graphicsDevice, Color color)
        {
            Texture2D texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new[] { color });
            return texture;
        }

        private void DrawFrame(SpriteBatch spriteBatch, Color color, int thickness)
        {
            Texture2D pixel = CreatePixelTexture(spriteBatch.GraphicsDevice, color);

            // Top border
            spriteBatch.Draw(pixel, new Rectangle(0, 0, ScreenWidth, thickness), color);
            // Bottom border
            spriteBatch.Draw(pixel, new Rectangle(0, ScreenHeight - thickness, ScreenWidth, thickness), color);
            // Left border
            spriteBatch.Draw(pixel, new Rectangle(0, 0, thickness, ScreenHeight), color);
            // Right border
            spriteBatch.Draw(pixel, new Rectangle(ScreenWidth - thickness, 0, thickness, ScreenHeight), color);
        }
    }
}
