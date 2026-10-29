namespace AgentDesk.Views;

public partial class SplashPage : ContentPage
{
	public SplashPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

#if ANDROID
        if (Window?.Handler?.PlatformView is Android.App.Activity activity)
        {
            activity.Window.SetStatusBarColor(Android.Graphics.Color.White);

            activity.Window.DecorView.SystemUiVisibility =
                (Android.Views.StatusBarVisibility)
                Android.Views.SystemUiFlags.LightStatusBar;
        }
#endif
    }
}