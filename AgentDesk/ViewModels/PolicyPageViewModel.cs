using AgentDesk.Helpers;
using AgentDesk.Models;
using AgentDesk.Models.Common;
using AgentDesk.Services;
using AgentDesk.ViewModels.Popups;
using AgentDesk.Views;
using AgentDesk.Views.Popups;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace AgentDesk.ViewModels;

#region old
//public partial class PolicyPageViewModel : BaseViewModel
//{
//    // Full list from the API (never filtered)
//    private readonly List<Policy> _allPolicies = new();

//    // List shown in the CollectionView (filtered by SearchText)
//    [ObservableProperty]
//    private ObservableCollection<Policy> policyList = new();

//    [ObservableProperty]
//    private bool isRefreshing;

//    [ObservableProperty]
//    private bool isAllExpanded = true;

//    [ObservableProperty]
//    private string searchText = string.Empty;

//    public PolicyPageViewModel()
//    {
//    }

//    public async Task OnAppearing()
//    {
//        SearchText = string.Empty;
//        await GetPolicysAsync();
//    }

//    // ---------------- Search ----------------

//    private CancellationTokenSource? _searchCts;

//    // Filters ~300 ms after the user stops typing
//    partial void OnSearchTextChanged(string value)
//    {
//        // Clearing the box (✕ button) should restore the list right away
//        if (string.IsNullOrEmpty(value))
//        {
//            _searchCts?.Cancel();
//            ApplyFilter();
//            return;
//        }

//        _ = DebounceFilterAsync();
//    }

//    private async Task DebounceFilterAsync()
//    {
//        _searchCts?.Cancel();
//        _searchCts = new CancellationTokenSource();
//        var token = _searchCts.Token;

//        try
//        {
//            await Task.Delay(300, token);
//            ApplyFilter();   // continues on the UI thread
//        }
//        catch (TaskCanceledException)
//        {
//            // a newer keystroke arrived, so ignore this one
//        }
//    }

//    private void ApplyFilter()
//    {
//        var text = SearchText?.Trim();

//        IEnumerable<Policy> items = _allPolicies;

//        if (!string.IsNullOrEmpty(text))
//        {
//            items = items.Where(p =>
//                Matches(p.PolicyNumber, text) ||
//                Matches(p.Client, text) ||
//                Matches(p.PolicyType, text) ||
//                Matches(p.PolicyStatus, text) ||
//                Matches(p.PayMode, text) ||
//                Matches(p.PlanTermPpt, text));
//        }

//        var result = items.ToList();

//        // Same items as now? Don't rebuild the CollectionView
//        if (result.SequenceEqual(PolicyList))
//            return;

//        PolicyList = new ObservableCollection<Policy>(result);
//    }

//    private static bool Matches(string? source, string text) =>
//        source?.Contains(text, StringComparison.OrdinalIgnoreCase) ?? false;

//    // ---------------- Expand / collapse all ----------------

//    [RelayCommand]
//    private void ToggleAll()
//    {
//        IsAllExpanded = !IsAllExpanded;

//        foreach (var policy in _allPolicies)
//            policy.IsExpanded = IsAllExpanded;
//    }

//    // ---------------- Commands ----------------

//    [RelayCommand]
//    private async Task AddPolicyAsync()
//    {
//        await _navigationService.NavigateToAsync(
//            nameof(AddClientPage));
//    }

//    [RelayCommand]
//    private async Task GetPolicysAsync()
//    {
//        if (IsBusy)
//            return;

//        try
//        {
//            IsBusy = true;

//            // Clear existing Policys
//            _allPolicies.Clear();
//            PolicyList.Clear();

//            // Check internet connection
//            if (!await NetworkService.CheckInternetAsync())
//                return;

//            // Show loader
//            await PopLoaderService.ShowAsync();

//            var request = new PolicyLister
//            {
//                SearchCriteria = new Policy(),

//                Pagination = new Pagination
//                {
//                    PageSize = 10,
//                    CurrentPage = 1
//                }
//            };

//            var response = await _policyService.GetAllAsync(request);

//            if (response == null || !response.IsSuccess)
//            {
//                await _dialogueService.ShowToastAsync(
//                    response?.ErrorMessage ?? "Unable to get policy.",
//                    ToastDuration.Long);

//                return;
//            }

//            Debug.WriteLine(JsonSerializer.Serialize(response.Value, new JsonSerializerOptions
//            {
//                WriteIndented = true
//            }));

//            if (response.Value is PolicyLister result)
//            {
//                _allPolicies.Clear();

