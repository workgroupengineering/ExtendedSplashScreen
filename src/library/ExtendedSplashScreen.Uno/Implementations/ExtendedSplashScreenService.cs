#if NETFX_CORE || __ANDROID__ || __IOS__ || __WASM__
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Uno;
using Windows.UI.Core;

namespace Nventive.ExtendedSplashScreen
{
#if __ANDROID__ || __IOS__
	[Uno.Preserve(AllMembers = true)]
#endif
	public class ExtendedSplashScreenService : IExtendedSplashScreenService
	{
		private readonly CoreDispatcher _dispatcher;
		private readonly IExtendedSplashScreen _extendedSplashScreen;
		private readonly ILogger _logger;

		public ExtendedSplashScreenService(
			CoreDispatcher dispatcher,
			IExtendedSplashScreen extendedSplashScreen,
			ILogger<ExtendedSplashScreenService> logger
		)
		{
			_dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
			_extendedSplashScreen = extendedSplashScreen ?? throw new ArgumentNullException(nameof(extendedSplashScreen));
			_logger = (ILogger)logger ?? NullLogger.Instance;
		}

		/// <inheritdoc />
		public void Show()
		{
			_ = _dispatcher.RunAsync(CoreDispatcherPriority.Normal, ShowOnUI);

			void ShowOnUI()
			{
				try
				{
					_extendedSplashScreen.Show();
				}
				catch (Exception e)
				{
					_logger.LogError(0, e, "Failed to show the extended splashscreen.");
				}
			}
		}

		/// <inheritdoc />
		public void Dismiss()
		{
			_ = _dispatcher.RunAsync(CoreDispatcherPriority.Normal, DismissOnUI);

			void DismissOnUI()
			{
				try
				{
					_extendedSplashScreen.Dismiss();
				}
				catch (Exception e)
				{
					_logger.LogError(0, e, "Failed to dismiss the extended splashscreen.");
				}
			}
		}
	}
}
#endif
