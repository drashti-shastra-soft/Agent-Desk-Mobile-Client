using AgentDesk.ViewModels;
using AgentDesk.ViewModels.Authentication;

namespace AgentDesk.Views.Authentication;

public partial class ChangePasswordPage : ContentPage
{
	public ChangePasswordPageViewModel ViewModel { get; set; }
    public ChangePasswordPage(ChangePasswordPageViewModel viewModel)
	{
		InitializeComponent();
        ViewModel= viewModel;
        BindingContext = ViewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ViewModel.ClearControl();
    }
}