using System.Numerics;
using MoggleMunch.Engine;
using Spectre.Console;

namespace MoggleMunch;

public class TestDrawings : IRenderable, IUpdatable
{
    private Vector2 circlePos = new(10, 10);
    private readonly float velocity = 1;

    public void Render(RenderEngine renderEngine)
    {
        renderEngine.DrawCircle(this.circlePos, 4, Color.Orange1, 3);
        renderEngine.DrawLine(new Vector2(2, 2), new Vector2(2, 20), Color.Aqua);
        renderEngine.DrawLine(new Vector2(2, 2), new Vector2(20, 2), Color.Aqua);
    }

    private bool movingRight = true;
    public void Update(GameEngine gameEngine)
    {
        if (this.circlePos.X > 15) this.movingRight = false;
        else if (this.circlePos.X < 5) this.movingRight = true;
        if (movingRight)
            this.circlePos += new Vector2(gameEngine.DeltaTime * this.velocity);
        else
            this.circlePos -= new Vector2(gameEngine.DeltaTime * this.velocity);
    }
}