using CommunityToolkit.Maui.Core;

namespace AgentDesk.Services.Interfaces;

public interface IDialogueService
{
    Task ShowToastAsync(
        string message,
        ToastDuration duration = ToastDuration.Short);

    Task ShowAlertAsync(
        string title,
        string message,
        string cancel = "OK");

    Task<bool> ShowConfirmationAsync(
        string title,
        string message,
        string accept,
        string cancel);

    Task<bool> ShowConfirmationAsync(
        string title,
        string message);
}
