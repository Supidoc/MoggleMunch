namespace MoggleMunch;

/// <summary>
/// Interface for a simple scoreboard store used by the menu system.
/// </summary>
public interface IScoreboard
{
    /// <summary>
    /// Record a score for a player.
    /// </summary>
    public void SetScore(string playername, int score);

    /// <summary>
    /// Retrieve the current scoreboard entries as a mapping of player name to score.
    /// </summary>
    public Dictionary<string, int> GetScoreboard();
}