using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public abstract class Control
    {
        public class ControlComparer : IComparer<Control>
        {
            public int Compare(Control x, Control y)
            {
                return x.Z.CompareTo(y.Z);
            }
        }

        public bool Hidden { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public abstract void Draw(Image surface, Graphics drawer, Point offset = new Point(0, 0));
    }
}
