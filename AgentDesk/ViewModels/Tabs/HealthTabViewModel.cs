using AgentDesk.Models;
using AgentDesk.ViewModels.Tabs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace AgentDesk.ViewModels;

public partial class HealthTabViewModel : ClientTabViewModelBase
{
    [ObservableProperty]
    [Required(ErrorMessage = "Please enter medical history.")]
    private string medicalHistory = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter current medications.")]
    private string currentMedications = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter smoking / alcohol habits.")]
    private string smokingAlcoholHabits = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter family medical history.")]
    private string familyMedicalHistory = string.Empty;

    public string MedicalHistoryError => GetFirstError(nameof(MedicalHistory));
    public bool HasMedicalHistoryError => !string.IsNullOrEmpty(MedicalHistoryError);

    public string CurrentMedicationsError => GetFirstError(nameof(CurrentMedications));
    public bool HasCurrentMedicationsError => !string.IsNullOrEmpty(CurrentMedicationsError);

    public string SmokingAlcoholHabitsError => GetFirstError(nameof(SmokingAlcoholHabits));
    public bool HasSmokingAlcoholHabitsError => !string.IsNullOrEmpty(SmokingAlcoholHabitsError);

    public string FamilyMedicalHistoryError => GetFirstError(nameof(FamilyMedicalHistory));
    public bool HasFamilyMedicalHistoryError => !string.IsNullOrEmpty(FamilyMedicalHistoryError);

    protected override async Task LoadCoreAsync()
    {
        //var response = await _clientService.GetHealthAsync(ClientId);

        //if (response?.IsSuccess == true && response.Value is ClientHealth m)
        //{
        //    MedicalHistory = m.MedicalHistory ?? string.Empty;
        //    CurrentMedications = m.CurrentMedications ?? string.Empty;
        //    SmokingAlcoholHabits = m.SmokingAlcoholHabits ?? string.Empty;
        //    FamilyMedicalHistory = m.FamilyMedicalHistory ?? string.Empty;
        //}
    }

    protected override void ClearFields()
    {
        MedicalHistory = CurrentMedications = SmokingAlcoholHabits = FamilyMedicalHistory = string.Empty;
    }

    //[RelayCommand]
    //private Task SaveAsync() => RunSaveAsync(async () =>
    //{
    //    var model = new ClientHealth
    //    {
    //        ClientId = ClientId,
    //        MedicalHistory = MedicalHistory.Trim(),
    //        CurrentMedications = CurrentMedications.Trim(),
    //        SmokingAlcoholHabits = SmokingAlcoholHabits.Trim(),
    //        FamilyMedicalHistory = FamilyMedicalHistory.Trim()
    //    };

    //    var r = await _clientService.SaveHealthAsync(model);
    //    return (r?.IsSuccess == true, r?.ErrorMessage);
    //}, "Health & lifestyle saved successfully.");
}
