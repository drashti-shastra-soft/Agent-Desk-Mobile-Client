using AgentDesk.ViewModels;
using AgentDesk.ViewModels.Authentication;

namespace AgentDesk.Views.Authentication;

public partial class ForgotPasswordPage : ContentPage
{
    public ForgotPasswordPageViewModel ViewModel { get; set; }
    public ForgotPasswordPage(ForgotPasswordPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = ViewModel;
    }
}