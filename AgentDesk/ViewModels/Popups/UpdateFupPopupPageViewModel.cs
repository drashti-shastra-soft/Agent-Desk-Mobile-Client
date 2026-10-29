using AgentDesk.Helpers;
using AgentDesk.Models;
using AgentDesk.Services;
using AgentDesk.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentDesk.ViewModels.Popups;

//public record UpdateFupResult(int PremiumPaidFor, string PaymentDetails);

//public partial class UpdateFupPopupPageViewModel : BaseViewModel
//{
//    private readonly TaskCompletionSource<UpdateFupResult?> _tcs = new();

//    // Await this to get the result (null = cancelled)
//    public Task<UpdateFupResult?> ResultTask => _tcs.Task;

//    public List<int> PremiumPaidForOptions { get; } = Enumerable.Range(1, 12).ToList();

//    [ObservableProperty] private Policy? policyModel;
//    [ObservableProperty] private string policyNumber = string.Empty;
//    [ObservableProperty] private string oldFup = "--";
//    [ObservableProperty] private string newFup = "--";
//    [ObservableProperty] private string payMode = "--";
//    [ObservableProperty] private int selectedPremiumPaidFor = 1;
//    [ObservableProperty] private string paymentDetails = string.Empty;

//    public UpdateFupPopupPageViewModel()
//    {
//    }

//    // Runs when the page's Policy query property sets PolicyModel
//    partial void OnPolicyModelChanged(Policy? value)
//    {
//        if (value is null)
//            return;

//        PolicyNumber = value.PolicyNumber ?? "--";
//        PayMode = value.PayMode ?? "--";

//        if (DateTime.TryParse(value.FUPDate?.ToString(), out var fup))
//        {
//            OldFup = fup.ToString("dd/MM/yyyy");
//            NewFup = fup.AddMonths(GetMonths(value.PayMode)).ToString("dd/MM/yyyy");
//        }
//    }

//    // M = monthly, Q = quarterly, H = half-yearly, Y = yearly
//    public static int GetMonths(string? payMode)
//    {
//        var key = payMode?.Trim().ToUpperInvariant().FirstOrDefault();
//        return key switch
//        {
//            'Q' => 3,
//            'H' => 6,
//            'Y' => 12,
//            _ => 1
//        };
//    }

//    [RelayCommand]
//    private async Task CancelAsync()
//    {
//        _tcs.TrySetResult(null);
//        await MopupService.Instance.PopAsync();
//    }

//    [RelayCommand]
//    private async Task UpdateAsync()
//    {
//        _tcs.TrySetResult(new UpdateFupResult(SelectedPremiumPaidFor, PaymentDetails));
//        await MopupService.Instance.PopAsync();
//    }

//    // Called by the page when the Android back button is pressed
//    public void SetCancelled() => _tcs.TrySetResult(null);
//}

public class PolicyPremiumUpdatedMessage : ValueChangedMessage<string>
{
    public PolicyPremiumUpdatedMessage(string policyNumber) : base(policyNumber) { }
}

public partial class UpdateFupPopupPageViewModel : BaseViewModel
{
    private readonly Policy _policy;
    private readonly DateTime? _oldFupDate;
    private readonly int _months;

    [ObservableProperty] private string policyNumber = "--";
    [ObservableProperty] private string oldFup = "--";
    [ObservableProperty] private string payMode = "--";
    [ObservableProperty]
    [Required(ErrorMessage = "Please enter payment amount.")]
    private string paymentDetails = string.Empty;

    public string PaymentDetailsError => GetFirstError(nameof(PaymentDetails));
    public bool HasPaymentDetailsError => !string.IsNullOrEmpty(PaymentDetailsError);

