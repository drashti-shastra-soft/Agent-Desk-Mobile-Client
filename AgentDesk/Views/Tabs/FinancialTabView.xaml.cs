namespace AgentDesk.Views.Tabs;

public partial class FinancialTabView : ContentView
{
    public FinancialTabView() => InitializeComponent();

    private void OnAccountTypePickerTapped(object sender, TappedEventArgs e) => accountTypePicker.Focus();
}
