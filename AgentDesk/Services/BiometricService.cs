using AgentDesk.Constants;
using AgentDesk.Services.Interfaces;
using Plugin.Maui.Biometric;

namespace AgentDesk.Services;

//public class BiometricService : IBiometricService
//{
//    private readonly IBiometric _biometric;

//    public BiometricService(IBiometric biometric)
//    {
//        _biometric = biometric;
//    }

//    public async Task<bool> IsAvailableAsync()
//    {
//        try
//        {
//            var status = await _biometric.GetAuthenticationStatusAsync();

//            return status == BiometricAuthenticationStatus.Available;
//        }
//        catch (Exception ex)
//        {
//            System.Diagnostics.Debug.WriteLine(
//                $"Biometric availability error: {ex}");

//            return false;
//        }
//    }

//    public async Task<bool> AuthenticateAsync()
//    {
//        try
//        {
//            var request = new AuthenticationRequest
//            {
//                Title = "AgentDesk Login",
//                Subtitle = "Biometric Authentication",
//                Description = "Authenticate to continue to AgentDesk",
//                NegativeText = "Cancel"
//            };

//            var result = await _biometric.AuthenticateAsync(
//                request,
//                CancellationToken.None);

//            return result.Status ==
//                   BiometricResponseStatus.Success;
//        }
//        catch (Exception ex)
//        {
//            System.Diagnostics.Debug.WriteLine(
//                $"Biometric authentication error: {ex}");

//            return false;
//        }
//    }

//    public async Task<bool> EnableAsync()
//    {
//        try
//        {
//            // First authenticate the user.
//            var authenticated =
//                await AuthenticateAsync();

//            if (!authenticated)
//                return false;

//            await SecureStorage.Default.SetAsync(
//                StorageKeys.BiometricEnabled,
//                "true");

//            return true;
//        }
//        catch (Exception ex)
//        {
//            System.Diagnostics.Debug.WriteLine(
//                $"Enable biometric error: {ex}");

//            return false;
//        }
//    }

//    public async Task DisableAsync()
//    {
//        SecureStorage.Default.Remove(
//            StorageKeys.BiometricEnabled);

//        await Task.CompletedTask;
//    }

//    public async Task<bool> IsEnabledAsync()
//    {
//        var value =
//            await SecureStorage.Default.GetAsync(
//                StorageKeys.BiometricEnabled);

//        return value == "true";
//    }
//}


public class BiometricService : IBiometricService
{
    private readonly IBiometric _biometric;

    public BiometricService(IBiometric biometric)
    {
        _biometric = biometric;
    }

    public async Task<bool> IsAvailableAsync()
    {
        try
        {
            // Check biometric hardware/status
            var status =
                await _biometric.GetAuthenticationStatusAsync();

            // Check whether biometric is enrolled
            var enrolledTypes =
                await _biometric.GetEnrolledBiometricTypesAsync();

            return status == BiometricHwStatus.Success
                   && enrolledTypes != null
                   && enrolledTypes.Any();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Biometric availability error: {ex}");

            return false;
        }
    }

    public async Task<bool> AuthenticateAsync()
    {
        try
        {
            if (!await IsAvailableAsync())
                return false;

            var request = new AuthenticationRequest
            {
                Title = "AgentDesk Login",
                Subtitle = "Biometric Authentication",
                Description =
                    "Authenticate to continue to AgentDesk",
                NegativeText = "Cancel",

                // Don't allow device PIN/password
                // as biometric fallback.
                AllowPasswordAuth = false
            };

            var result =
                await _biometric.AuthenticateAsync(
                    request,
                    CancellationToken.None);

            return result.Status ==
                   BiometricResponseStatus.Success;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Biometric authentication error: {ex}");

            return false;
        }
    }

    public async Task<bool> EnableAsync()
    {
        try
        {
            // Make sure device supports biometric
            if (!await IsAvailableAsync())
                return false;

            // Ask the user to authenticate once
            var authenticated =
                await AuthenticateAsync();

            if (!authenticated)
                return false;

            // Only save true after successful authentication
            await SecureStorage.Default.SetAsync(
                StorageKeys.BiometricEnabled,
                "true");

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Enable biometric error: {ex}");

            return false;
        }
    }

    public async Task DisableAsync()
    {
        SecureStorage.Default.Remove(
            StorageKeys.BiometricEnabled);

        await Task.CompletedTask;
    }

    public async Task<bool> IsEnabledAsync()
    {
        var value =
            await SecureStorage.Default.GetAsync(
                StorageKeys.BiometricEnabled);

        return value == "true";
    }
}