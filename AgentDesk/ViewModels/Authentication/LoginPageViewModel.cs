using AgentDesk.Constants;
using AgentDesk.Helpers;
using AgentDesk.Models.Auth;
using AgentDesk.Services;
using AgentDesk.Views;
using AgentDesk.Views.Authentication;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace AgentDesk.ViewModels;

public partial class LoginPageViewModel : BaseViewModel
{
    private bool isLoadingRememberedLogin;
    private bool hasLoadedRememberedLogin;

    [ObservableProperty]
    private bool rememberMe;

    [ObservableProperty]
    private string passwordIcon = "icon_hide";

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter email address.")]
    [CustomValidation(typeof(ValidationHelper),nameof(ValidationHelper.ValidateEmail))]
    private string emailAddress = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter password.")]
    private string password = string.Empty;

    [ObservableProperty]
    private bool isPasswordVisible;

    public string EmailAddressError =>
        GetFirstError(nameof(EmailAddress));

    public bool HasEmailAddressError =>
        !string.IsNullOrEmpty(EmailAddressError);

    public string PasswordError =>
        GetFirstError(nameof(Password));

    public bool HasPasswordError =>
        !string.IsNullOrEmpty(PasswordError);

    public LoginPageViewModel()
    {
        ErrorsChanged += OnValidationErrorsChanged;
    }


    public async Task OnAppearing()
    {
       await LoadRememberedLoginAsync();
    }
    public void OnDisappearing()
    {
        if (!RememberMe)
        {
            ClearCredentials();
        }

        ResetValidation();
    }

