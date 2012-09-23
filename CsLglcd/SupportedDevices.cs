using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsLglcd.Interop;

namespace CsLglcd
{
    [Flags]
    public enum SupportedDevices : uint
    {
        None = AppletCapabilities.LGLCD_APPLET_CAP_BASIC,
        BlackAndWhite = AppletCapabilities.LGLCD_APPLET_CAP_BW,
        QVGA = AppletCapabilities.LGLCD_APPLET_CAP_QVGA
    }
}
