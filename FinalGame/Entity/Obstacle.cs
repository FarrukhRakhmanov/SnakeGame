using Microsoft.Xna.Framework;

namespace SnakeGame.Entity
{
    public class Obstacle
    {
        public Rectangle Position { get; set; }
        public Color Color { get; set; }

        public Obstacle(Rectangle position, Color color)
        {
            Position = position;
            Color = color;
        }
    }
}
