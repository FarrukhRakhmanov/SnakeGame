using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Entity
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
            snakeBody.Add(new SnakeBody(300, 180));


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
            var tail = snakeBody.Last();
            snakeBody.Add(new SnakeBody(tail.xPosition, tail.yPosition));
        }

        public bool Collided()
        {

            Rectangle head = GetHead();

            // Check for wall collision
            bool wallCollision = head.Left < 60 ||
                                 head.Right > (ScreenWidth * 0.9) - 60 ||
                                 head.Top < 60 ||
                                 head.Bottom > (ScreenHeight * 0.9) - 60;


            if (wallCollision)
            {
                return true;
            }

            // Check for self-collision
            for (int i = 1; i < snakeBody.Count; i++)
            {
                bool selfCollision = snakeBody[0].xPosition == snakeBody[i].xPosition &&
                                     snakeBody[0].yPosition == snakeBody[i].yPosition;
                if (selfCollision)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Collided(Obstacle obstacle)
        {

            Rectangle head = GetHead();

            // Check for wall collision
            bool wallCollision = head.Left < 60 ||
                                 head.Right > (ScreenWidth*0.9) - 60 ||
                                 head.Top < 60 ||
                                 head.Bottom > (ScreenHeight*0.9) - 60;

            // Check for obstacle collision
            bool obstacleCollision = obstacle.Position.Intersects(head);

            if (wallCollision || obstacleCollision)
            {
                return true;
            }

            // Check for self-collision
            for (int i = 1; i < snakeBody.Count; i++)
            {
                bool selfCollision = snakeBody[0].xPosition == snakeBody[i].xPosition &&
                                     snakeBody[0].yPosition == snakeBody[i].yPosition;
                if (selfCollision)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
