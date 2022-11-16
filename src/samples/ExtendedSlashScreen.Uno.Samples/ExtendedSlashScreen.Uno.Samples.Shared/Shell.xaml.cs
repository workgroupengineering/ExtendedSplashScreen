using Nventive.ExtendedSplashScreen;
#if WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
#else
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
#endif

namespace ExtendedSlashScreen.Uno.Samples
{
	public sealed partial class Shell : UserControl
	{
		public static Shell Instance { get; private set; }

		public Shell(LaunchActivatedEventArgs e)
		{
			this.InitializeComponent();

			Instance = this;

#if WINDOWS_UWP
			AppExtendedSplashScreen.SplashScreen = e?.SplashScreen;
#endif

			NavigationFrame.Navigate(typeof(MainPage), e.Arguments);
		}

		public IExtendedSplashScreen ExtendedSplashScreen => this.AppExtendedSplashScreen;

		public Frame NavigationFrame => this.RootNavigationFrame;
	}
}
