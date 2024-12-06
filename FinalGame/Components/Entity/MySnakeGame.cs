using SnakeGame.Components.Levels;
using System.Collections.Generic;

namespace SnakeGame.Components.Entity
{
    public class MySnakeGame
    {
        public Snake snake;
        public List<Food> food;
        public List<Food> poisonedFood { get; set; }
        public Obstacle obstacle;
        public Score score;
        public int life;
        public Level level { get; set; }
        public MySnakeGame(int life)
        {
            snake = new Snake();
            food = new List<Food>();
            poisonedFood = new List<Food>();
            score = new Score();
            this.life = life;
        }
    }
}
