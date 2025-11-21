using MoggleEngine.Interfaces;

namespace MoggleEngine;

/// <summary>
/// Generic level container that manages renderables and updatables and provides a lifecycle (Load/Unload/Update/Render).
/// </summary>
public abstract class Level : IUpdatable
{
    private readonly List<IRenderable> renderables = new();

    private readonly List<IUpdatable> updatables = new();
    /// <summary>
    /// Whether the level has been loaded (Load called and resources registered).
    /// </summary>
    public bool Loaded { get; private set; }

    /// <summary>
    /// The UI renderable representing this level (panels, viewport, HUD etc.). Implementations must supply a renderable.
    /// </summary>
    public abstract Spectre.Console.Rendering.IRenderable UiRenderable { get; }

    /// <summary>
    /// Unique identifier for the level instance.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Calls Update on all registered IUpdatable instances. Implementations can override to extend behaviour.
    /// </summary>
    public virtual void Update()
    {
        foreach (IUpdatable updatable in this.updatables.ToList()) updatable.Update();
    }

    /// <summary>
    /// Marks the level as loaded;
    /// </summary>
    public virtual void Load()
    {
        this.Loaded = true;
    }

    /// <summary>
    /// Marks the level as unloaded;
    /// </summary>
    public virtual void Unload()
    {
        this.Loaded = false;
    }

    /// <summary>
    /// Renders all registered <see cref="IRenderable"/> objects that are visible in ascending Z-index order.
    /// </summary>
    public virtual void Render()
    {
        foreach (IRenderable r in this.renderables.Where(r => r.Visible).OrderBy(r => r.ZIndex).ToList()) r.Render();
    }

    /// <summary>
    /// Register an <see cref="IUpdatable"/> to be updated each update cycle.
    /// </summary>
    public void RegisterUpdatable(IUpdatable updatable)
    {
        this.updatables.Add(updatable);
    }

    /// <summary>
    /// Register an <see cref="IRenderable"/> to be rendered each frame.
    /// </summary>
    public void RegisterRenderable(IRenderable renderable)
    {
        this.renderables.Add(renderable);
    }

    /// <summary>
    /// Unregister an <see cref="IUpdatable"/> so it is no longer updated.
    /// </summary>
    public void UnregisterUpdatable(IUpdatable updatable)
    {
        this.updatables.Remove(updatable);
    }

    /// <summary>
    /// Unregister an <see cref="IRenderable"/> so it is no longer rendered.
    /// </summary>
    public void UnregisterRenderable(IRenderable renderable)
    {
        this.renderables.Remove(renderable);
    }
}