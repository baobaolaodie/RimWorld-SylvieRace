#nullable enable
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace SylvieMod;

/// <summary>
/// Incident worker for Sylvie's arrival event.
/// </summary>
public class IncidentWorker_SylvieTrader : IncidentWorker_TraderCaravanArrival
{
    #region Incident Worker Implementation

    /// <summary>
    /// Checks if the incident can fire now.
    /// </summary>
    protected override bool CanFireNowSub(IncidentParms parms)
    {
        if (Current.Game.GetComponent<SylvieGameComponent>().hasSylvieSpawned)
            return false;

        Map target = (Map)parms.target;

        // 检查温和部落派系存不存在
        Faction? tribeCivil = Find.FactionManager.FirstFactionOfDef(FactionDefOf.TribeCivil);
        if (tribeCivil == null || tribeCivil.HostileTo(Faction.OfPlayer))
            return false;

        return base.CanFireNowSub(parms) && target.IsPlayerHome;
    }

    /// <summary>
    /// Executes the incident worker.
    /// </summary>
    protected override bool TryExecuteWorker(IncidentParms parms)
    {
        // 强制锁定派系温和部落
        Faction? tribeCivil = Find.FactionManager.FirstFactionOfDef(FactionDefOf.TribeCivil);
        if (tribeCivil == null || tribeCivil.HostileTo(Faction.OfPlayer))
            return false;

        parms.faction = tribeCivil;

        // Set trader kind if not provided
        if (parms.traderKind == null)
        {
            if (parms.faction.def.caravanTraderKinds.NullOrEmpty())
                return false;
            parms.traderKind = parms.faction.def.caravanTraderKinds.RandomElement();
        }

        // Execute base worker
        if (!base.TryExecuteWorker(parms))
            return false;

        Map target = (Map)parms.target;
        List<Pawn> traders = target.mapPawns.AllPawnsSpawned
            .Where(p => p.trader != null && p.Faction == parms.faction)
            .ToList();

        if (traders.NullOrEmpty())
            return false;

        SpawnSylvieAndSendOffer(traders.RandomElement(), target);
        return true;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Spawns Sylvie and sends the offer letter.
    /// </summary>
    private void SpawnSylvieAndSendOffer(Pawn traderLeader, Map map)
    {
        Pawn? sylvie = SylviePawnGenerator.GenerateSylvie(traderLeader.Faction);
        if (sylvie == null)
        {
            Log.Error("[SylvieMod] Failed to generate Sylvie pawn");
            return;
        }

        GenSpawn.Spawn(sylvie, traderLeader.Position, map);
        sylvie.guest.SetGuestStatus(traderLeader.Faction, GuestStatus.Prisoner);
        traderLeader.lord?.AddPawn(sylvie);

        Current.Game.GetComponent<SylvieGameComponent>().hasSylvieSpawned = true;
        Current.Game.GetComponent<SylvieGameComponent>().RegisterSylviePawn(sylvie);

        SendOfferLetter(traderLeader, sylvie, map);
    }

    /// <summary>
    /// Sends the offer letter to the player.
    /// </summary>
    private void SendOfferLetter(Pawn trader, Pawn sylvie, Map map)
    {
        LetterDef? letterDef = SylvieDefNames.Letter_OfferLetterDef;
        if (letterDef == null)
        {
            Log.Error("[SylvieMod] Could not find Sylvie_OfferLetter LetterDef");
            return;
        }

        // 使用 LetterMaker 创建 Letter，确保正确分配 Load ID
        ChoiceLetter_SylvieOffer letter = (ChoiceLetter_SylvieOffer)LetterMaker.MakeLetter(letterDef);
        letter.Configure(trader, sylvie, map);
        Find.LetterStack.ReceiveLetter(letter);
    }

    #endregion
}
