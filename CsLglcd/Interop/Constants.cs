using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd.Interop
{
    enum Errors : uint
    {
        // Initialization
        ERROR_SUCCESS = 0U,
        RPC_S_SERVER_UNAVAILABLE = 1722U,
        ERROR_OLD_WIN_VERSION = 1150U,
        ERROR_NO_SYSTEM_RESOURCES = 1450U,
        ERROR_ALREADY_INITIALIZED = 1247U,
        //Connection
        ERROR_SERVICE_NOT_ACTIVE = 1062U,
        ERROR_INVALID_PARAMETER = 87U,
        ERROR_FILE_NOT_FOUND = 2U,
        ERROR_ALREADY_EXISTS = 183U,
        RPC_X_WRONG_PIPE_VERSION = 1832U,

        //Buttons
        ERROR_DEVICE_NOT_CONNECTED = 1167U,
        //Update
        ERROR_ACCESS_DENIED = 5U,
        //Set as Foreground app
        ERROR_LOCK_FAILED = 167U,
    }

    enum DescriptorErrors : int
    {
        LGLCD_INVALID_CONNECTION = -1,
        LGLCD_INVALID_DEVICE = -1,
    }

    enum DeviceTypes
    {
        LGLCD_DEVICE_BW = (0x00000001),
        LGLCD_DEVICE_QVGA = (0x00000002),
    }

    enum Buttons : uint
    {
        None = (0x00000000),
        // Common Soft-Buttons available through the SDK
        LGLCDBUTTON_LEFT = (0x00000100),
        LGLCDBUTTON_RIGHT = (0x00000200),
        LGLCDBUTTON_OK = (0x00000400),
        LGLCDBUTTON_CANCEL = (0x00000800),
        LGLCDBUTTON_UP = (0x00001000),
        LGLCDBUTTON_DOWN = (0x00002000),
        LGLCDBUTTON_MENU = (0x00004000),

        // Soft-Button masks. Kept for backwards compatibility
        LGLCDBUTTON_BUTTON0 = (0x00000001),
        LGLCDBUTTON_BUTTON1 = (0x00000002),
        LGLCDBUTTON_BUTTON2 = (0x00000004),
        LGLCDBUTTON_BUTTON3 = (0x00000008),
        [Obsolete]
        LGLCDBUTTON_BUTTON4 = (0x00000010),
        [Obsolete]
        LGLCDBUTTON_BUTTON5 = (0x00000020),
        [Obsolete]
        LGLCDBUTTON_BUTTON6 = (0x00000040),
        [Obsolete]
        LGLCDBUTTON_BUTTON7 = (0x00000080),
    }

    enum Formats : uint
    {
        LGLCD_BMP_FORMAT_160x43x1 = (0x00000001),
        LGLCD_BMP_FORMAT_QVGAx32 = (0x00000003),
    }

    enum BitmapSizes : int
    {
        LGLCD_BMP_WIDTH = 160,
        LGLCD_BMP_HEIGHT = 43,
        LGLCD_BMP_BPP = 1,
        Size = LGLCD_BMP_WIDTH * LGLCD_BMP_HEIGHT,
    }

    enum BWBitmapSizes : int
    {
        LGLCD_BW_BMP_WIDTH = 160,
        LGLCD_BW_BMP_HEIGHT = 43,
        LGLCD_BW_BMP_BPP = 1,
        Size = LGLCD_BW_BMP_WIDTH * LGLCD_BW_BMP_HEIGHT,
    }

    enum QVGABitmapSizes : int
    {
        LGLCD_QVGA_BMP_WIDTH = 320,
        LGLCD_QVGA_BMP_HEIGHT = 240,
        LGLCD_QVGA_BMP_BPP = 4,
        Size = LGLCD_QVGA_BMP_WIDTH * LGLCD_QVGA_BMP_HEIGHT * LGLCD_QVGA_BMP_BPP,
    }

    /// <summary cref="GSdk.Net.Lglcd.Priorities">
    /// </summary>
    [Flags]
    enum Priorities : uint
    {
        LGLCD_PRIORITY_IDLE_NO_SHOW = 0,
        LGLCD_PRIORITY_BACKGROUND = 64,
        LGLCD_PRIORITY_NORMAL = 128,
        LGLCD_PRIORITY_ALERT = 255,
    }

    /// <summary>
    /// Should be Or'd with <see cref="GSdk.Net.Lglcd.Priorities"/>
    /// </summary>
    [Flags]
    enum UpdateModes : uint
    {
        LGLCD_SYNC_UPDATE = 0x80000000,
        LGLCD_SYNC_COMPLETE_WITHIN_FRAME = 0xC0000000,
        LGLCD_ASYNC_UPDATE = 0,
    }

    internal enum Notifications : uint
    {
        LGLCD_NOTIFICATION_DEVICE_ARRIVAL = (0x00000001),
        LGLCD_NOTIFICATION_DEVICE_REMOVAL = (0x00000002),
        LGLCD_NOTIFICATION_CLOSE_CONNECTION = (0x00000003),
        LGLCD_NOTIFICATION_APPLET_DISABLED = (0x00000004),
        LGLCD_NOTIFICATION_APPLET_ENABLED = (0x00000005),
        [Obsolete]
        LGLCD_NOTIFICATION_TERMINATE_APPLET = (0x00000006),
    }

    /// <summary>
    /// They should be Or'd
    /// </summary>
    [Flags]
    enum AppletCapabilities : uint
    {
        LGLCD_APPLET_CAP_BASIC = (0x00000000),
        LGLCD_APPLET_CAP_BW = (0x00000001),
        LGLCD_APPLET_CAP_QVGA = (0x00000002),
    }

    enum ForegroundYesNoFlags : int
    {
        LGLCD_LCD_FOREGROUND_APP_NO = (0),
        LGLCD_LCD_FOREGROUND_APP_YES = (1),
    }
}
