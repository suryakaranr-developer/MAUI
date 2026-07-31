namespace CompanyName.AppName.Extensions.MAUI.Contracts;

public interface IComponentWrapper
{
    string Id { get; }

    string ToXaml();

    object GetNativeView();
}