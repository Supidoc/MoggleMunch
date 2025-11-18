using System.Device.Gpio;
using GpioHat.Enums;
using NLog;

namespace GpioHat;

public class LedDirect : Led, ILed
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();


    internal GpioController gpioController;

    public LedDirect(LedColor color, GpioController gpioController) : base(color)
    {
        this.gpioController = gpioController;
        gpioController.OpenPin(this.pinNumber, PinMode.Output);
        log.Trace($"Created {color} led");
    }

    public override bool Enabled
    {
        get => this.gpioController.Read(this.pinNumber) == PinValue.High;
        set
        {
            if (this.Enabled != value)
            {
                this.gpioController.Write(this.pinNumber, value);
                string stateString = value ? "on" : "off";
                log.Trace($"Led {this.LedColor} turned {stateString}");
            }
        }
    }
}