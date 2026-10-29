using AgentDesk.Constants;
using AgentDesk.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.APIService;

public static class SessionManager
{
    private static bool _isLoggingOut;

    public static async Task LogoutAsync()
    {
        if (_isLoggingOut)
            return;

        _isLoggingOut = true;

        try
        {
            
            // Clear App Session
            
            AppSession.AuthToken = null;
            AppSession.CurrentUser = null;

            
            // Clear SecureStorage
            
            SecureStorage.Default.Remove(
                StorageKeys.AccessToken);

            SecureStorage.Default.Remove(
                StorageKeys.UserId);

            SecureStorage.Default.Remove(
                StorageKeys.CurrentUser);

            
            // Optional: clear Remember Me
            
            // If you want to keep remembered email/password,
            // don't clear these.
            //
            // AppSetting.SetEmail(string.Empty);
            // AppSetting.SetPassword(string.Empty);
            // AppSetting.SetRememberMe(false);

            
            // Navigate to Login
            
            await MainThread.InvokeOnMainThreadAsync(
                async () =>
                {
                    await Shell.Current.GoToAsync(
                        "//LoginPage");
                });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Logout error: {ex}");
        }
        finally
        {
            _isLoggingOut = false;
        }
    }
}
