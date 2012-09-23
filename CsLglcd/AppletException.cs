using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd
{
    public class AppletException : LglcdException
    {
        public AppletException(string msg) : base(msg) { }
        public AppletException(uint errorCode, string msg) : base(errorCode, msg) { }
    }
}
