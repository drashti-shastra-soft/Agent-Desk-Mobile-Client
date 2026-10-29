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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.ViewModels;

public partial class AgencyPageViewModel : BaseViewModel
{

    [ObservableProperty]
    private ObservableCollection<Agency> agencyList = new();

    [ObservableProperty]
    private bool isRefreshing;
    public AgencyPageViewModel()
    {

    }

    public async Task OnAppearing()
    {
        await GetAgencyAsync();
    }

    [RelayCommand]
    private async Task GetAgencyAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            // Clear existing agency
            AgencyList.Clear();

            // Check internet connection
            if (!await NetworkService.CheckInternetAsync())
                return;

            // Show loader
            await PopLoaderService.ShowAsync();

            var request = new AgencyLister
            {
                SearchCriteria = new Agency(),

                Pagination = new Pagination
                {
                    PageSize = 10,
                    CurrentPage = 1
                }
            };

            var response = await _agencyService.GetAllAsync(request);

            if (response == null || !response.IsSuccess)
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Unable to get agency.",
                    ToastDuration.Long);

                return;
            }

            if (response.Value is AgencyLister result)
            {
                AgencyList.Clear();

                if (result.List != null)
                {
                    foreach (var agency in result.List)
                    {
                        AgencyList.Add(agency);
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
            // Hide loader if it was displayed
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
            await GetAgencyAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task EditAgencyAsync(Agency agency)
    {
        if (agency == null)
            return;
        await PopLoaderService.ShowAsync();
        await _navigationService.NavigateToAsync(
            nameof(AddAgencyPage), new Dictionary<string, object>
            {
                { "Agency", agency }
            });
        await PopLoaderService.HideAsync();
    }

    //[RelayCommand]
    //private async Task DeleteAgencyAsync(Agency agency)
    //{
    //    if (agency == null || agency.Id <= 0)
    //        return;

    //    try
    //    {
    //        var confirm = await _dialogueService.ShowConfirmationAsync(
    //            "Delete Agency",
    //            "Are you sure you want to delete this Agency?",
    //            "Yes",
    //            "No");

    //        if (!confirm)
    //            return;

    //        IsBusy = true;

    //        var response = await _agencyService.DeleteAsync(
    //            agency.Id, AppSession.CurrentUser.Id);


    //        if (response != null && response.IsSuccess)
    //        {
    //            // Remove from CollectionView immediately
    //            AgencyList.Remove(agency);

    //            await _dialogueService.ShowToastAsync(
    //                "Agency deleted successfully.",
    //                ToastDuration.Short);
    //        }
    //        else
    //        {
    //            await _dialogueService.ShowToastAsync(
    //                response?.ErrorMessage ?? "Failed to delete agency.",
    //                ToastDuration.Long);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await _dialogueService.ShowToastAsync(
    //            ex.Message,
    //            ToastDuration.Long);
    //    }
    //    finally
    //    {
    //        IsBusy = false;
    //    }
    //}



    [RelayCommand]
    private async Task DeleteAgencyAsync(Agency agency)
    {
        if (agency == null || agency.Id <= 0)
            return;

        try
        {
            var confirm = await _dialogueService.ShowConfirmationAsync(
                "Delete Agency",
                "Are you sure you want to delete this Agency?");

            if (!confirm)
                return;

            IsBusy = true;
            await PopLoaderService.ShowAsync();
            var response = await _agencyService.DeleteAsync(
                agency.Id,
                AppSession.CurrentUser.Id);

            if (response != null && response.IsSuccess)
            {
                AgencyList.Remove(agency);

                await _dialogueService.ShowToastAsync(
                    "Agency deleted successfully.",
                    ToastDuration.Short);
            }
            else
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Failed to delete agency.",
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


    [RelayCommand]
    private async Task AddAgencyAsync()
    {
        await _navigationService.NavigateToAsync(
            nameof(AddAgencyPage));
    }
}