using System.Numerics;
using MoggleEngine;
using Spectre.Console;

namespace MoggleMunch;

/// <summary>
/// Helper object that draws example lines and circles into the world for testing rendering.
/// </summary>
public class TestDrawings : GameObject
{
    private readonly Vector2 circlePos = new(10, 10);

    public TestDrawings()
    {
        this.Visible = true;
        this.ZIndex = 0;
    }

    public override void Render()
    {
        GameEngine.Instance.RenderEngine.DrawCircle(this.circlePos, 4, Color.Orange1, 3);
        GameEngine.Instance.RenderEngine.DrawLine(new Vector2(2, 2), new Vector2(2, 20), Color.Aqua);
        GameEngine.Instance.RenderEngine.DrawLine(new Vector2(2, 2), new Vector2(20, 2), Color.Aqua);
    }

    public override void Update()
    {
    }
}