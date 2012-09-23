using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd.UI.Windows
{
    public class UserControl : Control
    {
        public Control Parent { get; protected internal set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}
