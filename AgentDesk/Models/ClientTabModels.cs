using CommunityToolkit.Mvvm.ComponentModel;

namespace AgentDesk.Models;

public enum ClientTab
{
    Client,
    Identification,
    Financial,
    Health,
    Documents
}

public partial class ClientTabItem : ObservableObject
{
    public ClientTab Tab { get; init; }
    public string Title { get; init; } = string.Empty;

    [ObservableProperty]
    private bool isSelected;
}

// ---- API models for the new tabs (adjust to match your backend DTOs) ----

public class ClientIdentification
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string PANNumber { get; set; }
    public string AadhaarNumber { get; set; }
    public string NomineeName { get; set; }
    public string NomineeRelationship { get; set; }
    public string NomineeContact { get; set; }
    public int CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
}

public class ClientFinancialInfo
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string AnnualIncome { get; set; }
    public string ExistingLoansEMI { get; set; }
    public string TaxSavingInvestments { get; set; }
    public string BankName { get; set; }
    public string AccountHolder { get; set; }
    public string AccountNumber { get; set; }
    public string IFSCCode { get; set; }
    public string Branch { get; set; }
    public string AccountType { get; set; }
    public int CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
}
public class ClientHealth
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string MedicalHistory { get; set; }
    public string CurrentMedications { get; set; }
    public string SmokingAlcoholHabits { get; set; }
    public string FamilyMedicalHistory { get; set; }
    public int CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
}

public class ClientDocument
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string FilePath { get; set; }
    public string Base64String { get; set; }
    public string MIME { get; set; }
    public int CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
}
