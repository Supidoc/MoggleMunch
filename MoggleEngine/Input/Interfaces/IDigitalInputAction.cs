namespace MoggleEngine.Input.Interfaces;

public interface IDigitalInputAction
{
    event EventHandler? KeyUpEvent;
    event EventHandler? KeyDownEvent;
    public bool KeyDown { get; }
}