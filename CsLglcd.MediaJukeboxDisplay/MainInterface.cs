//Description:
//
//Author:
//
//Plugin Creation Date:
//__/__/____
//Version Number:
//0.0.1
//Template Created by Mr ChriZ


#region Libraries
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
#endregion

namespace CsLglcd.MediaJukeboxDisplay
{
    #region Interop Program ID Registration
    //The interop services allow the plugin to be registered with a CLSID so Media Center
    //Can find it
    //The Prog ID must match with that in the registry in order for 
    //MC to be able to pick up the plugin
    [System.Runtime.InteropServices.ProgId("CsLglcd.MediaJukeboxDisplay")]
    #endregion
    public partial class MainInterface : UserControl
    {
        #region Attributes
        //This is the Interface to Media Center
        //This is set when Media Center calls the Init Method        
        private MediaJukebox.MediaJukeboxAutomation mediaCenterReference;
        private AppletMediaJukebox AMJ;
        #endregion

        #region Constructor
        /// <summary>
        /// This is the main constructor for the plugin
        /// </summary>
        public MainInterface()
        {
            InitializeComponent();
        }
        #endregion

        #region Media Center Initialisation
        /// <summary>
        /// After the plugin has been created Media Center 
        /// will call the following method, giving us a reference
        /// to the Media Center interface.
        /// </summary>
        /// <param name="mediaCenterReference">
        /// Media Center Reference
        /// </param>        
        public void Init(MediaJukebox.MediaJukeboxAutomation mcr)
        {
            try
            {
                mediaCenterReference = mcr;
                AMJ = new AppletMediaJukebox(mediaCenterReference);
                // We don't need this, we will fire everything through a 1 second updating timer
                //mcr.FireMJEvent += new MediaJukebox.IMJAutomationEvents_FireMJEventEventHandler(MediaJukeboxHandler);
                Disposed += new EventHandler(MainInterface_Disposed);
            }
            catch (Exception e)
            {
                ExceptionHandler(e);
            }
            //Placing anything outside of this
            //try catch may cause MC to fail to open.
            //Play safe and insert it try area! 
        }

        protected virtual void MediaJukeboxHandler(string bstrType, string bstrParam1, string bstrParam2)
        {
            try
            {
                switch (bstrType)
                {
                    case "MJEvent type: MCCommand":
                        /*
                            MCC: NOTIFY_TRACK_CHANGE

                                Fired when a new track starts to play
                                string3 is the zone 

                            MCC: NOTIFY_PLAYERSTATE_CHANGE

                                Fired when a the player state changes
                                    A new track starts to play
                                    Playback is stopped, paused, or started 
                                string3 is the zone 
                     
                            MCC: NOTIFY_VOLUME_CHANGED

                            MC 12.0.217 or higher
                            Fired when the volume is changed
                            string3 is the zone 
                        */
                        switch (bstrParam1)
                        {
                            case "MCC: NOTIFY_TRACK_CHANGE":
                                break;
                            case "MCC: NOTIFY_PLAYERSTATE_CHANGE":
                                break;
                            case "MCC: NOTIFY_VOLUME_CHANGED":
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler(e);
            }
        }

        private void ExceptionHandler(Exception e)
        {
            MessageBox.Show("A Fatal error has occured while creating plugin:-" + e.Message +
                    "\n The Failure Occured" +
                    "\n In Class Object " + e.Source +
                    "\n when calling Method " + e.TargetSite +
                    "\n \n The following Inner Exception was caused" + e.InnerException +
                    "\n \n The Stack Trace Follows: \n\n" + e.StackTrace);
            Enabled = false;
        }

        private void MainInterface_Disposed(object sender, EventArgs e)
        {
            AMJ.Dispose();
        }

        #endregion

    }
}