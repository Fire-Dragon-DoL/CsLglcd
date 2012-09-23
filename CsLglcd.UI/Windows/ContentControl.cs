using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd.UI.Windows
{
    public class ContentControl : Control
    {
        private class UserControlComparer : IComparer<UserControl>
        {
            public int Compare(UserControl x, UserControl y)
            {
                return x.Z.CompareTo(y.Z);
            }
        }

        private SortedSet<UserControl> m_Items = new SortedSet<UserControl>(new UserControlComparer());
        protected SortedSet<UserControl> Items { get { return m_Items; } set { m_Items = value; } }

        public void AddControl(UserControl item)
        {
            m_Items.Add(item);
        }

        public void RemoveControl(UserControl item)
        {
            m_Items.Remove(item);
        }
    }
}
