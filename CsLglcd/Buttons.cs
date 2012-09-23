using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd
{
    using InteropButtons = Interop.Buttons;

    [Flags]
    public enum Buttons : uint
    {
        None = 0U,

        //Qvga
        Left = InteropButtons.LGLCDBUTTON_LEFT,
        Right = InteropButtons.LGLCDBUTTON_RIGHT,
        Up = InteropButtons.LGLCDBUTTON_UP,
        Down = InteropButtons.LGLCDBUTTON_DOWN,
        Ok = InteropButtons.LGLCDBUTTON_OK,
        Cancel = InteropButtons.LGLCDBUTTON_DOWN,
        Menu = InteropButtons.LGLCDBUTTON_MENU,

        //Bw
        Button0 = InteropButtons.LGLCDBUTTON_BUTTON0,
        Button1 = InteropButtons.LGLCDBUTTON_BUTTON1,
        Button2 = InteropButtons.LGLCDBUTTON_BUTTON2,
        Button3 = InteropButtons.LGLCDBUTTON_BUTTON3,
        #pragma warning disable 0612, 0618
        [Obsolete]
        Button4 = InteropButtons.LGLCDBUTTON_BUTTON4,
        [Obsolete]
        Button5 = InteropButtons.LGLCDBUTTON_BUTTON5,
        [Obsolete]
        Button6 = InteropButtons.LGLCDBUTTON_BUTTON6,
        [Obsolete]
        Button7 = InteropButtons.LGLCDBUTTON_BUTTON7,
        #pragma warning restore 0612, 0618
    }
}
