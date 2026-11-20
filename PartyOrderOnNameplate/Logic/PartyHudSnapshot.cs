using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;

namespace PartyOrderOnNameplate.Logic;

public sealed class PartyHudSnapshot
{
    public List<(uint EntityId, int Order)> Members { get; } = new();

    public unsafe bool Refresh()
    {
        var updatedMembers = new List<(uint EntityId, int Order)>();
        var agentHud = AgentHUD.Instance();
        if (agentHud != null)
        {
            for (var slot = 0; slot < 8; slot++)
            {
                var member = agentHud->PartyMembers[slot];
                if (member.EntityId != 0)
                    updatedMembers.Add((member.EntityId, member.Index + 1));
            }
        }

        updatedMembers.Sort((left, right) => left.Order.CompareTo(right.Order));

        var partyChanged = Members.Count != updatedMembers.Count;
        for (var index = 0; !partyChanged && index < Members.Count; index++)
            partyChanged = Members[index] != updatedMembers[index];

        if (partyChanged)
        {
            Members.Clear();
            Members.AddRange(updatedMembers);
        }

        return partyChanged;
    }
}
