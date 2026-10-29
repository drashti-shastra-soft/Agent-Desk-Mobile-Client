namespace AgentDesk.CustomeControls;

public partial class LabeledEntry : ContentView
{
    private CommunityToolkit.Maui.Behaviors.MaskedBehavior? _maskedBehavior;
    public LabeledEntry() => InitializeComponent();

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(LabeledEntry), string.Empty);

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LabeledEntry), string.Empty);

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(LabeledEntry), string.Empty,
            BindingMode.TwoWay);

    public static readonly BindableProperty ErrorTextProperty =
        BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(LabeledEntry), string.Empty);

    public static readonly BindableProperty HasErrorProperty =
        BindableProperty.Create(nameof(HasError), typeof(bool), typeof(LabeledEntry), false);

    public static readonly BindableProperty IsRequiredProperty =
        BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(LabeledEntry), false);

    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(LabeledEntry), Keyboard.Text);

    public static readonly BindableProperty MaskProperty =
       BindableProperty.Create(nameof(Mask), typeof(string), typeof(LabeledEntry), string.Empty,
           propertyChanged: (b, _, newValue) => ((LabeledEntry)b).UpdateMask(newValue as string));

    public static readonly BindableProperty MaxLengthProperty =
        BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(LabeledEntry), 50);

    public static readonly BindableProperty IsMultilineProperty =
        BindableProperty.Create(nameof(IsMultiline), typeof(bool), typeof(LabeledEntry), false,
            propertyChanged: (b, _, _) => ((LabeledEntry)b).OnPropertyChanged(nameof(IsSingleLine)));

    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public string ErrorText { get => (string)GetValue(ErrorTextProperty); set => SetValue(ErrorTextProperty, value); }
    public bool HasError { get => (bool)GetValue(HasErrorProperty); set => SetValue(HasErrorProperty, value); }
    public bool IsRequired { get => (bool)GetValue(IsRequiredProperty); set => SetValue(IsRequiredProperty, value); }
    public Keyboard Keyboard { get => (Keyboard)GetValue(KeyboardProperty); set => SetValue(KeyboardProperty, value); }
    public string Mask { get => (string)GetValue(MaskProperty); set => SetValue(MaskProperty, value); }
    public int MaxLength { get => (int)GetValue(MaxLengthProperty); set => SetValue(MaxLengthProperty, value); }
    public bool IsMultiline { get => (bool)GetValue(IsMultilineProperty); set => SetValue(IsMultilineProperty, value); }
    public bool IsSingleLine => !IsMultiline;

    private void UpdateMask(string? mask)
    {
        if (innerEntry == null)
            return;

        if (_maskedBehavior != null)
        {
            innerEntry.Behaviors.Remove(_maskedBehavior);
            _maskedBehavior = null;
        }

        if (!string.IsNullOrWhiteSpace(mask))
        {
            _maskedBehavior = new CommunityToolkit.Maui.Behaviors.MaskedBehavior
            {
                Mask = mask
            };
            innerEntry.Behaviors.Add(_maskedBehavior);
        }
    }
}
