namespace SnakeGame.Entity
{
    public class Score
    {
        private int currentScore;
        public Score()
        {
            currentScore = 0;
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
