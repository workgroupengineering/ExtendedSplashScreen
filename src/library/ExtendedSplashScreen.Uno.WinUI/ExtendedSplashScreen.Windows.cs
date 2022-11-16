#if WINDOWS
using Windows.ApplicationModel.Activation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Nventive.ExtendedSplashScreen
{
	public partial class ExtendedSplashScreen
	{
		public FrameworkElement GetNativeSplashScreen(SplashScreen splashScreen)
		{
			// Splashscreen is not supported yet on winui https://github.com/microsoft/microsoft-ui-xaml/issues/4055
			return default(FrameworkElement);
		}
	}
}
#endif
