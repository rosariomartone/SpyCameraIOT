﻿using com.microsoft.maker.SecuritySystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Devices.AllJoyn;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;

namespace SpyCameraIOT
{
    public class AppController
    {
        /// <summary>
        /// Configuration settings for app
        /// </summary>
        public AppSettings XmlSettings;

        /// <summary>
        /// Storage type
        /// </summary>
        public IStorage Storage;

        /// <summary>
        /// Camera type
        /// </summary>
        public ICamera Camera;

        /// <summary>
        /// Server that runs the web interface
        /// </summary>
        public WebServer Server;

        private AllJoynManager alljoynManager;

        /// <summary>
        /// Provides status if the controller has been initialized or not
        /// </summary>
        public bool IsInitialized { get; private set; } = false;

        private string[] cameras = { "Cam1" };
        internal static DispatcherTimer uploadPicturesTimer;
        private static DispatcherTimer deletePicturesTimer;
        private const int uploadInterval = 10; //Value in seconds
        private const int deleteInterval = 1; //Value in hours

        public AppController()
        {
            Server = new WebServer();
            XmlSettings = new AppSettings();
        }

        /// <summary>
        /// Initializes the controller:  Loads settings, starts web server, sets up the camera and storage providers,
        /// tries to log into OneDrive (if OneDrive is selected), and starts the file upload and deletion timers
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
            try
            {
                // Load settings from file
                XmlSettings = await AppSettings.RestoreAsync("Settings.xml");

                // Create securitysystem-cameradrop sub folder if it doesn't exist
                StorageFolder folder = KnownFolders.PicturesLibrary;
                if (await folder.TryGetItemAsync(AppSettings.FolderName) == null)
                {
                    await folder.CreateFolderAsync(AppSettings.FolderName);
                }

                // Start web server on port 8000
                if (!Server.IsRunning)
                    Server.Start(8000);

                Camera = CameraFactory.Get(XmlSettings.CameraType);
                Storage = StorageFactory.Get(XmlSettings.StorageProvider);

                await Camera.Initialize();

                // Try to log into OneDrive using existing Access Token in settings file
                if (Storage.GetType() == typeof(OneDrive))
                {
                    var oneDrive = App.Controller.Storage as OneDrive;

                    if (oneDrive != null)
                    {
                        if (!oneDrive.IsLoggedIn())
                        {
                            try
                            {
                                await oneDrive.AuthorizeWithRefreshToken(XmlSettings.OneDriveRefreshToken);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);

                                // Log telemetry event about this exception
                                var events = new Dictionary<string, string> { { "Controller", ex.Message } };
                                TelemetryHelper.TrackEvent("FailedToLoginOneDrive", events);
                            }
                        }
                    }
                }

                this.alljoynManager = new AllJoynManager();
                await this.alljoynManager.Initialize(this.Camera, this.Storage);

                //Timer controlling camera pictures with motion
                uploadPicturesTimer = new DispatcherTimer();
                uploadPicturesTimer.Interval = TimeSpan.FromSeconds(uploadInterval);
                uploadPicturesTimer.Tick += uploadPicturesTimer_Tick;
                uploadPicturesTimer.Start();

                //Timer controlling deletion of old pictures
                deletePicturesTimer = new DispatcherTimer();
                deletePicturesTimer.Interval = TimeSpan.FromHours(deleteInterval);
                deletePicturesTimer.Tick += deletePicturesTimer_Tick;
                deletePicturesTimer.Start();

                IsInitialized = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Controller.Initialize() Error: " + ex.Message);

                // Log telemetry event about this exception
                var events = new Dictionary<string, string> { { "Controller", ex.Message } };
                TelemetryHelper.TrackEvent("FailedToInitialize", events);
            }
        }

        /// <summary>
        /// Disposes the file upload and deletion timers, camera, and storage
        /// </summary>
        public void Dispose()
        {
            try
            {
                uploadPicturesTimer?.Stop();
                deletePicturesTimer?.Stop();
                Camera?.Dispose();
                Storage?.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Controller.Dispose(): " + ex.Message);

                // Log telemetry event about this exception
                var events = new Dictionary<string, string> { { "Controller", ex.Message } };
                TelemetryHelper.TrackEvent("FailedToDispose", events);
            }

            IsInitialized = false;
        }

        private void uploadPicturesTimer_Tick(object sender, object e)
        {

            try
            {
                Storage.UploadPictures();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("uploadPicturesTimer_Tick() Exception: " + ex.Message);

                // Log telemetry event about this exception
                var events = new Dictionary<string, string> { { "Controller", ex.Message } };
                TelemetryHelper.TrackEvent("FailedToDispose", events);
            }

        }

        private void deletePicturesTimer_Tick(object sender, object e)
        {
            Storage.DeleteExpiredPictures();
        }
    }
}
