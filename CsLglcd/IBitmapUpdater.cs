using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd
{
    public interface IBitmapUpdater : IImageUpdater
    {
        Bitmap CreateValidImage();
        void SetPixels(Bitmap image);
    }
}
