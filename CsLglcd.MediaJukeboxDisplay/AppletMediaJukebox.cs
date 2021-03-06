﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaJukebox;
using CsLglcd;
using System.Threading;
using System.Drawing;
using CsLglcd.UI.Windows;

namespace CsLglcd.MediaJukeboxDisplay
{
    class AppletMediaJukebox : IDisposable
    {
        public MediaJukeboxAutomation MJ { get; private set; }
        public MainInterface UI { get; private set; }
        public readonly object SyncRoot = new object();

        private Lglcd LgLcdLib;
        private Applet LglcdApplet;
        private Device<QvgaImageUpdater> QvgaDevice;
        private volatile bool m_DeviceRemoved;
        private Timer m_InitializationTimer;
        private DateTime m_RaisePriorityUntil;
        private UpdatePriorities m_UpdatePriority = UpdatePriorities.Normal;

        public bool DeviceRemoved { get { return m_DeviceRemoved; } }

        public bool CanUpdateDevice
        {
            get
            {
                bool result = true;
                result = result && !DeviceRemoved && LglcdApplet.Connected && QvgaDevice.Attached;

                return result;
            }
        }

        public AppletMediaJukebox(MediaJukeboxAutomation mj, MainInterface ui)
        {
            MJ = mj;
            UI = ui;
            LgLcdLib = new Lglcd();

            LglcdApplet = new Applet()
            {
                SupportedDevices = SupportedDevices.QVGA,
                Title = "Media Jukebox"
            };
            QvgaDevice = new Device<QvgaImageUpdater>()
            {
                Applet = LglcdApplet
            };

            LglcdApplet.DeviceArrival += new EventHandler<DeviceEventArgs>(LglcdApplet_DeviceArrival);
            LglcdApplet.DeviceRemoval += new EventHandler<DeviceEventArgs>(LglcdApplet_DeviceRemoval);

            m_RaisePriorityUntil = DateTime.Now.Add(Properties.Settings.Default.RaisedPriorityTimeout);

            /*try
            {
                Initialize();
                InitializeLcdForm();
                UpdateValues();
                UpdateGraphics();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("A Fatal error has occured while creating plugin:-" + e.Message +
                        "\n The Failure Occured" +
                        "\n In Class Object " + e.Source +
                        "\n when calling Method " + e.TargetSite +
                        "\n \n The following Inner Exception was caused" + e.InnerException +
                        "\n \n The Stack Trace Follows: \n\n" + e.StackTrace);
            }*/

            TrackChange += new EventHandler<MediaJukeboxEventArgs>(AppletMediaJukebox_TrackChange);
            PlayerStateChange += new EventHandler<MediaJukeboxEventArgs>(AppletMediaJukebox_PlayerStateChange);

            InitializeLcdForm();

            m_InitializationTimer = new Timer(new TimerCallback(TimerHandler), null, TimeSpan.Zero, TimeSpan.FromSeconds(1.0));
        }

        private Bitmap m_DisplaySurface;
        private Graphics m_DisplayDrawer;

        private QvgaScreen m_Form;
        private ContainerControl m_Controls;
        private TextControl m_CurrentTrackTextControl;
        private ProgressBarControl m_CurrentTrackProgressControl;
        private ContainerControl m_CurrentTrackControls;
        private TextControl m_CurrentTrackPositionTextControl;
        private TextControl m_CurrentTrackDurationTextControl;
        private TextControl m_PreviousTrackTextControl;
        private TextControl m_NextTrackTextControl;

        private void UpdateGraphics()
        {
            if (!CanUpdateDevice)
                return;

            m_Form.Draw(m_DisplaySurface, m_DisplayDrawer);
            m_DisplayDrawer.Flush();
            QvgaDevice.SpecializedImageUpdater.SetPixels(m_DisplaySurface);
            QvgaDevice.Update(m_UpdatePriority);

            // FIXME: Understand why ForegroundApplet and UpdatePriority.Alert don't raise application over others
            /*System.Windows.Forms.MessageBox.Show(string.Format("ForegroundApplet: {0}\nm_UpdatePriority: {1}",
                QvgaDevice.ForegroundApplet,
                Enum.GetName(m_UpdatePriority.GetType(), m_UpdatePriority)
            ));*/
        }

