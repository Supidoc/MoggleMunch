namespace MoggleEngine.Interfaces;

/// <summary>
/// Interface for objects that are updated every update tick. Implementations should perform their per-frame logic
/// in <see cref="Update"/>. Also requires a stable <see cref="Id"/> for registration and tracking.
/// </summary>
public interface IUpdatable
{
    /// <summary>
    /// Unique identifier for the updatable instance.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Called by the level each update cycle.
    /// </summary>
    public void Update();
}