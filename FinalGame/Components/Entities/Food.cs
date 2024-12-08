using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Components.Entities
{
    public class Food
    {
        public static int ScreenWidth = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.9);
        public static int ScreenHeight = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.9);
        Random random = new Random();
        Rectangle spritePosition = new Rectangle(0, 0, 60, 60);
        Rectangle position;
        public int foodSize = 60;
        public void Create(Snake snake, Food poisonedFood, int distanceFromTheScreenEdge, params List<Obstacle>[] obstacles)
        {
            int x;
            int y;
            int offset = foodSize * (distanceFromTheScreenEdge / 2);
            int maxX = (ScreenWidth - foodSize * distanceFromTheScreenEdge) / foodSize;
            int maxY = (ScreenHeight - foodSize * distanceFromTheScreenEdge) / foodSize;

            while (true)
            {
                x = random.Next(1, maxX) * foodSize + offset;
                y = random.Next(1, maxY) * foodSize + offset;

                if (IsValidFoodPosition(snake, poisonedFood, x, y, obstacles))
                {
                    position = new Rectangle(x, y, foodSize, foodSize);
                    break;
                }
            }
        }

        private bool IsValidFoodPosition(Snake snake, Food poisonedFood, int x, int y, params List<Obstacle>[] obstacles)
        {
            bool isInSnake = snake.GetBody().Any(snakeBody => snakeBody.xPosition == x && snakeBody.yPosition == y);
            bool isPoisonedFood = poisonedFood.GetPosition().X == x && poisonedFood.GetPosition().Y == y;
            bool isWithinBounds = x >= 60 && x <= ScreenWidth - 60 && y >= 60 && y <= ScreenHeight - 60;

            bool collidesWithObstacle = obstacles.Any(obstacleList => obstacleList.Any(obstacle => obstacle.Position.X == x && obstacle.Position.Y == y));

            return !isInSnake && !isPoisonedFood && isWithinBounds && !collidesWithObstacle;
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
