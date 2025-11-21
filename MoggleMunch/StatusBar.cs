using Spectre.Console;
using Spectre.Console.Rendering;

namespace MoggleMunch;

/// <summary>
/// Small status bar (HUD) helper used to display key/value GUI items as a grid.
/// </summary>
public class StatusBar
{
    private Grid grid;

    public Dictionary<string, string> GuiItems = new();

    private Layout layout;

    public StatusBar()
    {
        this.layout = new Layout("root").SplitColumns(new Layout("progress"),
            new Layout("playerinfo"));
        this.grid = new Grid();
    }

    /// <summary>
    /// The Spectre.Console renderable used by the GUI to include the status bar.
    /// </summary>
    public IRenderable Renderable
    {
        get => this.grid;
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
        this.grid = new Grid();

        this.grid.Expand();
        this.grid.AddColumn();
        this.grid.AddColumn();

        foreach (KeyValuePair<string, string> item in this.GuiItems)
            this.grid.AddRow(new Text(item.Key, new Style(Color.Black)).LeftJustified(),
                new Text(item.Value, new Style(Color.Black)).LeftJustified());
    }

    /// <summary>
    /// Optional initialization hook.
    /// </summary>
    public void Init()
    {
    }
}