//                if (result.List != null)
//                {
//                    foreach (var policy in result.List)
//                    {
//                        policy.IsExpanded = IsAllExpanded;   // new cards follow the header button state
//                        _allPolicies.Add(policy);
//                    }
//                }

//                ApplyFilter();   // fills PolicyList and keeps any text already typed
//            }
//        }
//        catch (Exception ex)
//        {
//            await _dialogueService.ShowToastAsync(
//                ex.Message,
//                ToastDuration.Long);
//        }
//        finally
//        {
//            // Hide loader if it was displayed
//            await PopLoaderService.HideAsync();
//            IsBusy = false;
//        }
//    }

//    [RelayCommand]
//    private async Task RefreshAsync()
//    {
//        if (IsBusy)
//            return;

//        try
//        {
//            IsRefreshing = true;
//            SearchText = string.Empty;
//            await GetPolicysAsync();
//        }
//        finally
//        {
//            IsRefreshing = false;
//        }
//    }

//    [RelayCommand]
//    private async Task EditPolicyAsync(Policy policy)
//    {
//        if (policy == null)
//            return;

//        await _navigationService.NavigateToAsync(
//            nameof(AddClientPage), new Dictionary<string, object>
//            {
//                { "Policy", policy }
//            });
//    }

//    [RelayCommand]
//    private async Task UpdateFupAsync(Policy policy)
//    {
//        if (policy == null)
//            return;

//        var viewModel = new UpdateFupPopupPageViewModel(policy);
//        var popup = new UpdateFupPopupPage(viewModel);
//        await MopupService.Instance.PushAsync(popup);
//    }

//    [RelayCommand]
//    private async Task DeletePolicyAsync(Policy policy)
//    {
//        if (policy == null || policy.Id <= 0)
//            return;

//        try
//        {
//            var confirm = await _dialogueService.ShowConfirmationAsync(
//               "Delete Policy",
//               "Are you sure you want to delete this policy?");

//            if (!confirm)
//                return;

//            IsBusy = true;
//            await PopLoaderService.ShowAsync();

//            var response = await _policyService.DeleteAsync(
//                policy.Id, AppSession.CurrentUser.Id);

//            if (response != null && response.IsSuccess)
//            {
//                // Remove from both lists so the item doesn't reappear after a search
//                _allPolicies.Remove(policy);
//                PolicyList.Remove(policy);

//                await _dialogueService.ShowToastAsync(
//                    "Policy deleted successfully.",
//                    ToastDuration.Short);
//            }
//            else
//            {
//                await _dialogueService.ShowToastAsync(
//                    response?.ErrorMessage ?? "Failed to delete policy.",
//                    ToastDuration.Long);
//            }
//        }
//        catch (Exception ex)
//        {
//            await _dialogueService.ShowToastAsync(
//                ex.Message,
//                ToastDuration.Long);
//        }
//        finally
//        {
//            await PopLoaderService.HideAsync();
//            IsBusy = false;
//        }
//    }
//}
#endregion
#region new 
//public partial class PolicyPageViewModel : BaseViewModel
//{
//    [ObservableProperty]
//    private ObservableCollection<Policy> policyList = new();

//    [ObservableProperty]
//    private bool isRefreshing;

//    [ObservableProperty]
//    private bool isAllExpanded = true;

//    [ObservableProperty]
//    private string searchText = string.Empty;

//    private CancellationTokenSource? _searchCts;
//    private int _loadVersion;          // lets us ignore out-of-date responses
//    private bool _suppressSearch;      // true while we clear SearchText from code

//    public PolicyPageViewModel()
//    {
//    }

//    public async Task OnAppearing()
//    {
//        await GetPolicysAsync();
//    }

//    // ---------------- Search (calls the API) ----------------

//    partial void OnSearchTextChanged(string value)
//    {
//        if (_suppressSearch)
//            return;

//        // Clearing the box reloads at once; typing waits until the user pauses
//        _ = DebounceSearchAsync(string.IsNullOrWhiteSpace(value) ? 0 : 400);
//    }

//    private async Task DebounceSearchAsync(int delayMs)
//    {
//        _searchCts?.Cancel();
//        _searchCts = new CancellationTokenSource();
//        var token = _searchCts.Token;

//        try
//        {
//            if (delayMs > 0)
//                await Task.Delay(delayMs, token);

//            await LoadPoliciesAsync(showLoader: false);
//        }
//        catch (TaskCanceledException)
//        {
//            // a newer keystroke arrived, so ignore this one
//        }
//    }

//    // Keyboard search key
//    [RelayCommand]
//    private async Task SearchAsync()
//    {
//        _searchCts?.Cancel();
//        await LoadPoliciesAsync(showLoader: false);
//    }

//    // ---------------- Expand / collapse all ----------------

