#if WINDOWS
using Windows.ApplicationModel.Activation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Nventive.ExtendedSplashScreen
{
	public partial class ExtendedSplashScreen
	{
		public DataTemplate WindowsDataTemplate
		{
			get { return (DataTemplate)GetValue(WindowsDataTemplateProperty); }
			set { SetValue(WindowsDataTemplateProperty, value); }
		}

		public static readonly DependencyProperty WindowsDataTemplateProperty =
			DependencyProperty.Register("WindowsDataTemplate", typeof(DataTemplate), typeof(ExtendedSplashScreen), new PropertyMetadata(default));

		public FrameworkElement GetNativeSplashScreen(SplashScreen splashScreen)
		{
			// Splash Screen is not supported yet on WinUI. See https://github.com/microsoft/microsoft-ui-xaml/issues/4055 for more details.
			return new ContentPresenter
			{
				ContentTemplate = WindowsDataTemplate
			};
		}
	}
}
#endif
