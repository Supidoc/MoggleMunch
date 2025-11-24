using MoggleEngine;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace MoggleMunch;

/// <summary>
/// Small status bar (HUD) helper used to display key/value GUI items as a grid.
/// </summary>
public class StatusBar
{
    public float Progress = 0.0f;

    public int Score = 0;

    
    private Grid grid;

    public Dictionary<string, string> GuiItems = new();

    private Layout layout;

    public StatusBar()
    {
        this.layout = new Layout("root").SplitColumns(new Layout("left"),
            new Layout("right"));
        this.layout["right"].SplitColumns(new Layout("score"),new Layout("playerinfo"));
        this.grid = new Grid();
    }

    /// <summary>
    /// The Spectre.Console renderable used by the GUI to include the status bar.
    /// </summary>
    public IRenderable Renderable
    {
        get => this.layout;
    }

    /// <summary>
    /// Unique identifier for the status bar instance.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Builds the internal grid from GuiItems so it can be rendered by Spectre.Console.
    /// </summary>
    public void Render()
    {
        GameEngine.Instance.RenderEngine.distanceFromEdgeTop = Math.Max(this.GuiItems.Count + 4, 5);
        GameEngine.Instance.RenderEngine.distanceFromEdgeLeft = 5;
        
        this.grid = new Grid();

        this.grid.Expand();
        this.grid.AddColumn();
        this.grid.AddColumn();

        foreach (KeyValuePair<string, string> item in this.GuiItems)
            this.grid.AddRow(new Text(item.Key, new Style(Color.NavajoWhite1)).RightJustified(),
                new Text(item.Value, new Style(Color.LightCyan1 )).RightJustified());
        
        this.layout["right"]["playerinfo"].Update(Align.Right(this.grid));
        this.layout["right"]["score"].Update(
            Align.Right(
                new Markup($"[yellow]Score:[/] [green]{this.Score}[/]").RightJustified()));
        this.layout["left"].Update(
            new BreakdownChart().Expand().AddItem(string.Empty, this.Progress, Color.Green1)
                .AddItem(string.Empty, 100.0f - this.Progress, Color.Gray)
                .HideTags());
    }   

    
    
    /// <summary>
    /// Optional initialization hook.
    /// </summary>
    public void Init()
    {
    }
}