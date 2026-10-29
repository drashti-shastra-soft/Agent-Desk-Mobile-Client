using AgentDesk.Helpers;
using AgentDesk.Models.Auth;
using AgentDesk.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.ViewModels;

public partial class ForgotPasswordPageViewModel : BaseViewModel
{
    [ObservableProperty]
    [Required(ErrorMessage = "Please enter email address.")]
    [CustomValidation(typeof(ValidationHelper),nameof(ValidationHelper.ValidateEmail))]
    private string emailAddress = string.Empty;

    public string EmailAddressError =>
        GetFirstError(nameof(EmailAddress));

    public bool HasEmailAddressError =>
        !string.IsNullOrEmpty(EmailAddressError);

    public ForgotPasswordPageViewModel()
    {
        ErrorsChanged += OnValidationErrorsChanged;
    }

    partial void OnEmailAddressChanged(string value)
    {
        if (!HasStartedEmailInput && string.IsNullOrWhiteSpace(value))
            return;

        HasStartedEmailInput = true;

        ValidateProperty(value, nameof(EmailAddress));
    }

    private void OnValidationErrorsChanged(
        object? sender,
        System.ComponentModel.DataErrorsChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.PropertyName) ||
            e.PropertyName == nameof(EmailAddress))
        {
            OnPropertyChanged(nameof(EmailAddressError));
            OnPropertyChanged(nameof(HasEmailAddressError));
        }
    }


    [RelayCommand]
    private async Task SubmitAsync()
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

            var request = new ForgotPassword
            {
                EmailAddress = EmailAddress.Trim()
            };

            var response = await _authService.ForgotPasswordAsync(request);

            if (response == null || !response.IsSuccess)
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ??
                    "Unable to process your request.",
                    ToastDuration.Long);

                return;
            }

            await _dialogueService.ShowToastAsync(
                "Success",
                ToastDuration.Short);

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

    [RelayCommand]
    private async Task BackToLoginAsync()
    {
        if (IsBusy)
            return;

        await Shell.Current.GoToAsync("..");
        //await _navigationService.PopAsync();
    }
}