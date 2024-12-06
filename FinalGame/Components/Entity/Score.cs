using System;
using System.IO;
using System.Text.Json;

namespace SnakeGame.Components.Entity
{
    public class Score
    {
        private int currentScore;

        public Score()
        {
            currentScore = 0;
        }

        public void SetScore(int score)
        {
            currentScore = score;
        }

        public void IncreaseScore()
        {
            currentScore += 1;
        }

        public void DecreaseScore()
        {
            currentScore -= 1;
        }

        public int GetScore()
        {
            return currentScore;
        }
    }
}
