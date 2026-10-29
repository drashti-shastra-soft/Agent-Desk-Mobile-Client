using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace AgentDesk
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set background color of the status bar
            //Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#0078B4")); // Blue color
            //Window?.SetNavigationBarColor(Android.Graphics.Color.ParseColor("#0078B4"));
            //Window.DecorView.SystemUiVisibility = 0;
            //if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            //{
            //    Window!.DecorView.SystemUiVisibility =
            //        (StatusBarVisibility)SystemUiFlags.LightStatusBar;
            //}
        }

 
    }
}
