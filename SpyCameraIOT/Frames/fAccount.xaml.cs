using SpyCameraIOT.Models;
using SpyCameraIOT.MSPassport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class fAccount : Page
    {
        private Account _account;
        private bool _isExistingAccount;

        public fAccount()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Check Microsoft Passport is setup and available on this machine
            if (await MicrosoftPassportHelper.MicrosoftPassportAvailableCheckAsync())
            {
                if (e.Parameter != null)
                {
                    _isExistingAccount = true;
                    // Set the account to the existing account being passed in
                    _account = (Account)e.Parameter;
                    UsernameTextBox.Text = _account.Username;
                    SignInPassport();
                }
            }
            else
            {
                // Microsoft Passport is not setup so inform the user
                PassportStatus.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 50, 170, 207));
                PassportStatusText.Text = "Microsoft Passport is not setup!\n" +
                    "Please go to Windows Settings and set up a PIN to use it.";
                PassportSignInButton.IsEnabled = false;
            }
        }

        private void PassportSignInButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Text = "";
            SignInPassport();
        }

        private async void SignInPassport()
        {
            if (AccountHelper.ValidateAccountCredentials(UsernameTextBox.Text))
            {
                // Create and add a new local account
                _account = AccountHelper.AddAccount(UsernameTextBox.Text);

                if (await MicrosoftPassportHelper.CreatePassportKeyAsync(UsernameTextBox.Text))
                {
                    _account.isUserLoggedIn = true;

                    App.account = _account;

                    this.Frame.Navigate(typeof(Frames.fHome));
                }
            }
            else
                ErrorMessage.Text = "Invalid Credentials";
        }
    }
}
