using AgentDesk.Services;
using CommunityToolkit.Maui.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AgentDesk.ViewModels.Tabs;

/// <summary>
/// Shared logic for the Identification / Financial / Health tab view models.
/// Assumes BaseViewModel provides: IsBusy, HasAttemptedValidation, ValidateForm(),
/// ValidateProperty(), ClearErrors(), GetFirstError(), _dialogueService.
/// </summary>
public abstract class ClientTabViewModelBase : BaseViewModel
{
    protected int ClientId;
    protected bool IsLoaded;

    protected ClientTabViewModelBase()
    {
        // refresh every *Error / Has*Error binding whenever validation state changes
        ErrorsChanged += (_, _) => OnPropertyChanged(string.Empty);
    }

    /// <summary>Re-validate a property live once the user has tried to submit.</summary>
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        var name = e.PropertyName;
        if (!HasAttemptedValidation || string.IsNullOrEmpty(name))
            return;

        var prop = GetType().GetProperty(name);
        if (prop != null && prop.GetCustomAttributes<ValidationAttribute>(true).Any())
            ValidateProperty(prop.GetValue(this), name);
    }

    public async Task LoadAsync(int clientId)
    {
        if (IsLoaded && ClientId == clientId)
            return;

        ClientId = clientId;

        HasAttemptedValidation = false;
        await LoadCoreAsync();
        ClearErrors();

        IsLoaded = true;
    }

    public void Reset()
    {
        IsLoaded = false;
        HasAttemptedValidation = false;
        ClearFields();
        ClearErrors();
    }

    protected abstract Task LoadCoreAsync();
    protected abstract void ClearFields();

    /// <summary>Validate -> check internet -> loader -> call API -> toast.</summary>
    protected async Task RunSaveAsync(
        Func<Task<(bool Success, string? Error)>> save,
        string successMessage)
    {
        if (IsBusy)
            return;

        if (!ValidateForm())
            return;

        if (!await NetworkService.CheckInternetAsync())
            return;

        try
        {
            IsBusy = true;
            await PopLoaderService.ShowAsync();

            var (success, error) = await save();

            await _dialogueService.ShowToastAsync(
                success ? successMessage : error ?? "Unable to save.",
                success ? ToastDuration.Short : ToastDuration.Long);
        }
        catch (Exception ex)
        {
            await _dialogueService.ShowToastAsync(ex.Message, ToastDuration.Long);
        }
        finally
        {
            IsBusy = false;
            await PopLoaderService.HideAsync();
        }
    }
}
