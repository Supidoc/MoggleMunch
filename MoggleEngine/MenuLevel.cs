using Spectre.Console.Rendering;

namespace MoggleEngine;

/// <summary>
/// Basic level implementation intended for menu screens. Subclasses can override lifecycle methods to provide behaviour.
/// </summary>
public class MenuLevel : Level
{
    /// <summary>
    /// The UI renderable for the menu level. Intended to be overridden or set by subclasses.
    /// </summary>
    public override IRenderable UiRenderable { get; }

    public override void Update()
    {
        base.Update();
    }

    public override void Load()
    {
        base.Load();
    }

    public override void Unload()
    {
        base.Unload();
    }

    public override void Render()
    {
        base.Render();
    }
}