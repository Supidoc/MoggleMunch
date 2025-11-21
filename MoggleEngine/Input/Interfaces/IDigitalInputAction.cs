namespace MoggleEngine.Input.Interfaces;

/// <summary>
/// Contract for higher-level digital input actions that expose key down state and key up/down events.
/// </summary>
public interface IDigitalInputAction
{
    /// <summary>
    /// Whether the key/action is currently pressed.
    /// </summary>
    public bool KeyDown { get; }
    /// <summary>
    /// Raised when the key/action is released.
    /// </summary>
    event EventHandler? KeyUpEvent;
    /// <summary>
    /// Raised when the key/action is pressed.
    /// </summary>
    event EventHandler? KeyDownEvent;
}