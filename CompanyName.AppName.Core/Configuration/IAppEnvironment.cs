namespace CompanyName.AppName.Core.Configuration;

/// <summary>
/// Describes which deployment environment the running build was produced for.
/// </summary>
/// <remarks>
/// An abstraction rather than a static so a service client can be pointed at a fake endpoint in a
/// test, and so <c>Core</c> stays free of the assembly-reflection the app head uses to read the
/// values the build stamped in. The concrete implementation lives in the app head, which is the
/// only layer that knows how the values got there.
/// </remarks>
public interface IAppEnvironment
{
    /// <summary>The environment name: <c>UAT</c>, <c>STAGE</c> or <c>RELEASE</c>.</summary>
    string Name { get; }

    /// <summary>Root address of the backend this build talks to, with a trailing slash.</summary>
    string ApiBaseUrl { get; }

    /// <summary>Whether this is a production build, for gating diagnostics and test affordances.</summary>
    bool IsProduction { get; }
}
