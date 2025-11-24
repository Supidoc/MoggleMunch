using MoggleMunch.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoggleMunch
{
    public class ScoreBoard : IScoreboard
    {

        public readonly string path;
        public readonly string fileName = "ScoreBoard.json"; // is there a better way to do this?

        public string PlayerName { get; set; } = string.Empty;

        public static ScoreBoard Instance { get; } = new ScoreBoard(Directory.GetCurrentDirectory());

        private ScoreBoard(string path)
        {
            this.path = path + fileName;
        }

        /// <summary>
        /// Retrieves the scoreboard data from a JSON file.
        /// </summary>
        /// <remarks>If the file specified by the path does not exist, an empty list is returned.  If the
        /// file exists but contains invalid or empty JSON, an empty list is also returned.</remarks>
        /// <returns>A list of <see cref="ScoreBoardData"/> objects representing the scoreboard data.  Returns an empty list if
        /// the file does not exist or if the JSON content is invalid or empty.</returns>
        public List<ScoreBoardData> GetScoreboard()
        {
            if (!File.Exists(path))
                return new List<ScoreBoardData>();

            string jsonString = File.ReadAllText(path);
            List<ScoreBoardData>? scoreBoard = JsonSerializer.Deserialize<List<ScoreBoardData>>(jsonString);
            return scoreBoard ?? new List<ScoreBoardData>();

        }

        /// <summary>
        /// Gets the top ten entries from the scoreboard.
        /// </summary>
        public List<ScoreBoardData> GetTopTen
        {
            get
            {
                List<ScoreBoardData> scoreBoard = GetScoreboard();
                int count = scoreBoard.Count < 10 ? scoreBoard.Count : 10;
                List<ScoreBoardData> topTen = scoreBoard.GetRange(0, count);
                return topTen;
            }
        }


        /// <summary>
        /// Adds a player's score to the scoreboard, sorts the scoreboard in descending order, and saves it to a file.
        /// </summary>
        /// <remarks>The method retrieves the current scoreboard, adds the new score, sorts the
        /// scoreboard, and saves the updated scoreboard to a file in JSON format. The file is overwritten if it already
        /// exists.</remarks>
        /// <param name="score">The score achieved by the player. Must be a non-negative integer.</param>
        /// <param name="timeStamp">The timestamp of the score, represented as a non-negative integer.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="PlayerName"/> is <see langword="null"/> or empty, if <paramref name="score"/> is
        /// negative, or if <paramref name="timeStamp"/> is negative.</exception>
        public void SetScore(int score, DateTime timeStamp)
        {
            if (string.IsNullOrEmpty(PlayerName))
            {
                throw new ArgumentException("Name required");
            }

            if (score < 0)
            {
                throw new ArgumentException("invalid negativ score");
            }

            List<ScoreBoardData> scoreBoard = GetScoreboard();
            scoreBoard.Add(new ScoreBoardData { PlayerName = PlayerName, Score = score, TimeStamp = timeStamp });
            scoreBoard.Sort();
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(scoreBoard, options);
            File.WriteAllText(path, jsonString);
        }

        public int PlayerHighscore
        {
            get
            {
                List<ScoreBoardData> scoreBoard = GetScoreboard();
                var playerScores = scoreBoard.Where(s => s.PlayerName == PlayerName);
                List<ScoreBoardData> scoreBoardDatas = playerScores.ToList();
                if (scoreBoardDatas.Any())
                {
                    return scoreBoardDatas.Max(s => s.Score);
                }
                else
                {
                    return 0;
                }
            }
        }
        
        public int LastPlayerScore
        {
            get
            {
                List<ScoreBoardData> scoreBoard = GetScoreboard();
                var playerScores = scoreBoard.Where(s => s.PlayerName == PlayerName).OrderBy(s => s.TimeStamp);
                List<ScoreBoardData> scoreBoardDatas = playerScores.ToList();
                if (scoreBoardDatas.Any())
                {
                    return scoreBoardDatas.Last().Score;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
