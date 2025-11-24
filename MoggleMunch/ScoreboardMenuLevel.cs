using System.Net.Sockets;
using MoggleEngine;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace MoggleMunch;

/// <summary>
/// Menu level that displays the title screen and a scoreboard area.
/// </summary>
public class ScoreboardMenuLevel : MenuLevel
{
    private readonly Layout layout;

    private readonly Rows rows;

    public ScoreboardMenuLevel()
    {
        Markup pressToStart = new("[blue]Press Center or Spacebar to start Game...[/]");
        pressToStart.Centered();
        this.layout = new Layout("root").SplitRows(new Layout("title"),
            new Layout("content"));
        this.layout["title"].Update(
            Align.Center(
                new FigletText("MoggleMunch").Color(Color.Blue),
                VerticalAlignment.Middle)
        );
        this.layout["title"].Size(13);
        this.layout["content"].SplitRows(new Layout("playerinfo"), new Layout("scoreboard"));
        this.layout["content"]["playerinfo"].Size(2);
        this.rows = new Rows(this.layout, pressToStart);
    }

    public override IRenderable UiRenderable
    {
        get => this.rows;
    }

    public override void Update()
    {
        base.Update();
        UpdateScoreboard();
        UpdatePlayerInfo();
    }

    /// <summary>
    /// Update the player info panel.
    /// </summary>
    private void UpdatePlayerInfo()
    {
        Grid grid = new();

        
        string[] firstRow = new[] { "Playername:", ScoreBoard.Instance.PlayerName, "", "Highscore:", ScoreBoard.Instance.PlayerHighscore.ToString() };
        string[] secondRow = new[] { "", "", "", "Last Score:", ScoreBoard.Instance.LastPlayerScore.ToString()  };

        grid.AddColumns(5);
        grid.AddRow(firstRow);
        grid.AddRow(secondRow);
        grid.Expand();
        this.layout["content"]["playerinfo"].Update(Align.Center(grid));
    }

    /// <summary>
    /// Update the scoreboard table
    /// </summary>
    private void UpdateScoreboard()
    {
        Table newScoreboard = new();
        newScoreboard.AddColumn(new TableColumn("Player").Centered());
        newScoreboard.AddColumn(new TableColumn("Score").Centered());
        newScoreboard.AddColumn(new TableColumn("Date").Centered());
        newScoreboard.AddColumn(new TableColumn("Time").Centered());
        newScoreboard.Centered();

        foreach (ScoreBoardData score in ScoreBoard.Instance.GetTopTen)
        {
            newScoreboard.AddRow(new string[]{score.PlayerName, score.Score.ToString(),score.TimeStamp.ToString("yyyy-MM-dd"),
                score.TimeStamp.ToString("HH:mm:ss")});
        }
        this.layout["content"]["scoreboard"].Update(newScoreboard);
    }
}