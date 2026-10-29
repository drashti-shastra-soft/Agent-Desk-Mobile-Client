using AgentDesk.Helpers;
using AgentDesk.Models;
using AgentDesk.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AgentDesk.ViewModels;

public partial class AddGroupPageViewModel : BaseViewModel
{
    private bool _isEdit = false;

    [ObservableProperty]
    private GroupModel? groupModel;

    //[ObservableProperty]
    //[Required(ErrorMessage = "Please enter code.")]
    //private string code = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter name.")]
    private string name = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter head name.")]
    private string headName = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter mobile number.")]
    [CustomValidation(
    typeof(ValidationHelper),
    nameof(ValidationHelper.ValidateMobileNumber))]
    private string mobileNumber = string.Empty;


    [ObservableProperty]
    private bool isActive = true;

    //public string CodeError => GetFirstError(nameof(Code));
    //public bool HasCodeError => !string.IsNullOrEmpty(CodeError);

    public string NameError => GetFirstError(nameof(Name));
    public bool HasNameError => !string.IsNullOrEmpty(NameError);

    public string HeadNameError => GetFirstError(nameof(HeadName));
    public bool HasHeadNameError => !string.IsNullOrEmpty(HeadNameError);

    public string MobileNumberError => GetFirstError(nameof(MobileNumber));
    public bool HasMobileNumberError =>
        !string.IsNullOrEmpty(MobileNumberError);

    public AddGroupPageViewModel()
    {
        ErrorsChanged += OnValidationErrorsChanged;
    }

    private void OnValidationErrorsChanged(
        object? sender,
        DataErrorsChangedEventArgs e)
    {
        var propertyName = e.PropertyName;

        //if (string.IsNullOrEmpty(propertyName) ||
        //    propertyName == nameof(Code))
        //{
        //    OnPropertyChanged(nameof(CodeError));
        //    OnPropertyChanged(nameof(HasCodeError));
        //}

        if (string.IsNullOrEmpty(propertyName) ||
            propertyName == nameof(Name))
        {
            OnPropertyChanged(nameof(NameError));
            OnPropertyChanged(nameof(HasNameError));
        }

        if (string.IsNullOrEmpty(propertyName) ||
            propertyName == nameof(HeadName))
        {
            OnPropertyChanged(nameof(HeadNameError));
            OnPropertyChanged(nameof(HasHeadNameError));
        }

        if (string.IsNullOrEmpty(propertyName) ||
            propertyName == nameof(MobileNumber))
        {
            OnPropertyChanged(nameof(MobileNumberError));
            OnPropertyChanged(nameof(HasMobileNumberError));
        }
    }

    //partial void OnCodeChanged(string value)
    //{
    //    if (HasAttemptedValidation)
    //        ValidateProperty(value, nameof(Code));
    //}

    partial void OnNameChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(Name));
    }

    partial void OnHeadNameChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(HeadName));
    }

    partial void OnMobileNumberChanged(string value)
    {
        if (!HasStartedMobileInput && string.IsNullOrWhiteSpace(value))
            return;

        HasStartedMobileInput = true;

        ValidateProperty(value, nameof(MobileNumber));
    }

    public async Task OnAppearing()
    {
        HasAttemptedValidation = false;
        HasStartedMobileInput = false;
        ClearErrors();

        if (GroupModel != null)
        {
            _isEdit = true;
            Title = "Edit Group";

            //Code = GroupModel.Code;
            Name = GroupModel.Name;
            HeadName = GroupModel.HeadName;
            MobileNumber = GetMobileNumberForEdit(GroupModel.MobileNumber);
            IsActive = GroupModel.IsActive;
        }
        else
        {
            _isEdit = false;
            Title = "Add Group";

            ClearControl();
        }
    }

    public void ClearControl()
    {
        //Code = string.Empty;
        Name = string.Empty;
        HeadName = string.Empty;
        MobileNumber = string.Empty;
        IsActive = true;

        HasAttemptedValidation = false;
        HasStartedMobileInput = false;
        ClearErrors();

        //OnPropertyChanged(nameof(CodeError));
        //OnPropertyChanged(nameof(HasCodeError));
        OnPropertyChanged(nameof(NameError));
        OnPropertyChanged(nameof(HasNameError));
        OnPropertyChanged(nameof(HeadNameError));
        OnPropertyChanged(nameof(HasHeadNameError));
        OnPropertyChanged(nameof(MobileNumberError));
        OnPropertyChanged(nameof(HasMobileNumberError));
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

            var group = new GroupModel
            {
                Id = _isEdit ? GroupModel!.Id : 0,
                CompanyId = AppSession.CurrentUser.CompanyId,
                //Code = Code.Trim(),
                Name = Name.Trim(),
                HeadName = HeadName.Trim(),
                MobileNumber = $"+91 {MobileNumber.Trim()}",
                IsActive = IsActive,
                CreatedBy = AppSession.CurrentUser?.Id ?? 0,

                //CreatedBy = _isEdit
                //    ? GroupModel!.CreatedBy
                //    : AppSession.CurrentUser?.Id ?? 0,

                LastModifiedBy = _isEdit
                    ? AppSession.CurrentUser?.Id
                    : null
            };

            var response = await _groupService.AddAsync(group);

            if (response == null || !response.IsSuccess)
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Unable to save group.",
                    ToastDuration.Long);

                return;
            }

            await _dialogueService.ShowToastAsync(
                _isEdit
                    ? "Group updated successfully."
                    : "Group added successfully.",
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

}

