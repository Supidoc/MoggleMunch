using System.Device.Gpio;

namespace GpioHat;

public abstract class Led: ILed
{
    protected int pinNumber = 0;
    protected Led(LedColor color)
    {
        switch (color)
        {
            case LedColor.Green:
                this.pinNumber = 16;
                break;
            case LedColor.Orange:
                this.pinNumber = 20;
                break;
            case LedColor.Red:
                this.pinNumber = 21;
                break;
        }
        
        LedColor = color;
    }
    

    public abstract bool Enabled { get; set; }
    public virtual bool Toggle()
    {
        Enabled = !Enabled;
        return Enabled;
    }

    public virtual LedColor LedColor { get; }
}