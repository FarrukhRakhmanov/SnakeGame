using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SnakeGame.Entity
{
    [Serializable]
    public class GameState
    {
        public DateTime PlayDate { get; set; }
        public int Score { get; set; }
        //public List<GameState> GameStates { get; set; }


        public void SaveGameState(int currentScore)
        {
            GameState gameState = new GameState
            {
                PlayDate = DateTime.Now,
                Score = currentScore
            };

            string jsonString = JsonSerializer.Serialize(gameState);

            // Append the serialized entity as a new line to the file
            File.AppendAllText(GetGameStateFilePath(), jsonString + Environment.NewLine);
        }


        public List<GameState> LoadGameStates()
        {
            List<GameState> GameStates = new List<GameState>();
            string filePath = GetGameStateFilePath();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        GameState gameState = JsonSerializer.Deserialize<GameState>(line);
                        GameStates.Add(gameState);
                    }
                }
            }
            return GameStates;
        }


        public string GetGameStateFilePath()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SnakeGame");
            Directory.CreateDirectory(folderPath); // Ensure the directory exists
            return Path.Combine(folderPath, "game_state.json");
        }
    }
}
