using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsLglcd.Interop;
using System.Drawing;

namespace CsLglcd
{
    public class QvgaImageUpdater : IBitmapUpdater
    {
        private lgLcdBitmapQVGAx32 bitmapQVGAx32;

        public bool Disposed { get; private set; }

        public QvgaImageUpdater()
        {
            bitmapQVGAx32.hdr.Format = Formats.LGLCD_BMP_FORMAT_QVGAx32;
        }

        public IntPtr UnmanagedBitmapHeaderPointer
        {
            get { return bitmapQVGAx32.Unmanaged; }
        }

        public byte[] Pixels
        {
            get
            {
                return bitmapQVGAx32.Pixels;
            }
            set
            {
                bitmapQVGAx32.Pixels = value;
            }
        }

        public void SetPixels(Bitmap image)
        {
            bitmapQVGAx32.SetPixels(image);
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
            bitmapQVGAx32.Dispose();

            Disposed = true;
        }

        ~QvgaImageUpdater()
        {
            Dispose(false);
        }
        #endregion
    }
}
