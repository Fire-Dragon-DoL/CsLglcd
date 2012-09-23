using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd
{
    /// <summary>
    /// Used to see buttons state, it supports sorting (you can put states in a sorted list)
    /// </summary>
    public class ButtonsState : IComparable, IComparable<ButtonsState>
    {
        #region Comparing operations
        public int CompareTo(object obj)
        {
            return GetHashCode().CompareTo(obj.GetHashCode());
        }

        public int CompareTo(ButtonsState other)
        {
            return CompareTo((object)other);
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public static bool operator ==(ButtonsState a, ButtonsState b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ButtonsState a, ButtonsState b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return m_PacketNumber;
        }
        #endregion

        private int m_PacketNumber;

        #region ctor
        public ButtonsState()
            : this(0U)
        {
        }

        public ButtonsState(int buttons)
        {
            m_PacketNumber = buttons;
        }

        public ButtonsState(uint buttons)
        {
            m_PacketNumber = (int)buttons;
        }

        public ButtonsState(Buttons buttons)
        {
            m_PacketNumber = (int)((uint)buttons);
        }
        #endregion

        public int PacketNumber { get { return m_PacketNumber; } }

        public Buttons Buttons { get { return (Buttons)(uint)m_PacketNumber; } }

        public bool IsButtonDown(Buttons button) { return ((Buttons)(uint)m_PacketNumber).HasFlag(button); }
        public bool IsButtonUp(Buttons button) { return !IsButtonDown(button); }

        public bool Up { get { return IsButtonDown(Buttons.Up); } }
        public bool Down { get { return IsButtonDown(Buttons.Down); } }
        public bool Left { get { return IsButtonDown(Buttons.Left); } }
        public bool Right { get { return IsButtonDown(Buttons.Right); } }
        public bool Ok { get { return IsButtonDown(Buttons.Ok); } }
        public bool Cancel { get { return IsButtonDown(Buttons.Cancel); } }
        public bool Menu { get { return IsButtonDown(Buttons.Menu); } }

        public bool Button0 { get { return IsButtonDown(Buttons.Button0); } }
        public bool Button1 { get { return IsButtonDown(Buttons.Button1); } }
        public bool Button2 { get { return IsButtonDown(Buttons.Button2); } }
        public bool Button3 { get { return IsButtonDown(Buttons.Button3); } }

        internal void SetButtons(Buttons buttons)
        {
            m_PacketNumber = (int)(uint)buttons;
        }

        internal void SetButtons(uint buttons)
        {
            m_PacketNumber = (int)buttons;
        }

        internal void SetButtons(int buttons)
        {
            m_PacketNumber = buttons;
        }

        public override string ToString()
        {
            return Buttons.ToString();
        }
    }
}
