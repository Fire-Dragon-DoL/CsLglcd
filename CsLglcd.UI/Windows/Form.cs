using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public class Form : ContentControl
    {
        public Bitmap Background { get; set; }
        public Bitmap Header { get; set; }
        public string Title { get; set; }
        public Bitmap Icon { get; set; }
        public Font BaseFont { get; set; }

        public Form()
        {
            Title = string.Empty;
            Background = CsLglcd.UI.Properties.Resources.qvga_background;
            Header = CsLglcd.UI.Properties.Resources.qvga_header;
            Width = Background.Width;
            Height = Background.Height;
            BaseFont = new Font(
                "monospace",
                12.0f,
                FontStyle.Bold
            );
            Icon = CsLglcd.UI.Properties.Resources.qvga_defaulticon;
        }

        public override void Draw(Bitmap surface, Graphics drawer = null)
        {
            if (Hidden) return;

            Graphics g = drawer;
            if (g == null)
                g = Graphics.FromImage(surface);

            g.FillRectangle(Brushes.Red, new Rectangle(0, 0, Width, Height));
            if (Background != null)
                g.DrawImage(Background);
            if (Header != null)
                g.DrawImage(Header);
            if (Icon != null)
                g.DrawImage(Icon, 3, 3);
            if (!string.IsNullOrWhiteSpace(Title))
            {
                g.DrawString(
                    Title,
                    BaseFont,
                    Brushes.Black,
                    30.0f,
                    5.0f
                );
                g.DrawString(
                    Title,
                    BaseFont,
                    Brushes.White,
                    29.0f,
                    4.0f
                );
            }

            foreach (UserControl control in Items)
                control.Draw(surface, g);

            g.Dispose();
        }
    }
}
