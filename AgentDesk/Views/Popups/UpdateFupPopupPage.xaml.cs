using AgentDesk.ViewModels.Popups;
using Mopups.Pages;

namespace AgentDesk.Views.Popups;

public partial class UpdateFupPopupPage : PopupPage
{
	public UpdateFupPopupPageViewModel ViewModel { get; set; }
    public UpdateFupPopupPage(UpdateFupPopupPageViewModel viewModel)
	{
		InitializeComponent();
		 ViewModel = viewModel;
        BindingContext = ViewModel;
	}

    private void OnPremiumPaidTapped(object sender, TappedEventArgs e)
    {
        premiumPaidPicker.Focus();
    }
}