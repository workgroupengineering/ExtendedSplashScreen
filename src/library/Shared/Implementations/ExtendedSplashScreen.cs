using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
#if WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace Nventive.ExtendedSplashScreen
{
	/// <summary>
	/// Displays a view that replicates the look and behavior of the native splash screen
	/// </summary>
	[TemplatePart(Name = SplashScreenPresenterPartName, Type = typeof(ContentPresenter))]
	[TemplateVisualState(GroupName = SplashScreenPresenterPartName, Name = NormalStateName)]
	[TemplateVisualState(GroupName = SplashScreenPresenterPartName, Name = DismissedStateName)]
    public partial class ExtendedSplashScreen : Control, IExtendedSplashScreen
	{
		private const string SplashScreenPresenterPartName = "SplashScreenPresenter";
		private const string NormalStateName = "Normal";
		private const string DismissedStateName = "Dismissed";

		private bool _isDismissed;

		public SplashScreen SplashScreen { get; set; }

		/// <summary>
		/// Alternative to <see cref="SplashScreen"/>.
		/// Splash Screen is not supported yet on WinUI. See https://github.com/microsoft/microsoft-ui-xaml/issues/4055 for more details.
		/// </summary>
		public DataTemplate WindowsDataTemplate
		{
			get { return (DataTemplate)GetValue(WindowsDataTemplateProperty); }
			set { SetValue(WindowsDataTemplateProperty, value); }
		}

		public static readonly DependencyProperty WindowsDataTemplateProperty =
			DependencyProperty.Register("WindowsDataTemplate", typeof(DataTemplate), typeof(ExtendedSplashScreen), new PropertyMetadata(default));

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (GetTemplateChild(SplashScreenPresenterPartName) is ContentPresenter splashScreenPresenter)
			{
				splashScreenPresenter.Content = GetNativeSplashScreen(SplashScreen);
			}

			UpdateSplashScreenState(useTransitions: false);
		}

		/// <inheritdoc />
		public void Show()
		{
			_isDismissed = false;

			UpdateSplashScreenState(useTransitions: true);
		}

		/// <inheritdoc />
		public void Dismiss()
		{
			if (_isDismissed)
			{
				return;
			}

			_isDismissed = true;
			UpdateSplashScreenState(useTransitions: true);
		}

		private void UpdateSplashScreenState(bool useTransitions)
		{
			var state = _isDismissed
				? DismissedStateName
				: NormalStateName;

			VisualStateManager.GoToState(this, state, useTransitions);
		}
	}
}
