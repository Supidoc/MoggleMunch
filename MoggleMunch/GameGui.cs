using MoggleEngine.Interfaces;
using Spectre.Console;
using IRenderable = Spectre.Console.Rendering.IRenderable;

namespace MoggleMunch;

/// <summary>
/// Game-specific GUI implementation that composes Spectre.Console Layout for HUD and viewport used by the game.
/// </summary>
public class GameGui : IGameGui
{
    private readonly Layout layout;

    public GameGui()
    {
        this.layout = new Layout("root").SplitRows(new Layout("gui"),
            new Layout("viewport"));
        this.layout["gui"].Update(
            new Panel(
                    Align.Center(
                        new Markup("Hello [blue]World![/]"),
                        VerticalAlignment.Middle))
                .Expand());
    }

    /// <summary>
    /// Update the viewport content with the rendered Canvas.
    /// </summary>
    public void UpdateViewport(Canvas canvas)
    {
        Panel panel = new(canvas);
        panel.Expand();
        this.layout["viewport"].Update(panel);
    }

    /// <summary>
    /// The IRenderable representing the whole GUI layout (used by levels when displaying).
    /// </summary>
    public IRenderable Renderable
    {
        get => this.layout;
    }

    /// <summary>
    /// Set the desired viewport height in rows.
    /// </summary>
    public void SetViewportHeight(int viewportHeight)
    {
        this.layout["viewport"].Size(viewportHeight);
    }
    
    public void SetGuiHeight(int guiHeight)
    {
        this.layout["gui"].Size(guiHeight);
    }

    /// <summary>
    /// Update the upper GUI panel with a new renderable (HUD, status, etc.).
    /// </summary>
    public void UpdateGui(IRenderable renderable)
    {
        this.layout["gui"].Update(new Panel(renderable).Expand());
    }
}