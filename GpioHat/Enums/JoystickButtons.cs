using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpioHat
{
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
}
