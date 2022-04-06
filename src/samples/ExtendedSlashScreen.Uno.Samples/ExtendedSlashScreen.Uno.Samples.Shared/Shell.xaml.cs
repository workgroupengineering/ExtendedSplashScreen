using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Nventive.ExtendedSplashScreen;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
		}

		public IExtendedSplashScreen ExtendedSplashScreen => this.AppExtendedSplashScreen;

		public Frame NavigationFrame => this.RootNavigationFrame;
	}
}
