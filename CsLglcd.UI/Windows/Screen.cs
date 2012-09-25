using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public class Screen
    {
        public Applet Applet { get; private set; }
        public Device Device { get; private set; }

        public Screen(Applet applet, Device device)
        {
            Applet = applet;
            Device = device;
        }
    }
}
