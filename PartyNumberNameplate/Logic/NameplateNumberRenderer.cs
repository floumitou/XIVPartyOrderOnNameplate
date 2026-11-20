using System.Collections.Generic;
using Dalamud.Game.Gui.NamePlate;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;

namespace PartyNumberNameplate.Logic;

public sealed class NameplateNumberRenderer : System.IDisposable
{
    private readonly INamePlateGui namePlateGui;
    private readonly PartyHudSnapshot partyHudSnapshot;

    public NameplateNumberRenderer(INamePlateGui namePlateGui, PartyHudSnapshot partyHudSnapshot)
    {
        this.namePlateGui = namePlateGui;
        this.partyHudSnapshot = partyHudSnapshot;
        this.namePlateGui.OnNamePlateUpdate += OnNamePlateDataUpdate;
    }

    public void Dispose()
    {
        this.namePlateGui.OnNamePlateUpdate -= OnNamePlateDataUpdate;
    }

    private void OnNamePlateDataUpdate(
        INamePlateUpdateContext context,
        IReadOnlyList<INamePlateUpdateHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            if (handler.PlayerCharacter is not { } playerCharacter)
                continue;

            foreach (var (entityId, order) in this.partyHudSnapshot.Members)
            {
                if (entityId != playerCharacter.EntityId)
                    continue;

                handler.NameParts.TextWrap = (new SeStringBuilder().Append($"[{order}] ").Build(), SeString.Empty);
                break;
            }
        }
    }
}
