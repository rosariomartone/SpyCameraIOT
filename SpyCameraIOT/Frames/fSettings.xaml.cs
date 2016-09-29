using Microsoft.Azure.Devices;
using SpyCameraIOT.IOT;
using SpyCameraIOT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpyCameraIOT.Frames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class fSettings : Page
    {
        public fSettings()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Frames.fHome));
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Device> devices = await IOTMessages.GetDevicesList();

            foreach (Device device in devices)
            {
                IOTDevice dev = new IOTDevice();

                if (device.Status.Equals(DeviceStatus.Enabled))
                    dev.IsEnabled = true;

                dev.LastExecutionTime = device.LastActivityTime;
                dev.ConnectionStateIOT = device.ConnectionState.ToString();
                dev.IdIOT = device.Id;

                HubSection hubSection = new HubSection();
                DataTemplate template = this.Resources["TemplateGrid"] as DataTemplate;

                TextBlock headerTextBlock = new TextBlock();
                headerTextBlock.Text = device.Id;
                hubSection.Header = headerTextBlock;
                hubSection.Padding = new Thickness(40, 30, 150, 44);
                headerTextBlock.Foreground = new SolidColorBrush(Colors.LightSteelBlue);

                hubSection.DataContext = dev;
                hubSection.ContentTemplate = template;
                hSettings.Sections.Add(hubSection);
            }
        }

        private void btnPing_Click(object sender, RoutedEventArgs e)
        {
            string deviceName = ((Button)sender).Tag.ToString();

            //Send a message to the device through IOT Hub 
        }
    }
}
