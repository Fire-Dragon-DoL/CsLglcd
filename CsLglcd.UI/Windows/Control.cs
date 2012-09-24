using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public abstract class Control
    {
        public bool Hidden { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public abstract void Draw(Bitmap surface, Graphics drawer = null);
    }
}
