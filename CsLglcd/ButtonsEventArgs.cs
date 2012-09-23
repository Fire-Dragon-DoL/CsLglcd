using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd
{
    public class ButtonsEventArgs : EventArgs
    {
        public Buttons Buttons { get; private set; }
        public ButtonsEventArgs(Buttons buttons) { Buttons = buttons; }
    }
}
