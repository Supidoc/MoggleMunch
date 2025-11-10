namespace GpioHat;

public interface ILed
{
    bool Enabled { get; set; }

    bool Toggle();

    LedColor LedColor { get; }

}