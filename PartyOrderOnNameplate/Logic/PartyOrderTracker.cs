using Dalamud.Plugin.Services;
using System;

namespace PartyOrderOnNameplate.Logic;

public sealed class PartyOrderTracker : IDisposable
{
    private readonly IFramework framework;
    private readonly Configuration configuration;

    private TimeSpan elapsed;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(1);

    public PartyHudSnapshot CurrentPartyList { get; } = new();

    public PartyOrderTracker(IFramework framework, Configuration configuration)
    {
        this.framework = framework;
        this.framework.Update += OnFrameworkUpdate;
        this.configuration = configuration;
    }

    public void Dispose()
    {
        this.framework.Update -= OnFrameworkUpdate;
    }

    private void OnFrameworkUpdate(IFramework framework)
    {
        if (!configuration.IsEnabled ||!UpdateInterval())
            return;

        if (CurrentPartyList.Refresh())
            Plugin.NamePlateGui.RequestRedraw();
    }

    private bool UpdateInterval()
    {
        var interval = TimeSpan.FromSeconds(configuration.PartyListUpdateInterval);
        elapsed += framework.UpdateDelta;
        if (elapsed < interval)
            return false;

        elapsed = TimeSpan.Zero;
        return true;
    }
}
