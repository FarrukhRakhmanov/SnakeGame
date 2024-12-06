using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Components.Entity
{
    public class Snake
    {
        public static int ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        private int bodyLength;
        public List<SnakeBody> snakeBody;
        private Direction direction;

        public Snake()
        {
            bodyLength = 60;
            snakeBody = new List<SnakeBody>();
            direction = Direction.RIGHT;
        }
        public void Create()
        {
            //Snake head
            snakeBody.Add(new SnakeBody(180, 420));


            //Snake body
            for (int i = 0; i < 3; i++)
            {
                snakeBody.Add(new SnakeBody(snakeBody[i].xPosition - bodyLength, snakeBody[i].yPosition));
            }

        }

        public List<SnakeBody> GetBody()
        {
            return snakeBody;
        }

        public void Move()
        {
            snakeBody[0].previousXposition = snakeBody[0].xPosition;
            snakeBody[0].previousYposition = snakeBody[0].yPosition;
            snakeBody[0].direction = direction; // Update the direction of the head

            if (direction is Direction.RIGHT)
            {
                snakeBody[0].xPosition += bodyLength;
            }
            else if (direction is Direction.LEFT)
            {
                snakeBody[0].xPosition -= bodyLength;
            }
            else if (direction is Direction.UP)
            {
                snakeBody[0].yPosition -= bodyLength;
            }
            else if (direction is Direction.DOWN)
            {
                snakeBody[0].yPosition += bodyLength;
            }

            for (int i = 1; i < snakeBody.Count; i++)
            {
                snakeBody[i].previousXposition = snakeBody[i].xPosition;
                snakeBody[i].previousYposition = snakeBody[i].yPosition;
                snakeBody[i].xPosition = snakeBody[i - 1].previousXposition;
                snakeBody[i].yPosition = snakeBody[i - 1].previousYposition;
                snakeBody[i].direction = snakeBody[i - 1].direction; // Update the direction of the body segment
            }
        }


        public void SetDirection(Direction direction)
        {
            this.direction = direction;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Rectangle GetHead()
        {
            return new Rectangle(snakeBody[0].xPosition, snakeBody[0].yPosition, 60, 60);
        }

        public void Grow()
        {
            for (int i = 0; i < 2; i++)
            {
                var tail = snakeBody.Last();
                snakeBody.Add(new SnakeBody(tail.xPosition, tail.yPosition));
            }
        }

        public bool Collided()
        {
            Rectangle head = GetHead();

            // Check for wall collision
            if (IsWallCollision(head))
                return true;

            // Check for self-collision
            if (IsSelfCollision())
                return true;

            return false;
        }

        public bool Collided(params List<Obstacle>[] obstacles)
        {
            Rectangle head = GetHead();

            // Check for wall collision
            if (IsWallCollision(head))
                return true;

            // Check for obstacle collision
            foreach (var obstacleList in obstacles)
            {
                foreach (var obstacle in obstacleList)
                {
                    if (obstacle != null && obstacle.Position.Intersects(head))
                        return true;
                }
            }

            // Check for self-collision
            if (IsSelfCollision())
                return true;

            return false;
        }

        private bool IsWallCollision(Rectangle head)
        {
            return head.Left < 60 ||
                   head.Right > ScreenWidth * 0.9 - 60 ||
                   head.Top < 60 ||
                   head.Bottom > ScreenHeight * 0.9 - 60;
        }

        private bool IsSelfCollision()
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                if (snakeBody[0].xPosition == snakeBody[i].xPosition &&
                    snakeBody[0].yPosition == snakeBody[i].yPosition)
                    return true;
            }
            return false;
        }
    }
}
