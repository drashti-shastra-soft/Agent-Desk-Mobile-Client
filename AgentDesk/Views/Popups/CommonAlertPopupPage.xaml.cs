using AgentDesk.ViewModels.Popups;
using Mopups.Pages;
using Mopups.Services;

namespace AgentDesk.Views.Popups;

public partial class CommonAlertPopupPage : PopupPage
{
    public CommonAlertPopupPageViewModel ViewModel { get; set; }

    public CommonAlertPopupPage(
        CommonAlertPopupPageViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();

        ViewModel.ActionEvent += OnActionEvent;
    }

    private async void OnActionEvent(object? sender, EventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
}