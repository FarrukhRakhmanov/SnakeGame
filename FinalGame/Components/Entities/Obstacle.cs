using Microsoft.Xna.Framework;

namespace SnakeGame.Components.Entities
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
