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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentDesk.ViewModels;

public partial class ClientPageViewModel:BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<Client> clientList = new();

    [ObservableProperty]
    private bool isRefreshing;

    public ClientPageViewModel()
    { 
    }

    [RelayCommand]
    private async Task AddClientAsync()
    {
        await _navigationService.NavigateToAsync(
            nameof(AddClientPage));
    }


    public async Task OnAppearing()
    {
        await GetClientsAsync();
    }

    [RelayCommand]
    private async Task GetClientsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            // Clear existing Clients
            ClientList.Clear();

            // Check internet connection
            if (!await NetworkService.CheckInternetAsync())
                return;

            // Show loader
            await PopLoaderService.ShowAsync();

            var request = new ClientLister
            {
                SearchCriteria = new Client(),

                Pagination = new Pagination
                {
                    PageSize = 10,
                    CurrentPage = 1
                }
            };

            var response = await _clientService.GetAllAsync(request);

            //Debug.WriteLine(System.Text.Json.JsonSerializer.Serialize(response.Value, new JsonSerializerOptions
            //{
            //    WriteIndented = true
            //}));

            if (response == null || !response.IsSuccess)
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Unable to get clients.",
                    ToastDuration.Long);

                return;
            }

            if (response.Value is ClientLister result)
            {
                ClientList.Clear();

                if (result.List != null)
                {
                    foreach (var client in result.List)
                    {
                        ClientList.Add(client);
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
            await GetClientsAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task EditClientAsync(Client client)
    {
        if (client == null)
            return;
        await PopLoaderService.ShowAsync();
        await _navigationService.NavigateToAsync(
            nameof(AddClientPage), new Dictionary<string, object>
            {
                { "Client", client }
            });
        await PopLoaderService.HideAsync();
    }

    [RelayCommand]
    private async Task DeleteClientAsync(Client client)
    {
        if (client == null || client.Id <= 0)
            return;

        try
        {
            var confirm = await _dialogueService.ShowConfirmationAsync(
                "Delete Client",
                "Are you sure you want to delete this client?");


            if (!confirm)
                return;

            IsBusy = true;
            await PopLoaderService.ShowAsync();
            var response = await _clientService.DeleteAsync(
                client.Id, AppSession.CurrentUser.Id);


            if (response != null && response.IsSuccess)
            {
                // Remove from CollectionView immediately
                ClientList.Remove(client);

                await _dialogueService.ShowToastAsync(
                    "Client deleted successfully.",
                    ToastDuration.Short);
            }
            else
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Failed to delete client.",
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
    //[RelayCommand]
    //private async Task DeleteClientAsync(Client client)
    //{
    //    if (client == null || client.Id <= 0)
    //        return;

    //    try
    //    {
    //        var confirm = await _dialogueService.ShowConfirmationAsync(
    //            "Delete Client",
    //            "Are you sure you want to delete this client?",
    //            "Yes",
    //            "No");

    //        if (!confirm)
    //            return;

    //        IsBusy = true;

    //        var response = await _clientService.DeleteAsync(
    //            client.Id, AppSession.CurrentUser.Id);


    //        if (response != null && response.IsSuccess)
    //        {
    //            // Remove from CollectionView immediately
    //            ClientList.Remove(client);

    //            await _dialogueService.ShowToastAsync(
    //                "Client deleted successfully.",
    //                ToastDuration.Short);
    //        }
    //        else
    //        {
    //            await _dialogueService.ShowToastAsync(
    //                response?.ErrorMessage ?? "Failed to delete client.",
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

}
