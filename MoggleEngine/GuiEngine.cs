using Spectre.Console;
using Spectre.Console.Rendering;

namespace MoggleEngine;

public class GuiEngine
{

    internal static GuiEngine Instance { get; } = new GuiEngine();
    public Layout Layout { get; private set; }

    private GuiEngine()
    {
        this.Layout = new Layout("root").SplitRows(new Layout("gui"),
            new Layout("viewport"));
        this.Layout["gui"].Update(
            new Panel(
                    Align.Center(
                        new Markup("Hello [blue]World![/]"),
                        VerticalAlignment.Middle))
                .Expand());

    }

    public void SetViewportHeight(int viewportHeight)
    {
        this.Layout["gui"].Size(4);
        this.Layout["viewport"].Size(viewportHeight);
    }

    public void UpdateViewport(Canvas canvas)
    {
        Panel panel = new Panel(canvas);
        panel.Expand();
        this.Layout["viewport"].Update(panel);
        
    }

    public void UpdateGui(IRenderable renderable)
    {
        this.Layout["gui"].Update(new Panel(renderable).Expand());
    }
    
}