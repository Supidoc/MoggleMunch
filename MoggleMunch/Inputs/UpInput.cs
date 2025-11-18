using System.Runtime.CompilerServices;
using MoggleEngine.Input;
using GpioHat;
using GpioHat.Enums;
using Spectre.Console;

namespace MoggleMunch.Inputs;

public sealed class UpInput : DigitalInputAction
{
    private static UpInput? instance;

    private UpInput()
    {
        ConsoleInputHandler.KeyUpEvent += KeyUpEventHandler;
        ConsoleInputHandler.KeyDownEvent += KeyDownEventHandler;

        
        DigitalInput gpio = new(() => Raspberry.Instance.Joystick != null &&
                                        (Raspberry.Instance.Joystick.State & JoystickButtons.Up) == JoystickButtons.Up,
            false);
        DigitalInput keyboard = new(() => this.keyPressed,
            false);
        
        RegisterInput(gpio);
        RegisterInput(keyboard);
    }

    
    public static UpInput Instance
    {
        get
        {
            
            instance ??= new UpInput();
            return instance;
        }
    }

    private bool keyPressed = false;

    private void KeyUpEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.UpArrow)
        {
            this.keyPressed = false;
        }
    }
    
    private void KeyDownEventHandler(object? sender, ConsoleKey consoleKey)
    {
        if (consoleKey == ConsoleKey.UpArrow)
        {
            this.keyPressed = true;
        }
    }
}