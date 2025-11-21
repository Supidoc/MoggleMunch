namespace MoggleEngine.Input.Interfaces;

/// <summary>
/// Low-level polled digital input contract. Implementations should fire events when the input transitions.
/// </summary>
public interface IDigitalInput
{
    /// <summary>
    /// Whether the input is currently in the down state.
    /// </summary>
    public bool KeyDown { get; }

    /// <summary>
    /// Raised when the input transitions from down to up.
    /// </summary>
    public event EventHandler KeyUpEvent;
    /// <summary>
    /// Raised when the input transitions from up to down.
    /// </summary>
    public event EventHandler KeyDownEvent;
}