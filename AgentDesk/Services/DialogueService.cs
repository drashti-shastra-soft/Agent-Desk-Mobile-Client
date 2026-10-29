using AgentDesk.Services.Interfaces;
using AgentDesk.ViewModels.Popups;
using AgentDesk.Views.Popups;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Mopups.Services;

namespace AgentDesk.Services;

public class DialogueService : IDialogueService
{
    public async Task ShowToastAsync(
        string message,
        ToastDuration duration = ToastDuration.Short)
    {
        var toast = Toast.Make(message, duration);

        await toast.Show();
    }

    public async Task ShowAlertAsync(string title,string message,string cancel = "OK")
    {
        var page = Application.Current?
            .Windows
            .FirstOrDefault()?
            .Page;

        if (page != null)
        {
            await page.DisplayAlert(
                title,
                message,
                cancel);
        }
    }

    public async Task<bool> ShowConfirmationAsync(string title,string message,string accept,string cancel)
    {
        return await Application.Current!.MainPage!.DisplayAlert(
            title,
            message,
            accept,
            cancel);
    }

    public async Task<bool> ShowConfirmationAsync(string title,string message)
    {
        var viewModel = new CommonPopupPageViewModel(title,message);

        var popup = new CommonPopupPage(viewModel);

        var confirmation = new TaskCompletionSource<bool>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        EventHandler? confirmedHandler = null;
        EventHandler? cancelledHandler = null;

        confirmedHandler = async (s, e) =>
        {
            viewModel.Confirmed -= confirmedHandler;
            viewModel.Cancelled -= cancelledHandler;

            try
            {
                await MopupService.Instance.PopAsync();
                confirmation.TrySetResult(true);
            }
            catch (Exception ex)
            {
                confirmation.TrySetException(ex);
            }
        };

        cancelledHandler = async (s, e) =>
        {
            viewModel.Confirmed -= confirmedHandler;
            viewModel.Cancelled -= cancelledHandler;

            try
            {
                await MopupService.Instance.PopAsync();
                confirmation.TrySetResult(false);
            }
            catch (Exception ex)
            {
                confirmation.TrySetException(ex);
            }
        };

        viewModel.Confirmed += confirmedHandler;
        viewModel.Cancelled += cancelledHandler;

        await MopupService.Instance.PushAsync(popup);

        return await confirmation.Task;
    }

}
