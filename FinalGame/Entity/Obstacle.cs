using Microsoft.Xna.Framework;

namespace SnakeGame.Entity
{
    public class Obstacle
    {
        public Rectangle Position { get; set; }
        public Obstacle(Rectangle position)
        {
            Position = position;
        }
    }
}
