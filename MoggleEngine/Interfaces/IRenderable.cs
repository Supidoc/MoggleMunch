namespace MoggleEngine.Interfaces;

public interface IRenderable
{
    public void Render();
    
    public bool Visible { get; }
    
    public int ZIndex { get; }
}