//    [RelayCommand]
//    private void ToggleAll()
//    {
//        IsAllExpanded = !IsAllExpanded;

//        foreach (var policy in PolicyList)
//            policy.IsExpanded = IsAllExpanded;
//    }

//    // ---------------- Loading ----------------

//    [RelayCommand]
//    private async Task AddPolicyAsync()
//    {
//        await _navigationService.NavigateToAsync(
//            nameof(AddClientPage));
//    }

//    // Full load with loader (page open, refresh)
//    [RelayCommand]
//    private async Task GetPolicysAsync()
//    {
//        if (IsBusy)
//            return;

//        try
//        {
//            IsBusy = true;
//            await LoadPoliciesAsync(showLoader: true);
//        }
//        finally
//        {
//            IsBusy = false;
//        }
//    }

//    private async Task LoadPoliciesAsync(bool showLoader)
//    {
//        var version = ++_loadVersion;

//        try
//        {
//            if (!await NetworkService.CheckInternetAsync())
//                return;

//            if (showLoader)
//            {
//                PolicyList = new ObservableCollection<Policy>();
//                await PopLoaderService.ShowAsync();
//            }

//            var text = SearchText?.Trim();

//            var request = new PolicyLister
//            {
//                SearchCriteria = new Policy
//                {
//                    // sent to the API; empty means no filter
//                    PolicyNumber = string.IsNullOrEmpty(text) ? null : text
//                },

//                Pagination = new Pagination
//                {
//                    PageSize = 10,
//                    CurrentPage = 1
//                }
//            };

//            var response = await _policyService.GetAllAsync(request);

//            // A newer search/refresh started while we waited, so drop this result
//            if (version != _loadVersion)
//                return;

//            if (response == null || !response.IsSuccess)
//            {
//                await _dialogueService.ShowToastAsync(
//                    response?.ErrorMessage ?? "Unable to get policy.",
//                    ToastDuration.Long);
//                return;
//            }

//            if (response.Value is PolicyLister result)
//            {
//                var items = result.List ?? new List<Policy>();

//                foreach (var policy in items)
//                    policy.IsExpanded = IsAllExpanded;   // new cards follow the header button state

//                PolicyList = new ObservableCollection<Policy>(items);
//            }
//        }
//        catch (Exception ex)
//        {
//            await _dialogueService.ShowToastAsync(
//                ex.Message,
//                ToastDuration.Long);
//        }
//        finally
//        {
//            if (showLoader)
//                await PopLoaderService.HideAsync();
//        }
//    }

//    [RelayCommand]
//    private async Task RefreshAsync()
//    {
//        if (IsBusy)
//            return;

//        try
//        {
//            IsRefreshing = true;

//            // Remove the search filter without triggering a second API call
//            _searchCts?.Cancel();
//            _suppressSearch = true;
//            SearchText = string.Empty;
//            _suppressSearch = false;

//            await GetPolicysAsync();
//        }
//        finally
//        {
//            IsRefreshing = false;
//        }
//    }

//    // ---------------- Item actions ----------------

//    [RelayCommand]
//    private async Task EditPolicyAsync(Policy policy)
//    {
//        if (policy == null)
//            return;

//        await _navigationService.NavigateToAsync(
//            nameof(AddClientPage), new Dictionary<string, object>
//            {
//                { "Policy", policy }
//            });
//    }

//    [RelayCommand]
//    private async Task UpdateFupAsync(Policy policy)
//    {
//        if (policy == null)
//            return;

//        var viewModel = new UpdateFupPopupPageViewModel(policy);
//        var popup = new UpdateFupPopupPage(viewModel);
//        await MopupService.Instance.PushAsync(popup);
//    }

//    [RelayCommand]
//    private async Task DeletePolicyAsync(Policy policy)
//    {
//        if (policy == null || policy.Id <= 0)
//            return;

//        try
//        {
//            var confirm = await _dialogueService.ShowConfirmationAsync(
//               "Delete Policy",
//               "Are you sure you want to delete this policy?");

//            if (!confirm)
//                return;

//            IsBusy = true;
//            await PopLoaderService.ShowAsync();

//            var response = await _policyService.DeleteAsync(
//                policy.Id, AppSession.CurrentUser.Id);

//            if (response != null && response.IsSuccess)
//            {
//                PolicyList.Remove(policy);

