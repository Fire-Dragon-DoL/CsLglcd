using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace CsLglcd.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    struct lgLcdConfigureContext
    {
        /// <summary>
        /// Set to NULL if not configurable
        /// </summary>
        public lgLcdOnConfigureCB configCallback;
        public IntPtr configContext;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct lgLcdNotificationContext
    {
        /// <summary>
        /// Set to NULL if not configurable
        /// </summary>
        public lgLcdOnNotificationCB notificationCallback;
        public IntPtr notifyContext;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct lgLcdSoftbuttonsChangedContext
    {
        /// <summary>
        /// Set to NULL if no softbutton notifications are needed
        /// </summary>
        public lgLcdOnSoftButtonsCB softbuttonsChangedCallback;
        public IntPtr softbuttonsChangedContext;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct lgLcdConnectContextExW
    {
        /// <summary>
        /// "Friendly name" display in the listing
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.LPWStr)]
        public string appFriendlyName;
        /// <summary>
        /// isPersistent determines whether this connection persists in the list
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool isPersistent;
        /// <summary>
        /// isAutostartable determines whether the client can be started by LCDMon
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool isAutostartable;
        public lgLcdConfigureContext onConfigure;
        /// <summary>
        /// --> Connection handle
        /// </summary>
        public int connection;
        /// <summary>
        /// New additions added in 1.03 revision
        /// Or'd combination of LGLCD_APPLET_CAP_... defines
        /// </summary>            
        public AppletCapabilities dwAppletCapabilitiesSupported;
        public uint dwReserved1;
        public lgLcdNotificationContext onNotify;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct lgLcdOpenByTypeContext
    {
        public int connection;
        /// <summary>
        /// Device type to open (either LGLCD_DEVICE_BW or LGLCD_DEVICE_QVGA)
        /// </summary>
        public DeviceTypes deviceType;
        public lgLcdSoftbuttonsChangedContext onSoftbuttonsChanged;
        /// <summary>
        /// --> Device handle
        /// </summary>
        public int device;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct lgLcdBitmapHeader
    {
        public Formats Format;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct lgLcdBitmap
    {
        // This is commented because it causes overlaps problems, ask C# why this, I didn't want to digg
        //[FieldOffset(0)]
        //public lgLcdBitmapHeader hdr;
        [FieldOffset(0)]
        public lgLcdBitmap160x43x1 bmp_mono;
        [FieldOffset(0)]
        public lgLcdBitmapQVGAx32 bmp_qvga32;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct lgLcdBitmap160x43x1 : IDisposable
    {
        /// <summary>
        /// Format = LGLCD_BMP_FORMAT_160x43x1
        /// </summary>
        public lgLcdBitmapHeader hdr;
        /// <summary>
        /// byte array of size LGLCD_BMP_WIDTH * LGLCD_BMP_HEIGHT, use AllocHGlobal to make code safe
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)BWBitmapSizes.Size)]
        internal byte[] pixels;
        public byte[] Pixels
        {
            set
            {
                if (value.Length != (int)BWBitmapSizes.Size)
                    throw new ArgumentException(string.Format("Length must be of size : {0}/{1}", value.Length, (int)BWBitmapSizes.Size));

                pixels = value;
            }

            get
            {
                if (pixels == null)
                    pixels = new byte[(int)BWBitmapSizes.Size];

                return pixels;
            }
        }

        public void SetPixels(Bitmap image)
        {
            int yPerWidth, yPerStride, bytePerPixel;
            if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb || image.Width != (int)BWBitmapSizes.LGLCD_BW_BMP_WIDTH || image.Height != (int)BWBitmapSizes.LGLCD_BW_BMP_HEIGHT)
                throw new ArgumentException(string.Format("Image must be {0} pixel format and must be of size {1}x{2}",
                    Enum.GetName(typeof(System.Drawing.Imaging.PixelFormat), System.Drawing.Imaging.PixelFormat.Format32bppArgb),
                    (int)BWBitmapSizes.LGLCD_BW_BMP_WIDTH,
                    (int)BWBitmapSizes.LGLCD_BW_BMP_HEIGHT));

            bytePerPixel = 4;
            var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < data.Height; ++y)
            {
                yPerWidth = y * data.Width;
                yPerStride = yPerWidth * bytePerPixel;
                for (int x = 0; x < data.Width; ++x)
                {
                    Pixels[yPerWidth + x] = Marshal.ReadByte(data.Scan0, 1 + yPerStride + (x * 4));
                }
            }

            image.UnlockBits(data);
        }

        public void Dispose()
        {
            if (m_Unmanaged != IntPtr.Zero)
                Marshal.FreeHGlobal(m_Unmanaged);
        }

        private IntPtr m_Unmanaged;

        public IntPtr Unmanaged
        {
            get
            {
                if (m_Unmanaged == IntPtr.Zero)
                    m_Unmanaged = Marshal.AllocHGlobal(Marshal.SizeOf(GetType()));

                Marshal.StructureToPtr(this, m_Unmanaged, false);

                return m_Unmanaged;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct lgLcdBitmapQVGAx32 : IDisposable
    {
        /// <summary>
        /// Format = LGLCD_BMP_FORMAT_160x43x1
        /// </summary>
        public lgLcdBitmapHeader hdr;
        /// <summary>
        /// byte array of size LGLCD_QVGA_BMP_WIDTH * LGLCD_QVGA_BMP_HEIGHT * LGLCD_QVGA_BMP_BPP, use AllocHGlobal to make code safe
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)QVGABitmapSizes.Size)]
        internal byte[] pixels;
        public byte[] Pixels
        {
            set
            {
                if (value.Length != (int)QVGABitmapSizes.Size)
                    throw new ArgumentException(string.Format("Length must be of size : {0}/{1}", value.Length, (int)QVGABitmapSizes.Size));

                pixels = value;
            }

            get
            {
                if (pixels == null)
                    pixels = new byte[(int)QVGABitmapSizes.Size];

                return pixels;
            }
        }

        public void SetPixels(Bitmap image)
        {
            if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb || image.Width != (int)QVGABitmapSizes.LGLCD_QVGA_BMP_WIDTH || image.Height != (int)QVGABitmapSizes.LGLCD_QVGA_BMP_HEIGHT)
                throw new ArgumentException(string.Format("Image must be {0} pixel format and must be of size {1}x{2}",
                    Enum.GetName(typeof(System.Drawing.Imaging.PixelFormat), System.Drawing.Imaging.PixelFormat.Format32bppArgb),
                    (int)QVGABitmapSizes.LGLCD_QVGA_BMP_WIDTH,
                    (int)QVGABitmapSizes.LGLCD_QVGA_BMP_HEIGHT));

            byte[] array = Pixels;
            var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(data.Scan0, Pixels, 0, data.Stride * data.Height);
            image.UnlockBits(data);
        }

        public void Dispose()
        {
            if (m_Unmanaged != IntPtr.Zero)
                Marshal.FreeHGlobal(m_Unmanaged);
        }

        private IntPtr m_Unmanaged;

        public IntPtr Unmanaged
        {
            get
            {
                if (m_Unmanaged == IntPtr.Zero)
                    m_Unmanaged = Marshal.AllocHGlobal(Marshal.SizeOf(GetType()));

                Marshal.StructureToPtr(this, m_Unmanaged, false);

                return m_Unmanaged;
            }
        }
    }
}
