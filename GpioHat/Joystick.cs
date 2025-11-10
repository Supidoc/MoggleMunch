namespace GpioHat;

public abstract class Joystick : IJoystick
{
    public virtual JoystickButtons State
    {
        get
        {
            JoystickButtons buttonState = JoystickButtons.None;
            buttonState |= (this.UpState ? JoystickButtons.Up : JoystickButtons.None);
            buttonState |= (this.DownState ? JoystickButtons.Down : JoystickButtons.None);
            buttonState |= (this.LeftState ? JoystickButtons.Left : JoystickButtons.None);
            buttonState |= (this.RightState ? JoystickButtons.Right : JoystickButtons.None);
            buttonState |= (this.CenterState ? JoystickButtons.Center : JoystickButtons.None);

            return buttonState;
        }
    }

    protected abstract bool UpState { get; }
    protected abstract bool DownState { get; }
    protected abstract bool LeftState { get; }
    protected abstract bool RightState { get; }
    protected abstract bool CenterState { get; }

}