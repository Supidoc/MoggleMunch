using GpioHat.Enums;

namespace GpioHat;

public abstract class Led : ILed
{
    protected int pinNumber;

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

        this.LedColor = color;
    }


    public abstract bool Enabled { get; set; }

    public virtual bool Toggle()
    {
        this.Enabled = !this.Enabled;
        return this.Enabled;
    }

    public virtual LedColor LedColor { get; }
}