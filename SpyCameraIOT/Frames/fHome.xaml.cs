using SpyCameraIOT.Models;
using SpyCameraIOT.Util;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpyCameraIOT.Frames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class fHome : Page
    {
        private Account _activeAccount;

        public fHome()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _activeAccount = App.account;

            if (_activeAccount != null && _activeAccount.isUserLoggedIn)
                UserNameText.Text = _activeAccount.Username;
        }

        private void Button_Forget_User_Click(object sender, RoutedEventArgs e)
        {
            // Remove it from Microsoft Passport
            MicrosoftPassportHelper.RemovePassportAccountAsync(_activeAccount);

            // Remove it from the local accounts list and resave the updated list
            AccountHelper.RemoveAccount(_activeAccount);

            // Navigate back to UserSelection page.
            Frame.Navigate(typeof(UserSelection));
        }

        private void Button_Restart_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserSelection));
        }
    }
}
