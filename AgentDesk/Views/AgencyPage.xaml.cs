using AgentDesk.Models;
using AgentDesk.ViewModels;

namespace AgentDesk.Views;


public partial class AgencyPage : ContentPage
{
    public AgencyPageViewModel ViewModel { get; set; }
    public AgencyPage(AgencyPageViewModel viewModel)
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