using AgentDesk.CustomeControls;
using AgentDesk.Models;
using AgentDesk.ViewModels;
using System.Text.RegularExpressions;

#if ANDROID
using Xamarin.Google.Crypto.Tink.Proto;
#endif

namespace AgentDesk.Views;

[QueryProperty(nameof(GroupModel), "GroupModel")]
public partial class AddGroupPage : ContentPage
{
    public AddGroupPageViewModel ViewModel { get; set; }

    public GroupModel GroupModel
    {
        set
        {
            ViewModel.GroupModel = value;
            value = null;
        }
    }

    public AddGroupPage(AddGroupPageViewModel viewModel)
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

    private void NameEntry_TextChanged(object sender,TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
            return;

        string input = e.NewTextValue ?? string.Empty;

        //string validText = Regex.Replace(input, @"[^a-zA-Z ]", "");
        string validText = Regex.Replace(input, @"[^a-zA-Z0-9 ./]", "");

        if (input != validText)
            entry.Text = validText;
    }
    private void HeadNameEntry_TextChanged(object sender,TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
            return;

        string input = e.NewTextValue ?? string.Empty;

        //string validText = Regex.Replace(input, @"[^a-zA-Z ]", "");
        string validText = Regex.Replace(input, @"[^a-zA-Z ./]", "");

        if (input != validText)
            entry.Text = validText;
    }
}
