using AgentDesk.Constants;
using AgentDesk.Helpers;
using AgentDesk.Models.Auth;
using AgentDesk.Services.Interfaces;
using AgentDesk.Views;
using System.Text.Json;

namespace AgentDesk.Services;
public class AppStartupService : IAppStartupService
{
    private readonly INavigationService _navigationService;

    public AppStartupService(
        INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Get saved authentication token
            var token =
                await SecureStorage.Default.GetAsync(
                    StorageKeys.AccessToken);

            // User is NOT logged in
            if (string.IsNullOrWhiteSpace(token))
            {
                AppSession.AuthToken = null;
                AppSession.CurrentUser = null;

                await _navigationService
                    .NavigateToAsync("//LoginPage");

                return;
            }

            // User is ALREADY logged in
            AppSession.AuthToken = token;

            // Restore User
            var userJson = await SecureStorage.Default.GetAsync(
                StorageKeys.CurrentUser);

            if (string.IsNullOrWhiteSpace(userJson))
            {
                AppSession.Clear();

                await _navigationService
                    .NavigateToAsync("//LoginPage");

                return;
            }

            var user = JsonSerializer.Deserialize<User>(userJson);

            if (user == null)
            {
                AppSession.Clear();

                await _navigationService
                    .NavigateToAsync("//LoginPage");

                return;
            }

            // Restore CurrentUser
            AppSession.CurrentUser = user;

            // Restore User Id
            var userId =
                await SecureStorage.Default.GetAsync(
                    StorageKeys.UserId);

            // If your AppSession has UserId:
            // if (int.TryParse(userId, out var id))
            // {
            //     AppSession.UserId = id;
            // }

            // Go directly to Home
            //await _navigationService.NavigateToAsync("//HomePage");

            await _navigationService.NavigateToAsync(nameof(HomePage));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Startup authentication error: {ex}");

            // If something goes wrong,
            // send user to Login.
            AppSession.AuthToken = null;
            AppSession.CurrentUser = null;

            await _navigationService
                .NavigateToAsync("//LoginPage");
        }
    }
}

//public class AppStartupService : IAppStartupService
//{
//    private readonly IBiometricService _biometricService;

//    private readonly INavigationService _navigationService;

//    public AppStartupService(
//        IBiometricService biometricService,
//        INavigationService navigationService)
//    {
//        _biometricService =
//            biometricService;

//        _navigationService =
//            navigationService;
//    }

//    public async Task InitializeAsync()
//    {
//        try
//        {
//            
//            // Get saved authentication token
//            

//            var token =
//                await SecureStorage.Default.GetAsync(
//                    StorageKeys.AccessToken);

//            
//            // No token = user is not logged in
//            

//            if (string.IsNullOrWhiteSpace(token))
//            {
//                await _navigationService
//                    .NavigateToAsync("//LoginPage");

//                return;
//            }

//            
//            // Restore session token
//            

//            AppSession.AuthToken = token;

//            
//            // Check biometric setting
//            

//            var biometricEnabled =
//                await _biometricService
//                    .IsEnabledAsync();

//            
//            // Biometric not enabled
//            // Go directly to Home
//            

//            if (!biometricEnabled)
//            {
//                await _navigationService
//                    .NavigateToAsync("//HomePage");

//                return;
//            }

//            
//            // Biometric is enabled
//            // Check hardware/enrollment
//            

//            var biometricAvailable =
//                await _biometricService
//                    .IsAvailableAsync();

//            if (!biometricAvailable)
//            {
//                // Biometric was enabled previously but is
//                // currently unavailable.
//                //
//                // Require normal login.
//                await _navigationService
//                    .NavigateToAsync("//LoginPage");

//                return;
//            }

//            
//            // Ask for fingerprint / Face ID
//            

//            var authenticated =
//                await _biometricService
//                    .AuthenticateAsync();

//            
//            // Biometric successful
//            

//            if (authenticated)
//            {
//                await _navigationService
//                    .NavigateToAsync("//HomePage");

//                return;
//            }

//            
//            // Biometric failed/cancelled
//            

//            // IMPORTANT:
//            // Do NOT disable biometric here.
//            //
//            // User can login normally.
//            // On the next app launch, biometric will still
//            // be enabled.
//            await _navigationService
//                .NavigateToAsync("//LoginPage");
//        }
//        catch (Exception ex)
//        {
//            System.Diagnostics.Debug.WriteLine(
//                $"Startup authentication error: {ex}");

//            await _navigationService
//                .NavigateToAsync("//LoginPage");
//        }
//    }
//}



