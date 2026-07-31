using System.ComponentModel;
using CompanyName.AppName.Design.MAUI.Components.Notifications;
using CompanyName.AppName.Design.MAUI.Enumerations;
using CompanyName.AppName.Design.MAUI.Extensions;
using CompanyName.AppName.Design.MAUI.Typography;

namespace CompanyName.AppName.Design.MAUI.Components.Inputs;

/// <summary>
/// StandardInput
/// </summary>
public class StandardInput : Grid, INotifyPropertyChanged
{
    Input _input;

    Grid _header;

    Body2 _titleLabel;

    Image _icon;

    Body2 _helper;

    Body2 _error;

    /// <summary>
    /// Input Text
    /// </summary>
    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    public static readonly BindableProperty TextProperty =
    BindableProperty.Create(nameof(Text), typeof(string), typeof(StandardInput), propertyChanged: (bindable, oldValue, newValue) =>
    {

    });

    /// <summary>
    /// Input placeholder
    /// </summary>
    public string Placeholder
    {
        get => (string) GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    public static readonly BindableProperty PlaceholderProperty =
    BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(StandardInput), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (StandardInput) bindable;
        control._input.Placeholder = newValue != null ? (string) newValue : string.Empty;
    });

    /// <summary>
    /// Title
    /// </summary>
    public string Title
    {
        get => (string) GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public static readonly BindableProperty TitleProperty =
    BindableProperty.Create(nameof(Title), typeof(string), typeof(StandardInput), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (StandardInput) bindable;
        control._header.IsVisible = newValue is string ? !string.IsNullOrEmpty((string) newValue) : false;
    });

    /// <summary>
    /// Descirption
    /// </summary>
    public string Description
    {
        get => (string) GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    public static readonly BindableProperty DescriptionProperty =
    BindableProperty.Create(nameof(Description), typeof(string), typeof(StandardInput), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (StandardInput) bindable;
        control._icon.IsVisible = !string.IsNullOrEmpty(control.Description);
    });

    /// <summary>
    /// Helper text
    /// </summary>
    public string HelperText
    {
        get => (string) GetValue(HelperTextProperty);
        set => SetValue(HelperTextProperty, value);
    }
    public static readonly BindableProperty HelperTextProperty =
    BindableProperty.Create(nameof(HelperText), typeof(string), typeof(StandardInput), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (StandardInput) bindable;
        if (newValue != null && !string.IsNullOrEmpty(newValue.ToString()))
        {
            control._helper.IsVisible = control.InputType == InputType.Default;
        }
        else
        {
            control._helper.IsVisible = false;
        }
    });

    /// <summary>
    /// Error text
    /// </summary>
    public string ErrorText
    {
        get => (string) GetValue(ErrorTextProperty);
        set => SetValue(ErrorTextProperty, value);
    }
    public static readonly BindableProperty ErrorTextProperty =
    BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(StandardInput), propertyChanged: (bindable, oldValue, newValue) =>
    {

    });

    /// <summary>
    /// Error description
    /// </summary>
    public string ErrorDescription
    {
        get => (string) GetValue(ErrorDescriptionProperty);
        set => SetValue(ErrorDescriptionProperty, value);
    }
    public static readonly BindableProperty ErrorDescriptionProperty =
    BindableProperty.Create(nameof(ErrorDescription), typeof(string), typeof(StandardInput), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (StandardInput) bindable;
        control._icon.IsVisible = !string.IsNullOrEmpty(control.ErrorDescription);
    });

    /// <summary>
    /// Input type
    /// </summary>
    public InputType InputType
    {
        get => (InputType) GetValue(InputTypeProperty);
        set => SetValue(InputTypeProperty, value);
    }
    public static readonly BindableProperty InputTypeProperty =
    BindableProperty.Create(nameof(InputType), typeof(InputType), typeof(StandardInput), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (StandardInput) bindable;
        control._icon.Source = control.GetIcon();
        control._helper.IsVisible = (InputType) newValue == InputType.Default;
        control._error.IsVisible = (InputType) newValue == InputType.Error;
    });

    /// <summary>
    /// Creates instance of StandardInput
    /// </summary>
    public StandardInput()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Initializes the component
    /// </summary>
    public void InitializeComponent()
    {
        RowDefinitions = new RowDefinitionCollection()
        {
            new RowDefinition(GridLength.Auto),
            new RowDefinition(GridLength.Auto),
            new RowDefinition(GridLength.Auto)
        };
        _header = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(48),
            },
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(48),
            },
            IsVisible = !string.IsNullOrEmpty(Title)
        };

        _titleLabel = new Body2()
        {
            Text = Title,
            VerticalOptions = LayoutOptions.Center
        };
        _titleLabel.BindingContext = this;
        _titleLabel.SetBinding(Label.TextProperty, nameof(this.Title));
        _header.Add(_titleLabel, 0, 0);

        _icon = new Image()
        {
            Source = GetIcon(),
            HeightRequest = 28,
            IsVisible = !string.IsNullOrEmpty(Description) || !string.IsNullOrEmpty(ErrorDescription)
        };

        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += (s, e) =>
        {
            if ((InputType == InputType.Default && !string.IsNullOrEmpty(Description)) ||
                InputType == InputType.Error && !string.IsNullOrEmpty(ErrorDescription))
            {
                Navigation.PushModalAsync(new Tooltip((VisualElement) s, InputType == InputType.Default ? Description : ErrorDescription), false);
            }
        };
        _icon.GestureRecognizers.Add(tapGestureRecognizer);
        _header.Add(_icon, 1, 0);

        this.Add(_header, 0, 0);

        _input = new Input()
        {
            Placeholder = Placeholder,
            Margin = 0
        };
        _input.BindingContext = this;
        _input.SetBinding(Entry.TextProperty, nameof(this.Text));
        _input.SetBinding(Entry.PlaceholderProperty, nameof(this.Placeholder));

        this.Add(_input, 0, 1);

        _helper = new Body2()
        {
            TextColor = Application.Current.RequestedTheme == AppTheme.Light ?
                    Application.Current.GetResource<Color>("PrimaryInfo") : Application.Current.GetResource<Color>("PrimaryInfo_Dark"),
            Margin = new Thickness(0, 8, 0, 0),
            IsVisible = !string.IsNullOrEmpty(HelperText)
        };
        _helper.BindingContext = this;
        _helper.SetBinding(Label.TextProperty, nameof(this.HelperText));

        _error = new Body2()
        {
            TextColor = Application.Current.RequestedTheme == AppTheme.Light ?
                    Application.Current.GetResource<Color>("Error") : Application.Current.GetResource<Color>("Error_Dark"),
            Margin = new Thickness(0, 8, 0, 0),
            IsVisible = !string.IsNullOrEmpty(ErrorText)
        };
        _error.BindingContext = this;
        _error.SetBinding(Label.TextProperty, nameof(this.ErrorText));

        this.Add(_helper, 0, 2);
        this.Add(_error, 0, 2);
    }

    /// <summary>
    /// Gets the icon to display based on the input type
    /// </summary>
    string GetIcon()
    {
        switch (InputType)
        {
            case InputType.Error:
                return Application.Current.RequestedTheme == AppTheme.Light ? "secondary_error_light.png" : "secondary_error_dark.png";
            default:
                return Application.Current.RequestedTheme == AppTheme.Light ? "primary_info_light.png" : "primary_info_dark.png";

        }
    }
}