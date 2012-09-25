﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public class TextControl : Control
    {
        public Font BaseFont { get; set; }
        public string Text { get; set; }
        public Brush Color { get; set; }

        public TextControl() : base() { Color = Brushes.White; }

        public override void Draw(Image surface, Graphics drawer, Point offset = new Point())
        {
            drawer.DrawString(Text, BaseFont, Color, new RectangleF((float)offset.X + X, (float)offset.Y + Y, (float)Width, (float)Height));
        }
    }
}
