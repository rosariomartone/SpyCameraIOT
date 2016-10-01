using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SpyCameraIOT.Frames
{
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
        }
    }
}
