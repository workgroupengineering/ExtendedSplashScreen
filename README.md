# Extended SplashScreen
Extended splashscreen allows to control when to dismiss the splashscreen. It also gives the developer the ability to add additional xaml content to display while the application is being loaded.

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg?style=flat-square)](LICENSE)
 ![Version](https://img.shields.io/nuget/v/Nventive.ExtendedSplashScreen.Uno?style=flat-square)
 ![Downloads](https://img.shields.io/nuget/dt/Nventive.ExtendedSplashScreen.Uno?style=flat-square)

## Getting Started
1. Install the latest version of `Nventive.ExtendedSplashScreen.Uno` or `Nventive.ExtendedSplashScreen.Uno.WinUI`.

1. Setup the root content of the window to be a custom `UserControl` instead of a `Frame`.
   (We called this `UserControl` **"Shell"** in the following steps.)
    ``` c#			
    Shell shell = window.Content as Shell;

    if (shell == null)
    {
      shell = new Shell(e);

      window.Content = rootFrame;
    }
    ```

1. In the `UserControl`, put the following:
   - Put a `Frame` (or anything else that you need) to display the app content.
   - Put the `ExtendedSplashScreen` control.
   
   It is important to put the `ExtendedSplashScreen` control _below_ than the `Frame` so that the splash screen hides the app content.
    ```XML
    <UserControl x:Class="ExtendedSlashScreen.Uno.Samples.Shell"
                ...
                xmlns:splash="using:Nventive.ExtendedSplashScreen">

      <Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">

        <Frame x:Name="RootNavigationFrame" />

        <splash:ExtendedSplashScreen x:Name="AppExtendedSplashScreen" />
      </Grid>
    </UserControl>
    ```

1. Expose the `IExtendedSplashScreen` publicly from the code behind of you `UserControl`.

   In this sample we do it by exposing a `ExtendedSplashScreen` property and containing the `IExtendedSplashScreen` and a static `Instance` property containing the latest instance of the `Shell`.
    ```C#
    public sealed partial class Shell : UserControl
    {
      public static Shell Instance { get; private set; }

      public Shell(LaunchActivatedEventArgs e)
      {
        this.InitializeComponent();

        Instance = this;

    #if WINDOWS_UWP // Do this only if you target UWP.
        AppExtendedSplashScreen.SplashScreen = e?.SplashScreen;
    #endif

        NavigationFrame.Navigate(typeof(MainPage), e.Arguments);
      }

      public IExtendedSplashScreen ExtendedSplashScreen => this.AppExtendedSplashScreen;

      public Frame NavigationFrame => this.RootNavigationFrame;
    }
    ```

1. Add code to dismiss the `ExtendedSplashScreen` for when your app is ready to be displayed.

    ```c#
    Shell.Instance.ExtendedSplashScreen.Dismiss();
    ```

1. Create a style for the `ExtendedSplashScreen` control.
    ```xml
    <Style x:Key="DefaultExtendedSplashScreenStyle"
          TargetType="splash:ExtendedSplashScreen">
      <Setter Property="Template">
        <Setter.Value>
          <ControlTemplate TargetType="splash:ExtendedSplashScreen">
            <Grid x:Name="RootGrid">
              <VisualStateManager.VisualStateGroups>
                <VisualStateGroup x:Name="SplashScreenStates">
                  <!-- The Normal visual state represents the state when the extended splash screen is shown. -->
                  <VisualState x:Name="Normal" />

                  <!-- The Dismissed visual state represents the state when the extended splash screen is dismissed. -->
                  <VisualState x:Name="Dismissed">
                    <Storyboard>
                      <ObjectAnimationUsingKeyFrames Storyboard.TargetName="RootGrid"
                                                     Storyboard.TargetProperty="Visibility">
              <DiscreteObjectKeyFrame KeyTime="0:0:0.150"
                                      Value="Collapsed" />
                      </ObjectAnimationUsingKeyFrames>
                      <DoubleAnimation Storyboard.TargetName="RootGrid"
                                       Storyboard.TargetProperty="Opacity"
                                       To="0"
                                       Duration="0:0:0.150" />
                    </Storyboard>
                  </VisualState>
                </VisualStateGroup>
              </VisualStateManager.VisualStateGroups>

              <!-- This ContentPresenter shows a copy of the native splashscreen. -->
              <ContentPresenter x:Name="SplashScreenPresenter" />

              <!-- You can add custom content in this template, such as a loading animation. -->
            </Grid>
          </ControlTemplate>
        </Setter.Value>
      </Setter>
    </Style>
    ```

1. Apply the style to the `ExtendedSplashScreen` control in your `UserControl`.
    ```xml
    <splash:ExtendedSplashScreen x:Name="AppExtendedSplashScreen"
                                 Style="{StaticResource DefaultExtendedSplashScreenStyle}" />
    ```
   > 💡 You can skip this part if you setup an implicit style.

## Android 12+
The native splash screen behavior changes starting at Android 12.
See [reference](https://developer.android.com/develop/ui/views/launch/splash-screen).

`ExtendedSplashScreen` supports the Android 12+ behavior.
You need to add the following code in your MainActivity.cs to override the default behavior.
```csharp
public sealed class MainActivity : Microsoft.UI.Xaml.ApplicationActivity
{
	protected override void OnCreate(Bundle bundle)
	{
		// Handle the splash screen transition.
		Nventive.ExtendedSplashScreen.ExtendedSplashScreen.AndroidSplashScreen = AndroidX.Core.SplashScreen.SplashScreen.InstallSplashScreen(this);

		// It's important to call base.OnCreate AFTER setting ExtendedSplashScreen.AndroidSplashScreen.
		base.OnCreate(bundle);
	}
}
```

Note that when you run your app in debug from Visual Studio (or other IDEs), the new SplashScreen icon doesn't show.
It shows when you run the app from the launcher (even debug builds).

## Trace Logs
You can enable trace logs on the `Nventive.ExtendedSplashScreen` namespace to get more information about the runtime behavior.

You can override the logger via `ExtendedSplashScreen.Logger`.
By default, one is created using the `AmbientLoggerFactory` from the `Uno.Core.Extensions.Logging.Singleton` package.


## Changelog

Please consult the [CHANGELOG](CHANGELOG.md) for more information about version
history.

## License

This project is licensed under the Apache 2.0 license - see the
[LICENSE](LICENSE) file for details.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on the process for
contributing to this project.

Be mindful of our [Code of Conduct](CODE_OF_CONDUCT.md).

