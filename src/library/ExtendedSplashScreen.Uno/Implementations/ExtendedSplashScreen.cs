#if NETFX_CORE || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
#endif