#region Old 
//public partial class AddGroupPageViewModel : BaseViewModel
//{
//    private bool _isEdit = false;

//    [ObservableProperty]
//    private GroupModel? groupModel;

//    [ObservableProperty]
//    private string code = string.Empty;

//    [ObservableProperty]
//    private string name = string.Empty;

//    [ObservableProperty]
//    private string headName = string.Empty;

//    [ObservableProperty]
//    private string mobileNumber = string.Empty;

//    [ObservableProperty]
//    private bool isActive = true;

//    public AddGroupPageViewModel()
//    {
//    }

//    public async Task OnAppearing()
//    {
//        if (GroupModel != null)
//        {
//            _isEdit = true;
//            Title = "Edit Group";
//            Code = GroupModel.Code;
//            Name = GroupModel.Name;
//            HeadName = GroupModel.HeadName;
//            MobileNumber = GroupModel.MobileNumber;
//            IsActive = GroupModel.IsActive;
//        }
//        else
//        {
//            _isEdit = false;
//            Title = "Add Group";

//            ClearControl();
//        }
//    }
//    private void ClearControl()
//    {
//        Code = string.Empty;
//        Name = string.Empty;
//        HeadName = string.Empty;
//        MobileNumber = string.Empty;
//        IsActive = true;
//    }

//    [RelayCommand]
//    private void ToggleActive()
//    {
//        IsActive = !IsActive;
//    }

//    [RelayCommand]
//    private async Task SubmitAsync()
//    {
//        if (IsBusy)
//            return;

//        if (!await ValidateControl())
//            return;

//        try
//        {
//            IsBusy = true;

//            await PopLoaderService.ShowAsync();

//            var group = new GroupModel
//            {
//                Id = _isEdit ? GroupModel!.Id : 0,
//                Code = Code.Trim(),
//                Name = Name.Trim(),
//                HeadName = HeadName.Trim(),
//                MobileNumber = MobileNumber.Trim(),
//                IsActive = IsActive,

//                CreatedBy = _isEdit
//                    ? GroupModel!.CreatedBy
//                    : AppSession.CurrentUser?.Id ?? 0,

//                LastModifiedBy = _isEdit
//                    ? AppSession.CurrentUser?.Id
//                    : null
//            };

//            var response = await _groupService.AddAsync(group);

//            if (response == null || !response.IsSuccess)
//            {
//                await _dialogueService.ShowToastAsync(
//                    response?.ErrorMessage ?? "Unable to save group.",
//                    ToastDuration.Long);

//                return;
//            }

//            await _dialogueService.ShowToastAsync(
//                _isEdit
//                    ? "Group updated successfully."
//                    : "Group added successfully.",
//                ToastDuration.Short);

//            await _navigationService.PopAsync();
//        }
//        catch (Exception ex)
//        {
//            await _dialogueService.ShowToastAsync(
//                ex.Message,
//                ToastDuration.Long);
//        }
//        finally
//        {
//            IsBusy = false;

//            await PopLoaderService.HideAsync();
//        }
//    }

//    private async Task<bool> ValidateControl()
//    {
//        if (string.IsNullOrWhiteSpace(Code))
//        {
//            await _dialogueService.ShowToastAsync(
//                "Please enter code!",
//                ToastDuration.Long);

//            return false;
//        }

//        if (string.IsNullOrWhiteSpace(Name))
//        {
//            await _dialogueService.ShowToastAsync(
//                "Please enter name!",
//                ToastDuration.Long);

//            return false;
//        }

//        if (string.IsNullOrWhiteSpace(HeadName))
//        {
//            await _dialogueService.ShowToastAsync(
//                "Please enter head name!",
//                ToastDuration.Long);

//            return false;
//        }

//        if (string.IsNullOrWhiteSpace(MobileNumber))
//        {
//            await _dialogueService.ShowToastAsync(
//                "Please enter mobile number!",
//                ToastDuration.Long);

//            return false;
//        }

//        return true;
//    }
//}

#endregion