    // Changing this also refreshes NewFup
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NewFup))]
    private int selectedPremiumPaidFor = 1;

    // -12 to 12, without 0
    //public List<int> PremiumPaidForOptions { get; } =
    //    Enumerable.Range(-12, 25).Where(x => x != 0).ToList();

    public List<int> PremiumPaidForOptions { get; } =
    Enumerable.Range(-6, 13).Where(x => x != 0).ToList();

    //// Calculated: old FUP + (selected value x pay-mode months)
    //public string NewFup =>
    //    _oldFupDate.HasValue
    //        ? _oldFupDate.Value.AddMonths(SelectedPremiumPaidFor * _months).ToString("dd/MM/yyyy")
    //        : "--";


    // Calculated: old FUP + (selected value x pay-mode months)
    private DateTime? NewFupDate =>
        _oldFupDate?.AddMonths(SelectedPremiumPaidFor * _months);

    public string NewFup =>
        NewFupDate?.ToString("dd/MM/yyyy") ?? "--";
    public UpdateFupPopupPageViewModel(Policy policy)
    {
        ErrorsChanged += OnValidationErrorsChanged;
        _policy = policy;
        _months = GetMonths(policy.PayMode);

        PolicyNumber = policy.PolicyNumber ?? "--";
        PayMode = policy.PayMode ?? "--";

        if (DateTime.TryParse(policy.FUPDate?.ToString(), out var fup))
        {
            _oldFupDate = fup;
            OldFup = fup.ToString("dd/MM/yyyy");
        }
    }

    private void OnValidationErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        var propertyName = e.PropertyName;

        if (string.IsNullOrEmpty(propertyName) ||
            propertyName == nameof(PaymentDetails))
        {
            OnPropertyChanged(nameof(PaymentDetailsError));
            OnPropertyChanged(nameof(HasPaymentDetailsError));
        }
    }

    partial void OnPaymentDetailsChanged(string value)
    {
        if (HasAttemptedValidation)
            ValidateProperty(value, nameof(PaymentDetails));
    }

    // M = monthly, Q = quarterly, H = half-yearly, Y = yearly
    private static int GetMonths(string? payMode)
    {
        var key = payMode?.Trim().ToUpperInvariant().FirstOrDefault();
        return key switch
        {
            'Q' => 3,
            'H' => 6,
            'Y' => 12,
            _ => 1
        };
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await MopupService.Instance.PopAsync();
    }

    //[RelayCommand]
    //private async Task UpdateAsync()
    //{
    //    if (!_oldFupDate.HasValue)
    //        return;

    //    if (SelectedPremiumPaidFor < 0)
    //    {
    //        // TODO: call the API that reverses premiums once its method is known
    //        await Shell.Current.DisplayAlert("Not supported", "Reversing premiums is not available yet.", "OK");
    //        return;
    //    }

    //    try
    //    {
    //        IsBusy = true;

    //        // One premium record per premium paid, one pay-mode period apart
    //        for (int i = 0; i < SelectedPremiumPaidFor; i++)
    //        {
    //            var premium = new PolicyPremium
    //            {
    //                PolicyNumber = _policy.PolicyNumber,
    //                DueDate = _oldFupDate.Value.AddMonths(i * _months),
    //                AdjDate = DateTime.Now,
    //                Premium = _policy.PremiumAmount?.ToString() ?? string.Empty,
    //                IsAdjusted = true,
    //                CreatedBy = AppSession.UserId,
    //                CreatedDate = DateTime.Now
    //            };

    //            var response = await _policyService.AddPremiumAsync(premium);

    //            if (!response.IsSuccess)
    //            {
    //                await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
    //                return;
    //            }
    //        }

    //        await MopupService.Instance.PopAsync();
    //        WeakReferenceMessenger.Default.Send(new PolicyPremiumUpdatedMessage(_policy.PolicyNumber));
    //    }
    //    finally
    //    {
    //        IsBusy = false;
    //    }
    //}

    [RelayCommand]
    private async Task UpdateAsync()
    {
        if (!_oldFupDate.HasValue || !NewFupDate.HasValue)
            return;

        if (!ValidateForm())
            return;

        try
        {
            IsBusy = true;
            await PopLoaderService.ShowAsync();

            var premium = new PolicyPremium
            {
                PolicyNumber = _policy.PolicyNumber,
                DueDate = _oldFupDate.Value,          // old FUP date
                AdjDate = NewFupDate.Value,           // new FUP date
                Premium = PaymentDetails,             // text from the Payment Details field
                IsAdjusted = true,
                CreatedBy = AppSession.CurrentUser.Id,
                CreatedDate = DateTime.Now
            };

            var response = await _policyService.AddPremiumAsync(premium);


            //Debug.WriteLine(System.Text.Json.JsonSerializer.Serialize(response.Value, new JsonSerializerOptions
            //{
            //    WriteIndented = true
            //}));

            if (!response.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
                return;
            }
            await PopLoaderService.HideAsync();
            WeakReferenceMessenger.Default.Send(new PolicyPremiumUpdatedMessage(_policy.PolicyNumber));
            await MopupService.Instance.PopAsync();
        }
        finally
        {
            await PopLoaderService.HideAsync();
            IsBusy = false;
        }
    }
}