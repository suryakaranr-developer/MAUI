using CompanyName.AppName.Design.MAUI.Material.Enumerations;
using CompanyName.AppName.Design.MAUI.Material.Tokens;
using Microsoft.Maui.Controls.Shapes;

// The control exposes a Label property, which shadows the control type of the same name.
using MauiLabel = Microsoft.Maui.Controls.Label;

namespace CompanyName.AppName.Design.MAUI.Material.Components;

/// <summary>
/// A Material Design 3 text field.
/// </summary>
/// <remarks>
/// <para>
/// The label starts inside the field and rises to the top edge once the field holds a value or
/// takes focus, so the field never loses the description of what it is asking for. That is the
/// behaviour a bare placeholder cannot give, and it is why this is a composite rather than a
/// styled <see cref="Entry"/>.
/// </para>
/// <para>
/// The outlined variant paints the label over its own surface-coloured background, which is how
/// the notch in the border is produced without cutting the border itself.
/// </para>
/// </remarks>
public class MaterialTextField : Grid
{
    /// <summary>Height of the input container.</summary>
    const double FieldHeight = 56;

    /// <summary>Horizontal inset of the text and the resting label.</summary>
    const double HorizontalInset = 16;

    /// <summary>Name the label travel is committed under.</summary>
    const string FloatAnimation = "MaterialLabelFloat";

    /// <summary>Time the label takes to rise or fall.</summary>
    const uint FloatDuration = 150;

    /// <summary>Font size of the label while resting inside the field.</summary>
    const double RestingLabelSize = 16;

    /// <summary>Font size of the label once floated to the edge.</summary>
    const double FloatedLabelSize = 12;

    readonly Border _container;

    readonly Entry _entry;

    readonly MauiLabel _label;

    readonly BodySmall _supporting;

    readonly BoxView _underline;

    bool _isFloated;

    /// <summary>
    /// Which of the two text field variants to paint.
    /// </summary>
    public MaterialTextFieldVariant Variant
    {
        get => (MaterialTextFieldVariant) GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly BindableProperty VariantProperty = BindableProperty.Create(
        nameof(Variant),
        typeof(MaterialTextFieldVariant),
        typeof(MaterialTextField),
        defaultValue: MaterialTextFieldVariant.Outlined,
        propertyChanged: (bindable, _, _) => ((MaterialTextField) bindable).ApplyState());

    /// <summary>
    /// Description of what the field is asking for.
    /// </summary>
    public string Label
    {
        get => (string) GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly BindableProperty LabelProperty = BindableProperty.Create(
        nameof(Label),
        typeof(string),
        typeof(MaterialTextField),
        propertyChanged: (bindable, _, newValue) =>
            ((MaterialTextField) bindable)._label.Text = (string) newValue);

    /// <summary>
    /// The field's value.
    /// </summary>
    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(MaterialTextField),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, _, newValue) =>
        {
            var control = (MaterialTextField) bindable;
            if (control._entry.Text != (string) newValue)
            {
                control._entry.Text = (string) newValue;
            }
            control.UpdateFloat();
        });

    /// <summary>
    /// Hint shown below the field.
    /// </summary>
    public string SupportingText
    {
        get => (string) GetValue(SupportingTextProperty);
        set => SetValue(SupportingTextProperty, value);
    }

    public static readonly BindableProperty SupportingTextProperty = BindableProperty.Create(
        nameof(SupportingText),
        typeof(string),
        typeof(MaterialTextField),
        propertyChanged: (bindable, _, _) => ((MaterialTextField) bindable).ApplyState());

    /// <summary>
    /// Whether the field is showing an error. Repaints the field and its supporting text.
    /// </summary>
    public bool HasError
    {
        get => (bool) GetValue(HasErrorProperty);
        set => SetValue(HasErrorProperty, value);
    }

    public static readonly BindableProperty HasErrorProperty = BindableProperty.Create(
        nameof(HasError),
        typeof(bool),
        typeof(MaterialTextField),
        defaultValue: false,
        propertyChanged: (bindable, _, _) => ((MaterialTextField) bindable).ApplyState());

