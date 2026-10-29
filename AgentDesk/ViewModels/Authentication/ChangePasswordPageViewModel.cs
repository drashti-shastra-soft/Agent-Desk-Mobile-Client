using AgentDesk.Helpers;
using AgentDesk.Models.Auth;
using AgentDesk.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AgentDesk.ViewModels.Authentication;

public partial class ChangePasswordPageViewModel : BaseViewModel
{
    [ObservableProperty]
    [Required(ErrorMessage = "Please enter current password.")]
    private string currentPassword = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter new password.")]
    private string newPassword = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter confirm password.")]
    [property: Compare(
        nameof(NewPassword),
        ErrorMessage = "New password and confirm password do not match.")]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private bool isCurrentPasswordHidden = true;

    [ObservableProperty]
    private bool isNewPasswordHidden = true;

    [ObservableProperty]
    private bool isConfirmPasswordHidden = true;

    public string CurrentPasswordError =>
       GetFirstError(nameof(CurrentPassword));

    public bool HasCurrentPasswordError =>
        !string.IsNullOrEmpty(CurrentPasswordError);

    public string NewPasswordError =>
        GetFirstError(nameof(NewPassword));

    public bool HasNewPasswordError =>
        !string.IsNullOrEmpty(NewPasswordError);

    public string ConfirmPasswordError =>
        GetFirstError(nameof(ConfirmPassword));

    public bool HasConfirmPasswordError =>
        !string.IsNullOrEmpty(ConfirmPasswordError);

    public ChangePasswordPageViewModel()
    {
        Title = "Change Password";
        ErrorsChanged += OnValidationErrorsChanged;
    }

    private void OnValidationErrorsChanged(
        object? sender,
        DataErrorsChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.PropertyName) ||
            e.PropertyName == nameof(CurrentPassword))
        {
            OnPropertyChanged(nameof(CurrentPasswordError));
            OnPropertyChanged(nameof(HasCurrentPasswordError));
        }

        if (string.IsNullOrEmpty(e.PropertyName) ||
            e.PropertyName == nameof(NewPassword))
        {
            OnPropertyChanged(nameof(NewPasswordError));
            OnPropertyChanged(nameof(HasNewPasswordError));

            // Confirm password must be revalidated when the
            // new password changes.
            if (HasAttemptedValidation)
            {
                ValidateProperty(
                    ConfirmPassword,
                    nameof(ConfirmPassword));
            }
        }

        if (string.IsNullOrEmpty(e.PropertyName) ||
            e.PropertyName == nameof(ConfirmPassword))
        {
            OnPropertyChanged(nameof(ConfirmPasswordError));
            OnPropertyChanged(nameof(HasConfirmPasswordError));
        }
    }

    partial void OnCurrentPasswordChanged(string value)
    {
        if (HasAttemptedValidation)
        {
            ValidateProperty(value, nameof(CurrentPassword));
        }
    }

    partial void OnNewPasswordChanged(string value)
    {
        if (HasAttemptedValidation)
        {
            ValidateProperty(value, nameof(NewPassword));

            ValidateProperty(
                ConfirmPassword,
                nameof(ConfirmPassword));
        }
    }

    partial void OnConfirmPasswordChanged(string value)
    {
        if (HasAttemptedValidation)
        {
            ValidateProperty(value, nameof(ConfirmPassword));
        }
    }

    //private async Task<bool> ValidateControl()
    //{
    //    if (string.IsNullOrWhiteSpace(CurrentPassword))
    //    {
    //        await _dialogueService.ShowToastAsync(
    //            "Please enter current password!",
    //            ToastDuration.Long);

    //        return false;
    //    }

    //    if (string.IsNullOrWhiteSpace(NewPassword) &&
    //        string.IsNullOrWhiteSpace(ConfirmPassword))
    //    {
    //        await _dialogueService.ShowToastAsync(
    //            "Please enter new password and confirm password!",
    //            ToastDuration.Long);

    //        return false;
    //    }

    //    if (string.IsNullOrWhiteSpace(NewPassword))
    //    {
    //        await _dialogueService.ShowToastAsync(
    //            "Please enter new password!",
    //            ToastDuration.Long);

    //        return false;
    //    }

    //    if (string.IsNullOrWhiteSpace(ConfirmPassword))
    //    {
    //        await _dialogueService.ShowToastAsync(
    //            "Please enter confirm password!",
    //            ToastDuration.Long);

    //        return false;
    //    }

    //    if (NewPassword != ConfirmPassword)
    //    {
    //        await _dialogueService.ShowToastAsync(
    //            "Not match new password and confirm password!",
    //            ToastDuration.Long);

    //        return false;
    //    }

    //    return true;
    //}

    [RelayCommand]
    private void ToggleCurrentPassword()
    {
        IsCurrentPasswordHidden = !IsCurrentPasswordHidden;
    }

    [RelayCommand]
    private void ToggleNewPassword()
    {
        IsNewPasswordHidden = !IsNewPasswordHidden;
    }

    [RelayCommand]
    private void ToggleConfirmPassword()
    {
        IsConfirmPasswordHidden = !IsConfirmPasswordHidden;
    }

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        if (IsBusy)
            return;

        if (!ValidateForm())
            return;


        if (!await NetworkService.CheckInternetAsync())
            return;

        //if (!await ValidateControl())
        //    return;

        try
        {
            IsBusy = true;

            await PopLoaderService.ShowAsync();

            if (AppSession.CurrentUser == null)
            {
                await _dialogueService.ShowToastAsync(
                    "User session not found.",
                    ToastDuration.Long);

                return;
            }

            var request = new ChangePassword
            {
                UserId = AppSession.CurrentUser.Id,
                CurrentPassword = CurrentPassword.Trim(),
                NewPassword = NewPassword.Trim(),
                ConfirmPassword = ConfirmPassword.Trim()
            };

            var response =
                await _authService.ChangePasswordAsync(request);

            if (response == null || !response.IsSuccess)
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ??
                    "Unable to change password.",
                    ToastDuration.Long);

                return;
            }

            await _dialogueService.ShowToastAsync(
                "Password has been changed successfully.",
                ToastDuration.Short);

            ClearControl();

            await _navigationService.PopAsync();
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

    public void ClearControl()
    {
        CurrentPassword = string.Empty;
        NewPassword = string.Empty;
        ConfirmPassword = string.Empty;

        IsCurrentPasswordHidden = true;
        IsNewPasswordHidden = true;
        IsConfirmPasswordHidden = true;

        HasAttemptedValidation = false;
        ClearErrors();

        OnPropertyChanged(nameof(CurrentPasswordError));
        OnPropertyChanged(nameof(HasCurrentPasswordError));
        OnPropertyChanged(nameof(NewPasswordError));
        OnPropertyChanged(nameof(HasNewPasswordError));
        OnPropertyChanged(nameof(ConfirmPasswordError));
        OnPropertyChanged(nameof(HasConfirmPasswordError));

    }
}
