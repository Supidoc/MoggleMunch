using MoggleEngine.Input;
using GpioHat;
using GpioHat.Enums;

namespace MoggleMunch.Inputs;

public sealed class CenterInput : DigitalInputAction
{
    private static CenterInput? instance;

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
    private bool keyPressed = false;

    private void KeyUpEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.Spacebar)
        {
            this.keyPressed = false;
        }
    }
    
    private void KeyDownEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.Spacebar)
        {
            this.keyPressed = true;
        }
    }
    
}