using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsLglcd.Interop;

namespace CsLglcd
{
    public static class Lglcd
    {
        public static bool Initialized { get; private set; }
        public static void Initialize()
        {
            if (!Initialized)
            {
                MethodsWrapper.Init();
                Initialized = true;
            }
            else
                throw new InitializationException("Library already initialized");
        }

        public static void Uninitialize()
        {
            if (Initialized)
            {
                MethodsWrapper.DeInit();
                Initialized = false;
            }
            else
                throw new InitializationException("Library not initialized");
        }
    }
}
