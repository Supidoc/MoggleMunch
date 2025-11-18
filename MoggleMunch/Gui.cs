using MoggleEngine;
using MoggleEngine.Interfaces;
using Spectre.Console;
using IRenderable = Spectre.Console.Rendering.IRenderable;

namespace MoggleMunch;

public class Gui: IUpdatable
{
    public static Gui Instance = new Gui();

    private Gui()
    {
        this.grid = new Grid();
    }

    private Grid grid;

    public Dictionary<string, string> GuiItems = new Dictionary<string, string>();

    public void Init()
    {

    }

    public void Update()
    {
        this.grid = new Grid();
        
        this.grid.Expand();
        this.grid.AddColumn();
        this.grid.AddColumn();

        foreach (var item in this.GuiItems)
        {
            this.grid.AddRow(new IRenderable[]
            {
                new Text(item.Key, new Style(Color.Black)).LeftJustified(),
                new Text(item.Value, new Style(Color.Black)).LeftJustified()
            });
        }

        GameEngine.Instance.Gui.UpdateGui(this.grid);
    }
    
}