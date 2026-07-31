using CompanyName.AppName.Extensions.MAUI.Contracts;
using Jint;
using Jint.Native;

namespace CompanyName.AppName.Extensions.MAUI.Wrappers;

public class SectionWrapper : ComponentWrapperBase
    {
        public SectionWrapper(Engine engine, JsValue thisObject, string componentName, object attributes, object[] children)
            : base(engine, thisObject, null, componentName, attributes, children)
        {
            InitializeNativeView();

            if (Attributes != null)
            {
                foreach (var attr in Attributes)
                {
                    if (attr.Key == "alignment")
                    {
                        alignment = attr.Value.ToString();
                    }
                }
            }
        }

        string _alignment;

        public string alignment
        {
            get
            {
                return _alignment;
            }
            set
            {
                _alignment = value;
                UpdateAlignment();
            }
        }

        public string getAlignment()
        {
            return alignment;
        }

        public void setAlignment(string alignment)
        {
            this.alignment = alignment;
        }

        void InitializeNativeView()
        {
            StackLayout layout = new StackLayout();
            switch (XamlType)
            {
                case Enumerations.XamlType.HorizontalStackLayout:
                    layout.Orientation = StackOrientation.Horizontal;
                    break;
                default:
                    layout.Orientation = StackOrientation.Vertical;
                    break;
            }

            layout.Margin = new Thickness(20, 10);

            if (Children != null)
            {
                foreach (var child in Children)
                {
                    layout.Children.Add((IView) ((IComponentWrapper) child).GetNativeView());
                }
            }

            NativeView = layout;
        }

        void UpdateAlignment()
        {
            LayoutOptions options;
            switch (alignment)
            {
                case "start":
                    options = LayoutOptions.Start;
                    break;
                case "center":
                    options = LayoutOptions.Center;
                    break;
                case "end":
                    options = LayoutOptions.End;
                    break;
                default:
                    options = LayoutOptions.Fill;
                    break;
            }
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var layout = (StackLayout) NativeView;
                layout.HorizontalOptions = options;
                layout.VerticalOptions = options;
            });
        }
    }