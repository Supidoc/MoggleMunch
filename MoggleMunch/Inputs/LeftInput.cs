using GpioHat;
using GpioHat.Enums;
using MoggleEngine.Input;

namespace MoggleMunch.Inputs;

/// <summary>
/// Singleton that aggregates joystick and keyboard sources for the 'Left' action.
/// </summary>
public sealed class LeftInput : DigitalInputAction
{
    private static LeftInput? instance;

    private bool keyPressed;

    private int test;

    private LeftInput()
    {
        ConsoleInputHandler.KeyUpEvent += KeyUpEventHandler;
        ConsoleInputHandler.KeyDownEvent += KeyDownEventHandler;

        DigitalInput gpio = new(() => Raspberry.Instance.Joystick != null &&
                                      (Raspberry.Instance.Joystick.State & JoystickButtons.Left) ==
                                      JoystickButtons.Left,
            false);
        DigitalInput keyboard = new(() => this.keyPressed,
            false);
        RegisterInput(gpio);
        RegisterInput(keyboard);
    }

    public static LeftInput Instance
    {
        get
        {
            instance ??= new LeftInput();
            return instance;
        }
    }

    private void KeyUpEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.LeftArrow) this.keyPressed = false;
    }

    private void KeyDownEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.LeftArrow)
        {
            this.test++;
            this.keyPressed = true;
        }
    }
}