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

public partial class GroupPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<GroupModel> groupList = new();

    [ObservableProperty]
    private bool isRefreshing;

    public GroupPageViewModel()
    {
    }

    public async Task OnAppearing()
    {
        await GetGroupsAsync();
    }

    [RelayCommand]
    private async Task GetGroupsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            // Clear existing groups
            GroupList.Clear();

            // Check internet connection
            if (!await NetworkService.CheckInternetAsync())
                return;

            // Show loader
            await PopLoaderService.ShowAsync();

            var request = new GroupLister
            {
                SearchCriteria = new GroupModel(),

                Pagination = new Pagination
                {
                    PageSize = 10,
                    CurrentPage = 1
                }
            };

            var response = await _groupService.GetAllAsync(request);

            if (response == null || !response.IsSuccess)
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Unable to get groups.",
                    ToastDuration.Long);

                return;
            }

            if (response.Value is GroupLister result)
            {
                GroupList.Clear();

                if (result.List != null)
                {
                    foreach (var group in result.List)
                    {
                        GroupList.Add(group);
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
            await GetGroupsAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task EditGroupAsync(GroupModel group)
    {
        if (group == null)
            return;
        await PopLoaderService.ShowAsync();
        await _navigationService.NavigateToAsync(
            nameof(AddGroupPage),new Dictionary<string, object>
            {
                { "GroupModel", group }
            });
        await PopLoaderService.HideAsync();
    }

    [RelayCommand]
    private async Task DeleteGroupAsync(GroupModel group)
    {
        if (group == null || group.Id <= 0)
            return;

        try
        {
            var confirm = await _dialogueService.ShowConfirmationAsync(
                "Delete Group",
                "Are you sure you want to delete this group?");

            if (!confirm)
                return;

            IsBusy = true;
            await PopLoaderService.ShowAsync();
            var response = await _groupService.DeleteAsync(
                group.Id, AppSession.CurrentUser.Id);


            if (response != null && response.IsSuccess)
            {
                // Remove from CollectionView immediately
                GroupList.Remove(group);

                await _dialogueService.ShowToastAsync(
                    "Group deleted successfully.",
                    ToastDuration.Short);
            }
            else
            {
                await _dialogueService.ShowToastAsync(
                    response?.ErrorMessage ?? "Failed to delete group.",
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
    //private async Task DeleteGroupAsync(GroupModel group)
    //{
    //    if (group == null || group.Id <= 0)
    //        return;

    //    try
    //    {
    //        var confirm = await _dialogueService.ShowConfirmationAsync(
    //            "Delete Group",
    //            "Are you sure you want to delete this group?",
    //            "Yes",
    //            "No");

    //        if (!confirm)
    //            return;

    //        IsBusy = true;

    //        var response = await _groupService.DeleteAsync(
    //            group.Id, AppSession.CurrentUser.Id);

    
    //        if (response != null && response.IsSuccess)
    //        {
    //            // Remove from CollectionView immediately
    //            GroupList.Remove(group);

    //            await _dialogueService.ShowToastAsync(
    //                "Group deleted successfully.",
    //                ToastDuration.Short);
    //        }
    //        else
    //        {
    //            await _dialogueService.ShowToastAsync(
    //                response?.ErrorMessage ?? "Failed to delete group.",
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
    private async Task AddGroupAsync()
    {
        await _navigationService.NavigateToAsync(
            nameof(AddGroupPage));
    }
}