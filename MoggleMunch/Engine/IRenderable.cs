using Spectre.Console;

namespace MoggleMunch.Engine;

public interface IRenderable
{
    public void Render(RenderEngine renderEngine);
}