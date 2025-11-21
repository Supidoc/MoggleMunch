using MoggleEngine.Input.Interfaces;

namespace MoggleEngine.Input;

/// <summary>
/// Base class that maps one or multiple low-level digital input signals into a single high-level input action with KeyUp and KeyDown events and exposes a simple KeyDown state.
/// Subclasses can register specific DigitalInput instances to be mapped using <see cref="RegisterInput"/>.
/// </summary>
public class DigitalInputAction : IDigitalInputAction
{
    private int test = 0;

    /// <summary>
    /// Raised when the associated input is released.
    /// </summary>
    public event EventHandler? KeyUpEvent;

    /// <summary>
    /// Raised when the associated input is pressed.
    /// </summary>
    public event EventHandler? KeyDownEvent;

    /// <summary>
    /// Whether the associated key/button is currently considered down.
    /// </summary>
    public bool KeyDown { get; private set; }

    /// <summary>
    /// Registers the input to receive events for the specified DigitalInput.
    /// </summary>
    protected void RegisterInput(DigitalInput digitalInput)
    {
        InputHandler.Instance.RegisterInput(digitalInput);
        digitalInput.KeyDownEvent += OnKeyDown;
        digitalInput.KeyUpEvent += OnKeyUp;
    }

    /// <summary>
    /// Called when the input goes into the down state. Raises the <see cref="KeyDownEvent"/>.
    /// </summary>
    private void OnKeyDown(object? sender, EventArgs eventArgs)
    {
        this.KeyDown = true;
        KeyDownEvent?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Called when the input goes into the up state. Raises the <see cref="KeyUpEvent"/>. Can be overridden by subclasses.
    /// </summary>
    protected virtual void OnKeyUp(object? sender, EventArgs eventArgs)
    {
        this.KeyDown = false;
        KeyUpEvent?.Invoke(this, EventArgs.Empty);
    }
}