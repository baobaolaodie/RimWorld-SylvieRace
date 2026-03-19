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
    protected override bool CanFireNowSub(IncidentParms parms)
    {
        if (Current.Game.GetComponent<SylvieGameComponent>().hasSylvieSpawned)
            return false;

        Map target = (Map)parms.target;
        return base.CanFireNowSub(parms) && target.IsPlayerHome;
    }

    protected override bool TryExecuteWorker(IncidentParms parms)
    {
        if (parms.faction == null)
        {
            parms.faction = Find.FactionManager.AllFactions
                .Where(f => !f.IsPlayer && !f.HostileTo(Faction.OfPlayer) && !f.Hidden && f.def.humanlikeFaction)
                .RandomElementWithFallback();
        }

        if (parms.faction == null)
            return false;

        if (parms.traderKind == null)
        {
            if (parms.faction.def.caravanTraderKinds.NullOrEmpty())
                return false;
            parms.traderKind = parms.faction.def.caravanTraderKinds.RandomElement();
        }

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

    private void SpawnSylvieAndSendOffer(Pawn traderLeader, Map map)
    {
        Pawn sylvie = SylviePawnGenerator.GenerateSylvie(traderLeader.Faction);
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

    private void SendOfferLetter(Pawn trader, Pawn sylvie, Map map)
    {
        LetterDef? letterDef = SylvieDefNames.Letter_OfferLetterDef;
        if (letterDef == null)
        {
            Log.Error("[SylvieMod] Could not find Sylvie_OfferLetter LetterDef");
            return;
        }

        ChoiceLetter_SylvieOffer letter = new ChoiceLetter_SylvieOffer();
        letter.def = letterDef;
        letter.Configure(trader, sylvie, map);
        Find.LetterStack.ReceiveLetter(letter);
    }
}
