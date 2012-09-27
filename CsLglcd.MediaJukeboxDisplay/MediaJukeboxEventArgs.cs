using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsLglcd.MediaJukeboxDisplay
{
    class MediaJukeboxEventArgs : EventArgs
    {
        public string Param1 { get; set; }
        public MediaJukeboxEventArgs(string param1) { Param1 = param1; }
    }
}
