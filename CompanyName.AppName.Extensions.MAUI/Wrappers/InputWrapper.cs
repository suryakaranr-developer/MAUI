using CompanyName.AppName.Design.MAUI.Components.Inputs;
using CompanyName.AppName.Design.MAUI.Enumerations;
using Jint;
using Jint.Native;


public class InputWrapper : ComponentWrapperBase
{
    string _text;

    public string text
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }

    public string getText()
    {
        return text;
    }

    public void setText(string text)
    {
        this.text = text;
    }

    string _placeholder;

    public string placeholder
    {
        get
        {
            return _placeholder;
        }
        set
        {
            _placeholder = value;
            OnPropertyChanged();
        }
    }

    public string getPlaceholder()
    {
        return placeholder;
    }

    public void setPlaceholder(string placeholder)
    {
        this.placeholder = placeholder;
    }

    string _title;

    public string title
    {
        get
        {
            return _title;
        }
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    public string getTitle()
    {
        return title;
    }

    public void setTitle(string title)
    {
        this.title = title;
    }

    string _description;

    public string description
    {
        get
        {
            return _description;
        }
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    public string getDescription()
    {
        return description;
    }

    public void setDescription(string description)
    {
        this.description = description;
    }

    string _helpertext;

    public string helpertext
    {
        get
        {
            return _helpertext;
        }
        set
        {
            _helpertext = value;
            OnPropertyChanged();
        }
    }

    public string getHelperText()
    {
        return helpertext;
    }

    public void setHelperText(string helpertext)
    {
        this.helpertext = helpertext;
    }

    string _errortext;

    public string errortext
    {
        get
        {
            return _errortext;
        }
        set
        {
            _errortext = value;
            OnPropertyChanged();
        }
    }

    public string getErrorText()
    {
        return errortext;
    }

    public void setErrorText(string errortext)
    {
        this.errortext = errortext;
    }

    string _errordesc;

    public string errordescription
    {
        get
        {
            return _errordesc;
        }
        set
        {
            _errordesc = value;
            OnPropertyChanged();
        }
    }

    public string getErrorDescription()
    {
        return errordescription;
    }

    public void setErrorDescription(string errordescription)
    {
        this.errordescription = errordescription;
    }

    string _type;

    public string type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            UpdateType();
        }
    }

    public string getType()
    {
        return type;
    }

    public void setType(string type)
    {
        this.type = type;
    }

    public InputWrapper(Engine engine, JsValue thisObject, string componentName, object attributes, object[] children)
        : base(engine, thisObject, null, componentName, attributes, children)
    {
        InitializeNativeView();

        ParseAttributes();
    }

    void InitializeNativeView()
    {
        var input = new StandardInput();

        input.BindingContext = this;
        input.SetBinding(StandardInput.TextProperty, nameof(text), BindingMode.TwoWay);
        input.SetBinding(StandardInput.PlaceholderProperty, nameof(placeholder));
        input.SetBinding(StandardInput.TitleProperty, nameof(title));
        input.SetBinding(StandardInput.DescriptionProperty, nameof(description));
        input.SetBinding(StandardInput.HelperTextProperty, nameof(helpertext));
        input.SetBinding(StandardInput.ErrorTextProperty, nameof(errortext));
        input.SetBinding(StandardInput.ErrorDescriptionProperty, nameof(errordescription));

        // TODO: implement OnTextChanged event

        NativeView = input;
    }

    void ParseAttributes()
    {
        if (Attributes != null)
        {
            foreach (var attr in Attributes)
            {
                if (attr.Key == "text")
                {
                    text = attr.Value.ToString();
                }
                else if (attr.Key == "placeholder")
                {
                    placeholder = attr.Value.ToString();
                }
                else if (attr.Key == "title")
                {
                    title = attr.Value.ToString();
                }
                else if (attr.Key == "description")
                {
                    description = attr.Value.ToString();
                }
                else if (attr.Key == "helpertext")
                {
                    helpertext = attr.Value.ToString();
                }
                else if (attr.Key == "errortext")
                {
                    errortext = attr.Value.ToString();
                }
                else if (attr.Key == "errordescription")
                {
                    errordescription = attr.Value.ToString();
                }
                else if (attr.Key == "type")
                {
                    type = attr.Value.ToString();
                }
            }
        }
    }

    void UpdateType()
    {
        var input = (StandardInput) NativeView;
        InputType inputType = InputType.Default;

        switch (type)
        {
            case "error":
                inputType = InputType.Error;
                break;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            input.InputType = inputType;
        });
    }
}

