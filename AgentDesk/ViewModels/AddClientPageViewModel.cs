using AgentDesk.Constants;
using AgentDesk.Helpers;
using AgentDesk.Models;
using AgentDesk.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AgentDesk.ViewModels;

public partial class AddClientPageViewModel : BaseViewModel
{
    private int _loadedClientId;

    // ------------------------------------------------------------------
    // Mode + tabs
    // ------------------------------------------------------------------

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAddMode))]
    private bool isEditMode;

    public bool IsAddMode => !IsEditMode;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsClientTab))]
    [NotifyPropertyChangedFor(nameof(IsIdentificationTab))]
    [NotifyPropertyChangedFor(nameof(IsFinancialTab))]
    [NotifyPropertyChangedFor(nameof(IsHealthTab))]
    [NotifyPropertyChangedFor(nameof(IsDocumentsTab))]
    private ClientTab selectedTab = ClientTab.Client;

    public bool IsClientTab => SelectedTab == ClientTab.Client;
    public bool IsIdentificationTab => SelectedTab == ClientTab.Identification;
    public bool IsFinancialTab => SelectedTab == ClientTab.Financial;
    public bool IsHealthTab => SelectedTab == ClientTab.Health;
    public bool IsDocumentsTab => SelectedTab == ClientTab.Documents;

    public ObservableCollection<ClientTabItem> Tabs { get; } = new()
    {
        new() { Tab = ClientTab.Client,         Title = "Client", IsSelected = true },
        new() { Tab = ClientTab.Identification, Title = "Identification" },
        new() { Tab = ClientTab.Financial,      Title = "Financial information" },
        new() { Tab = ClientTab.Health,         Title = "Health & Lifestyle" },
        new() { Tab = ClientTab.Documents,      Title = "Document" },
    };

    public IdentificationTabViewModel IdentificationVm { get; } = new();
    public FinancialTabViewModel FinancialVm { get; } = new();
    public HealthTabViewModel HealthVm { get; } = new();
    public DocumentsTabViewModel DocumentsVm { get; } = new();

    [RelayCommand]
    private async Task SelectTabAsync(ClientTabItem? tab)
    {
        if (tab == null || !IsEditMode || ClientModel == null)
            return;

        foreach (var t in Tabs)
            t.IsSelected = ReferenceEquals(t, tab);

        SelectedTab = tab.Tab;

        var clientId = ClientModel.Id;

        switch (tab.Tab)
        {
            case ClientTab.Identification: await IdentificationVm.LoadAsync(clientId); break;
            case ClientTab.Financial:      await FinancialVm.LoadAsync(clientId);      break;
            case ClientTab.Health:         await HealthVm.LoadAsync(clientId);         break;
            case ClientTab.Documents:      await DocumentsVm.LoadAsync(clientId);      break;
        }
    }

    private void ResetTabs()
    {
        SelectedTab = ClientTab.Client;

        foreach (var t in Tabs)
            t.IsSelected = t.Tab == ClientTab.Client;

        IdentificationVm.Reset();
        FinancialVm.Reset();
        HealthVm.Reset();
        DocumentsVm.Reset();
    }

    // ------------------------------------------------------------------
    // Client tab (your existing fields)
    // ------------------------------------------------------------------

    [ObservableProperty]
    private ObservableCollection<GroupModel> groupList = new();

    [ObservableProperty]
    private Client? clientModel;

    [ObservableProperty]
    [Required(ErrorMessage = "Please select group.")]
    private GroupModel? selectedGroup;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter first name.")]
    private string firstName = string.Empty;

    // Middle name is optional -> no [Required]
    [ObservableProperty]
    private string middleName = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter last name.")]
    private string lastName = string.Empty;

    [ObservableProperty]
    [CustomValidation(
        typeof(ValidationHelper),
        nameof(ValidationHelper.ValidateEmail))]
    private string emailAddress = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter mobile number.")]
    [CustomValidation(
        typeof(ValidationHelper),
        nameof(ValidationHelper.ValidateMobileNumber))]
    private string mobileNumber = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please select date of birth.")]
    private DateTime? dateOfBirth;

    [ObservableProperty]
    [Required(ErrorMessage = "Please select gender.")]
    private AppEnum.Gender? gender;

    public List<AppEnum.Gender> GenderList { get; } =
        Enum.GetValues<AppEnum.Gender>().ToList();

    [ObservableProperty]
    private AppEnum.MaritalStatus? maritalStatus;

    public List<AppEnum.MaritalStatus> MaritalStatusList { get; } =
        Enum.GetValues<AppEnum.MaritalStatus>().ToList();

    [ObservableProperty]
    private string occupation = string.Empty;

    [ObservableProperty]
    private bool isActive = true;

    public string SelectedGroupError => GetFirstError(nameof(SelectedGroup));
    public bool HasSelectedGroupError => !string.IsNullOrEmpty(SelectedGroupError);

    public string FirstNameError => GetFirstError(nameof(FirstName));
    public bool HasFirstNameError => !string.IsNullOrEmpty(FirstNameError);

    public string LastNameError => GetFirstError(nameof(LastName));
    public bool HasLastNameError => !string.IsNullOrEmpty(LastNameError);

    public string MobileNumberError => GetFirstError(nameof(MobileNumber));
    public bool HasMobileNumberError => !string.IsNullOrEmpty(MobileNumberError);

    public string DateOfBirthError => GetFirstError(nameof(DateOfBirth));
    public bool HasDateOfBirthError => !string.IsNullOrEmpty(DateOfBirthError);

    public string GenderError => GetFirstError(nameof(Gender));
    public bool HasGenderError => !string.IsNullOrEmpty(GenderError);

    public string EmailAddressError => GetFirstError(nameof(EmailAddress));
    public bool HasEmailAddressError => !string.IsNullOrEmpty(EmailAddressError);

    public AddClientPageViewModel()
    {
        ErrorsChanged += OnValidationErrorsChanged;
    }

    public async Task OnAppearing()
    {
        HasAttemptedValidation = false;
        ClearErrors();

        await LoadGroupsAsync();

        if (ClientModel != null)
        {
            IsEditMode = true;
            Title = "Edit Client";

            SelectedGroup = GroupList.FirstOrDefault(g => g.Id == ClientModel.GroupId);
            FirstName = ClientModel.FirstName;
            MiddleName = ClientModel.MiddleName;
            LastName = ClientModel.LastName;
            EmailAddress = ClientModel.EmailAddress;
            MobileNumber = GetMobileNumberForEdit(ClientModel.PhoneNumber);
            DateOfBirth = ClientModel.DOB;

            Gender = Enum.TryParse<AppEnum.Gender>(ClientModel.Gender, out var parsedGender)
                ? parsedGender
                : null;

            MaritalStatus = Enum.TryParse<AppEnum.MaritalStatus>(
                ClientModel.MaritalStatus, out var parsedMarital)
                ? parsedMarital
                : null;

            Occupation = ClientModel.Occupation;
            IsActive = ClientModel.IsActive;

            // Opening a different client -> reset the other tabs
            if (_loadedClientId != ClientModel.Id)
            {
                _loadedClientId = ClientModel.Id;
                ResetTabs();
            }
        }
        else
        {
            IsEditMode = false;
            Title = "Add Client";

            _loadedClientId = 0;
            ResetTabs();
            ClearControl();
        }
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

            var client = new Client
            {
                Id = IsEditMode ? ClientModel!.Id : 0,
                CompanyId = AppSession.CurrentUser.CompanyId,
                GroupId = SelectedGroup?.Id ?? 0,
                Group = SelectedGroup?.Name,

                FirstName = FirstName.Trim(),
                MiddleName = MiddleName.Trim(),
                LastName = LastName.Trim(),
                EmailAddress = string.IsNullOrWhiteSpace(EmailAddress)
                    ? null
                    : EmailAddress.Trim(),
                PhoneNumber = $"+91 {MobileNumber.Trim()}",
                DOB = DateOfBirth,
                Gender = Gender?.ToString(),
                MaritalStatus = MaritalStatus?.ToString(),
                Occupation = string.IsNullOrWhiteSpace(Occupation)
                    ? null
                    : Occupation.Trim(),

                IsActive = IsActive,

                CreatedBy = AppSession.CurrentUser?.Id ?? 0,

                LastModifiedBy = IsEditMode
                    ? AppSession.CurrentUser?.Id
                    : null
            };

            var response = await _clientService.AddAsync(client);

            if (response == null || !response.IsSuccess)
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Unable to save client.",
                    ToastDuration.Long);

                return;
            }

            await _dialogueService.ShowToastAsync(
                IsEditMode
                    ? "Client updated successfully."
                    : "Client added successfully.",
                ToastDuration.Short);

            // Add mode: go back (as before).
            // Edit mode: stay so the user can continue with the other tabs.
            if (!IsEditMode)
                await _navigationService.PopAsync();
        }
        catch (Exception ex)
        {
            await _dialogueService.ShowToastAsync(ex.Message, ToastDuration.Long);
        }
        finally
        {
            IsBusy = false;
            await PopLoaderService.HideAsync();
        }
    }

    [RelayCommand]
    private async Task LoadGroupsAsync()
    {
        try
        {
            var response = await _groupService.GetActiveGroupAsync();

            if (response == null || !response.IsSuccess)
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Unable to get groups.",
                    ToastDuration.Long);

                return;
            }

            GroupList.Clear();

            if (response.Value is List<GroupModel> groups)
            {
                foreach (var group in groups)
                    GroupList.Add(group);
            }
        }
        catch (Exception)
        {
            await _dialogueService.ShowToastAsync(
                "Unable to get groups.",
                ToastDuration.Long);
        }
    }

    private void OnValidationErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        var propertyName = e.PropertyName;

        if (string.IsNullOrEmpty(propertyName) || propertyName == nameof(SelectedGroup))
        {
            OnPropertyChanged(nameof(SelectedGroupError));
            OnPropertyChanged(nameof(HasSelectedGroupError));
        }

        if (string.IsNullOrEmpty(propertyName) || propertyName == nameof(FirstName))
        {
            OnPropertyChanged(nameof(FirstNameError));
            OnPropertyChanged(nameof(HasFirstNameError));
        }

        if (string.IsNullOrEmpty(propertyName) || propertyName == nameof(LastName))
        {
            OnPropertyChanged(nameof(LastNameError));
            OnPropertyChanged(nameof(HasLastNameError));
        }

        if (string.IsNullOrEmpty(propertyName) || propertyName == nameof(MobileNumber))
        {
            OnPropertyChanged(nameof(MobileNumberError));
            OnPropertyChanged(nameof(HasMobileNumberError));
        }

        if (string.IsNullOrEmpty(propertyName) || propertyName == nameof(DateOfBirth))
        {
            OnPropertyChanged(nameof(DateOfBirthError));
            OnPropertyChanged(nameof(HasDateOfBirthError));
        }

        if (string.IsNullOrEmpty(propertyName) || propertyName == nameof(Gender))
        {
            OnPropertyChanged(nameof(GenderError));
            OnPropertyChanged(nameof(HasGenderError));
        }

        if (string.IsNullOrEmpty(propertyName) || propertyName == nameof(EmailAddress))
        {
            OnPropertyChanged(nameof(EmailAddressError));
            OnPropertyChanged(nameof(HasEmailAddressError));
        }
    }

    partial void OnSelectedGroupChanged(GroupModel? value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(SelectedGroup));
    }

    partial void OnFirstNameChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(FirstName));
    }

    partial void OnLastNameChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(LastName));
    }

    partial void OnMobileNumberChanged(string value)
    {
        if (!HasStartedMobileInput && string.IsNullOrWhiteSpace(value))
            return;

        HasStartedMobileInput = true;

        ValidateProperty(value, nameof(MobileNumber));
    }

    partial void OnDateOfBirthChanged(DateTime? value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(DateOfBirth));
    }

    partial void OnGenderChanged(AppEnum.Gender? value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(Gender));
    }

    partial void OnEmailAddressChanged(string value)
    {
        if (HasStartedEmailInput || !string.IsNullOrWhiteSpace(value))
        {
            HasStartedEmailInput = true;
            ValidateProperty(value, nameof(EmailAddress));
        }
    }

    public void ClearControl()
    {
        SelectedGroup = null;
        FirstName = string.Empty;
        MiddleName = string.Empty;
        LastName = string.Empty;
        EmailAddress = string.Empty;
        MobileNumber = string.Empty;
        DateOfBirth = null;
        Gender = null;
        MaritalStatus = null;
        Occupation = string.Empty;
        IsActive = true;

        HasStartedEmailInput = false;
        HasAttemptedValidation = false;
        ClearErrors();

        OnPropertyChanged(nameof(SelectedGroupError));
        OnPropertyChanged(nameof(HasSelectedGroupError));
        OnPropertyChanged(nameof(FirstNameError));
        OnPropertyChanged(nameof(HasFirstNameError));
        OnPropertyChanged(nameof(LastNameError));
        OnPropertyChanged(nameof(HasLastNameError));
        OnPropertyChanged(nameof(MobileNumberError));
        OnPropertyChanged(nameof(HasMobileNumberError));
        OnPropertyChanged(nameof(DateOfBirthError));
        OnPropertyChanged(nameof(HasDateOfBirthError));
        OnPropertyChanged(nameof(GenderError));
        OnPropertyChanged(nameof(HasGenderError));
        OnPropertyChanged(nameof(EmailAddressError));
        OnPropertyChanged(nameof(HasEmailAddressError));
    }
}