using System.Collections.Generic;
using Dalamud.Game.ClientState.Party;

namespace PartyNumberNameplate.Logic;

public sealed class PartyList
{
    public List<(IPartyMember Member, int Order)> Members { get; } = new();

    public void Refresh()
    {
        Members.Clear();
        for (var index = 0; index < Plugin.PartyList.Length; index++)
        {
            var member = Plugin.PartyList[index];
            if (member is not null)
                Members.Add((member, index + 1));
        }
    }
}
