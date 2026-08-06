using System.Reflection;
using CompanyName.AppName.Core.Configuration;

namespace CompanyName.AppName.MAUI.Configuration;

/// <summary>
/// Reads the environment the build was stamped with by <c>Build/Environments.props</c>.
/// </summary>
/// <remarks>
/// The values arrive as <see cref="AssemblyMetadataAttribute"/> entries rather than a generated
/// source file, so there is no build-ordering concern and nothing environment-specific to keep out
/// of source control. Resolved once at construction because the attributes cannot change while the
/// process is alive. A local <c>dotnet build</c> with no <c>-p:AppEnvironment</c> yields RELEASE,
/// matching the default in the props file.
/// </remarks>
public sealed class BuildAppEnvironment : IAppEnvironment
{
    public BuildAppEnvironment()
    {
        var metadata = typeof(BuildAppEnvironment).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>();

        foreach (var entry in metadata)
        {
            switch (entry.Key)
            {
                case "AppEnvironment" when !string.IsNullOrWhiteSpace(entry.Value):
                    Name = entry.Value;
                    break;
                case "ApiBaseUrl" when !string.IsNullOrWhiteSpace(entry.Value):
                    ApiBaseUrl = entry.Value;
                    break;
            }
        }
    }

    public string Name { get; } = "RELEASE";

    public string ApiBaseUrl { get; } = string.Empty;

    public bool IsProduction => string.Equals(Name, "RELEASE", StringComparison.OrdinalIgnoreCase);
}
