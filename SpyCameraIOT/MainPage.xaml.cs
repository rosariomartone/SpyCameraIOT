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

        private void OpenPage(string page)
        {
            switch(page)
            {
                case "Home":
                    mainFrame.Navigate(typeof(Frames.fHome));

                    break;
                case "Account":
                    mainFrame.Navigate(typeof(Frames.fAccount));

                    break;
                case "Live":
                    mainFrame.Navigate(typeof(Frames.fLive));

                    break;
                case "Settings":
                    mainFrame.Navigate(typeof(Frames.fSettings));

                    break;
                case "Info":
                    mainFrame.Navigate(typeof(Frames.fInfo));

                    break;
                case "Feedback":
                    mainFrame.Navigate(typeof(Frames.fFeedback));

                    break;
                case "Alert":
                    mainFrame.Navigate(typeof(Frames.fAlert));

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
