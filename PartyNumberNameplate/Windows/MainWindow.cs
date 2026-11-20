using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace PartyNumberNameplate.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly Plugin plugin;

    // We give this window a hidden ID using ##.
    // The user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin)
        : base("PartyNumberNameplate##PNNP_MainWindow", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(200, 90),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text("Settings");
        ImGui.Spacing();

        var enabled = plugin.Configuration.IsEnabled;
        if (ImGui.Checkbox("Enabled", ref enabled))
        {
            plugin.Configuration.IsEnabled = enabled;
            Plugin.Log.Information($"Enabled set to {enabled}");
            plugin.Configuration.Save();
        }
    }
}
