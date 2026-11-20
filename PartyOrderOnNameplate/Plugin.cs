using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using PartyOrderOnNameplate.Logic;
using PartyOrderOnNameplate.Windows;

namespace PartyOrderOnNameplate;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    [PluginService] internal static IFramework Framework { get; private set; } = null!;
    [PluginService] internal static INamePlateGui NamePlateGui { get; private set; } = null!;

    private const string ConfigCommandName = "/poonconf";
    private const string ToggleCommandName = "/poon";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("PartyOrderOnNameplate");
    private MainWindow MainWindow { get; init; }
    internal PartyOrderTracker PartyOrderTracker { get; init; }
    private NameplateNumberRenderer NameplateNumberRenderer { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        MainWindow = new MainWindow(this);
        PartyOrderTracker = new PartyOrderTracker(Framework, Configuration);
        NameplateNumberRenderer = new NameplateNumberRenderer(NamePlateGui, PartyOrderTracker.CurrentPartyList, Configuration);

        WindowSystem.AddWindow(MainWindow);

        CommandManager.AddHandler(ConfigCommandName, new CommandInfo(OnConfigCommand)
        {
            HelpMessage = "Opens the PartyOrderOnNameplate configuration window"
        });

        CommandManager.AddHandler(ToggleCommandName, new CommandInfo(OnToggleCommand)
        {
            HelpMessage = "Toggles party order on nameplate on and off"
        });


        // Tell the UI system that we want our windows to be drawn through the window system
        PluginInterface.UiBuilder.Draw += WindowSystem.Draw;
        // Adds another button doing the same but for the main ui of the plugin
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUi;
    }

    public void Dispose()
    {
        // Unregister all actions to not leak anything during disposal of plugin
        PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;
        PluginInterface.UiBuilder.OpenMainUi -= ToggleMainUi;
        NameplateNumberRenderer.Dispose();
        PartyOrderTracker.Dispose();

        WindowSystem.RemoveAllWindows();

        MainWindow.Dispose();

        CommandManager.RemoveHandler(ConfigCommandName);
        CommandManager.RemoveHandler(ToggleCommandName);
    }

    private void OnConfigCommand(string command, string args)
    {
        MainWindow.Toggle();
    }

    private void OnToggleCommand(string command, string args)
    {
        SetEnabled(!Configuration.IsEnabled);
    }

    internal void SetEnabled(bool enabled)
    {
        if (Configuration.IsEnabled == enabled)
            return;

        Configuration.IsEnabled = enabled;
        Configuration.Save();
        PartyOrderTracker.SetEnabled(enabled);
        NamePlateGui.RequestRedraw();
        Log.Information($"Party order nameplates {(enabled ? "enabled" : "disabled")}");
    }

    public void ToggleMainUi() => MainWindow.Toggle();
}
