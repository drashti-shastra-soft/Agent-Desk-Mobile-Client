using AgentDesk.Models;
using AgentDesk.ViewModels;

namespace AgentDesk.Views;

[QueryProperty(nameof(Client), "Client")]
public partial class AddClientPage : ContentPage
{
    public AddClientPageViewModel ViewModel { get; set; }

    public Client Client
    {
        set
        {
            ViewModel.ClientModel = value;
            value = null;
        }
    }

    public AddClientPage(AddClientPageViewModel viewModel)
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
