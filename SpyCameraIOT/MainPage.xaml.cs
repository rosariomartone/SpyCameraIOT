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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpyCameraIOT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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

        //private async void alertMessage()  
        //{
        //    var messageDialog = new MessageDialog("You need to login with your Microsoft and set up your preference in Settings.");
        //    await messageDialog.ShowAsync();
        //}

        private async void alertDialog()
        {
            var dialog = new ContentDialog()
            {
                Title = "SpyCameraIOT: WARNING",
                //RequestedTheme = ElementTheme.Dark,
                //FullSizeDesired = true,
                MaxWidth = this.ActualWidth // Required for Mobile!
            };

            // Setup Content
            var panel = new StackPanel();

            panel.Children.Add(new TextBlock
            {
                Text = "You need to login with your Microsoft and set up your preference in Settings.",
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

            // Add Buttons
            dialog.PrimaryButtonText = "OK";
            dialog.IsPrimaryButtonEnabled = false;
            dialog.SecondaryButtonText = "Cancel";

            // Show Dialog
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
    }
}
