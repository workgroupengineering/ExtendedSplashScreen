#if __ANDROID__
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Extensions.Logging;
using System;
using Uno.UI;
using Windows.Foundation;
#if WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using UnoCanvas = Microsoft.UI.Xaml.Controls.Canvas;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using UnoCanvas = Windows.UI.Xaml.Controls.Canvas;
#endif

namespace Nventive.ExtendedSplashScreen
{
	[Preserve(AllMembers = true)]
	public partial class ExtendedSplashScreen
	{
		private static ExtendedSplashScreen _instance;
		private static Bitmap _splashBitmap;
		private static AndroidX.Core.SplashScreen.SplashScreen _androidSplashScreen;
		private static bool? _isNativeViewValid;

		/// <summary>
		/// Gets or sets the Android SplashScreen.<br/>
		/// Set this in your the OnCreate of your Activity using the result from InstallSplashScreen before calling base.OnCreate.<br/>
		/// Setting this changes the behavior of the ExtendedSplashScreen to use the Android 12+ native SplashScreen (instead of the WindowBackground).
		/// <example>
		/// <code>
		/// protected override void OnCreate(Bundle bundle)
		/// { 
		///	    // Handle the splash screen transition.
		///	    ExtendedSplashScreen.AndroidSplashScreen = AndroidX.Core.SplashScreen.SplashScreen.InstallSplashScreen(this);
		/// 
		///	    base.OnCreate(bundle);
		///	}
		///	</code>
		/// </example>
		/// </summary>
		public static AndroidX.Core.SplashScreen.SplashScreen AndroidSplashScreen
		{
			get => _androidSplashScreen;
			set
			{
				_androidSplashScreen = value;
				_androidSplashScreen.ExitAnimation += OnExitAnimation;
			}
		}

		private static void OnExitAnimation(object sender, AndroidX.Core.SplashScreen.SplashScreen.ExitAnimationEventArgs e)
		{
			Trace("Started OnExitAnimation.");

			var view = e.SplashScreenViewProvider.View;

			// It seems like the view can sometimes be invalid according to https://github.com/nventive/ExtendedSplashScreen/issues/76.
			_isNativeViewValid = view.Width > 0 && view.Height > 0;

			if (_isNativeViewValid.Value)
			{
				_splashBitmap = GetBitmapFromView(view);
			}
			else
			{
				Trace("Native view is invalid because at least one of its dimension is 0.");
			}

			if (_instance._splashScreenPresenter != null)
			{
				Trace("Setting ExtendedSplashScreen content.");
				_instance._splashScreenPresenter.Content = _instance.GetNativeSplashScreen(null);
				Trace("Set ExtendedSplashScreen content. Removing native SplashScreen.");

				// Remove the native splash now that we've set the extended one.
				e.SplashScreenViewProvider.Remove();
				Trace("Removed native SplashScreen.");
			}
			Trace("Finished OnExitAnimation.");
		}

		public static Bitmap GetBitmapFromView(View view)
		{
			Trace("GetBitmapFromView: Generating bitmap.");

			//Define a bitmap with the same size as the view
			Bitmap returnedBitmap = Bitmap.CreateBitmap(view.Width, view.Height, Bitmap.Config.Argb8888);

			Trace("GetBitmapFromView: Generating canvas.");

			//Bind a canvas to it
			Android.Graphics.Canvas canvas = new Android.Graphics.Canvas(returnedBitmap);
			//Get the view's background

			Drawable bgDrawable = view.Background;
			if (bgDrawable != null)
			{
				Trace("GetBitmapFromView: Drawing background.");

				//has background drawable, then draw it on the canvas
				bgDrawable.Draw(canvas);
			}
			else
			{
				Trace("GetBitmapFromView: Drawing white background.");

				//does not have background drawable, then draw white background on the canvas
				canvas.DrawColor(Android.Graphics.Color.White);
			}

			Trace("GetBitmapFromView: Drawing view.");

			// draw the view on the canvas
			view.Draw(canvas);

			Trace("GetBitmapFromView: Generated bitmap.");

			//return the bitmap
			return returnedBitmap;
		}

		private FrameworkElement GetNativeSplashScreen(Windows.ApplicationModel.Activation.SplashScreen splashScreen)
		{
			try
			{
				if (AndroidSplashScreen != null && _splashBitmap == null && !_isNativeViewValid.HasValue)
				{
					Trace("AndroidSplashScreen is not null. Waiting for native splashscreen animation exit to get the native view.");

					_instance = this;
					return null;
				}

				var activity = ContextHelper.Current as Activity;

				// Get the splash screen size
				var splashScreenSize = GetSplashScreenSize(activity);

				View splashView = null;

				if (_isNativeViewValid.Value)
				{
					var imageView = new ImageView(activity);
					imageView.SetImageBitmap(_splashBitmap);
					splashView = imageView;
				}

				if (splashView == null)
				{
					Trace("Android 12 splash screen bitmap unavailable. Falling back to default behavior.");

					// Get the theme's windowBackground (which we use as splash screen)
					var attribute = new Android.Util.TypedValue();
					activity?.Theme.ResolveAttribute(Android.Resource.Attribute.WindowBackground, attribute, true);
					var windowBackgroundResourceId = attribute.ResourceId;

					// Create the splash screen surface
					splashView = new Android.Views.View(activity);
					splashView.SetBackgroundResource(attribute.ResourceId);
				}

				// We use a Canvas to ensure it's clipped but not resized (important when device has soft-keys)
				var element = new UnoCanvas
				{
					// We set a transparent background to prevent touches from going through
					Background = new SolidColorBrush(Android.Graphics.Color.Transparent),
					// We use a Border to ensure proper layout
					Children =
					{
						new Border()
						{
							Width = splashScreenSize.Width,
							Height = splashScreenSize.Height,
							Child = VisualTreeHelper.AdaptNative(splashView),
						}
					},
				};

				return element;
			}
			catch (Exception e)
			{
				Logger.LogError(0, e, "Error while getting native splash screen.");

				return null;
			}
		}

		private static Size GetSplashScreenSize(Activity activity)
		{
			var physicalDisplaySize = new Android.Graphics.Point();
			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
			{
				// The windowBackground takes the size of the screen (only when using Theme.AppCompat.*)
				activity.WindowManager.DefaultDisplay.GetRealSize(physicalDisplaySize);
			}
			else
			{
				// The windowBackground takes the size of the screen minus the bottom navigation bar
				activity.WindowManager.DefaultDisplay.GetSize(physicalDisplaySize);
			}

			return new Size(
				ViewHelper.PhysicalToLogicalPixels(physicalDisplaySize.X),
				ViewHelper.PhysicalToLogicalPixels(physicalDisplaySize.Y)
			);
		}

		private static void Trace(string message)
		{
			if (Logger.IsEnabled(LogLevel.Trace))
			{
				Logger.LogTrace(message);
			}
		}
	}
}
#endif
