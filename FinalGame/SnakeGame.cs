/* Program.cs
 * Snake Game built using .Net MonoGame Framework
 * 
 * Revision History
 * Farrukh Rakhmanov, Valentine Ohalebo, 11.22.2024: Created
 * Farrukh Rakhmanov, Valentine Ohalebo, 12.07.2024: Final Revision
 * 
 */
using SnakeGame.Components.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using SnakeGame.Components.Entities;

namespace SnakeGame
{
    public class SnakeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;

        private Platform _platform = Platform.Windows;

        Menu _menu;
        public static SnakeGame Instance { get; private set; }
        public static Viewport Viewport
        {
            get
            {
                return Instance.GraphicsDevice.Viewport;
            }

        }
        public static Vector2 ScreenSize
        {
            get
            {
                return new Vector2(Viewport.Width, Viewport.Height);
            }
        }
        public SnakeGame()
        {
            // This must be set or your game will crash
            Instance = this;
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(260);
            IsMouseVisible = true;
            SetWindowSize();           
        }

        public SnakeGame(Platform platform)
        {
            // This must be set or your game will crash
            Instance = this;

            _platform = platform;

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";            

            if (_platform == Platform.Android || _platform == Platform.iOS)
            {
                IsMouseVisible = false;
            }
            else
            {
                SetWindowSize();

                IsMouseVisible = true;
            }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            if (_platform == Platform.Android || _platform == Platform.iOS)
            {
                // Enable touch input
                TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap
                    | GestureType.Flick | GestureType.FreeDrag;
            }


            _screenManager = new ScreenManager(this);
            SetWindowSize();
            base.Initialize();
        }

        private void SetWindowSize()
        {
            int screenWidth = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.9);
            int screenHeight = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.9);

            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _screenManager.ChangeScreen(new MenuScreen(_screenManager), Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _screenManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CadetBlue);
            _screenManager.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}



