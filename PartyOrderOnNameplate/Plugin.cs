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
    [PluginService] internal static IObjectTable ObjectTable { get; private set; } = null!;
    [PluginService] internal static IPlayerState PlayerState { get; private set; } = null!;
    [PluginService] internal static INamePlateGui NamePlateGui { get; private set; } = null!;

    private const string ConfigCommandName = "/poonconf";
    private const string ToggleCommandName = "/poon";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("PartyOrderOnNameplate");
    private MainWindow MainWindow { get; init; }
    internal PartyOrderNamePlateCoordinator PartyOrderNamePlateCoordinator { get; init; }
    private NameplateNumberRenderer NameplateNumberRenderer { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        MainWindow = new MainWindow(this);
        PartyOrderNamePlateCoordinator = new PartyOrderNamePlateCoordinator(Framework, Configuration);
        NameplateNumberRenderer = new NameplateNumberRenderer(NamePlateGui, PartyOrderNamePlateCoordinator.CurrentPartyList, Configuration);

        WindowSystem.AddWindow(MainWindow);

        CommandManager.AddHandler(ConfigCommandName, new CommandInfo(OnConfigCommand)
        {
            HelpMessage = "Opens the PartyOrderOnNameplate configuration window"
        });

        CommandManager.AddHandler(ToggleCommandName, new CommandInfo(OnToggleCommand)
        {
            HelpMessage = "Toggles party order on nameplate on and off"
        });

        PluginInterface.UiBuilder.Draw += WindowSystem.Draw;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUi;
        PluginInterface.UiBuilder.OpenConfigUi += ToggleMainUi;
    }

    public void Dispose()
    {
        PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;
        PluginInterface.UiBuilder.OpenMainUi -= ToggleMainUi;
        PluginInterface.UiBuilder.OpenConfigUi -= ToggleMainUi;
        NameplateNumberRenderer.Dispose();
        PartyOrderNamePlateCoordinator.Dispose();

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
        PartyOrderNamePlateCoordinator.SetEnabled(enabled);
        NamePlateGui.RequestRedraw();
        Log.Information($"Party order nameplates {(enabled ? "enabled" : "disabled")}");
    }

    public void ToggleMainUi() => MainWindow.Toggle();
}