    public async Task LoadRememberedLoginAsync()
    {
        if (hasLoadedRememberedLogin)
        {
            if (!RememberMe)
            {
                ClearCredentials();
            }

            ResetValidation();
            return;
        }

        isLoadingRememberedLogin = true;

        try
        {
            RememberMe = AppSetting.GetRememberMe();

            if (RememberMe)
            {
                EmailAddress = AppSetting.GetEmail();
                Password = await AppSetting.GetPasswordAsync()
                           ?? string.Empty;
            }
            else
            {
                ClearCredentials();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Unable to load remembered login: {ex}");

            ClearCredentials();
        }
        finally
        {
            isLoadingRememberedLogin = false;
            hasLoadedRememberedLogin = true;
        }

        ResetValidation();
    }

    private void ClearCredentials()
    {
        EmailAddress = string.Empty;
        Password = string.Empty;
    }
    public void ResetValidation()
    {
        HasStartedEmailInput = false;
        HasAttemptedValidation = false;

        ClearErrors();

        OnPropertyChanged(nameof(EmailAddressError));
        OnPropertyChanged(nameof(HasEmailAddressError));
        OnPropertyChanged(nameof(PasswordError));
        OnPropertyChanged(nameof(HasPasswordError));
    }
    partial void OnEmailAddressChanged(string value)
    {
        if (!HasStartedEmailInput && string.IsNullOrWhiteSpace(value))
            return;

        HasStartedEmailInput = true;

        ValidateProperty(value, nameof(EmailAddress));
    }

    partial void OnPasswordChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(Password));
    }

    partial void OnIsPasswordVisibleChanged(bool value)
    {
        OnPropertyChanged(nameof(PasswordIcon));
    }

    private void OnValidationErrorsChanged(object? sender,DataErrorsChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.PropertyName) ||
            e.PropertyName == nameof(EmailAddress))
        {
            OnPropertyChanged(nameof(EmailAddressError));
            OnPropertyChanged(nameof(HasEmailAddressError));
        }

        if (string.IsNullOrEmpty(e.PropertyName) ||
            e.PropertyName == nameof(Password))
        {
            OnPropertyChanged(nameof(PasswordError));
            OnPropertyChanged(nameof(HasPasswordError));
        }
    }

    partial void OnRememberMeChanged(bool value)
    {
        if (isLoadingRememberedLogin)
            return;

        AppSetting.SetRememberMe(value);

        if (!value)
        {
            // Remove saved credentials when Remember Me is disabled.
            AppSetting.SetEmail(string.Empty);
            AppSetting.SetPassword(string.Empty);
        }
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
            return;

        if (!ValidateForm())
            return;

        if (!await NetworkService.CheckInternetAsync())
            return;

        try
        {
            IsBusy = true;

            await PopLoaderService.ShowAsync();

            var credential = new Credentials
            {
                EmailAddress = EmailAddress.Trim(),
                Password = Password.Trim()
            };

            var response = await _authService.LoginAsync(credential);

            if (response == null || !response.IsSuccess)
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Login failed.",
                    ToastDuration.Long);

                return;
            }

            //if (response == null || !response.IsSuccess)
            //{
            //    // Uncheck Remember Me
            //    RememberMe = false;

            //    // Clear saved credentials
            //    AppSetting.SetRememberMe(false);
            //    AppSetting.SetEmail(string.Empty);
            //    AppSetting.SetPassword(string.Empty);

            //    await _dialogueService.ShowToastAsync(
            //        response?.ErrorMessage ?? "Login failed.",
            //        ToastDuration.Long);

            //    return;
            //}

            if (response.Value is not User user)
            {
                await _dialogueService.ShowToastAsync(
                    "Invalid user response.",
                    ToastDuration.Long);

                return;
            }

            // Store logged-in user
            AppSession.CurrentUser = user;

            // Store authentication token
            if (!string.IsNullOrWhiteSpace(user.Token))
            {
                AppSession.AuthToken = user.Token;

                await SecureStorage.Default.SetAsync(
                    StorageKeys.AccessToken,
                    user.Token);
            }

            // Store User Id
            if (user.Id != 0)
            {
                await SecureStorage.Default.SetAsync(
                    StorageKeys.UserId,
                    user.Id.ToString());
            }

            // Store complete User object
            var userJson = JsonSerializer.Serialize(user);

            await SecureStorage.Default.SetAsync(
                StorageKeys.CurrentUser,
                userJson);

            // Remember Me
            if (RememberMe)
            {
                AppSetting.SetEmail(EmailAddress.Trim());
                AppSetting.SetPassword(Password);
            }
            else
            {
                AppSetting.SetEmail(string.Empty);
                AppSetting.SetPassword(string.Empty);
            }

            AppSetting.SetRememberMe(RememberMe);

            // Login successful

            //await _dialogueService.ShowToastAsync(
            //    "Login successful!",
            //    ToastDuration.Short);

            // Go to Home

            //await _navigationService.NavigateToAsync("//HomePage");

            await _navigationService.NavigateToAsync(nameof(HomePage));
        }
        catch (Exception ex)
        {
            await _dialogueService.ShowToastAsync(
                ex.Message,
                ToastDuration.Long);
        }
        finally
        {
            await PopLoaderService.HideAsync();
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ForgotPasswordAsync()
    {
        await _navigationService.NavigateToAsync(nameof(ForgotPasswordPage));
    }

    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
        PasswordIcon = IsPasswordVisible
        ? "icon_unhide"
        : "icon_hide";
    }

    [RelayCommand]
    private void ToggleRememberMe()
    {
        RememberMe = !RememberMe;
    }


    #region withBiometric 
    //[RelayCommand]
    //private async Task LoginAsync()
    //{
    //    if (IsBusy)
    //        return;

    //    if (!await NetworkService.CheckInternetAsync())
    //        return;

    //    if (!await ValidateControl())
    //        return;

    //    try
    //    {
    //        IsBusy = true;

    //        await PopLoaderService.ShowAsync();

    //        var credential = new Credentials
    //        {
    //            EmailAddress = EmailAddress.Trim(),

    //            Password = Password.Trim()
    //        };

    //        var response =
    //            await _authService.LoginAsync(credential);

    //        if (response == null ||
    //            !response.IsSuccess)
    //        {
    //            await _dialogueService
    //                .ShowToastAsync(
    //                    response?.ErrorMessage ??
    //                    "Login failed.",
    //                    ToastDuration.Long);

    //            return;
    //        }

    //        if (response.Value is not User user)
    //        {
    //            await _dialogueService
    //                .ShowToastAsync(
    //                    "Invalid user response.",
    //                    ToastDuration.Long);

    //            return;
    //        }

    //        // Store logged-in user
    //        AppSession.CurrentUser = user;

    //        // Store authentication token
    //        if (!string.IsNullOrWhiteSpace(user.Token))
    //        {
    //            AppSession.AuthToken = user.Token;
    //            await SecureStorage.Default.SetAsync(StorageKeys.AccessToken,user.Token);
    //        }


    //        if (user.Id != 0)
    //        {
    //            await SecureStorage.Default.SetAsync(
    //                StorageKeys.UserId,
    //                user.Id.ToString());
    //        }
    //        // Remember Me
    //        if (RememberMe)
    //        {
    //            AppSetting.SetEmail(EmailAddress.Trim());

    //            AppSetting.SetPassword(Password);
    //        }
    //        else
    //        {
    //            AppSetting.SetEmail(string.Empty);

    //            AppSetting.SetPassword(string.Empty);
    //        }

    //        AppSetting.SetRememberMe(
    //            RememberMe);


    //        //await _navigationService.NavigateToAsync(nameof(HomePage));
    //        //await _navigationService.NavigateToAsync("//HomePage");

    //        await HandleBiometricSetupAsync();
    //        await _dialogueService.ShowToastAsync( "Login successful!",ToastDuration.Short);
    //    }
    //    catch (Exception ex)
    //    {
    //        await _dialogueService.ShowToastAsync( ex.Message,ToastDuration.Long);
    //    }
    //    finally
    //    {
    //        await PopLoaderService.HideAsync();
    //        IsBusy = false;
    //    }
    //}
    #endregion

    //private async Task HandleBiometricSetupAsync()
    //{
    //    try
    //    {
    //        
    //        // Has the user already answered the biometric
    //        // setup question?
    //        

    //        var setupCompleted =
    //            await SecureStorage.Default.GetAsync(
    //                StorageKeys.BiometricSetupCompleted);

    //        if (setupCompleted == "true")
    //        {
    //            await _navigationService
    //                .NavigateToAsync("//HomePage");

    //            return;
    //        }

    //        
    //        // Check whether biometric is available
    //        

    //        var available =
    //            await _biometricService
    //                .IsAvailableAsync();

    //        if (!available)
    //        {
    //            // Don't ask again on every login.
    //            await SecureStorage.Default.SetAsync(
    //                StorageKeys.BiometricSetupCompleted,
    //                "true");

    //            await _navigationService
    //                .NavigateToAsync("//HomePage");

    //            return;
    //        }

    //        
    //        // Ask user
    //        

    //        var enable =
    //            await Application.Current!
    //                .MainPage!
    //                .DisplayAlert(
    //                    "Enable Biometric Login",
    //                    "Would you like to use fingerprint or Face ID for faster login?",
    //                    "Enable",
    //                    "Not Now");

    //        // Important:
    //        // After the user answers once, don't ask again
    //        // automatically.
    //        await SecureStorage.Default.SetAsync(
    //            StorageKeys.BiometricSetupCompleted,
    //            "true");

    //        
    //        // User selected Not Now
    //        

    //        if (!enable)
    //        {
    //            await _navigationService
    //                .NavigateToAsync("//HomePage");

    //            return;
    //        }

    //        
    //        // User selected Enable
    //        

    //        var enabled =
    //            await _biometricService
    //                .EnableAsync();

    //        if (enabled)
    //        {
    //            await _dialogueService
    //                .ShowToastAsync(
    //                    "Biometric login enabled.",
    //                    ToastDuration.Short);
    //        }
    //        else
    //        {
    //            await _dialogueService
    //                .ShowToastAsync(
    //                    "Biometric login could not be enabled.",
    //                    ToastDuration.Long);
    //        }

    //        await _navigationService
    //            .NavigateToAsync("//HomePage");
    //    }
    //    catch (Exception ex)
    //    {
    //        System.Diagnostics.Debug.WriteLine(
    //            $"Biometric setup error: {ex}");

    //        // Login itself succeeded, so don't trap
    //        // the user on the login page.
    //        await _navigationService
    //            .NavigateToAsync("//HomePage");
    //    }
    //}
}

