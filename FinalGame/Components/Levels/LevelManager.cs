using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SnakeGame.Components.Levels
{
    public class LevelManager
    {
        private List<Level> levels;
        public Level currentLevel = null;

        public LevelManager()
        {
            levels = new List<Level>();
        }

        public void AddLevel(Level level)
        {
            levels.Add(level);
        }

        public void LoadLevel(int index)
        {
            currentLevel = levels[index];
        }

        public void Update(GameTime gameTime)
        {
            currentLevel?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentLevel?.Draw(spriteBatch);
        }


        public string CurrentLevelName()
        {
            if (currentLevel != null)
            {
                return currentLevel?.Name;
            }
            else
            {
                return "Unknown";
            }
        }
    }
}
