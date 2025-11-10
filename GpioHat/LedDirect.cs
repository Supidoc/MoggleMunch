using System.Device.Gpio;

namespace GpioHat;

public class LedDirect : Led, ILed
{
    private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    

    internal GpioController gpioController;

    public LedDirect(LedColor color, GpioController gpioController) : base(color)
    {


        this.gpioController = gpioController;
        gpioController.OpenPin(this.pinNumber, PinMode.Output);
        log.Trace($"Created {color} led");
    }
    
    public override bool Enabled { get => gpioController.Read(this.pinNumber) == PinValue.High;
        set
        {
            if(this.Enabled != value){            
                gpioController.Write(pinNumber, value);
                string stateString = value ? "on" : "off";
                log.Trace($"Led {this.LedColor} turned {stateString}");}
        }
    }
    

}