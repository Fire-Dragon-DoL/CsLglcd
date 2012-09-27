using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsLglcd.Interop;
using System.Runtime.InteropServices;

namespace CsLglcd
{
    public class Device : IDisposable
    {
        #region Private
        private lgLcdOpenByTypeContext openByTypeContext;

        private Applet m_Applet;
        private Devices m_DeviceType;
        private Buttons m_LastButtons;
        private uint m_LastButtonsState;
        private ButtonsState m_ButtonsState = new ButtonsState();
        private bool m_ForegroundApplet;

        private void EnsureImageUpdaterNotNull()
        {
            if (ImageUpdater == null)
                throw new DeviceException("Missing ImageUpdater");
        }

        private void EnsureAttached()
        {
            if (!Attached)
                throw new DeviceException("Device not activated");
        }

        private void EnsureNotAttached()
        {
            if (Attached)
                throw new DeviceException("Device already activated");
        }

        private void EnsureNotDisposed()
        {
            if (Disposed)
                throw new ObjectDisposedException(ToString());
        }

        private uint SoftButtonsHandler([In] int device, [In] uint dwButtons, [In] IntPtr pContext)
        {
            Buttons buttons = (Buttons)dwButtons;
            OnButtonsDown(buttons & ~m_LastButtons);
            OnButtonsUp(m_LastButtons & ~buttons);

            m_LastButtons = buttons;
            return 0U;
        }

        private void FireDetach()
        {
            openByTypeContext.connection = (int)DescriptorErrors.LGLCD_INVALID_CONNECTION;
            openByTypeContext.device = (int)DescriptorErrors.LGLCD_INVALID_DEVICE;

            Attached = false;
        }

        #region Event methods
        protected void OnButtonsDown(Buttons buttons)
        {
            if (ButtonsDown != null) ButtonsDown(this, new ButtonsEventArgs(buttons));
        }

        protected void OnButtonsUp(Buttons buttons)
        {
            if (ButtonsUp != null) ButtonsUp(this, new ButtonsEventArgs(buttons));
        }
        #endregion
        #endregion

        #region Public
        public Device()
        {
            openByTypeContext.onSoftbuttonsChanged.softbuttonsChangedCallback = new lgLcdOnSoftButtonsCB(SoftButtonsHandler);
            DeviceType = Devices.BlackAndWhite;
            FireDetach();
        }

        public bool Disposed { get; private set; }
        public bool Attached { get; private set; }
        public IImageUpdater ImageUpdater { get; set; }

        /// <summary>
        /// Event fired when button is pressed
        /// </summary>
        public event EventHandler<ButtonsEventArgs> ButtonsDown;
        /// <summary>
        /// Event fired when button is released
        /// </summary>
        public event EventHandler<ButtonsEventArgs> ButtonsUp;

        public Applet Applet
        {
            get { return m_Applet; }
            set
            {
                EnsureNotDisposed();
                EnsureNotAttached();
                m_Applet = value;
            }
        }

        public Devices DeviceType
        {
            get { return m_DeviceType; }
            set
            {
                EnsureNotDisposed();
                EnsureNotAttached();
                m_DeviceType = value;
            }
        }

        public bool ForegroundApplet
        {
            get { return m_ForegroundApplet; }
            set
            {
                EnsureAttached();

                // NOTE: This has been disabled because we want the possibility to keep forcing ForegroundApplet = true if we are "fighting" against another applet doing the same
                /*if (value == m_ForegroundApplet)
                    return;*/

                uint result = MethodsWrapper.SetAsLCDForegroundApp(openByTypeContext.device, value ? ForegroundYesNoFlags.LGLCD_LCD_FOREGROUND_APP_YES : ForegroundYesNoFlags.LGLCD_LCD_FOREGROUND_APP_NO);
                switch (result)
                {
                    case (uint)Errors.ERROR_SUCCESS:
                        break;
                    case (uint)Errors.ERROR_LOCK_FAILED:
                        throw new DeviceException(result, "The operation could not be completed");
                    default:
                        throw new DeviceException(result, "Problems showing applet as foreground");
                }

                m_ForegroundApplet = value;
            }
        }

