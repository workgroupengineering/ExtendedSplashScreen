using System;
using System.Collections.Generic;
using System.Text;

namespace Nventive.ExtendedSplashScreen
{
	public interface IExtendedSplashScreenService
	{
		/// <summary>
		/// Show the extended splashscreen.
		/// </summary>
		void Show();

		/// <summary>
		/// Hides the extended splashscreen.
		/// </summary>
		void Dismiss();
	}
}
