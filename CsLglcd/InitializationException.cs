using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd
{
    public class InitializationException : LglcdException
    {
        public InitializationException(string msg) : base(msg) {}
    }
}
