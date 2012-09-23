using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd
{
    public class DeviceEventArgs : EventArgs
    {
        public Devices Device { get; private set; }
        public DeviceEventArgs(Devices device) : base() { Device = device; }
    }
}
