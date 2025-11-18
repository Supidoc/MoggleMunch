namespace GpioHat.Enums;

[Flags]
public enum JoystickButtons
{
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,
    Center = 16
}