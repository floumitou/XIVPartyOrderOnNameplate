using Dalamud.Plugin.Services;
using System;

namespace PartyOrderOnNameplate.Logic;

public sealed class PartyOrderNamePlateCoordinator : IDisposable
{
    private readonly IFramework framework;
    private readonly Configuration configuration;

    private TimeSpan elapsed;
    private bool isSubscribed;

    public PartyHudSnapshot CurrentPartyList { get; } = new();

    public PartyOrderNamePlateCoordinator(IFramework framework, Configuration configuration)
    {
        this.framework = framework;
        this.configuration = configuration;
        if (this.configuration.IsEnabled)
            Enable();
    }

    public void SetEnabled(bool enabled)
    {
        if (enabled)
            Enable();
        else
            Disable();
    }

    public void Dispose()
    {
        Disable();
    }

    private void Enable()
    {
        if (isSubscribed)
            return;

        elapsed = TimeSpan.Zero;
        CurrentPartyList.Refresh();
        framework.Update += OnFrameworkUpdate;
        isSubscribed = true;
    }

    private void Disable()
    {
        if (!isSubscribed)
            return;

        framework.Update -= OnFrameworkUpdate;
        isSubscribed = false;
    }

    private void OnFrameworkUpdate(IFramework framework)
    {
        if (!UpdateInterval())
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
