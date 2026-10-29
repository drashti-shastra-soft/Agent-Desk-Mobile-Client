using AgentDesk.APIService;
using AgentDesk.Services;
using AgentDesk.ViewModels.Popups;
using AgentDesk.Views;
using AgentDesk.Views.Authentication;
using AgentDesk.Views.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace AgentDesk.ViewModels;

public partial class HomePageViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<HomeMenuItem> menuItems = new();

    public HomePageViewModel()
    {
        LoadMenuItems();
    }

    private void LoadMenuItems()
    {
        MenuItems =
        [

            new HomeMenuItem
            {
                Title = "Agency",
                Icon = "home_blue_icon",
                Route = nameof(AgencyPage)
            },
            new HomeMenuItem
            {
                Title = "Group",
                Icon = "group_blue_icon",
                Route = nameof(GroupPage)
            },
            new HomeMenuItem
            {
                Title = "Client",
                Icon = "client_blue_icon",
                Route = nameof(ClientPage)
            },
            new HomeMenuItem
            {
                Title = "Policy",
                Icon = "policies_blue_icon",
                Route = nameof(PolicyPage)
            },
            new HomeMenuItem
            {
                Title = "Change Password",
                Icon = "icon_changepassword.png",
                Route = nameof(ChangePasswordPage)
            },
            new HomeMenuItem
            {
                Title = "Logout",
                Icon = "icon_logout.png",
                Route = "Logout"
            }
        ];
    }

    [RelayCommand]
    private async Task OpenMenuAsync(HomeMenuItem? item)
    {
        if (item is null)
            return;

        //if (item.Route == "Logout")
        //{
        //    bool confirm = await Shell.Current.DisplayAlert(
        //        "Logout",
        //        "Are you sure you want to logout?",
        //        "Yes",
        //        "No");

        //    if (!confirm)
        //        return;

        //    await SessionManager.LogoutAsync();
        //    return;
        //}


        if (item.Route == "Logout")
        {
            var confirm = await _dialogueService.ShowConfirmationAsync(
                 "Logout",
                 "Are you sure you want to logout?");

            if (!confirm)
                return;

            await SessionManager.LogoutAsync();
            return;
        }


        await _navigationService.NavigateToAsync(item.Route);
    }

    //[RelayCommand]
    //private async Task OpenMenuAsync(HomeMenuItem? item)
    //{
    //    if (item is null)
    //        return;

    //    // Handle Logout separately
    //    if (item.Route == "Logout")
    //    {
    //        bool confirm = await Shell.Current.DisplayAlert(
    //            "Logout",
    //            "Are you sure you want to logout?",
    //            "Yes",
    //            "No");

    //        if (!confirm)
    //            return;

    //        await SessionManager.LogoutAsync();
    //        return;
    //    }

    //    // Check internet connection
    //    if (!await NetworkService.CheckInternetAsync())
    //        return;

    //    try
    //    {
    //        // Show loader
    //        await PopLoaderService.ShowAsync();

    //        // Navigate to selected page
    //        await _navigationService.NavigateToAsync(item.Route);
    //    }
    //    catch (Exception ex)
    //    {
    //        System.Diagnostics.Debug.WriteLine(
    //            $"Navigation error: {ex.Message}");
    //    }
    //    finally
    //    {
    //        // Hide loader
    //        await PopLoaderService.HideAsync();
    //    }
    //}


}

