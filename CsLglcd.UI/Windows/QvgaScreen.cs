using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public class QvgaScreen : Screen
    {
        /// <summary>
        /// Printed over Background
        /// </summary>
        public Image AppBackground { get; set; }
        public Image Background { get; set; }
        public Image Header { get; set; }
        public string Title { get { return Applet.Title; } }
        public Image IconBackground { get; set; }
        public Image Icon { get; set; }
        public Font BaseFont { get; set; }

        public int HeaderHeight { get { return 30; } }

        public override int Width { get { return Background.Width; } }
        public override int Height { get { return Background.Height; } }

        public QvgaScreen(Applet applet, Device device)
            : base(applet, device)
        {
            Background = CsLglcd.UI.Properties.Resources.qvga_background;
            Header = CsLglcd.UI.Properties.Resources.qvga_header;
            BaseFont = Fonts.Qvga.Normal;
            IconBackground = CsLglcd.UI.Properties.Resources.qvga_background_headericon;
        }

        public override void Draw(Image surface, Graphics drawer, Point offset = new Point())
        {
            drawer.Clear(Color.Black);
            if (Background != null)
                drawer.DrawImage(Background);
            if (AppBackground != null)
                drawer.DrawImage(AppBackground);
            if (Header != null)
                drawer.DrawImage(Header);
            if (IconBackground != null)
                drawer.DrawImage(IconBackground, 3, 3);
            if (Icon != null)
                drawer.DrawImage(Icon, 3, 3);
            if (!string.IsNullOrWhiteSpace(Title))
            {
                drawer.DrawString(
                    Title,
                    BaseFont,
                    Brushes.Black,
                    30.0f,
                    5.0f
                );
                drawer.DrawString(
                    Title,
                    BaseFont,
                    Brushes.White,
                    29.0f,
                    4.0f
                );
            }
            base.Draw(surface, drawer, new Point(offset.X, offset.Y + HeaderHeight));
        }
    }
}
