using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd
{
    public class LglcdException : Exception
    {
        public uint? ErrorCode { get; protected set; }

        public LglcdException() : base() { }
        public LglcdException(string msg) : base(msg) { }
        public LglcdException(string msg, Exception innerException) : base(msg, innerException) { }
        public LglcdException(uint errorCode) : base() { ErrorCode = errorCode; }
        public LglcdException(uint errorCode, string msg) : base(msg) { ErrorCode = errorCode; }
        public LglcdException(uint errorCode, string msg, Exception innerException) : base(msg, innerException) { ErrorCode = errorCode; }

        public override string ToString()
        {
            if (!ErrorCode.HasValue)
                return base.ToString();
            else
                return string.Format("[# {0}] {1}", ErrorCode.Value, base.ToString());
        }
    }
}
