using AgentDesk.ViewModels.Popups;
using Mopups.Pages;
using Mopups.Services;

namespace AgentDesk.Views.Popups;

public partial class CommonPopupPage : PopupPage
{
    public CommonPopupPageViewModel ViewModel { get; set; }

    public CommonPopupPage(CommonPopupPageViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;
        BindingContext = ViewModel;
        //ViewModel.Cancelled += OnCancelled;
    }

    //private async void OnCancelled(object? sender, EventArgs e)
    //{
    //    await MopupService.Instance.PopAsync();
    //}

}