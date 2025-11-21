using Spectre.Console;

namespace MoggleEngine.Interfaces;

/// <summary>
/// Interface for the game-specific GUI used by <see cref="GameLevel"/> implementations. It allows the level to update
/// the displayed viewport Canvas.
/// </summary>
public interface IGameGui : IGui
{
    /// <summary>
    /// Called by the level when the rendered Canvas has been refreshed and should be displayed.
    /// </summary>
    public void UpdateViewport(Canvas canvas);
}