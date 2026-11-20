using System.Collections.Generic;
using Dalamud.Game.Gui.NamePlate;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;

namespace PartyOrderOnNameplate.Logic;

public sealed class NameplateNumberRenderer : System.IDisposable
{
    private readonly INamePlateGui namePlateGui;
    private readonly PartyHudSnapshot partyHudSnapshot;
    private readonly Configuration configuration;

    public NameplateNumberRenderer(
        INamePlateGui namePlateGui,
        PartyHudSnapshot partyHudSnapshot,
        Configuration configuration)
    {
        this.namePlateGui = namePlateGui;
        this.partyHudSnapshot = partyHudSnapshot;
        this.configuration = configuration;
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

                var number = new SeStringBuilder().Append($"[{order}]").Build();
                switch (this.configuration.NumberPosition)
                {
                    case NumberPosition.Left:
                        handler.NameParts.TextWrap = (new SeStringBuilder().Append($"[{order}] ").Build(), SeString.Empty);
                        break;
                    case NumberPosition.Right:
                        if (handler.FreeCompanyTag.Payloads.Count > 0)
                        {
                            handler.FreeCompanyTagParts.OuterWrap =
                                (SeString.Empty, new SeStringBuilder().Append($" {number}").Build());
                        }
                        else
                        {
                            handler.NameParts.TextWrap =
                                (SeString.Empty, new SeStringBuilder().Append($" {number}").Build());
                        }
                        break;
                    case NumberPosition.Top:
                    case NumberPosition.Bottom:
                        handler.DisplayTitle = true;
                        handler.IsPrefixTitle = this.configuration.NumberPosition == NumberPosition.Top;
                        handler.TitleParts.Text = number;
                        handler.TitleParts.TextWrap = null;
                        break;
                }

                break;
            }
        }
    }
}
