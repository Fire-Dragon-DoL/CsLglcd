using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows.Old
{
    public class UserControl : Control
    {
        public Image Image { get; set; }
        public Control Parent { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override void Draw(Bitmap surface, Graphics drawer = null)
        {
            if (Hidden) return;
            if (Image == null) return;

            drawer.DrawImage(Image, X, Y);
        }
    }
}
