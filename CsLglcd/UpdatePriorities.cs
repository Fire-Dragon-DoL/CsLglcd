using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsLglcd.Interop;

namespace CsLglcd
{
    public enum UpdatePriorities : uint
    {
        IdleNoShow = Priorities.LGLCD_PRIORITY_IDLE_NO_SHOW,
        Background = Priorities.LGLCD_PRIORITY_BACKGROUND,
        Normal = Priorities.LGLCD_PRIORITY_NORMAL,
        Alert = Priorities.LGLCD_PRIORITY_ALERT
    }
}
