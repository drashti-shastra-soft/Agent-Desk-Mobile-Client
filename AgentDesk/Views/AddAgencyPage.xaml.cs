using AgentDesk.Models;
using AgentDesk.ViewModels;
using System.Text.RegularExpressions;

namespace AgentDesk.Views;

[QueryProperty(nameof(Agency), "Agency")]
public partial class AddAgencyPage : ContentPage
{
    public AddAgencyPageViewModel ViewModel { get; set; }

    public Agency Agency
    {
        set
        {
            ViewModel.AgencyModel = value;
            value = null;
        }
    }
    public AddAgencyPage(AddAgencyPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = ViewModel;
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        ViewModel.ClearControl();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await ViewModel.OnAppearing();
    }

    private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
            return;

        string input = e.NewTextValue ?? string.Empty;

        string validText = Regex.Replace(input, @"[^a-zA-Z ]", "");

        if (input != validText)
            entry.Text = validText;
    }

    private void OnAgentTypePickerTapped(object sender, TappedEventArgs e)
    {
        agencyTypePicker.Focus();
    }
}