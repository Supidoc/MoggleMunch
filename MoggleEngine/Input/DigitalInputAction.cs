using MoggleEngine.Input.Interfaces;

namespace MoggleEngine.Input;

public class DigitalInputAction : IDigitalInputAction
{
    protected void RegisterInput(DigitalInput digitalInput)
    {
        InputHandler.Instance.RegisterInput(digitalInput);
        digitalInput.KeyDownEvent += OnKeyDown;
        digitalInput.KeyUpEvent += OnKeyUp;
    }

    public event EventHandler? KeyUpEvent;

    public event EventHandler? KeyDownEvent;
    public bool KeyDown { get; private set; }

    private int test = 0;
    
    private void OnKeyDown(object? sender, EventArgs eventArgs)
    {
        this.KeyDown = true;
        KeyDownEvent?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnKeyUp(object? sender, EventArgs eventArgs)
    {
        this.KeyDown = false;
        KeyUpEvent?.Invoke(this, EventArgs.Empty);
    }
}