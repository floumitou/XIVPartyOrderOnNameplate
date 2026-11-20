using Dalamud.Configuration;
using System;

namespace PartyOrderOnNameplate;

[Serializable]
public class Configuration : IPluginConfiguration
{

    public int Version { get; set; } = 0;

    public bool IsEnabled { get; set; } = true;

    public bool DisableForSelf { get; set; } = true;

    public float PartyListUpdateInterval { get; set; } = 1.0f;

    public NumberPosition NumberPosition { get; set; } = NumberPosition.Left;

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
