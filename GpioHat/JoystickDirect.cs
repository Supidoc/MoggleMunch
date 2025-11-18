using System.Device.Gpio;
using NLog;

namespace GpioHat;

public class JoystickDirect : Joystick
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
    private readonly int centerPin;
    private readonly int downPin;


    private readonly GpioController gpioController;
    private readonly int leftPin;
    private readonly int rightPin;
    private readonly int upPin;

    public JoystickDirect(GpioController gpioController, int upPin = 19, int downPin = 13, int leftPin = 6,
        int rightPin = 5, int centerPin = 26)
    {
        this.gpioController = gpioController;
        this.upPin = upPin;
        this.downPin = downPin;
        this.leftPin = leftPin;
        this.rightPin = rightPin;
        this.centerPin = centerPin;

        this.gpioController.OpenPin(upPin, PinMode.InputPullUp);
        this.gpioController.OpenPin(downPin, PinMode.InputPullUp);
        this.gpioController.OpenPin(leftPin, PinMode.InputPullUp);
        this.gpioController.OpenPin(rightPin, PinMode.InputPullUp);
        this.gpioController.OpenPin(centerPin, PinMode.InputPullUp);

        log.Trace("Created Joystick");
    }

    protected override bool UpState
    {
        get => !(bool)this.gpioController.Read(this.upPin);
    }

    protected override bool DownState
    {
        get => !(bool)this.gpioController.Read(this.downPin);
    }

    protected override bool LeftState
    {
        get => !(bool)this.gpioController.Read(this.leftPin);
    }

    protected override bool RightState
    {
        get => !(bool)this.gpioController.Read(this.rightPin);
    }

    protected override bool CenterState
    {
        get => !(bool)this.gpioController.Read(this.centerPin);
    }
}