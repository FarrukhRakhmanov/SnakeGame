using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Components.Levels;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SnakeGame.Components.Entities;
using SnakeGame.Components.Entities;

namespace SnakeGame.Components.Screens
{
    public class HelpScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteFont _fontText;
        private ScreenManager _screenManager;
        private ContentManager Content;
        private Menu _menu;
        private MouseState currentMouseState;
        private Vector2 mousePosition;
        List<string> instructions = new List<string>
        {
    "How to Play Snake",
    "1. Objective:",
    "   Control the snake to eat food and grow longer. Avoid hitting the walls, obstacles,",
    "   and the snake's own body to achieve the highest score.",
    "",
    "2. Controls:",
    "   - Use the arrow keys (Up, Down, Left, Right) to move the snake.",
    "   - The snake will continue to move in the chosen direction until you change it.",
    "",
    "3. Gameplay:",
    "   - The snake starts at a fixed length. Each time it eats food, it grows longer.",
    "   - The game ends if the snake:",
    "     - Hits the walls, poisoned food or boundaries of the play area.",
    "     - Runs into its own body.",
    "",
    "4. Scoring:",
    "   - Points are earned every time the snake eats food. The score increases with each item consumed.",
    "",
    "5. Tips for Success:",
    "   - Plan your movements to avoid boxing the snake into corners or colliding with itself.",
    "   - Use open spaces to maneuver safely and keep the snake manageable as it grows.",
    "",
    "6. Pause and Exit:",
    "   - Press the Space key to pause the game.",
    "   - Press the Esc key to exit the game at any time.",
    "     Enjoy the game and aim for the highest score!"
        };


        public HelpScreen(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }

        public HelpScreen(ScreenManager screenManager, ContentManager content)
        {
            _screenManager = screenManager;
            Content = content;
        }

        public override void LoadContent(ContentManager content)
        {
            _fontText = content.Load<SpriteFont>("Fonts/MyFont");
            _font = content.Load<SpriteFont>("Fonts/File");
            Content = content;
            _menu = new Menu(_font, Color.DarkBlue);
            _menu.AddMenuItem(new MenuItem("Back to Main menu", new Rectangle(100, 100, 420, 50), BackToMainMenu, Color.DarkViolet, Color.Orange));
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
            int stringLocation = 200;
            spriteBatch.Begin();
            _menu.Draw(spriteBatch);
            foreach (string instruction in instructions)
            {
                spriteBatch.DrawString(_fontText, instruction, new Vector2(250, stringLocation), Color.White);
                stringLocation += 30;
            }
            spriteBatch.End();
        }
        public void BackToMainMenu()
        {
            _screenManager.ChangeScreen(new MenuScreen(_screenManager), Content);
        }


    }

}
