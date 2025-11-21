namespace MoggleEngine.Interfaces;

/// <summary>
/// Contract for objects that can be rendered by a level. Provides visibility and Z-order plus a Render method.
/// </summary>
public interface IRenderable
{
    /// <summary>
    /// Unique identifier for the renderable.
    /// </summary>
    public Guid Id { get; }
    /// <summary>
    /// Whether the renderable should be considered during level rendering.
    /// </summary>
    public bool Visible { get; }

    /// <summary>
    /// Z-order used when sorting renderables. Lower values are rendered first.
    /// </summary>
    public int ZIndex { get; }
    /// <summary>
    /// Called by the level to render the object into the render engine.
    /// </summary>
    public void Render();
}