#if __WASM__
using System;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Nventive.ExtendedSplashScreen
{
	public partial class ExtendedSplashScreen
	{
		private FrameworkElement GetNativeSplashScreen(SplashScreen splashScreen)
		{
			// ExtendedSplashscreen is not implemented on WASM.
			return null;
		}
	}
}
#endif
