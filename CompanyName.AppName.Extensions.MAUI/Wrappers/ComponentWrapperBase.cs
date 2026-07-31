using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using CompanyName.AppName.Extensions.MAUI.Contracts;
using CompanyName.AppName.Extensions.MAUI.Enumerations;
using CompanyName.AppName.Extensions.MAUI.Jsx.Extensions;
using Jint;
using Jint.Native;


public abstract class ComponentWrapperBase : IComponentWrapper, INotifyPropertyChanged
{
    /// <summary>
    /// One gate per script engine, serialising every callback that runs against it.
    /// </summary>
    /// <remarks>
    /// A Jint <see cref="Engine"/> is not thread safe. Component callbacks are raised from the UI
    /// thread but run off it, so without a gate two components handled at once would step on the
    /// same engine state. Keyed weakly so an engine's gate is collected along with the engine.
    /// </remarks>
    static readonly ConditionalWeakTable<Engine, SemaphoreSlim> ScriptGates = new();

    /// <summary>
    /// Raised when a callback into script throws, which would otherwise be invisible.
    /// </summary>
    public static event EventHandler<Exception>? ScriptError;

    public event PropertyChangedEventHandler PropertyChanged;

    protected Engine _engine;

    public string BasePath { get; private set; }

    public string Id { get; private set; }

    public string ComponentName { get; private set; }

    public XamlType XamlType { get; private set; }

    public IDictionary<string, object> Attributes { get; private set; }

    public IComponentWrapper[] Children { get; private set; }

    [JsonIgnore]
    public object NativeView { get; protected set; }

    public JsValue ThisObject { get; private set; }

    public ComponentWrapperBase(Engine engine, JsValue thisObject, string basePath, string componentName, object attributes, object[] children)
    {
        _engine = engine;
        BasePath = basePath;
        ComponentName = componentName;
        XamlType = componentName.MatchType<XamlType>();
        Attributes = (IDictionary<string, object>) attributes;
        ThisObject = thisObject;

        if (children != null && children.Length > 0)
        {
            var childList = new List<IComponentWrapper>();
            ParseChildren(childList, children);
            Children = childList.ToArray();
        }

        if (Attributes != null)
        {
            object value;
            if (Attributes.TryGetValue("id", out value))
            {
                Id = value.ToString();
            }
        }
    }

    public string getId()
    {
        return Id;
    }

    public void setId(string id)
    {
        Id = id;
    }

    void ParseChildren(List<IComponentWrapper> list, object children)
    {
        if (children != null)
        {
            if (children is object[])
            {
                var childList = (object[]) children;
                foreach (var child in childList)
                {
                    ParseChildren(list, child);
                }
            }
            else if (children is IComponentWrapper)
            {
                list.Add((IComponentWrapper) children);
            }
        }
    }

    /// <summary>
    /// Runs a component's script callback, holding the component inert until it completes.
    /// </summary>
    /// <param name="callback">Work that calls into script.</param>
    /// <remarks>
    /// Call from the UI thread. The callback runs off it, one at a time per engine, so a slow
    /// handler cannot block rendering and two handlers cannot corrupt the engine between them.
    /// </remarks>
    protected void InvokeScriptCallback(Action callback)
    {
        CanExecute = false;

        var gate = ScriptGates.GetValue(_engine, _ => new SemaphoreSlim(1, 1));

        Task.Run(async () =>
        {
            await gate.WaitAsync().ConfigureAwait(false);
            try
            {
                callback();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error invoking javascript method. Message: {e.Message}");
                ScriptError?.Invoke(this, e);
            }
            finally
            {
                gate.Release();

                // CanExecute is bound to the native view, so it has to be raised where the
                // view can act on it.
                MainThread.BeginInvokeOnMainThread(() => CanExecute = true);
            }
        });
    }

    /// <summary>
    /// Indicates if components like buttons are executable in the view.
    /// </summary>
    public bool CanExecute
    {
        get
        {
            return _canExecute;
        }
        set
        {
            _canExecute = value;
            OnPropertyChanged();
        }
    }

    bool _canExecute = true;

    public string ToXaml()
    {
        throw new NotImplementedException();
    }

    public object GetNativeView()
    {
        return NativeView;
    }

    /// <summary>
    /// Call on property changed event
    /// </summary>
    /// <param name="propertyName">Name of changed property</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this,
        new PropertyChangedEventArgs(propertyName));
    }

    public void SetContext(object context)
    {
        ThisObject = (JsValue) context;
    }
}