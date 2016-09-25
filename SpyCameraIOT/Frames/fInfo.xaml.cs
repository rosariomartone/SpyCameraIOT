using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using SpyCameraIOT.IOT;
using Microsoft.Azure.Devices.Client;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpyCameraIOT.Frames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class fInfo : Page
    {
        public fInfo()
        {
            this.InitializeComponent();
        }

        private void tbDeviceInfo_Loaded(object sender, RoutedEventArgs e)
        {
            getInfo();
        }

        private void getInfo()
        {
            tbDeviceInfo.Text = App.IsMobile;

            string DeviceConnectionString = "HostName=SpyCameraIOT.azure-devices.net;DeviceId=device8f2d0f5d3dfe4670a1ac29370f670623;SharedAccessKey=XereImdIBJc43Ss6cCQRS4xvS3SFmHZICIW5HxvMj6o=";
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);
            IOTMessages.SendEvent(deviceClient).Wait();
        }
    }
}
