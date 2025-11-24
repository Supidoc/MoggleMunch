namespace MoggleMunch.Interfaces;

/// <summary>
/// Interface for a simple scoreboard store used by the menu system.
/// </summary>
public interface IScoreboard
{
    /// <summary>
    /// Record a score for a player.
    /// </summary>
    public void SetScore(int score, DateTime timeStamp);

    /// <summary>
    /// Retrieve the current scoreboard entries as a mapping of player name to score.
    /// </summary>
    public List<ScoreBoardData> GetScoreboard();

    /// <summary>
    /// Gets a list of the top ten scores from the scoreboard.
    /// </summary>
    public List<ScoreBoardData> GetTopTen { get; }
}