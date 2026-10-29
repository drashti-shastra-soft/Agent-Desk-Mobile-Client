namespace AgentDesk.Views.Tabs;

public partial class IdentificationTabView : ContentView
{
    public IdentificationTabView() => InitializeComponent();

    private bool _isFormattingPan;

    private void PanNumber_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        if (_isFormattingPan || sender is not Entry entry)
            return;

        string input = (e.NewTextValue ?? string.Empty)
            .ToUpperInvariant();

        var result = new System.Text.StringBuilder();

        foreach (char c in input)
        {
            int position = result.Length;

            bool valid = position < 5
                ? c is >= 'A' and <= 'Z'
                : position < 9
                    ? c is >= '0' and <= '9'
                    : c is >= 'A' and <= 'Z';

            if (valid)
                result.Append(c);

            if (result.Length == 10)
                break;
        }

        string pan = result.ToString();

        if (entry.Text != pan)
        {
            _isFormattingPan = true;
            entry.Text = pan;
            entry.CursorPosition = pan.Length;
            _isFormattingPan = false;
        }
    }

}


