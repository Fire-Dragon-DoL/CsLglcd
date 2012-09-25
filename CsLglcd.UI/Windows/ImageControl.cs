using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public class ImageControl : Control
    {
        public Image Image { get; set; }

        public ImageControl(Image image)
            : base()
        {
            Image = image;
            Width = Image.Width;
            Height = Image.Height;
        }

        public override void Draw(Image surface, Graphics drawer, Point offset = new Point())
        {
            drawer.DrawImage(surface, offset.X + X, offset.Y + Y, Width, Height);
        }
    }
}
