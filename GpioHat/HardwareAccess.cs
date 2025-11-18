using System.Device.Gpio;
using GpioHat.Enums;

namespace GpioHat;

public enum HardwareAccess
{
    Raspberry, // Zugriff via GPIO vom Raspberry Pi
    TinyK22 // Zugriff via TinyK22 (UART) 
}

public class Raspberry
{
    public static Raspberry Instance { get; } = new Raspberry();

    private Raspberry()
    {
    }
    
    public IJoystick? Joystick { get; private set; }

    public ILed? RedLed { get; private set; }
    public ILed? OrangeLed { get; private set; }
    public ILed? GreenLed { get; private set; }

    public void Init(HardwareAccess hardwareAccess)
    {
        if (hardwareAccess == HardwareAccess.Raspberry)
        {
            GpioController gpio = new();
            this.Joystick = new JoystickDirect(gpio);
            this.RedLed = new LedDirect(LedColor.Red, gpio);
            this.OrangeLed = new LedDirect(LedColor.Orange, gpio);
            this.GreenLed = new LedDirect(LedColor.Green, gpio);
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public bool GpioSupport()
    {
        try
        {
            GpioController gpio = new();
            return true;

        }
        catch (Exception e)
        {
            return false;
        }
    }
}