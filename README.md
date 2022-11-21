# Extended SplashScreen
Extended splashscreen allows to control when to dismiss the splascreen. It also gives the developer the ability to add additional xaml content to display while the application is being loaded.

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

## Getting Started
Install the latest version of `Nventive.ExtendedSplashScreen.Uno` or `Nventive.ExtendedSplashScreen.Uno.WinUI`.

In order for this package to work, you also need to setup the rootframe in app.xaml.cs to be a custom UserControl instead of a frame.
``` c#			
Shell rootFrame = window.Content as Shell;

// just ensure that the window is active
if (rootFrame == null)
{
  // Use a UserControl that will contain the frame for naviation as a rootframe.
  rootFrame = new Shell(e);

  window.Content = rootFrame;
}
 ```

In the User control, you need to put a frame that will be used for navigation and which will show the content of the app, as well as the Extended splashscreen.
It is important to put the Splascreen lower than the frame so that the splash screen content goes over the content of the frame.
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

In the code behind of the user control you can set the splashscreen for UWP, and make a reference to the shell in Instance and make a public property to represent the ExtendedSplashscreen.            
```C#
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

    NavigationFrame.Navigate(typeof(MainPage), e.Arguments);
  }

  public IExtendedSplashScreen ExtendedSplashScreen => this.AppExtendedSplashScreen;

  public Frame NavigationFrame => this.RootNavigationFrame;
}
```

You can reference the instance property later for navigation and you can dismiss the ExtendedSplashScreen with this line of code: 

```c#
Shell.Instance.ExtendedSplashScreen.Dismiss();
```

Then, all you need to do is to set the style.
```xml
<Style x:Key="DefaultExtendedSplashScreenStyle"
       TargetType="splash:ExtendedSplashScreen">
  <Setter Property="Template">
    <Setter.Value>
      <ControlTemplate TargetType="splash:ExtendedSplashScreen">
        <Grid x:Name="RootGrid">
          <VisualStateManager.VisualStateGroups>
             <VisualStateGroup x:Name="SplashScreenStates">
              <VisualState x:Name="Normal" />
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

          <!-- Here you can put the content you want in your ExtendedSplashscreen. The Content presenter here is the splashscreen that you have. You can put anything you want below it to make it appear over the splashscreen. -->
          <ContentPresenter x:Name="SplashScreenPresenter" />
        </Grid>
      </ControlTemplate>
    </Setter.Value>
  </Setter>
</Style>
```

## Features

{More details/listing of features of the project}

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

