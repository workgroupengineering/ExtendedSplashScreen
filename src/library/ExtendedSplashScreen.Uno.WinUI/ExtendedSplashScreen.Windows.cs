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
			return new ContentPresenter
			{
				ContentTemplate = WindowsDataTemplate
			};
		}
	}
}
#endif
