using System.Threading.Tasks;
#if WINUI
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
#else
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
#endif

namespace ExtendedSlashScreen.Uno.Samples
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			// Simulate some initialization work
			await Task.Delay(3000);

			// Dismiss ExtendedSplashScreen when the Main page is ready for being displayed
			Shell.Instance.ExtendedSplashScreen.Dismiss();
		}
	}
}
