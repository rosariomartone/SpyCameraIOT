﻿using SpyCameraIOT.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.ApplicationInsights;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SpyCameraIOT
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            // Initialize AI telemetry in the app.
            WindowsAppInitializer.InitializeAsync();

            // Timer uploading hearbeat telemetry event
            HeartbeatTimer = new DispatcherTimer();
            HeartbeatTimer.Tick += HeartbeatTimer_Tick;
            HeartbeatTimer.Interval = new TimeSpan(0, heartbeatInterval, 0);    // tick every heartbeatInterval
            HeartbeatTimer.Start();

            // Retrieve and set environment settings
            OSVersion = EnvironmentSettings.GetOSVersion();
            appVersion = EnvironmentSettings.GetAppVersion();
            deviceName = EnvironmentSettings.GetDeviceName();
            ipAddress = EnvironmentSettings.GetIPAddress();

            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;

            Controller = new AppController();
        }

        /// <summary>
        /// Invoked when the heartbeat timer ticks.
        /// </summary>
        void HeartbeatTimer_Tick(object sender, object e)
        {
            // Log telemetry event that the device is alive

            TelemetryHelper.TrackMetric("DeviceHeartbeat", heartbeatInterval);
        }

        /// <summary>
        /// Invoked when the application resumes from suspend.
        /// </summary>
        private async void OnResuming(Object sender, Object e)
        {
            GlobalStopwatch.Start();
            await Controller.Initialize();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            GlobalStopwatch.Start();

            await Controller.Initialize();

            TelemetryHelper.TrackEvent("Application Launched");

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            // Track App suspension/exit:
            GlobalStopwatch.Stop();
            TelemetryHelper.TrackMetric("AppRuntime", GlobalStopwatch.Elapsed.TotalMilliseconds);

            var metrics = new Dictionary<string, string> { { "appRuntime", GlobalStopwatch.Elapsed.TotalMilliseconds.ToString() } };
            TelemetryHelper.TrackEvent("UserRuntime", metrics);

            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        #region personalizedMethods

        private DispatcherTimer HeartbeatTimer;
        public static AppController Controller;
        public static Stopwatch GlobalStopwatch = Stopwatch.StartNew();
        private int heartbeatInterval = 1;  // 1 min

        // Environment settings
        private string OSVersion = "";
        private string appVersion = "";
        private string deviceName = "";
        private string ipAddress = "";
        public static Account account { get; set; }

        public static string IOTUrl = "SpyCameraIOT.azure-devices.net";

        //Desktop device
        public static string deviceIDMobile = "Windows10Desktop";
        public static string deviceKeyMobile = "7JHjJb1mm7jvzSutBA5rINarwxNwChJmu+o1rH9+qPA=";

        //Mobile device
        //public static string deviceIDMobile = "Windows10Mobile";
        //public static string deviceKeyMobile = "Newuhmd2VGTT2v3OD7AtPRhBulmkplO3N6BBRgVRxUk=";

        //IOT device
        //public static string deviceIDMobile = "Windows10IOT";
        //public static string deviceKeyMobile = "4N9pSNtuivosWRGRhaCO+y/cByMKbiK0qelniAO5PqU=";

        public enum CameraType
        {
            Ip,
            Usb
        }

        public static string IsMobile
        {
            get
            {
                var qualifiers = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues;
                return qualifiers["DeviceFamily"].ToString();
            }
        }

#endregion

    }
}
