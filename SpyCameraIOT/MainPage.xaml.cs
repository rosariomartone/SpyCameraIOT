using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Runtime.InteropServices;
using Microsoft.Azure.Devices.Client;
using System.Text;
using SpyCameraIOT.IOT;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpyCameraIOT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static DeviceClient deviceClient;

        public MainPage()
        {
            this.InitializeComponent();

        }

        #region menu

        private void OpenCloseMenu()
        {
            svMain.IsPaneOpen = !svMain.IsPaneOpen;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            OpenCloseMenu();
        }

        #endregion

        #region menuSelection

        private bool isLoggedIn()
        {
            if (App.account != null && App.account.isUserLoggedIn)
                return true;
            else
                return false;
        }

        private bool isUserSetUp()
        {
            if (App.account != null && App.account.isUserSetUp)
                return true;
            else
                return false;
        }

        private async void alertDialog()
        {
            var dialog = new ContentDialog()
            {
                Title = "SpyCameraIOT: WARNING",
                MaxWidth = this.ActualWidth
            };

            var panel = new StackPanel();

            panel.Children.Add(new TextBlock
            {
                Text = "You need to login and set up your preference before using this functionality.",
                TextWrapping = TextWrapping.Wrap,
            });

            var cb = new CheckBox
            {
                Content = "Agree"
            };

            cb.SetBinding(CheckBox.IsCheckedProperty, new Binding
            {
                Source = dialog,
                Path = new PropertyPath("IsPrimaryButtonEnabled"),
                Mode = BindingMode.TwoWay,
            });

            panel.Children.Add(cb);
            dialog.Content = panel;

            dialog.PrimaryButtonText = "OK";
            dialog.IsPrimaryButtonEnabled = false;
            dialog.SecondaryButtonText = "Cancel";

            var result = await dialog.ShowAsync();
        }

        private void OpenPage(string page)
        {
            switch(page)
            {
                case "Home":
                    if (isLoggedIn())
                        mainFrame.Navigate(typeof(Frames.fHome));
                    else
                        alertDialog();

                    break;
                case "Account":
                    mainFrame.Navigate(typeof(Frames.fAccount));

                    break;
                case "Live":
                    if (isLoggedIn() && isUserSetUp())
                        mainFrame.Navigate(typeof(Frames.fLive));
                    else
                        alertDialog();

                    break;
                case "Settings":
                    if (isLoggedIn())
                        mainFrame.Navigate(typeof(Frames.fSettings));
                    else
                        alertDialog();

                    break;
                case "Info":
                    mainFrame.Navigate(typeof(Frames.fInfo));

                    break;
                case "Feedback":
                    mainFrame.Navigate(typeof(Frames.fFeedback));

                    break;
                case "Alert":
                    if (isLoggedIn() && isUserSetUp())
                        mainFrame.Navigate(typeof(Frames.fAlert));
                    else
                        alertDialog();

                    break;
            }

            svMain.IsPaneOpen = false;
        }

        private void spHome_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenPage("Home");
        }

        private void spAccount_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenPage("Account");
        }

        private void spLive_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenPage("Live");
        }

        private void spSettings_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenPage("Settings");
        }

        private void spInfo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenPage("Info");
        }

        private void spFeedback_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenPage("Feedback");
        }

        private void spAlert_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenPage("Alert");
        }

        #endregion

        private async void mainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            if(App.IsMobile.Equals("IoT"))
            {
                spHome.Visibility = Visibility.Collapsed;
                spAccount.Visibility = Visibility.Collapsed;
                spLive.Visibility = Visibility.Collapsed;
                spSettings.Visibility = Visibility.Collapsed;
                spFeedback.Visibility = Visibility.Collapsed;
                spAlert.Visibility = Visibility.Collapsed;
            }

            while (true)
            {
                deviceClient = DeviceClient.Create(App.IOTUrl, new DeviceAuthenticationWithRegistrySymmetricKey(App.deviceIDMobile, App.deviceKeyMobile));

                Microsoft.Azure.Devices.Client.Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                if (App.IsMobile.Equals("IoT"))
                    txtMainMessages.Text = txtMainMessages.Text + "\n" + Encoding.ASCII.GetString(receivedMessage.GetBytes());
                else
                    alertDialog(Encoding.ASCII.GetString(receivedMessage.GetBytes()));

                if (App.IsMobile.Equals("IoT"))
                    await IOTMessages.SendCloudToDeviceMessageAsync("Windows10Desktop", "SPYCameraIOT > Ping received from " + App.deviceIDMobile + " on " + System.DateTime.Now.ToString());

                await deviceClient.CompleteAsync(receivedMessage);
            }
        }

        private async void alertDialog(string message)
        {
            var dialog = new ContentDialog()
            {
                Title = "SpyCameraIOT: MESSAGES",
                MaxWidth = this.ActualWidth
            };

            var panel = new StackPanel();

            panel.Children.Add(new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
            });

            var cb = new CheckBox
            {
                Content = "Agree"
            };

            cb.SetBinding(CheckBox.IsCheckedProperty, new Binding
            {
                Source = dialog,
                Path = new PropertyPath("IsPrimaryButtonEnabled"),
                Mode = BindingMode.TwoWay,
            });

            panel.Children.Add(cb);
            dialog.Content = panel;

            dialog.PrimaryButtonText = "OK";
            dialog.IsPrimaryButtonEnabled = false;
            dialog.SecondaryButtonText = "Cancel";

            var result = await dialog.ShowAsync();
        }
    }
}
