using AgentDesk.Helpers;
using AgentDesk.Models;
using AgentDesk.ViewModels.Tabs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace AgentDesk.ViewModels;

public partial class IdentificationTabViewModel : ClientTabViewModelBase
{
    [ObservableProperty]
    [Required(ErrorMessage = "Please enter PAN number.")]
    //[RegularExpression(@"^[A-Za-z]{5}[0-9]{4}[A-Za-z]$", ErrorMessage = "Please enter a valid PAN number.")]
    private string panNumber = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter Aadhar number.")]
    //[RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhar number must be 12 digits.")]
    private string aadharNumber = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter nominee name.")]
    private string nomineeName = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter nominee relationship.")]
    private string nomineeRelationship = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter nominee mobile number.")]
    [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateMobileNumber))]
    private string nomineeMobileNumber = string.Empty;

    public string PanNumberError => GetFirstError(nameof(PanNumber));
    public bool HasPanNumberError => !string.IsNullOrEmpty(PanNumberError);

    public string AadharNumberError => GetFirstError(nameof(AadharNumber));
    public bool HasAadharNumberError => !string.IsNullOrEmpty(AadharNumberError);

    public string NomineeNameError => GetFirstError(nameof(NomineeName));
    public bool HasNomineeNameError => !string.IsNullOrEmpty(NomineeNameError);

    public string NomineeRelationshipError => GetFirstError(nameof(NomineeRelationship));
    public bool HasNomineeRelationshipError => !string.IsNullOrEmpty(NomineeRelationshipError);

    public string NomineeMobileNumberError => GetFirstError(nameof(NomineeMobileNumber));
    public bool HasNomineeMobileNumberError => !string.IsNullOrEmpty(NomineeMobileNumberError);

    protected override async Task LoadCoreAsync()
    {
        var response = await _clientService.GetClientIdentificationByIdAsync(ClientId);

        if (response?.IsSuccess == true && response.Value is ClientIdentification m)
        {
            PanNumber = m.PANNumber ?? string.Empty;
            AadharNumber = m.AadhaarNumber ?? string.Empty;
            NomineeName = m.NomineeName ?? string.Empty;
            NomineeRelationship = m.NomineeRelationship ?? string.Empty;
            NomineeMobileNumber = m.NomineeContact ?? string.Empty;
        }
    }

    partial void OnPanNumberChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(PanNumber));
    }

    partial void OnAadharNumberChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(AadharNumber));
    }

    partial void OnNomineeNameChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(NomineeName));
    }

    partial void OnNomineeRelationshipChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(NomineeRelationship));
    }

    partial void OnNomineeMobileNumberChanged(string value)
    {
        if (!HasStartedMobileInput && string.IsNullOrWhiteSpace(value))
            return;
        HasStartedMobileInput = true;
        ValidateProperty(value, nameof(NomineeMobileNumber));
    }

    protected override void ClearFields()
    {
        PanNumber = AadharNumber = NomineeName = NomineeRelationship = NomineeMobileNumber = string.Empty;
    }

    [RelayCommand]
    private Task SaveAsync() => RunSaveAsync(async () =>
    {
        var model = new ClientIdentification
        {
            ClientId = ClientId,
            PANNumber = PanNumber.Trim().ToUpperInvariant(),
            AadhaarNumber = AadharNumber.Trim(),
            NomineeName = NomineeName.Trim(),
            NomineeRelationship = NomineeRelationship.Trim(),
            NomineeContact = NomineeMobileNumber.Trim()
        };

        var r = await _clientService.AddClientIdentificationAsync(model);
        return (r?.IsSuccess == true, r?.ErrorMessage);
    }, "Client Identification saved successfully.");
}
