using AgentDesk.ViewModels;

namespace AgentDesk.Views;

public partial class GroupPage : ContentPage
{
    public GroupPageViewModel ViewModel { get; set; }

    public GroupPage(GroupPageViewModel viewModel)
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