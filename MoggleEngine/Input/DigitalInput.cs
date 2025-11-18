using MoggleEngine.Input.Interfaces;

namespace MoggleEngine.Input;

public class DigitalInput : IDigitalInput, IUpdatableInput
{
    private readonly Func<bool> getState;
    private readonly bool logicZeroIsKeyDown;

    private bool previousLogicState;

    public DigitalInput(Func<bool> getState, bool logicZeroIsKeyDown)
    {
        this.logicZeroIsKeyDown = logicZeroIsKeyDown;
        this.getState = getState;
        this.previousLogicState = getState();
    }

    public bool KeyDown { get; private set; }

    public event EventHandler? KeyUpEvent;

    public event EventHandler? KeyDownEvent;

    // This is internal
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