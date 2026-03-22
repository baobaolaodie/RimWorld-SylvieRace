#nullable enable
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace SylvieMod;

/// <summary>
/// Custom choice letter for Sylvie's offer from the trader.
/// </summary>
public class ChoiceLetter_SylvieOffer : ChoiceLetter
{
    public Pawn? sylvie;
    public Map? map;
    public int price = 100;

    /// <summary>
    /// Configures the letter with trader and target pawn information.
    /// </summary>
    public void Configure(Pawn trader, Pawn targetPawn, Map targetMap)
    {
        sylvie = targetPawn;
        map = targetMap;
        relatedFaction = trader.Faction;
        lookTargets = new LookTargets(targetPawn);
        Label = "Sylvie_OfferLabel".Translate();
        Text = "Sylvie_OfferText".Translate(trader.Name.ToStringShort);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_References.Look(ref sylvie, "sylvie");
        Scribe_References.Look(ref map, "map");
        Scribe_Values.Look(ref price, "price", 100);
        
        // 确保 lookTargets 正确序列化
        if (Scribe.mode == LoadSaveMode.LoadingVars && sylvie != null)
        {
            lookTargets = new LookTargets(sylvie);
        }
    }

    public override IEnumerable<DiaOption> Choices
    {
        get
        {
            yield return CreateBuyOption();
            yield return CreateRefuseOption();
        }
    }

    private DiaOption CreateBuyOption()
    {
        DiaOption buyOption = new DiaOption("Sylvie_PayAndTakeHer".Translate(price.ToString()));
        
        int currentSilver = CountAvailableSilver();
        
        if (currentSilver >= price)
        {
            buyOption.action = () => ExecutePurchase();
            buyOption.resolveTree = true;
        }
        else
        {
            buyOption.Disable("Sylvie_NotEnoughSilver".Translate(currentSilver.ToString()));
        }

        return buyOption;
    }

    private DiaOption CreateRefuseOption()
    {
        return new DiaOption("Sylvie_Refuse".Translate())
        {
            action = () => Find.LetterStack.RemoveLetter(this),
            resolveTree = true
        };
    }

    private int CountAvailableSilver()
    {
        if (map == null) return 0;

        int count = 0;
        foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.Silver))
        {
            if (!thing.IsForbidden(Faction.OfPlayer))
            {
                count += thing.stackCount;
            }
        }
        return count;
    }

    private void ExecutePurchase()
    {
        if (map == null) return;

        DeductSilver();
        TransferSylvieToPlayer();
        Find.LetterStack.RemoveLetter(this);
    }

    private void DeductSilver()
    {
        if (map == null) return;

        int remaining = price;
        List<Thing> silverList = map.listerThings.ThingsOfDef(ThingDefOf.Silver).ToList();
        
        foreach (Thing thing in silverList)
        {
            if (remaining <= 0) break;
            if (thing.IsForbidden(Faction.OfPlayer)) continue;

            int count = Mathf.Min(thing.stackCount, remaining);
            thing.SplitOff(count).Destroy();
            remaining -= count;
        }
    }

    private void TransferSylvieToPlayer()
    {
        if (sylvie == null || sylvie.Dead) return;

        if (!sylvie.Spawned && map != null)
        {
            GenSpawn.Spawn(sylvie, map.Center, map);
        }

        sylvie.guest.SetGuestStatus(null);
        sylvie.SetFaction(Faction.OfPlayer, null);
        
        Messages.Message(
            "Sylvie_TradeSuccess".Translate(),
            new LookTargets(sylvie),
            MessageTypeDefOf.PositiveEvent
        );
    }
}
