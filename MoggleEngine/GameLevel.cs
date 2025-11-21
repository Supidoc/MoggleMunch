using MoggleEngine.Interfaces;
using IRenderable = Spectre.Console.Rendering.IRenderable;

namespace MoggleEngine;

/// <summary>
/// Base class for game levels that contain and manage GameObject instances.
/// Extends <see cref="Level"/> with object registration and a reference to a game-specific GUI.
/// </summary>
public abstract class GameLevel : Level
{
    protected GameLevel()
    {
        this.GameObjects = new List<GameObject>();
    }

    /// <summary>
    /// Collection of game objects that belong to this level.
    /// </summary>
    public List<GameObject> GameObjects { get; }

    /// <summary>
    /// The renderable UI exposed by the level's game GUI.
    /// </summary>
    public override Spectre.Console.Rendering.IRenderable UiRenderable
    {
        get => this.GameGui.Renderable;
    }

    /// <summary>
    /// The game-specific GUI implementation used to display the viewport and HUD.
    /// </summary>
    public abstract IGameGui GameGui { get; }

    public Guid Id { get; } = Guid.NewGuid();

    public override void Render()
    {
        base.Render();
        GameEngine.Instance.RenderEngine.RenderCanvas();
        this.GameGui.UpdateViewport(GameEngine.Instance.RenderEngine.Canvas);
    }

    /// <summary>
    /// Adds a <see cref="GameObject"/> to the level. If the level is already loaded the object is registered immediately.
    /// </summary>
    public void AddGameObject(GameObject gameObject)
    {
        if (this.Loaded) RegisterGameObject(gameObject);
        this.GameObjects.Add(gameObject);
    }

    /// <summary>
    /// Removes a <see cref="GameObject"/> from the level and unregisters it if loaded.
    /// </summary>
    public void RemoveGameObject(GameObject gameObject)
    {
        if (this.Loaded) UnregisterGameObject(gameObject);

        this.GameObjects.Remove(gameObject);
    }

    /// <summary>
    /// Load the level and register contained game objects.
    /// </summary>
    public override void Load()
    {
        base.Load();

        foreach (GameObject gameObject in this.GameObjects) RegisterGameObject(gameObject);
    }

    /// <summary>
    /// Unload the level and unregister contained game objects.
    /// </summary>
    public override void Unload()
    {
        base.Unload();
        foreach (GameObject gameObject in this.GameObjects) UnregisterGameObject(gameObject);
    }

    private void RegisterGameObject(GameObject gameObject)
    {
        RegisterRenderable(gameObject);
        RegisterUpdatable(gameObject);
    }

    private void UnregisterGameObject(GameObject gameObject)
    {
        UnregisterRenderable(gameObject);
        UnregisterUpdatable(gameObject);
    }
}