using CompanyName.AppName.Design.MAUI.Components.Buttons.StandardButtons;
using CompanyName.AppName.Extensions.MAUI.Enumerations;
using Jint;
using Jint.Native;
using Jint.Runtime.Descriptors;

public class ButtonWrapper : ComponentWrapperBase, IConvertible
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

    public object clicked { get; set; }

    public ButtonWrapper(Engine engine, JsValue thisObject, string componentName, object attributes, object[] children)
        : base(engine, thisObject, null, componentName, attributes, children)
    {
        InitializeNativeView();

        if (Attributes != null)
        {
            foreach (var attr in Attributes)
            {
                if (attr.Key == "text")
                {
                    text = attr.Value.ToString();
                }
                else if (attr.Key == "clicked")
                {
                    clicked = attr.Value;
                }
            }
        }
    }

    void InitializeNativeView()
    {
        switch (XamlType)
        {
            case XamlType.PrimaryButton:
                NativeView = new PrimaryButton();
                break;
            case XamlType.SecondaryButton:
                NativeView = new SecondaryButton();
                break;
            case XamlType.TertiaryButton:
                NativeView = new TertiaryButton();
                break;
        }

        if (NativeView == null)
        {
            throw new InvalidCastException($"Error casting object {ComponentName} to {XamlType.ToString()}");
        }

        var button = (Button) NativeView;
        button.BindingContext = this;
        button.SetBinding(Button.TextProperty, nameof(text));
        button.SetBinding(VisualElement.IsEnabledProperty, nameof(CanExecute));
        button.Command = new Command(() => InvokeScriptCallback(() =>
        {
            var senderObj = new object[] { this };

            var sender = JsValue.FromObject(_engine, this);
            if (clicked.GetType() == typeof(Func<JsValue, JsValue[], JsValue>))
            {
                _engine.Invoke(JsValue.FromObject(_engine, clicked), ThisObject, JsValue.FromObject(_engine, senderObj));
            }
            else if (clicked is PropertyDescriptor)
            {
                var clickDescriptor = (PropertyDescriptor) clicked;
                _engine.Invoke(clickDescriptor.Value, ThisObject, new JsValue[] { sender });
            }
        }));
    }

    public TypeCode GetTypeCode()
    {
        return TypeCode.Object;
    }

    public bool ToBoolean(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public byte ToByte(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public char ToChar(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public DateTime ToDateTime(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public decimal ToDecimal(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public double ToDouble(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public short ToInt16(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public int ToInt32(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public long ToInt64(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public sbyte ToSByte(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public float ToSingle(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public string ToString(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public object ToType(Type conversionType, IFormatProvider provider)
    {
        if (conversionType == typeof(JsValue))
        {
            return JsValue.FromObject(_engine, this);
        }
        throw new NotImplementedException();
    }

    public ushort ToUInt16(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public uint ToUInt32(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public ulong ToUInt64(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
}

