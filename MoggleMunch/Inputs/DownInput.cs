using GpioHat;
using GpioHat.Enums;
using MoggleEngine.Input;

namespace MoggleMunch.Inputs;

/// <summary>
/// Singleton that aggregates joystick and keyboard sources for the 'Down' action.
/// </summary>
public sealed class DownInput : DigitalInputAction
{
    private static DownInput? instance;

    private bool keyPressed;

    private DownInput()
    {
        ConsoleInputHandler.KeyUpEvent += KeyUpEventHandler;
        ConsoleInputHandler.KeyDownEvent += KeyDownEventHandler;

        DigitalInput gpio = new(() => Raspberry.Instance.Joystick != null &&
                                      (Raspberry.Instance.Joystick.State & JoystickButtons.Down) ==
                                      JoystickButtons.Down,
            false);
        DigitalInput keyboard = new(() => this.keyPressed,
            false);
        RegisterInput(gpio);
        RegisterInput(keyboard);
    }

    public static DownInput Instance
    {
        get
        {
            instance ??= new DownInput();
            return instance;
        }
    }

    private void KeyUpEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.DownArrow) this.keyPressed = false;
    }

    private void KeyDownEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.DownArrow) this.keyPressed = true;
    }
}