        private void InitializeLcdForm()
        {
            m_DisplaySurface = QvgaDevice.SpecializedImageUpdater.CreateValidImage();
            m_DisplayDrawer = Graphics.FromImage(m_DisplaySurface);

            m_Form = new QvgaScreen(LglcdApplet, QvgaDevice)
            {
                Icon = Properties.Resources.qvga_mediajukebox,
                AppBackground = Properties.Resources.qvga_background
            };

            m_Controls = new ContainerControl();
            m_Form.Control = m_Controls;

            m_CurrentTrackTextControl = new TextControl()
            {
                Width = 304,
                Height = 48,
                X = 8,
                Y = 0,
                Z = 0
            };
            m_CurrentTrackTextControl.Format.Alignment = StringAlignment.Center;
            m_CurrentTrackTextControl.Format.LineAlignment = StringAlignment.Far;

            m_CurrentTrackProgressControl = new ProgressBarControl()
            {
                X = 8,
                Y = 56,
                Z = 1
            };
            m_CurrentTrackPositionTextControl = new TextControl()
            {
                X = 8,
                Y = 76,
                Z = 2,
                Width = 304
            };
            m_CurrentTrackDurationTextControl = new TextControl()
            {
                X = m_CurrentTrackPositionTextControl.X,
                Y = m_CurrentTrackPositionTextControl.Y,
                Z = 3,
                Width = m_CurrentTrackPositionTextControl.Width
            };
            m_CurrentTrackDurationTextControl.Format.Alignment = StringAlignment.Far;

            m_CurrentTrackControls = new ContainerControl()
            {
                Y = (m_Form.Height / 2) - (m_CurrentTrackPositionTextControl.Y) + 8,
                Z = 1
            };

            m_PreviousTrackTextControl = new TextControl()
            {
                X = 8,
                Y = 0,
                Z = 0,
                Height = 48,
                Width = 304
            };
            m_PreviousTrackTextControl.Format.LineAlignment = StringAlignment.Near;
            m_PreviousTrackTextControl.Format.Alignment = StringAlignment.Center;

            m_NextTrackTextControl = new TextControl()
            {
                X = 8,
                Y = 144,
                Z = 2,
                Height = 64,
                Width = 304
            };
            m_NextTrackTextControl.Format.LineAlignment = StringAlignment.Far;
            m_NextTrackTextControl.Format.Alignment = StringAlignment.Center;

            m_CurrentTrackControls.AddControl(m_CurrentTrackTextControl);
            m_CurrentTrackControls.AddControl(m_CurrentTrackProgressControl);
            m_CurrentTrackControls.AddControl(m_CurrentTrackPositionTextControl);
            m_CurrentTrackControls.AddControl(m_CurrentTrackDurationTextControl);

            m_Controls.AddControl(m_PreviousTrackTextControl);
            m_Controls.AddControl(m_CurrentTrackControls);
            m_Controls.AddControl(m_NextTrackTextControl);
        }

        public void Initialize()
        {
            lock (SyncRoot)
            {
                if (!LglcdApplet.Connected)
                    LglcdApplet.Connect();
                if (!QvgaDevice.Attached)
                    QvgaDevice.Attach();
            }
        }

        private void TimerHandler(object param1)
        {
            UpdateHandler();
        }

        private void UpdateHandler()
        {
            try
            {
                Initialize();
                UpdateValues();
                UpdateGraphics();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("A Fatal error has occured while creating plugin:-" + e.Message +
                        "\n The Failure Occured" +
                        "\n In Class Object " + e.Source +
                        "\n when calling Method " + e.TargetSite +
                        "\n \n The following Inner Exception was caused" + e.InnerException +
                        "\n \n The Stack Trace Follows: \n\n" + e.StackTrace);
            }
        }

