using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;

namespace PartyNumberNameplate.Logic;

public sealed class PartyHudSnapshot
{
    public List<(string Name, int Order)> Members { get; } = new();

    public unsafe void Refresh()
    {
        Members.Clear();
        var agentHud = AgentHUD.Instance();
        if (agentHud == null)
            return;

        for (var slot = 0; slot < 8; slot++)
        {
            var member = agentHud->PartyMembers[slot];
            if (member.ContentId != 0)
                Members.Add((member.Name.ToString(), member.Index + 1));
        }

        Members.Sort((left, right) => left.Order.CompareTo(right.Order));
    }
}