public class HomeMenuItem
{
    public string Title { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public string Route { get; set; } = string.Empty;
}




#region Old 
//public partial class HomePageViewModelV1 : BaseViewModel
//{
//    [ObservableProperty]
//    private bool biometricEnabled;

//    public HomePageViewModel()
//    {
//        Title = "Home";
//    }

//    public async Task InitializeAsync()
//    {
//        try
//        {
//            BiometricEnabled =
//                await _biometricService
//                    .IsEnabledAsync();
//        }
//        catch (Exception ex)
//        {
//            System.Diagnostics.Debug.WriteLine(
//                $"Home initialization error: {ex}");
//        }
//        finally
//        {
//            await PopLoaderService.HideAsync();
//        }
//    }

//    [RelayCommand]
//    private async Task OpenSettingsAsync()
//    {
//        var action =
//            await Application.Current!
//                .MainPage!
//                .DisplayActionSheet(
//                    "Settings",
//                    "Cancel",
//                    null,
//                    BiometricEnabled
//                        ? "Disable Biometric Login"
//                        : "Enable Biometric Login",
//                    "Logout");

//        if (action == "Enable Biometric Login")
//        {
//            await ToggleBiometricAsync();
//        }
//        else if (action == "Disable Biometric Login")
//        {
//            await ToggleBiometricAsync();
//        }
//        else if (action == "Logout")
//        {
//            await LogoutAsync();
//        }
//    }

//    [RelayCommand]
//    private async Task ToggleBiometricAsync()
//    {
//        if (IsBusy)
//            return;

//        try
//        {
//            IsBusy = true;


//            // Currently enabled


//            if (BiometricEnabled)
//            {
//                var disable =
//                    await Application.Current!
//                        .MainPage!
//                        .DisplayAlert(
//                            "Disable Biometric Login",
//                            "Are you sure you want to disable biometric login?",
//                            "Disable",
//                            "Cancel");

//                if (!disable)
//                    return;

//                await _biometricService
//                    .DisableAsync();

//                BiometricEnabled = false;

//                await _dialogueService
//                    .ShowToastAsync(
//                        "Biometric login disabled.",
//                        ToastDuration.Short);

//                return;
//            }


//            // Currently disabled


//            var available =
//                await _biometricService
//                    .IsAvailableAsync();

//            if (!available)
//            {
//                await _dialogueService
//                    .ShowToastAsync(
//                        "Biometric authentication is not available on this device.",
//                        ToastDuration.Long);

//                return;
//            }

//            var enabled =
//                await _biometricService
//                    .EnableAsync();

//            if (!enabled)
//            {
//                await _dialogueService
//                    .ShowToastAsync(
//                        "Biometric authentication was not enabled.",
//                        ToastDuration.Long);

//                return;
//            }


//            // Mark setup completed


//            await SecureStorage.Default.SetAsync(
//                StorageKeys.BiometricSetupCompleted,
//                "true");

//            BiometricEnabled = true;

//            await _dialogueService
//                .ShowToastAsync(
//                    "Biometric login enabled.",
//                    ToastDuration.Short);
//        }
//        catch (Exception ex)
//        {
//            await _dialogueService
//                .ShowToastAsync(
//                    ex.Message,
//                    ToastDuration.Long);
//        }
//        finally
//        {
//            IsBusy = false;
//        }
//    }

//    [RelayCommand]
//    private async Task LogoutAsync()
//    {
//        if (IsBusy)
//            return;

//        try
//        {
//            var logout =
//                await Application.Current!
//                    .MainPage!
//                    .DisplayAlert(
//                        "Logout",
//                        "Are you sure you want to logout?",
//                        "Logout",
//                        "Cancel");

//            if (!logout)
//                return;

//            IsBusy = true;


//            // Clear application session


//            AppSession.AuthToken =
//                string.Empty;

//            AppSession.CurrentUser = null;


//            // Remove authentication token


//            SecureStorage.Default.Remove(
//                StorageKeys.AccessToken);

//            SecureStorage.Default.Remove(
//                StorageKeys.UserId);


//            // Remove biometric configuration


//            SecureStorage.Default.Remove(
//                StorageKeys.BiometricEnabled);

//            SecureStorage.Default.Remove(
//                StorageKeys.BiometricSetupCompleted);


//            // Go to Login


//            await _navigationService
//                .NavigateToAsync("//LoginPage");
//        }
//        catch (Exception ex)
//        {
//            await _dialogueService
//                .ShowToastAsync(
//                    ex.Message,
//                    ToastDuration.Long);
//        }
//        finally
//        {
//            IsBusy = false;
//        }
//    }
//}
#endregion