//                await _dialogueService.ShowToastAsync(
//                    "Policy deleted successfully.",
//                    ToastDuration.Short);
//            }
//            else
//            {
//                await _dialogueService.ShowToastAsync(
//                    response?.ErrorMessage ?? "Failed to delete policy.",
//                    ToastDuration.Long);
//            }
//        }
//        catch (Exception ex)
//        {
//            await _dialogueService.ShowToastAsync(
//                ex.Message,
//                ToastDuration.Long);
//        }
//        finally
//        {
//            await PopLoaderService.HideAsync();
//            IsBusy = false;
//        }
//    }
//}
#endregion

public partial class PolicyPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<Policy> policyList = new();

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private bool isAllExpanded = true;

    [ObservableProperty]
    private string searchText = string.Empty;

    public PolicyPageViewModel()
    {
    }

    [RelayCommand]
    private async Task AddPolicyAsync()
    {
        await _navigationService.NavigateToAsync(
            nameof(AddClientPage));
    }

    public async Task OnAppearing()
    {
        await GetPolicysAsync();
    }

    [RelayCommand]
    private void ToggleAll()
    {
        IsAllExpanded = !IsAllExpanded;

        foreach (var policy in PolicyList)
            policy.IsExpanded = IsAllExpanded;
    }

    // Called when the user presses Search on the MAUI SearchBar.
    //[RelayCommand]
    //private async Task SearchPoliciesAsync()
    //{
    //    await GetPolicysAsync();
    //}

    private CancellationTokenSource? _searchCts;

    partial void OnSearchTextChanged(string value)
    {
        _ = DebounceSearchAsync();
    }

    private async Task DebounceSearchAsync()
    {
        _searchCts?.Cancel();
        _searchCts?.Dispose();

        var cts = new CancellationTokenSource();
        _searchCts = cts;

        try
        {
            // Wait until the user pauses typing.
            await Task.Delay(400, cts.Token);

            // Wait for any existing API operation to finish.
            while (IsBusy)
            {
                await Task.Delay(100, cts.Token);
            }

            if (!cts.IsCancellationRequested)
            {
                await GetPolicysAsync();
            }
        }
        catch (OperationCanceledException)
        {
            // A newer search has replaced this one.
        }
    }

    [RelayCommand]
    private async Task GetPolicysAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            if (!await NetworkService.CheckInternetAsync())
            {
                await _dialogueService.ShowToastAsync(
                    "Please check your internet connection.",
                    ToastDuration.Long);

                return;
            }

            await PopLoaderService.ShowAsync();

            var request = new PolicyLister
            {
                SearchCriteria = new Policy
                {
                    // Empty search text returns all policies,
                    // provided the backend treats it as no filter.
                    //PolicyNumber = SearchText?.Trim()
                     PolicyNumber = string.IsNullOrWhiteSpace(SearchText)
                         ? null
                         : SearchText.Trim()
                },

                Pagination = new Pagination
                {
                    PageSize = 10,
                    CurrentPage = 1
                }
            };

            var response = await _policyService.GetAllAsync(request);

            if (response == null || !response.IsSuccess)
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Unable to get policies.",
                    ToastDuration.Long);

                return;
            }

            if (response.Value is PolicyLister result)
            {
                PolicyList.Clear();

                if (result.List != null)
                {
                    foreach (var policy in result.List)
                    {
                        policy.IsExpanded = IsAllExpanded;
                        PolicyList.Add(policy);
                    }
                }
            }
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
    private async Task RefreshAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsRefreshing = true;
            await GetPolicysAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task EditPolicyAsync(Policy policy)
    {
        if (policy == null)
            return;

        await _navigationService.NavigateToAsync(
            nameof(AddClientPage),
            new Dictionary<string, object>
            {
                { "Policy", policy }
            });
    }

    [RelayCommand]
    private async Task UpdateFupAsync(Policy policy)
    {
        if (policy == null)
            return;

        var viewModel = new UpdateFupPopupPageViewModel(policy);
        var popup = new UpdateFupPopupPage(viewModel);

        await MopupService.Instance.PushAsync(popup);
    }

    [RelayCommand]
    private async Task DeletePolicyAsync(Policy policy)
    {
        if (policy == null || policy.Id <= 0)
            return;

        if (IsBusy)
            return;

        try
        {
            var confirm = await _dialogueService.ShowConfirmationAsync(
                "Delete Policy",
                "Are you sure you want to delete this policy?");

            if (!confirm)
                return;

            IsBusy = true;
            await PopLoaderService.ShowAsync();

            var response = await _policyService.DeleteAsync(
                policy.Id,
                AppSession.CurrentUser.Id);

            if (response != null && response.IsSuccess)
            {
                PolicyList.Remove(policy);

                await _dialogueService.ShowToastAsync(
                    "Policy deleted successfully.",
                    ToastDuration.Short);
            }
            else
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Failed to delete policy.",
                    ToastDuration.Long);
            }
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
}
