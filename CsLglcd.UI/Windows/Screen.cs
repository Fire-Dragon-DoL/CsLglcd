using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public abstract class Screen : IDrawable
    {
        public Applet Applet { get; private set; }
        public Device Device { get; private set; }

        public Control Control { get; set; }

        public abstract int Width { get; }
        public abstract int Height { get; }

        public Screen(Applet applet, Device device)
        {
            Applet = applet;
            Device = device;
        }

        public virtual void Draw(Image surface, Graphics drawer, Point offset = new Point())
        {
            if (!Control.Hidden)
                Control.Draw(surface, drawer, offset);
        }
    }
}
