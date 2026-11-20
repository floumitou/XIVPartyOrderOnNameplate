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
        : base("Party Number Nameplate##PNNP_MainWindow", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(400, 150),
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

        var position = plugin.Configuration.NumberPosition;
        if (ImGui.BeginCombo("Number Position", position.ToString()))
        {
            foreach (var option in Enum.GetValues<NumberPosition>())
            {
                var selected = option == position;
                if (ImGui.Selectable(option.ToString(), selected))
                {
                    plugin.Configuration.NumberPosition = option;
                    plugin.Configuration.Save();
                    Plugin.NamePlateGui.RequestRedraw();
                }

                if (selected)
                    ImGui.SetItemDefaultFocus();
            }

            ImGui.EndCombo();
        }

        ImGui.TextDisabled("Top and Bottom use the nameplate title line.");

        var updateInterval = plugin.Configuration.PartyListUpdateInterval;
        ImGui.PushItemWidth(220f);
        if (ImGui.SliderFloat("Party List Update Interval (s)", ref updateInterval, 0f, 10.0f))
        {
            plugin.Configuration.PartyListUpdateInterval = updateInterval;
            plugin.Configuration.Save();
        }
        ImGui.PopItemWidth();
        ImGui.SameLine();
        ImGui.TextDisabled("(?)");
        if (ImGui.IsItemHovered())
        {
            ImGui.SetNextWindowSize(new Vector2(350, 0));
            ImGui.BeginTooltip();
            ImGui.TextWrapped("Approx. how often the plugin checks the party list for changes. Though even on 0.0 (every frame) the Plugin should not have a significant impact on performance.");
            ImGui.EndTooltip();
        }
    }
}
