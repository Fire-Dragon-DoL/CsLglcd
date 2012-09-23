using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CsLglcd.Interop
{
    using Methodsx86 = x86.Methods;
    using Methodsx64 = x64.Methods;

    /// <summary>
    /// Methods contained in this class automatically calls 64bit or 32bit methods, to allow "Any CPU" compiles
    /// </summary>
    static class MethodsWrapper
    {
        /// <summary>
        /// // Initialize the library by calling this function.
        /// </summary>
        /// <returns></returns>
        public static uint Init()
        {
            if (Environment.Is64BitProcess)
                return Methodsx64.lgLcdInit();
            else
                return Methodsx86.lgLcdInit();
        }

        /// <summary>
        /// Must be called to release the library and free all allocated structures.
        /// </summary>
        /// <returns></returns>
        public static uint DeInit()
        {
            if (Environment.Is64BitProcess)
                return Methodsx64.lgLcdDeInit();
            else
                return Methodsx86.lgLcdDeInit();
        }

        /// <summary>
        /// Connect as a client to the LCD subsystem. Provide name to be
        /// displayed for user when viewing the user interface of the LCD module,
        /// as well as a configuration callback and context, and a flag that states
        /// whether this client is startable by LCDMon
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static uint ConnectEx([In, Out] ref lgLcdConnectContextExW ctx)
        {
            if (Environment.Is64BitProcess)
                return Methodsx64.lgLcdConnectExW(ref ctx);
            else
                return Methodsx86.lgLcdConnectExW(ref ctx);
        }

        /// <summary>
        /// Must be called to release the connection and free all allocated resources
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static uint Disconnect(int connection)
        {
            if (Environment.Is64BitProcess)
                return Methodsx64.lgLcdDisconnect(connection);
            else
                return Methodsx86.lgLcdDisconnect(connection);
        }

        /// <summary>
        /// Opens an LCD of the specified type. If no such device is available, returns
        /// an error.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static uint OpenByType([In, Out] ref lgLcdOpenByTypeContext ctx)
        {
            if (Environment.Is64BitProcess)
                return Methodsx64.lgLcdOpenByType(ref ctx);
            else
                return Methodsx86.lgLcdOpenByType(ref ctx);
        }

        /// <summary>
        /// Closes the LCD. Must be paired with lgLcdOpen()/lgLcdOpenByType().
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static uint Close([In] int device)
        {
            if (Environment.Is64BitProcess)
                return Methodsx64.lgLcdClose(device);
            else
                return Methodsx86.lgLcdClose(device);
        }

        /// <summary>
        /// Reads the state of the soft buttons for the device.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static uint ReadSoftButtons([In] int device, [In, Out] ref Buttons buttons)
        {
            if (Environment.Is64BitProcess)
                return Methodsx64.lgLcdReadSoftButtons(device, ref buttons);
            else
                return Methodsx86.lgLcdReadSoftButtons(device, ref buttons);
        }

        /// <summary>
        /// Provides a bitmap to be displayed on the LCD. The priority field
        /// further describes the way in which the bitmap is to be applied.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="bitmap"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static uint UpdateBitmap([In] int device, [In] IntPtr bitmap, [In] Priorities priority)
        {
            if (Environment.Is64BitProcess)
                return Methodsx64.lgLcdUpdateBitmap(device, bitmap, priority);
            else
                return Methodsx86.lgLcdUpdateBitmap(device, bitmap, priority);
        }

        /// <summary>
        /// Sets the calling application as the shown application on the LCD, and stops
        /// any type of rotation among other applications on the LCD.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="foregroundYesNoFlag"></param>
        /// <returns></returns>
        public static uint SetAsLCDForegroundApp([In] int device, [In] ForegroundYesNoFlags foregroundYesNoFlag)
        {
            if (Environment.Is64BitProcess)
                return Methodsx64.lgLcdSetAsLCDForegroundApp(device, foregroundYesNoFlag);
            else
                return Methodsx86.lgLcdSetAsLCDForegroundApp(device, foregroundYesNoFlag);
        }
    }
}
