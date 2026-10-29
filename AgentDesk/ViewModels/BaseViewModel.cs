using AgentDesk.Helpers;
using AgentDesk.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace AgentDesk.ViewModels;

public partial class BaseViewModel : ObservableValidator
{
    protected readonly IAuthService _authService;
    public readonly IDialogueService _dialogueService;
    protected readonly INavigationService _navigationService;
    protected readonly IGroupService _groupService;
    protected readonly IBiometricService _biometricService;
    protected readonly IAgencyService _agencyService;
    protected readonly IClientService _clientService;
    protected readonly IPolicyService _policyService;



    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private bool hasAttemptedValidation;

    [ObservableProperty]
    public bool hasStartedMobileInput;

    [ObservableProperty]
    public bool hasStartedEmailInput;


    public BaseViewModel()
    {
        _authService = ServiceHelper.GetService<IAuthService>();
        _dialogueService = ServiceHelper.GetService<IDialogueService>();
        _navigationService = ServiceHelper.GetService<INavigationService>();
        _groupService = ServiceHelper.GetService<IGroupService>();
        _biometricService = ServiceHelper.GetService<IBiometricService>();
        _agencyService = ServiceHelper.GetService<IAgencyService>();
        _clientService = ServiceHelper.GetService<IClientService>();
        _policyService = ServiceHelper.GetService<IPolicyService>();
    }

    protected bool ValidateForm()
    {
        HasAttemptedValidation = true;
        ValidateAllProperties();

        return !HasErrors;
    }

    [RelayCommand]
    protected async Task BackAsync()
    {
        if (IsBusy)
            return;
        await _navigationService.PopAsync();
    }

    protected string GetFirstError(string propertyName)
    {
        return GetErrors(propertyName)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault(e =>
                !string.IsNullOrWhiteSpace(e))
            ?? string.Empty;
    }


    public string GetMobileNumberForEdit(string? mobileNumber)
    {
        if (string.IsNullOrWhiteSpace(mobileNumber))
            return string.Empty;

        // Keep only digits
        var digits = new string(
            mobileNumber.Where(char.IsDigit).ToArray());

        // Remove +91 country code
        if (digits.StartsWith("91") && digits.Length > 10)
            digits = digits.Substring(2);

        // Maximum 10 digits
        if (digits.Length > 10)
            digits = digits.Substring(0, 10);

        return digits;
    }

    //protected string GetDisplayName<TEnum>(TEnum value) where TEnum : struct, Enum
    //{
    //    var member = typeof(TEnum)
    //        .GetMember(value.ToString())
    //        .FirstOrDefault();

    //    return member?
    //        .GetCustomAttributes(typeof(DisplayAttribute), false)
    //        .Cast<DisplayAttribute>()
    //        .FirstOrDefault()?
    //        .GetName() ?? value.ToString();
    //}


}


