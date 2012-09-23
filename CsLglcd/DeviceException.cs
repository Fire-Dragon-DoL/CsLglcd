using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd
{
    class DeviceException : LglcdException
    {
        public DeviceException(string msg) : base(msg) { }
        public DeviceException(uint errorCode, string msg) : base(errorCode, msg) { }
    }
}
