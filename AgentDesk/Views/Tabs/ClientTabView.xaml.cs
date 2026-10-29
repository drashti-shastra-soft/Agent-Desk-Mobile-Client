using System.Text.RegularExpressions;

namespace AgentDesk.Views.Tabs;

public partial class ClientTabView : ContentView
{
    public ClientTabView()
    {
        InitializeComponent();
        dateOfBirthPicker.MaximumDate = DateTime.Today;
    }

    private void OnPickerTapped(object sender, TappedEventArgs e) => groupPicker.Focus();

    private void OnGenderPickerTapped(object sender, TappedEventArgs e) => genderPicker.Focus();

    private void OnMaritalPickerTapped(object sender, TappedEventArgs e) => maritalPicker.Focus();

    private void OnDateOfBirthPickerTapped(object sender, TappedEventArgs e) => dateOfBirthPicker.Focus();

    private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
            return;

        string input = e.NewTextValue ?? string.Empty;
        string validText = Regex.Replace(input, @"[^a-zA-Z ./]", "");

        if (input != validText)
            entry.Text = validText;
    }
}
