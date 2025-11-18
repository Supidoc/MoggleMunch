using MoggleEngine.Input;
using GpioHat;
using GpioHat.Enums;

namespace MoggleMunch.Inputs;

public sealed class LeftInput : DigitalInputAction
{
    private static LeftInput? instance;

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
    
    private bool keyPressed = false;

    private void KeyUpEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.LeftArrow)
        {
            this.keyPressed = false;
        }
    }

    private int test = 0;
    private void KeyDownEventHandler(object? sender, ConsoleKey consoleKey)
    {

        if (consoleKey == ConsoleKey.LeftArrow)
        {
            this.test++;
            this.keyPressed = true;
        }
    }
}