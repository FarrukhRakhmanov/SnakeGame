using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace SnakeGame.Entity
{
    public class Food
    {
        public static int ScreenWidth = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.9);
        public static int ScreenHeight = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.9);
        Random random = new Random();
        Rectangle spritePosition = new Rectangle(0, 0, 60, 60);
        Rectangle position;
        public int foodSize = 60;
        public void Create(Snake snake, Food poisonedFood, int distanceFromTheScreenEdge)
        {
            int x;
            int y;
            while (true)
            {
                x = random.Next(1, (ScreenWidth - (foodSize * distanceFromTheScreenEdge)) / foodSize) * foodSize + foodSize * (distanceFromTheScreenEdge/2);
                y = random.Next(1, (ScreenHeight - (foodSize * distanceFromTheScreenEdge)) / foodSize) * foodSize + foodSize * (distanceFromTheScreenEdge/2);

                if (!snake.GetBody().Any(snakeBody => snakeBody.xPosition == x && snakeBody.yPosition == y) &&
                    poisonedFood.GetPosition().X != x && poisonedFood.GetPosition().Y != y
                    && x <= ScreenWidth-60 || x >= 60 && y <= ScreenHeight-60 && y >= 60)
                {
                    position = new Rectangle(x, y, foodSize, foodSize);
                    break;
                }
            }
        }

        public void Create(Snake snake, Food poisonedFood, Obstacle obstacle, int distanceFromTheScreenEdge)
        {
            int x;
            int y;
            while (true)
            {
                x = random.Next(1, (ScreenWidth - (foodSize * distanceFromTheScreenEdge)) / foodSize) * foodSize + foodSize * (distanceFromTheScreenEdge / 2);
                y = random.Next(1, (ScreenHeight - (foodSize * distanceFromTheScreenEdge)) / foodSize) * foodSize + foodSize * (distanceFromTheScreenEdge / 2);

                if (!snake.GetBody().Any(snakeBody => snakeBody.xPosition == x && snakeBody.yPosition == y) &&
                    poisonedFood.GetPosition().X != x && poisonedFood.GetPosition().Y != y
                    && x <= ScreenWidth - 60 || x >= 60 && y <= ScreenHeight - 60 && y >= 60
                    && obstacle.Position.X != x && obstacle.Position.Y != y)
                {
                    position = new Rectangle(x, y, foodSize, foodSize);
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
