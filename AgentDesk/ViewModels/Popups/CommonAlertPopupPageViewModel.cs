using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AgentDesk.ViewModels.Popups;

public partial class CommonAlertPopupPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string title = "Alert";

    [ObservableProperty]
    private string message = string.Empty;

    [ObservableProperty]
    private string buttonText = "OK";

    public event EventHandler? ActionEvent;

    public CommonAlertPopupPageViewModel(
        string title = "Alert",
        string message = "",
        string buttonText = "OK")
    {
        Title = title;
        Message = message;
        ButtonText = buttonText;
    }

    [RelayCommand]
    private void Ok()
    {
        ActionEvent?.Invoke(this, EventArgs.Empty);
    }
}