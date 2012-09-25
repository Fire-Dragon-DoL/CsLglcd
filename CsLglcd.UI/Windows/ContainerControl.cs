using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsLglcd.UI.Windows
{
    public class ContainerControl : Control
    {
        private IComparer<Control> m_ComparerControl = new Control.ControlComparer();
        
        protected List<Control> Items = new List<Control>();

        public IComparer<Control> ComparerControl
        {
            get { return m_ComparerControl; }
            set { m_ComparerControl = value; }
        }

        public List<Control> Controls
        {
            get
            {
                Items.Sort(ComparerControl);
                return Items;
            }
        }

        public void AddControl(Control control)
        {
            Items.Add(control);
        }
        public bool RemoveControl(Control control)
        {
            return Items.Remove(control);
        }

        public override void Draw(Image surface, Graphics drawer, Point offset = new Point())
        {
            foreach (Control control in Controls)
            {
                if (control.Hidden)
                    continue;

                control.Draw(surface, drawer, new Point(offset.X + X, offset.Y + Y));
            }
        }
    }
}
