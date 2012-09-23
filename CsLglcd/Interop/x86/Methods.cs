using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CsLglcd.Interop.x86
{
    class Methods
    {
        /// <summary>
        /// // Initialize the library by calling this function.
        /// </summary>
        /// <returns></returns>
        [DllImport("Lglcd_x86.dll", CharSet = CharSet.Unicode)]
        public extern static uint lgLcdInit();

        /// <summary>
        /// Must be called to release the library and free all allocated structures.
        /// </summary>
        /// <returns></returns>
        [DllImport("Lglcd_x86.dll", CharSet = CharSet.Unicode)]
        public extern static uint lgLcdDeInit();

        /// <summary>
        /// Connect as a client to the LCD subsystem. Provide name to be
        /// displayed for user when viewing the user interface of the LCD module,
        /// as well as a configuration callback and context, and a flag that states
        /// whether this client is startable by LCDMon
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        [DllImport("Lglcd_x86.dll", CharSet = CharSet.Unicode)]
        public extern static uint lgLcdConnectExW([In, Out] ref lgLcdConnectContextExW ctx);

        /// <summary>
        /// Must be called to release the connection and free all allocated resources
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        [DllImport("Lglcd_x86.dll", CharSet = CharSet.Unicode)]
        public extern static uint lgLcdDisconnect(int connection);

        /// <summary>
        /// Opens an LCD of the specified type. If no such device is available, returns
        /// an error.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        [DllImport("Lglcd_x86.dll", CharSet = CharSet.Unicode)]
        public extern static uint lgLcdOpenByType([In, Out] ref lgLcdOpenByTypeContext ctx);

        /// <summary>
        /// Closes the LCD. Must be paired with lgLcdOpen()/lgLcdOpenByType().
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [DllImport("Lglcd_x86.dll", CharSet = CharSet.Unicode)]
        public extern static uint lgLcdClose([In] int device);

        /// <summary>
        /// Reads the state of the soft buttons for the device.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        [DllImport("Lglcd_x86.dll", CharSet = CharSet.Unicode)]
        public extern static uint lgLcdReadSoftButtons([In] int device, [In, Out] ref Buttons buttons);

        /// <summary>
        /// Provides a bitmap to be displayed on the LCD. The priority field
        /// further describes the way in which the bitmap is to be applied.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="bitmap"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        [DllImport("Lglcd_x86.dll", CharSet = CharSet.Unicode)]
        public extern static uint lgLcdUpdateBitmap([In] int device, [In] IntPtr bitmap, [In] Priorities priority);

        /// <summary>
        /// Sets the calling application as the shown application on the LCD, and stops
        /// any type of rotation among other applications on the LCD.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="foregroundYesNoFlag"></param>
        /// <returns></returns>
        [DllImport("Lglcd_x86.dll", CharSet = CharSet.Unicode)]
        public extern static uint lgLcdSetAsLCDForegroundApp([In] int device, [In] ForegroundYesNoFlags foregroundYesNoFlag);
    }
}
