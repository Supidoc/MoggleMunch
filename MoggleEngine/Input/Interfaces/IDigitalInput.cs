namespace MoggleEngine.Input.Interfaces;

public interface IDigitalInput
{
    public bool KeyDown { get; }

    public event EventHandler KeyUpEvent;
    public event EventHandler KeyDownEvent;
}