    /// <summary>
    /// Whether the field masks its value.
    /// </summary>
    public bool IsPassword
    {
        get => (bool) GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword),
        typeof(bool),
        typeof(MaterialTextField),
        defaultValue: false,
        propertyChanged: (bindable, _, newValue) =>
            ((MaterialTextField) bindable)._entry.IsPassword = (bool) newValue);

    /// <summary>
    /// Keyboard offered for the field.
    /// </summary>
    public Keyboard EntryKeyboard
    {
        get => (Keyboard) GetValue(EntryKeyboardProperty);
        set => SetValue(EntryKeyboardProperty, value);
    }

    public static readonly BindableProperty EntryKeyboardProperty = BindableProperty.Create(
        nameof(EntryKeyboard),
        typeof(Keyboard),
        typeof(MaterialTextField),
        defaultValue: Keyboard.Default,
        propertyChanged: (bindable, _, newValue) =>
            ((MaterialTextField) bindable)._entry.Keyboard = (Keyboard) newValue);

    public MaterialTextField()
    {
        RowDefinitions = new RowDefinitionCollection
        {
            new RowDefinition { Height = GridLength.Auto },
            new RowDefinition { Height = GridLength.Auto }
        };
        RowSpacing = 4;

        _entry = new Entry
        {
            BackgroundColor = Colors.Transparent,
            FontFamily = MaterialFont.Regular,
            FontSize = RestingLabelSize,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(HorizontalInset - 4, 0)
        };
        _entry.TextChanged += OnEntryTextChanged;
        _entry.Focused += OnEntryFocusChanged;
        _entry.Unfocused += OnEntryFocusChanged;

        _underline = new BoxView
        {
            HeightRequest = 1,
            VerticalOptions = LayoutOptions.End,
            IsVisible = false
        };

        _container = new Border
        {
            HeightRequest = FieldHeight,
            Padding = 0,
            Content = new Grid { Children = { _entry, _underline } }
        };

        // The label lives outside the container so it can sit over the top border rather than be
        // clipped by it.
        _label = new MauiLabel
        {
            FontFamily = MaterialFont.Regular,
            FontSize = RestingLabelSize,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            InputTransparent = true,
            Padding = new Thickness(4, 0),
            Margin = new Thickness(HorizontalInset - 4, 0, 0, 0)
        };

        _supporting = new BodySmall
        {
            Margin = new Thickness(HorizontalInset, 0, HorizontalInset, 0),
            IsVisible = false
        };

        var fieldLayer = new Grid { Children = { _container, _label } };

        SetRow((BindableObject) fieldLayer, 0);
        SetRow((BindableObject) _supporting, 1);

        Children.Add(fieldLayer);
        Children.Add(_supporting);

        ApplyState();
        UpdateFloat();
    }

    void OnEntryTextChanged(object? sender, TextChangedEventArgs e)
    {
        Text = e.NewTextValue;
    }

    void OnEntryFocusChanged(object? sender, FocusEventArgs e)
    {
        ApplyState();
        UpdateFloat();
    }

    /// <summary>
    /// Floats the label to the top edge, or drops it back inside the field.
    /// </summary>
    /// <remarks>
    /// The label floats whenever the field holds a value or has focus. Without the value check it
    /// would drop back over the text the user just typed.
    /// </remarks>
    void UpdateFloat()
    {
        var shouldFloat = _entry.IsFocused || !string.IsNullOrEmpty(Text);

        if (shouldFloat == _isFloated)
        {
            return;
        }

        _isFloated = shouldFloat;

        var fromY = _label.TranslationY;
        var toY = shouldFloat ? -(FieldHeight / 2) : 0;

        var fromSize = _label.FontSize;
        var toSize = shouldFloat ? FloatedLabelSize : RestingLabelSize;

        this.AbortAnimation(FloatAnimation);

        new Animation(progress =>
        {
            _label.TranslationY = fromY + ((toY - fromY) * progress);
            _label.FontSize = fromSize + ((toSize - fromSize) * progress);
        })
        .Commit(this, FloatAnimation, length: FloatDuration, easing: Easing.CubicOut);
    }

    /// <summary>
    /// Paints the container, label and supporting text for the current variant and state.
    /// </summary>
    void ApplyState()
    {
        var focused = _entry.IsFocused;

        _supporting.Text = SupportingText;
        _supporting.IsVisible = !string.IsNullOrEmpty(SupportingText);

        _entry.SetRole(Entry.TextColorProperty, MaterialRole.OnSurface);

        // An error outranks focus: a field the user is fixing should stay red while they work.
        MaterialRole accent;
        if (HasError)
        {
            accent = MaterialRole.Error;
        }
        else if (focused)
        {
            accent = MaterialRole.Primary;
        }
        else
        {
            accent = MaterialRole.OnSurfaceVariant;
        }

        _label.SetRole(MauiLabel.TextColorProperty, accent);
        _supporting.SetRole(MauiLabel.TextColorProperty, accent);

        if (Variant == MaterialTextFieldVariant.Filled)
        {
            // The filled variant is a tonal box with only a bottom edge, so the border is a
            // separate rule rather than a stroke around the container.
            _container.SetRoleBrush(Border.BackgroundProperty, MaterialRole.SurfaceContainerHighest);
            _container.StrokeThickness = 0;
            _container.StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(MaterialShape.ExtraSmall, MaterialShape.ExtraSmall, 0, 0)
            };

            _underline.IsVisible = true;
            _underline.HeightRequest = focused || HasError ? 2 : 1;
            _underline.SetRole(BoxView.ColorProperty, accent);

            // A filled field has no border for the label to interrupt, so it needs no background
            // of its own to notch one out.
            _label.BackgroundColor = Colors.Transparent;
            return;
        }

        _container.BackgroundColor = Colors.Transparent;
        _container.StrokeThickness = focused || HasError ? 2 : 1;
        _container.SetRoleBrush(Border.StrokeProperty, accent);
        _container.StrokeShape = new RoundRectangle { CornerRadius = MaterialShape.ExtraSmall };

        _underline.IsVisible = false;

        // Painting the floated label's own background in the surface colour is what cuts the
        // notch out of the outline underneath it.
        _label.SetRole(BackgroundColorProperty, MaterialRole.Surface);
    }
}
