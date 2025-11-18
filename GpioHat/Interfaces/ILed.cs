using GpioHat.Enums;

namespace GpioHat;

public interface ILed
{
    bool Enabled { get; set; }

    LedColor LedColor { get; }

    bool Toggle();
}