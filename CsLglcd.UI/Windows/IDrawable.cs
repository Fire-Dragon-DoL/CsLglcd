using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public interface IDrawable
    {
        void Draw(Image surface, Graphics drawer, Point offset = new Point());
    }
}
