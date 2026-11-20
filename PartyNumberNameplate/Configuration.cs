using Dalamud.Configuration;
using System;

namespace PartyNumberNameplate;

[Serializable]
public class Configuration : IPluginConfiguration
{

    public int Version { get; set; } = 0;

    public bool IsEnabled { get; set; } = true;

    public float PartyListUpdateInterval { get; set; } = 1.0f;

    public NumberPosition NumberPosition { get; set; } = NumberPosition.Left;

    // The below exists just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}

public enum NumberPosition
{
    Left,
    Right,
    Top,
    Bottom
}
