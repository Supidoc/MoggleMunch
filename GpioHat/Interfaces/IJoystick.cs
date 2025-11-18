using GpioHat.Enums;

namespace GpioHat;

public interface IJoystick
{
    JoystickButtons State { get; }
}