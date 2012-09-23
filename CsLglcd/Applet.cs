using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsLglcd.Interop;
using System.Runtime.InteropServices;

namespace CsLglcd
{
    public class Applet : IDisposable
    {
        #region Private
        internal lgLcdConnectContextExW connectContextEx;
        private bool m_Configurable;
        private SupportedDevices m_SupportedDevices;

        private void EnsureConnected()
        {
            if (!Connected)
                throw new AppletException("Connection not estabilished");
        }

        private void EnsureNotConnected()
        {
            if (Connected)
                throw new AppletException("Connection already estabilished");
        }

        private void EnsureNotDisposed()
        {
            if (Disposed)
                throw new ObjectDisposedException(ToString());
        }

        private uint ConfigureHandler([In] int connection, [In] IntPtr pContext)
        {
            OnConfigure();
            return 0U;
        }

        private uint NotificationHandler([In] int connection, [In] IntPtr pContext, [In] Notifications notificationCode, [In] uint notifyParam1, [In] uint notifyParam2, [In] uint notifyParam3, [In] uint notifyParam4)
        {
            DeviceTypes deviceType;
            switch (notificationCode)
            {
                case Notifications.LGLCD_NOTIFICATION_APPLET_ENABLED:
                case Notifications.LGLCD_NOTIFICATION_APPLET_DISABLED:
                    Enabled = notificationCode == Notifications.LGLCD_NOTIFICATION_APPLET_ENABLED;
                    OnEnabledChanged();
                    break;
                case Notifications.LGLCD_NOTIFICATION_CLOSE_CONNECTION:
                    if (Connected)
                    {
                        FireDisconnect();
                        OnConnectionLost();
                    }
                    break;
                case Notifications.LGLCD_NOTIFICATION_DEVICE_ARRIVAL:
                    deviceType = (DeviceTypes)notifyParam1;
                    OnDeviceArrival((Devices)Enum.ToObject(typeof(Devices), deviceType));
                    break;
                case Notifications.LGLCD_NOTIFICATION_DEVICE_REMOVAL:
                    deviceType = (DeviceTypes)notifyParam1;
                    OnDeviceRemoval((Devices)Enum.ToObject(typeof(Devices), deviceType));
                    break;
                default:
                    throw new InvalidOperationException("Invalid notification code (Applet termination code is deprecated!)");
            }
            return 0U;
        }

        private void FireDisconnect()
        {
            connectContextEx.onConfigure.configCallback = null;
            connectContextEx.onNotify.notificationCallback = null;
            connectContextEx.connection = (int)DescriptorErrors.LGLCD_INVALID_CONNECTION;
            Connected = false;
        }

        #region Event methods
        protected void OnEnabledChanged()
        {
            if (EnabledChanged != null) EnabledChanged(this, EventArgs.Empty);
        }

        protected void OnConnectionLost()
        {
            if (ConnectionLost != null) ConnectionLost(this, EventArgs.Empty);
        }

        protected void OnConfigure()
        {
            if (Configure != null) Configure(this, EventArgs.Empty);
        }

        protected void OnDeviceArrival(Devices device)
        {
            if (DeviceArrival != null) DeviceArrival(this, new DeviceEventArgs(device));
        }

        protected void OnDeviceRemoval(Devices device)
        {
            if (DeviceRemoval != null) DeviceRemoval(this, new DeviceEventArgs(device));
        }
        #endregion
        #endregion

        #region Public
        public Applet()
        {
            Configurable = false;
            Enabled = true;
            FireDisconnect();
        }

        public bool Disposed { get; private set; }
        public bool Connected { get; private set; }
        public bool Enabled { get; private set; }

        /// <summary>
        /// This event is fired only if Configurable is true
        /// </summary>
        public event EventHandler Configure;
        public event EventHandler EnabledChanged;
        public event EventHandler ConnectionLost;
        public event EventHandler<DeviceEventArgs> DeviceArrival;
        public event EventHandler<DeviceEventArgs> DeviceRemoval;

        public string Title
        {
            get { return connectContextEx.appFriendlyName; }
            set
            {
                EnsureNotDisposed();
                EnsureNotConnected();
                connectContextEx.appFriendlyName = value;
            }
        }

        public bool Autostartable
        {
            get { return connectContextEx.isAutostartable; }
            set
            {
                EnsureNotDisposed();
                EnsureNotConnected();
                connectContextEx.isAutostartable = value;
            }
        }

        public SupportedDevices SupportedDevices
        {
            get { return m_SupportedDevices; }
            set
            {
                EnsureNotDisposed();
                EnsureNotConnected();
                m_SupportedDevices = value;
            }
        }

        public bool Configurable
        {
            get { return m_Configurable; }
            set
            {
                EnsureNotDisposed();
                EnsureNotConnected();
                m_Configurable = value;
            }
        }

        public void Connect()
        {
            EnsureNotDisposed();
            EnsureNotConnected();

            if (Configurable)
                connectContextEx.onConfigure.configCallback = new lgLcdOnConfigureCB(ConfigureHandler);
            connectContextEx.onNotify.notificationCallback = new lgLcdOnNotificationCB(NotificationHandler);
            connectContextEx.dwAppletCapabilitiesSupported = (AppletCapabilities)Enum.ToObject(typeof(AppletCapabilities), SupportedDevices);

            uint result = MethodsWrapper.ConnectEx(ref connectContextEx);
            switch (result)
            {
                case (uint)Errors.ERROR_SUCCESS:
                    break;
                case (uint)Errors.ERROR_SERVICE_NOT_ACTIVE:
                    throw new AppletException(result, "Lglcd.Initialize() has not been called yet");
                case (uint)Errors.ERROR_INVALID_PARAMETER:
                    throw new AppletException(result, "Title not set");
                case (uint)Errors.ERROR_FILE_NOT_FOUND:
                    throw new AppletException(result, "Lglcd not running on the system");
                case (uint)Errors.ERROR_ALREADY_EXISTS:
                    throw new AppletException(result, "Client already connected");
                case (uint)Errors.RPC_X_WRONG_PIPE_VERSION:
                    throw new AppletException(result, "Problems detecting protocol");
                default:
                    throw new AppletException(result, "Problems estabilishing connection");
            }

            Connected = true;
        }

        public void Disconnect()
        {
            EnsureConnected();

            uint result = MethodsWrapper.Disconnect(connectContextEx.connection);
            switch (result)
            {
                case (uint)Errors.ERROR_SUCCESS:
                    break;
                case (uint)Errors.ERROR_SERVICE_NOT_ACTIVE:
                    throw new AppletException(result, "Lglcd.Initialize() has not been called yet");
                case (uint)Errors.ERROR_INVALID_PARAMETER:
                    throw new AppletException(result, "Invalid connection");
                default:
                    throw new AppletException(result, "Problems disconnecting");
            }

            FireDisconnect();
        }

        public override string ToString()
        {
            return string.Format("Applet: [# {0}] {1}", connectContextEx.connection, Title == null ? string.Empty : Title);
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
            try
            {
                if (Connected)
                    Disconnect();
            }
            catch { }

            Disposed = true;
        }

        ~Applet()
        {
            Dispose(false);
        }
        #endregion
        #endregion
    }
}
