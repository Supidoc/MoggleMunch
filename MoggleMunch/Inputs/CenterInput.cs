using GpioHat;
using GpioHat.Enums;
using MoggleEngine.Input;

namespace MoggleMunch.Inputs;

/// <summary>
/// Singleton that aggregates joystick and keyboard sources for the 'Center' (start/confirm) action.
/// </summary>
public sealed class CenterInput : DigitalInputAction
{
    private static CenterInput? instance;
    private bool keyPressed;

    private CenterInput()
    {
        ConsoleInputHandler.KeyUpEvent += KeyUpEventHandler;
        ConsoleInputHandler.KeyDownEvent += KeyDownEventHandler;

        DigitalInput gpioCenter = new(() => Raspberry.Instance.Joystick != null &&
                                            (Raspberry.Instance.Joystick.State & JoystickButtons.Center) ==
                                            JoystickButtons.Center,
            false);
        DigitalInput keyboard = new(() => this.keyPressed,
            false);
        RegisterInput(gpioCenter);
        RegisterInput(keyboard);
    }

    public static CenterInput Instance
    {
        get
        {
            instance ??= new CenterInput();
            return instance;
        }
    }

    private void KeyUpEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.Spacebar) this.keyPressed = false;
    }

    private void KeyDownEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.Spacebar) this.keyPressed = true;
    }
}