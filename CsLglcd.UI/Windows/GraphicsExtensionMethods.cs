using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public static class GraphicsExtensionMethods
    {
        public static void DrawImage(this Graphics drawer, Image image)
        {
            drawer.DrawImage(image, 0, 0);
        }
    }
}
