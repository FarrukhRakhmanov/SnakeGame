using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace SnakeGame.Entity
{
    public class PoisonedFood
    {
        public static int ScreenWidth = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width*0.9);
        public static int ScreenHeight = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height*0.9);
        Random random = new Random();
        public int poisonedFoodSize = 60;
        public Rectangle spritePosition = new Rectangle(0, 0, 60, 60);
        Rectangle position;


        public void Create(Snake snake, Food food)
        {
            int x;
            int y;
            while (true)
            {
                x = random.Next(1, (ScreenWidth - (poisonedFoodSize * 4)) / poisonedFoodSize) * poisonedFoodSize + poisonedFoodSize*2;
                y = random.Next(1, (ScreenHeight - (poisonedFoodSize * 4)) / poisonedFoodSize) * poisonedFoodSize + poisonedFoodSize * 2;

                if (!snake.GetBody().Any(snakeBody => snakeBody.xPosition == x && snakeBody.yPosition == y) &&
                    food.GetPosition().X != x && food.GetPosition().Y != y
                    && x <= ScreenWidth - 60 || x >= 60 && y <= ScreenHeight - 60 && y >= 60)

                {
                    position = new Rectangle(x, y, poisonedFoodSize, poisonedFoodSize);

                    break;
                }
            }
        }

        public Rectangle GetPosition()
        {
            return position;
        }

        public Rectangle? GetSpritePosition()
        {
            return spritePosition;
        }

        public bool WasEated(Rectangle rectangle)
        {
            return rectangle == GetPosition();
        }
    }
}
