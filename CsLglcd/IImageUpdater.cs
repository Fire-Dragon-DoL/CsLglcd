using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CsLglcd
{
    public interface IImageUpdater : IDisposable
    {
        IntPtr UnmanagedBitmapHeaderPointer { get; }
        byte[] Pixels { get; set; }
    }
}
