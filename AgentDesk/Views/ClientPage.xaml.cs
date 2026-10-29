using AgentDesk.ViewModels;
using System.Threading.Tasks;

namespace AgentDesk.Views;

public partial class ClientPage : ContentPage
{
    public ClientPageViewModel ViewModel { get; set; }
    public ClientPage(ClientPageViewModel viewModel)
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