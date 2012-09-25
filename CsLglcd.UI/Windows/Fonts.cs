using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public static class Fonts
    {
        public static class Qvga
        {
            public static readonly Font Normal = new Font(
                "monospace",
                12.0f,
                FontStyle.Bold
            );
            public static readonly Font Small = new Font(
                "monospace",
                10.0f,
                FontStyle.Bold
            );
        }
        public static class Monochrome
        {
            public static readonly Font Normal = new Font(
                "monospace",
                6.0f,
                FontStyle.Bold
            );
            public static readonly Font Small = new Font(
                "monospace",
                8.0f,
                FontStyle.Regular
            );
        }
    }
}