        private void UpdateValues()
        {
            IMJCurPlaylistAutomation currentPlaylist = MJ.GetCurPlaylist();
            IMJFileAutomation currentTrack = currentPlaylist.GetFile(currentPlaylist.Position);
            IMJPlaybackAutomation currentPlaybackTrack = MJ.GetPlayback();

            // Update values
            if (currentPlaylist.Position == 0 && !currentPlaylist.Continuous)
                m_PreviousTrackTextControl.Text = "-";
            else
                m_PreviousTrackTextControl.Text = currentPlaylist.GetFile(currentPlaylist.GetPreviousFile()).Name;
            m_CurrentTrackTextControl.Text = currentTrack.Name;
            if (!currentPlaylist.GetCanPlayNext() || (currentPlaylist.Position >= (currentPlaylist.GetNumberFiles() - 1) && !currentPlaylist.Continuous))
                m_NextTrackTextControl.Text = "-";
            else
                m_NextTrackTextControl.Text = currentPlaylist.GetFile(currentPlaylist.GetNextFile()).Name;

            m_CurrentTrackProgressControl.Min = 0;
            m_CurrentTrackProgressControl.Max = currentTrack.Duration;
            m_CurrentTrackProgressControl.Current = currentPlaybackTrack.Position;

            m_CurrentTrackPositionTextControl.Text = TimeSpan.FromSeconds(Convert.ToDouble(currentPlaybackTrack.Position)).ToString("g");
            m_CurrentTrackDurationTextControl.Text = TimeSpan.FromSeconds(Convert.ToDouble(currentTrack.Duration)).ToString("g");

            if (m_RaisePriorityUntil > DateTime.Now)
            {
                if (!QvgaDevice.ForegroundApplet)
                    QvgaDevice.ForegroundApplet = true;
                m_UpdatePriority = UpdatePriorities.Alert;
            }
            else
            {
                if (QvgaDevice.ForegroundApplet)
                    QvgaDevice.ForegroundApplet = false;
                m_UpdatePriority = UpdatePriorities.Normal;
            }
        }

        #region Events
        public event EventHandler<MediaJukeboxEventArgs> TrackChange;
        public event EventHandler<MediaJukeboxEventArgs> PlayerStateChange;
        public event EventHandler<MediaJukeboxEventArgs> VolumeChange;

        internal protected void OnTrackChange(string param1)
        {
            if (TrackChange != null) TrackChange(this, new MediaJukeboxEventArgs(param1));
        }
        internal protected void OnPlayerStateChange(string param1)
        {
            if (PlayerStateChange != null) PlayerStateChange(this, new MediaJukeboxEventArgs(param1));
        }
        internal protected void OnVolumeChange(string param1)
        {
            if (VolumeChange != null) VolumeChange(this, new MediaJukeboxEventArgs(param1));
        }
        #endregion

        #region Event Handlers Methods
        private void LglcdApplet_DeviceRemoval(object sender, DeviceEventArgs e)
        {
            m_DeviceRemoved = true;
        }

        private void LglcdApplet_DeviceArrival(object sender, DeviceEventArgs e)
        {
            m_DeviceRemoved = false;
        }

        private void AppletMediaJukebox_PlayerStateChange(object sender, MediaJukeboxEventArgs e)
        {
        }

        private void AppletMediaJukebox_TrackChange(object sender, MediaJukeboxEventArgs e)
        {
            if (Properties.Settings.Default.IncreasePriorityOnTrackChange)
            {
                DateTime now = DateTime.Now;
                if (m_RaisePriorityUntil < now)
                    m_RaisePriorityUntil = now.Add(Properties.Settings.Default.RaisedPriorityTimeout);
                else
                    m_RaisePriorityUntil = m_RaisePriorityUntil.Add(Properties.Settings.Default.RaisedPriorityTimeout);
            }
        }
        #endregion

        public void Dispose()
        {
            lock (SyncRoot)
                m_InitializationTimer.Dispose();
            m_DisplayDrawer.Dispose();
            m_DisplaySurface.Dispose();
            QvgaDevice.Dispose();
            LglcdApplet.Dispose();
            LgLcdLib.Dispose();
        }
    }
}
