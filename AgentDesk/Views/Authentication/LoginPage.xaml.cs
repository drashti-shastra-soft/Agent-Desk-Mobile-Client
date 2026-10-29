using AgentDesk.ViewModels;
using AgentDesk.ViewModels.Popups;
using AgentDesk.Views.Popups;
using Mopups.Services;

namespace AgentDesk.Views.Authentication;

public partial class LoginPage : ContentPage
{
    public LoginPageViewModel ViewModel { get; set; }
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = ViewModel;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.OnAppearing();

    }

    //    protected override bool OnBackButtonPressed()
    //    {
    //#if ANDROID
    //        MainThread.BeginInvokeOnMainThread(async () =>
    //        {
    //            bool exit = await DisplayAlert(
    //                "Exit App",
    //                "Are you sure you want to exit?",
    //                "Yes",
    //                "No");

    //            if (exit)
    //            {
    //                Android.OS.Process.KillProcess(
    //                    Android.OS.Process.MyPid());
    //            }
    //        });

    //        return true;
    //#else
    //        return base.OnBackButtonPressed();
    //#endif
    //    }

    //    protected override bool OnBackButtonPressed()
    //    {
    //        MainThread.BeginInvokeOnMainThread(async () =>
    //        {
    //            var viewModel = new CommonPopupPageViewModel(
    //                "Exit App",
    //                "Are you sure you want to exit?");

    //            var popup = new CommonPopupPage(viewModel);

    //            viewModel.Confirmed += async (s, e) =>
    //            {
    //                await MopupService.Instance.PopAsync();

    //#if ANDROID
    //                Android.OS.Process.KillProcess(
    //                    Android.OS.Process.MyPid());
    //#endif
    //            };

    //            viewModel.Cancelled += async (s, e) =>
    //            {
    //                await MopupService.Instance.PopAsync();
    //            };

    //            await MopupService.Instance.PushAsync(popup);
    //        });

    //        return true;
    //    }


    protected override bool OnBackButtonPressed()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            bool confirm = await ViewModel._dialogueService.ShowConfirmationAsync(
                "Exit App",
                "Are you sure you want to exit?");

            if (!confirm)
                return;

#if ANDROID
            Android.OS.Process.KillProcess(
                Android.OS.Process.MyPid());
#endif
        });

        return true;
    }

    //protected override void OnDisappearing()
    //{
    //    base.OnDisappearing();

    //    ViewModel.OnDisappearing();

    //}
}