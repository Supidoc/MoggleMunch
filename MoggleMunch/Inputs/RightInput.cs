using GpioHat;
using GpioHat.Enums;
using MoggleEngine.Input;

namespace MoggleMunch.Inputs;

/// <summary>
/// Singleton that aggregates joystick and keyboard sources for the 'Right' action.
/// </summary>
public sealed class RightInput : DigitalInputAction
{
    private static RightInput? instance;

    private bool keyPressed;

    private RightInput()
    {
        ConsoleInputHandler.KeyUpEvent += KeyUpEventHandler;
        ConsoleInputHandler.KeyDownEvent += KeyDownEventHandler;

        DigitalInput gpio = new(() => Raspberry.Instance.Joystick != null &&
                                      (Raspberry.Instance.Joystick.State & JoystickButtons.Right) ==
                                      JoystickButtons.Right,
            false);
        DigitalInput keyboard = new(() => this.keyPressed,
            false);
        RegisterInput(gpio);
        RegisterInput(keyboard);
    }

    public static RightInput Instance
    {
        get
        {
            instance ??= new RightInput();
            return instance;
        }
    }

    private void KeyUpEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.RightArrow) this.keyPressed = false;
    }

    private void KeyDownEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.RightArrow) this.keyPressed = true;
    }
}