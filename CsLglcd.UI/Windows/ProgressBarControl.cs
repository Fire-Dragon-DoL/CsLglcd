using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public class ProgressBarControl : Control
    {
        public dynamic Min { get; set; }
        public dynamic Max { get; set; }
        public dynamic Current { get; set; }

        public ProgressBarControl()
            : base()
        {
            Min = 0;
            Max = 100;
            Current = 33;
        }

        public Image BarFillerLeft { get { return Properties.Resources.qvga_bar_fill_left; } }
        public Image BarFillerRight { get { return Properties.Resources.qvga_bar_fill_right; } }
        public Image BarEmpty { get { return Properties.Resources.qvga_bar_empty; } }
        public Image BarGradient { get { return Properties.Resources.qvga_bar_gradient; } }
        public readonly Brush BarColor = new SolidBrush(Color.FromArgb(255, 0, 153, 102));

        // (Max - Min) : 100 = Current : x
        public float Percentage
        {
            get
            {
                if (Current <= Min)
                    return 0f;
                if (Current >= Max)
                    return 1f;
                return ((float)Current) / (Max - Min);
            }
        }

        public override void Draw(Image surface, Graphics drawer, Point offset = new Point())
        {
            // BarLeft and right are positioned 1 pixel farther from empty bar positioning
            int MaxSlices = (BarEmpty.Width + 1 + 1) / BarFillerLeft.Width;
            float PercentagePerSlice = 1f / MaxSlices;
            int SlicesPerPercentage = (int)Math.Round((Percentage / PercentagePerSlice), 0);

            int xposition, yposition;
            // old xposition = 8, old yposition = 217
            xposition = offset.X + X;
            yposition = offset.Y + Y;

            // Bar Empty
            drawer.DrawImage(BarEmpty, xposition + 1, yposition);

            // Bar Left
            if (SlicesPerPercentage > 0)
                drawer.DrawImage(BarFillerLeft, xposition, yposition);

            // Bar Middle
            if (SlicesPerPercentage > 1)
                drawer.FillRectangle(
                    BarColor,
                    BarFillerLeft.Width * 2,
                    yposition + 1,
                    BarFillerLeft.Width * (SlicesPerPercentage - (SlicesPerPercentage == MaxSlices ? 2 : 1)),
                    BarFillerLeft.Height - 2
                );

            // Bar Right
            if (SlicesPerPercentage >= MaxSlices)
                drawer.DrawImage(BarFillerRight, BarEmpty.Width + 2, yposition);

            // Gradient
            if (SlicesPerPercentage > 0)
                drawer.DrawImage(
                    BarGradient,
                    new Rectangle(
                        xposition + 1, yposition,
                        (SlicesPerPercentage * BarFillerLeft.Width) - 2,
                        BarGradient.Height
                    ),
                    new Rectangle(
                        0, 0,
                        (SlicesPerPercentage * BarFillerLeft.Width) - 2,
                        BarGradient.Height
                    ),
                    GraphicsUnit.Pixel
                );
        }
    }
}
