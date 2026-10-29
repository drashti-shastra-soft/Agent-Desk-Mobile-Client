using AgentDesk.Constants;
using AgentDesk.Helpers;
using AgentDesk.Models;
using AgentDesk.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AgentDesk.Constants.AppEnum;

namespace AgentDesk.ViewModels;

public partial class AddAgencyPageViewModel:BaseViewModel
{

    private bool _isEdit = false;

    [ObservableProperty]
    private Agency? agencyModel;

    [ObservableProperty]
    [Required(ErrorMessage = "Please select agency type.")]
    private AppEnum.AgencyType? agencyType;

    public List<AppEnum.AgencyType> AgencyTypeList { get; } =
        Enum.GetValues<AppEnum.AgencyType>().ToList();

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter code.")]
    private string code = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter agency name.")]
    private string name = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter company name.")]
    private string companyName = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter email address.")]
    [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateEmail))]
    private string emailAddress = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter mobile number.")]
    [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateMobileNumber))]
    private string mobileNumber = string.Empty;

    [ObservableProperty]
    private bool isActive = true;


    public string AgencyTypeError => GetFirstError(nameof(AgencyType));
    public bool HasAgencyTypeError => !string.IsNullOrEmpty(AgencyTypeError);

    public string CodeError => GetFirstError(nameof(Code));
    public bool HasCodeError => !string.IsNullOrEmpty(CodeError);

    public string NameError => GetFirstError(nameof(Name));
    public bool HasNameError => !string.IsNullOrEmpty(NameError);

    public string CompanyNameError => GetFirstError(nameof(CompanyName));
    public bool HasCompanyNameError =>
        !string.IsNullOrEmpty(CompanyNameError);

    public string EmailAddressError => GetFirstError(nameof(EmailAddress));
    public bool HasEmailAddressError =>
        !string.IsNullOrEmpty(EmailAddressError);

    public string MobileNumberError => GetFirstError(nameof(MobileNumber));
    public bool HasMobileNumberError =>
        !string.IsNullOrEmpty(MobileNumberError);

    public AddAgencyPageViewModel()
    {
        ErrorsChanged += OnValidationErrorsChanged;
    }


    public async Task OnAppearing()
    {
        HasAttemptedValidation = false;
        HasStartedMobileInput = false;
        ClearErrors();

        if (AgencyModel != null)
        {
            _isEdit = true;
            Title = "Edit Agency";

            //AgencyType = Enum.TryParse<AppEnum.AgencyType>(AgencyModel.AgencyType, out var agencyType)
            //       ? agencyType
            //       : null;

            var agencyType = Enum.GetValues<AppEnum.AgencyType>()
               .Where(x =>
                   x.ToString() == AgencyModel.AgencyType ||
                   x.GetDisplayName() == AgencyModel.AgencyType)
               .Select(x => (AppEnum.AgencyType?)x)
               .FirstOrDefault();

            AgencyType = agencyType;

            //var agencyType = Enum.GetValues<AppEnum.AgencyType>()
            //    .FirstOrDefault(x =>
            //    x.ToString() == AgencyModel.AgencyType ||
            //    GetDisplayName(x) == AgencyModel.AgencyType);

            //AgencyType = Enum.GetValues<AppEnum.AgencyType>()
            //    .Any(x =>
            //        x.ToString() == AgencyModel.AgencyType ||
            //        GetDisplayName(x) == AgencyModel.AgencyType)
            //    ? agencyType
            //    : null;

            Code = AgencyModel.Code;
            Name = AgencyModel.Name;
            EmailAddress = AgencyModel.EmailAddress;
            CompanyName = AgencyModel.CompanyName;
            MobileNumber = GetMobileNumberForEdit(AgencyModel.MobileNumber);
            IsActive = AgencyModel.IsActive;
        }
        else
        {
            _isEdit = false;
            Title = "Add Agency";

            ClearControl();
        }
    }

    private void OnValidationErrorsChanged(
        object? sender,
        DataErrorsChangedEventArgs e)
    {
        var propertyName = e.PropertyName;

        if (string.IsNullOrEmpty(propertyName) ||
            propertyName == nameof(AgencyType))
        {
            OnPropertyChanged(nameof(AgencyTypeError));
            OnPropertyChanged(nameof(HasAgencyTypeError));
        }

        if (string.IsNullOrEmpty(propertyName) ||
            propertyName == nameof(Code))
        {
            OnPropertyChanged(nameof(CodeError));
            OnPropertyChanged(nameof(HasCodeError));
        }

        if (string.IsNullOrEmpty(propertyName) ||
            propertyName == nameof(Name))
        {
            OnPropertyChanged(nameof(NameError));
            OnPropertyChanged(nameof(HasNameError));
        }

        if (string.IsNullOrEmpty(propertyName) ||
            propertyName == nameof(CompanyName))
        {
            OnPropertyChanged(nameof(CompanyNameError));
            OnPropertyChanged(nameof(HasCompanyNameError));
        }

        if (string.IsNullOrEmpty(propertyName) ||
            propertyName == nameof(EmailAddress))
        {
            OnPropertyChanged(nameof(EmailAddressError));
            OnPropertyChanged(nameof(HasEmailAddressError));
        }

        if (string.IsNullOrEmpty(propertyName) ||
            propertyName == nameof(MobileNumber))
        {
            OnPropertyChanged(nameof(MobileNumberError));
            OnPropertyChanged(nameof(HasMobileNumberError));
        }
    }

    partial void OnCodeChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(Code));
    }

    partial void OnNameChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(Name));
    }

    partial void OnCompanyNameChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(CompanyName));
    }

    partial void OnEmailAddressChanged(string value)
    {
        if (!HasStartedEmailInput && string.IsNullOrWhiteSpace(value))
            return;

        HasStartedEmailInput = true;

        ValidateProperty(value, nameof(EmailAddress));
    }

    //partial void OnMobileNumberChanged(string value)
    //{
    //    if (HasAttemptedValidation)
    //        ValidateProperty(value, nameof(MobileNumber));
    //}
    partial void OnMobileNumberChanged(string value)
    {
        if (!HasStartedMobileInput && string.IsNullOrWhiteSpace(value))
            return;

        HasStartedMobileInput = true;

        ValidateProperty(value, nameof(MobileNumber));
    }


    [RelayCommand]
    private void ToggleActive()
    {
        IsActive = !IsActive;
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

            var agency = new Agency
            {
                Id = _isEdit ? AgencyModel!.Id : 0,
                CompanyId = AppSession.CurrentUser.CompanyId,
                //AgencyType = AgencyType?.ToString(),

                // Using in BaseViewModel
                //AgencyType = AgencyType.HasValue
                //    ? GetDisplayName(AgencyType.Value)
                //    : null,

                // Using Extention 
                AgencyType = AgencyType?.GetDisplayName(),
                Code = Code.Trim(),
                Name = Name.Trim(),
                MobileNumber = $"+91 {MobileNumber.Trim()}",
                EmailAddress = EmailAddress.Trim(),
                CompanyName = CompanyName.Trim(),
                IsActive = IsActive,
                CreatedBy = AppSession.CurrentUser?.Id ?? 0,

                //CreatedBy = _isEdit
                //    ? AgencyModel!.CreatedBy
                //    : AppSession.CurrentUser?.Id ?? 0,

                LastModifiedBy = _isEdit
                    ? AppSession.CurrentUser?.Id
                    : null
            };

            var response = await _agencyService.AddAsync(agency);

            if (response == null || !response.IsSuccess)
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Unable to save agency.",
                    ToastDuration.Long);

                return;
            }

            await _dialogueService.ShowToastAsync(
                _isEdit
                    ? "Agency updated successfully."
                    : "Agency added successfully.",
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
            IsBusy = false;

            await PopLoaderService.HideAsync();
        }
    }


    public void ClearControl()
    {
        Code = string.Empty;
        Name = string.Empty;
        CompanyName = string.Empty;
        EmailAddress = string.Empty;
        MobileNumber = string.Empty;
        IsActive = true;

        HasAttemptedValidation = false;
        HasStartedEmailInput = false;
        HasStartedMobileInput = false;
        ClearErrors();

        OnPropertyChanged(nameof(AgencyTypeError));
        OnPropertyChanged(nameof(HasAgencyTypeError));
        OnPropertyChanged(nameof(CodeError));
        OnPropertyChanged(nameof(HasCodeError));
        OnPropertyChanged(nameof(NameError));
        OnPropertyChanged(nameof(HasNameError));
        OnPropertyChanged(nameof(CompanyNameError));
        OnPropertyChanged(nameof(HasCompanyNameError));
        OnPropertyChanged(nameof(EmailAddressError));
        OnPropertyChanged(nameof(HasEmailAddressError));
        OnPropertyChanged(nameof(MobileNumberError));
        OnPropertyChanged(nameof(HasMobileNumberError));
    }
}
