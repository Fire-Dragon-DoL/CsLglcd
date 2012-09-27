using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsLglcd.Interop;

namespace CsLglcd
{
    /// <summary>
    /// When the object is disposed (or destroyed) library will be uninitialized.
    /// When the object is instanciated, library is initialized, this will allow nice using construct.
    /// </summary>
    public class Lglcd : IDisposable
    {
        public bool Disposed { get; private set; }
        private static bool m_Initialized;
        public static bool Initialized { get { return m_Initialized; } }

        public Lglcd()
        {
            if (m_Initialized)
                throw new InitializationException("Library already initialized");
            MethodsWrapper.Init();
            m_Initialized = true;
        }

        #region Dispose handling
        /// <summary>
        /// Calls automatically Disconnect if required
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                // Managed resources
            }

            // Unmanaged resources
            if (Initialized)
            {
                MethodsWrapper.DeInit();
                m_Initialized = false;
            }

            Disposed = true;
        }

        ~Lglcd()
        {
            Dispose(false);
        }
        #endregion
    }
}
