using AgentDesk.ViewModels;

namespace AgentDesk.Views;

public partial class PolicyPage : ContentPage
{
	public PolicyPageViewModel ViewModel { get; set; }
	public PolicyPage(PolicyPageViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
		BindingContext = ViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.OnAppearing();
    }
}