        public void Attach()
        {
            EnsureNotDisposed();
            EnsureNotAttached();

            openByTypeContext.connection = Applet.connectContextEx.connection;
            openByTypeContext.deviceType = (DeviceTypes)Enum.ToObject(typeof(DeviceTypes), DeviceType);

            uint result = MethodsWrapper.OpenByType(ref openByTypeContext);
            switch (result)
            {
                case (uint)Errors.ERROR_SUCCESS:
                    break;
                case (uint)Errors.ERROR_SERVICE_NOT_ACTIVE:
                    throw new DeviceException(result, "Lglcd.Initialize() has not been called yet");
                case (uint)Errors.ERROR_INVALID_PARAMETER:
                    throw new DeviceException(result, "Invalid applet connection or device type");
                case (uint)Errors.ERROR_ALREADY_EXISTS:
                    throw new DeviceException(result, "Device already activated for used applet");
                default:
                    throw new DeviceException(result, "Problems attaching the device");
            }

            Attached = true;
        }

        public void Detach()
        {
            EnsureAttached();

            uint result = MethodsWrapper.Close(openByTypeContext.device);
            switch (result)
            {
                case (uint)Errors.ERROR_SUCCESS:
                    break;
                case (uint)Errors.ERROR_SERVICE_NOT_ACTIVE:
                    throw new DeviceException(result, "Lglcd.Initialize() has not been called yet");
                case (uint)Errors.ERROR_INVALID_PARAMETER:
                    throw new DeviceException(result, "Device handle is invalid");
                default:
                    throw new DeviceException(result, "Problems detaching the device");
            }

            FireDetach();
        }

        /// <summary>
        /// Return the buttons state of the device, it uses a sort of small cache to reduce memory consumption.
        /// </summary>
        /// <returns></returns>
        public ButtonsState GetState()
        {
            EnsureAttached();

            Interop.Buttons buttons = Interop.Buttons.None;
            uint result = MethodsWrapper.ReadSoftButtons(openByTypeContext.device, ref buttons);
            switch (result)
            {
                case (uint)Errors.ERROR_SUCCESS:
                    break;
                case (uint)Errors.ERROR_SERVICE_NOT_ACTIVE:
                    throw new DeviceException(result, "Lglcd.Initialize() has not been called yet");
                case (uint)Errors.ERROR_INVALID_PARAMETER:
                    throw new DeviceException(result, "Device handle is invalid");
                case (uint)Errors.ERROR_DEVICE_NOT_CONNECTED:
                    // Not connected means it has been usb-removed, that's why I don't use "detached"
                    throw new DeviceException(result, "Device not connected");
                default:
                    throw new DeviceException(result, "Problems getting device buttons state");
            }

            if ((uint)buttons != m_LastButtonsState)
            {
                m_LastButtonsState = (uint)buttons;
                m_ButtonsState = new ButtonsState(m_LastButtonsState);
            }

            return m_ButtonsState;
        }

        public bool Update(UpdatePriorities updatePriority = UpdatePriorities.Normal, UpdateStyles updateStyle = UpdateStyles.Async, bool avoidNotConnectedExceptions = true)
        {
            EnsureAttached();
            EnsureImageUpdaterNotNull();

            uint result = MethodsWrapper.UpdateBitmap(openByTypeContext.device, ImageUpdater.UnmanagedBitmapHeaderPointer, ((uint)updateStyle) | ((uint)updatePriority));
            switch (result)
            {
                case (uint)Errors.ERROR_SUCCESS:
                    break;
                case (uint)Errors.ERROR_SERVICE_NOT_ACTIVE:
                    throw new DeviceException(result, "Lglcd.Initialize() has not been called yet");
                case (uint)Errors.ERROR_INVALID_PARAMETER:
                    throw new DeviceException(result, "Device handle is invalid or image is invalid");
                case (uint)Errors.ERROR_DEVICE_NOT_CONNECTED:
                    // We try to avoid throwing this exception type because it happens really often
                    if (avoidNotConnectedExceptions)
                        return false;
                    // Not connected means it has been usb-removed, that's why I don't use "detached"
                    throw new DeviceException(result, "Device not connected");
                case (uint)Errors.ERROR_ACCESS_DENIED:
                    throw new DeviceException(result, "Synchronous operation was not displayed on the LCD within the frame interval (30 ms). Error happens only with UpdateStyles.SyncComplete");
                default:
                    throw new DeviceException(result, "Problems updating device image");
            }

            return true;
        }

        public override string ToString()
        {
            return string.Format("Device: {0}, Applet: {1}", Enum.GetName(DeviceType.GetType(), DeviceType), Applet != null ? Applet.ToString() : "None");
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
            if (Attached)
                Detach();

            Disposed = true;
        }

        ~Device()
        {
            Dispose(false);
        }
        #endregion
        #endregion
    }

    public class Device<T> : Device where T : IImageUpdater, new()
    {
        public Device() : base() { ImageUpdater = new T(); }

        public T SpecializedImageUpdater
        {
            get { return (T)ImageUpdater; }
            set { ImageUpdater = value; }
        }
    }
}
