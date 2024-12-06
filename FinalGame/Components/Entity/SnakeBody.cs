namespace SnakeGame.Components.Entity
{
    public class SnakeBody
    {
        public int xPosition { get; set; }
        public int yPosition { get; set; }
        public int previousXposition { get; set; }
        public int previousYposition { get; set; }
        public Direction direction { get; set; }

        public SnakeBody(int x, int y)
        {
            xPosition = x;
            yPosition = y;
            direction = Direction.RIGHT;
        }
    }
}
