using Dalamud.Plugin.Services;
using System;

namespace PartyNumberNameplate.Logic;

public sealed class PartyOrderTracker : IDisposable
{
    private readonly IFramework framework;

    private TimeSpan elapsed;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(1);

    public PartyList CurrentPartyList { get; } = new();

    public PartyOrderTracker(IFramework framework)
    {
        this.framework = framework;
        this.framework.Update += OnFrameworkUpdate;
    }

    public void Dispose()
    {
        this.framework.Update -= OnFrameworkUpdate;
    }

    private void OnFrameworkUpdate(IFramework framework)
    {
        if (!UpdateInterval())
            return;

        CurrentPartyList.Refresh();

        foreach (var (member, order) in CurrentPartyList.Members)
            Plugin.Log.Information($"Order: {order}, Name: {member.Name}");
    }

    private bool UpdateInterval()
    {
        elapsed += framework.UpdateDelta;
        if (elapsed < Interval)
            return false;

        elapsed -= Interval;
        return true;
    }
}
