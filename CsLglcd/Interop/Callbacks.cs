using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CsLglcd.Interop
{
    /// <summary>
    /// Callback used to allow client to pop up a "configuration panel"
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="pContext"></param>
    /// <returns></returns>
    delegate uint lgLcdOnConfigureCB([In] int connection, [In] IntPtr pContext);

    /// <summary>
    /// Callback used to notify client of events, such as device arrival, ...
    /// Arrival, removal, applet enable/disable supported as of version 3.0.
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="pContext"></param>
    /// <param name="notificationCode"></param>
    /// <param name="notifyParam1"></param>
    /// <param name="notifyParam2"></param>
    /// <param name="notifyParam3"></param>
    /// <param name="notifyParam4"></param>
    /// <returns></returns>
    delegate uint lgLcdOnNotificationCB([In] int connection, [In] IntPtr pContext, [In] Notifications notificationCode, [In] uint notifyParam1, [In] uint notifyParam2, [In] uint notifyParam3, [In] uint notifyParam4);

    /// <summary>
    /// Callback used to notify client of soft button change
    /// </summary>
    /// <param name="device"></param>
    /// <param name="dwButtons"></param>
    /// <param name="pContext"></param>
    /// <returns></returns>
    delegate uint lgLcdOnSoftButtonsCB([In] int device, [In] uint dwButtons, [In] IntPtr pContext);

}
