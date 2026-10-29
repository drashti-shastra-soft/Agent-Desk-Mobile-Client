using AgentDesk.Models;
using AgentDesk.ViewModels.Tabs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AgentDesk.ViewModels;

public partial class FinancialTabViewModel : ClientTabViewModelBase
{
    private const string AmountPattern = @"^\d+(\.\d{1,2})?$";

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter annual income.")]
    [RegularExpression(AmountPattern, ErrorMessage = "Please enter a valid amount.")]
    private string annualIncome = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter existing loans EMI.")]
    [RegularExpression(AmountPattern, ErrorMessage = "Please enter a valid amount.")]
    private string existingLoansEmi = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter tax saving investments.")]
    [RegularExpression(AmountPattern, ErrorMessage = "Please enter a valid amount.")]
    private string taxSavingInvestments = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter bank name.")]
    private string bankName = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter account holder name.")]
    private string accountHolder = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter account number.")]
    [RegularExpression(@"^\d{9,18}$", ErrorMessage = "Please enter a valid account number.")]
    private string accountNumber = string.Empty;

    [ObservableProperty]
    [Required(ErrorMessage = "Please enter IFSC code.")]
    [RegularExpression(@"^[A-Za-z]{4}0[A-Za-z0-9]{6}$", ErrorMessage = "Please enter a valid IFSC code.")]
    private string ifscCode = string.Empty;

    public List<string> AccountTypeList { get; } = new() { "Saving Account", "Current Account" };

    [ObservableProperty]
    private string accountType = "Saving Account";

    public string AnnualIncomeError => GetFirstError(nameof(AnnualIncome));
    public bool HasAnnualIncomeError => !string.IsNullOrEmpty(AnnualIncomeError);

    public string ExistingLoansEmiError => GetFirstError(nameof(ExistingLoansEmi));
    public bool HasExistingLoansEmiError => !string.IsNullOrEmpty(ExistingLoansEmiError);

    public string TaxSavingInvestmentsError => GetFirstError(nameof(TaxSavingInvestments));
    public bool HasTaxSavingInvestmentsError => !string.IsNullOrEmpty(TaxSavingInvestmentsError);

    public string BankNameError => GetFirstError(nameof(BankName));
    public bool HasBankNameError => !string.IsNullOrEmpty(BankNameError);

    public string AccountHolderError => GetFirstError(nameof(AccountHolder));
    public bool HasAccountHolderError => !string.IsNullOrEmpty(AccountHolderError);

    public string AccountNumberError => GetFirstError(nameof(AccountNumber));
    public bool HasAccountNumberError => !string.IsNullOrEmpty(AccountNumberError);

    public string IfscCodeError => GetFirstError(nameof(IfscCode));
    public bool HasIfscCodeError => !string.IsNullOrEmpty(IfscCodeError);

    protected override async Task LoadCoreAsync()
    {
        //var response = await _clientService.GetFinancialAsync(ClientId);

        //if (response?.IsSuccess == true && response.Value is ClientFinancial m)
        //{
        //    AnnualIncome = m.AnnualIncome.ToString(CultureInfo.InvariantCulture);
        //    ExistingLoansEmi = m.ExistingLoansEmi.ToString(CultureInfo.InvariantCulture);
        //    TaxSavingInvestments = m.TaxSavingInvestments.ToString(CultureInfo.InvariantCulture);
        //    BankName = m.BankName ?? string.Empty;
        //    AccountHolder = m.AccountHolder ?? string.Empty;
        //    AccountNumber = m.AccountNumber ?? string.Empty;
        //    IfscCode = m.IfscCode ?? string.Empty;
        //    AccountType = string.IsNullOrWhiteSpace(m.AccountType) ? "Saving Account" : m.AccountType;
        //}
    }

    protected override void ClearFields()
    {
        AnnualIncome = ExistingLoansEmi = TaxSavingInvestments = string.Empty;
        BankName = AccountHolder = AccountNumber = IfscCode = string.Empty;
        AccountType = "Saving Account";
    }

    private static decimal ToDecimal(string value) =>
        decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var d) ? d : 0;

    //[RelayCommand]
    //private Task SaveAsync() => RunSaveAsync(async () =>
    //{
    //    var model = new ClientFinancial
    //    {
    //        ClientId = ClientId,
    //        AnnualIncome = ToDecimal(AnnualIncome),
    //        ExistingLoansEmi = ToDecimal(ExistingLoansEmi),
    //        TaxSavingInvestments = ToDecimal(TaxSavingInvestments),
    //        BankName = BankName.Trim(),
    //        AccountHolder = AccountHolder.Trim(),
    //        AccountNumber = AccountNumber.Trim(),
    //        IfscCode = IfscCode.Trim().ToUpperInvariant(),
    //        AccountType = AccountType
    //    };

    //    var r = await _clientService.SaveFinancialAsync(model);
    //    return (r?.IsSuccess == true, r?.ErrorMessage);
    //}, "Financial information saved successfully.");
}
