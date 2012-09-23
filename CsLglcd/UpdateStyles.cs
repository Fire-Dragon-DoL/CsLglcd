using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsLglcd.Interop;

namespace CsLglcd
{
    public enum UpdateStyles : uint
    {
        Sync = UpdateModes.LGLCD_SYNC_UPDATE,
        SyncComplete = UpdateModes.LGLCD_SYNC_COMPLETE_WITHIN_FRAME,
        Async = UpdateModes.LGLCD_ASYNC_UPDATE
    }
}
