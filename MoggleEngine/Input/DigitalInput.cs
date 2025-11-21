using MoggleEngine.Input.Interfaces;

namespace MoggleEngine.Input;

/// <summary>
/// Represents a digital (boolean) input source that is polled each frame. It translates changes of the underlying
/// logic state into key down/up events and exposes a KeyDown property.
/// </summary>
public class DigitalInput : IDigitalInput, IUpdatableInput
{
    private readonly Func<bool> getState;
    private readonly bool logicZeroIsKeyDown;

    private bool previousLogicState;

    /// <summary>
    /// Initializes a new instance of the <see cref="DigitalInput"/> class.
    /// </summary>
    /// <param name="getState">The function used to get the current state of the input.</param>
    /// <param name="logicZeroIsKeyDown">
    /// A value indicating whether a logic state of zero represents a key down (<c>true</c>) or key up (<c>false</c>).
    /// </param>
    public DigitalInput(Func<bool> getState, bool logicZeroIsKeyDown)
    {
        this.logicZeroIsKeyDown = logicZeroIsKeyDown;
        this.getState = getState;
        this.previousLogicState = getState();
    }

    /// <summary>
    /// Whether the input is currently considered down.
    /// </summary>
    public bool KeyDown { get; private set; }

    /// <summary>
    /// Fired when the input transitions from down to up.
    /// </summary>
    public event EventHandler? KeyUpEvent;

    /// <summary>
    /// Fired when the input transitions from up to down.
    /// </summary>
    public event EventHandler? KeyDownEvent;

    /// <remarks>
    /// This is internal but can't be declared as such because it's implementing an interface method.
    /// </remarks>
    void IUpdatableInput.Update()
    {
        bool state = this.getState();
        if (this.previousLogicState != state)
            if (this.logicZeroIsKeyDown != state)
                OnKeyDown();
            else
                OnKeyUp();

        this.previousLogicState = state;
    }

    private void OnKeyDown()
    {
        this.KeyDown = true;
        KeyDownEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnKeyUp()
    {
        this.KeyDown = false;
        KeyUpEvent?.Invoke(this, EventArgs.Empty);
    }
}