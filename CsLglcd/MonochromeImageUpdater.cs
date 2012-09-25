using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsLglcd.Interop;
using System.Drawing;

namespace CsLglcd
{
    public class MonochromeImageUpdater : IBitmapUpdater
    {
        private lgLcdBitmap160x43x1 bitmap160x43x1;

        public bool Disposed { get; private set; }

        public MonochromeImageUpdater()
        {
            bitmap160x43x1.hdr.Format = Formats.LGLCD_BMP_FORMAT_160x43x1;
        }

        public IntPtr UnmanagedBitmapHeaderPointer
        {
            get { return bitmap160x43x1.Unmanaged; }
        }

        public byte[] Pixels
        {
            get
            {
                return bitmap160x43x1.Pixels;
            }
            set
            {
                bitmap160x43x1.Pixels = value;
            }
        }

        public Bitmap CreateValidImage()
        {
            return new Bitmap(160, 43, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public void SetPixels(Bitmap image)
        {
            bitmap160x43x1.SetPixels(image);
        }

        #region Dispose handling
        /// <summary>
        /// Calls automatically Disconnect if required
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                // Managed resources
            }

            // Unmanaged resources
            bitmap160x43x1.Dispose();

            Disposed = true;
        }

        ~MonochromeImageUpdater()
        {
            Dispose(false);
        }
        #endregion
    }
}
