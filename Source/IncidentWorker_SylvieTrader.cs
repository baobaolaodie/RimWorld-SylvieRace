using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

#nullable disable
namespace SylvieMod;

public class IncidentWorker_SylvieTrader : IncidentWorker_TraderCaravanArrival
{
  protected override bool CanFireNowSub(IncidentParms parms)
  {
    if (Current.Game.GetComponent<SylvieGameComponent>().hasSylvieSpawned)
      return false;
    Map target = (Map) parms.target;
    return base.CanFireNowSub(parms) && target.IsPlayerHome;
  }

  protected override bool TryExecuteWorker(IncidentParms parms)
  {
    if (parms.faction == null)
      parms.faction = Find.FactionManager.AllFactions.Where<Faction>((Func<Faction, bool>) (f => !f.IsPlayer && !f.HostileTo(Faction.OfPlayer) && !f.Hidden && f.def.humanlikeFaction)).RandomElementWithFallback<Faction>();
    if (parms.faction == null)
      return false;
    if (parms.traderKind == null)
      parms.traderKind = parms.faction.def.caravanTraderKinds.RandomElement<TraderKindDef>();
    if (!base.TryExecuteWorker(parms))
      return false;
    Map target = (Map) parms.target;
    List<Pawn> list = target.mapPawns.AllPawnsSpawned.Where<Pawn>((Func<Pawn, bool>) (p => p.trader != null && p.Faction == parms.faction)).ToList<Pawn>();
    if (list.NullOrEmpty<Pawn>())
      return false;
    this.SpawnSylvieAndSendOffer(list.RandomElement<Pawn>(), target);
    return true;
  }

  private void SpawnSylvieAndSendOffer(Pawn traderLeader, Map map)
  {
    PawnKindDef pawnKindDef = PawnKindDef.Named("Sylvie_PawnKind");
    XenotypeDef named1 = DefDatabase<XenotypeDef>.GetNamed("Baseliner", false);
    PawnKindDef kind = pawnKindDef;
    Faction faction = traderLeader.Faction;
    PlanetTile? tile = PlanetTile.Invalid;
    float? nullable1 = new float?(19f);
    float? nullable2 = new float?(19f);
    XenotypeDef xenotypeDef = named1;
    Gender? nullable3 = new Gender?(Gender.Female);
    float? minChanceToRedressWorldPawn = new float?();
    float? fixedBiologicalAge = nullable1;
    float? fixedChronologicalAge = nullable2;
    Gender? fixedGender = nullable3;
    XenotypeDef forcedXenotype = xenotypeDef;
    FloatRange? excludeBiologicalAgeRange = new FloatRange?();
    FloatRange? biologicalAgeRange = new FloatRange?();
    Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, tile: tile, forceGenerateNewPawn: true, minChanceToRedressWorldPawn: minChanceToRedressWorldPawn, fixedBiologicalAge: fixedBiologicalAge, fixedChronologicalAge: fixedChronologicalAge, fixedGender: fixedGender, forcedXenotype: forcedXenotype, excludeBiologicalAgeRange: excludeBiologicalAgeRange, biologicalAgeRange: biologicalAgeRange));
    if (pawn.genes != null)
    {
      GeneDef named2 = DefDatabase<GeneDef>.GetNamed("Skin_SheerWhite", false);
      GeneDef named3 = DefDatabase<GeneDef>.GetNamed("Hair_SnowWhite", false);
      if (named2 != null)
      {
        List<Gene> geneList = new List<Gene>();
        foreach (Gene gene in pawn.genes.GenesListForReading)
        {
          if (gene.def.endogeneCategory == EndogeneCategory.Melanin)
            geneList.Add(gene);
        }
        for (int index = 0; index < geneList.Count; ++index)
          pawn.genes.RemoveGene(geneList[index]);
        pawn.genes.AddGene(named2, false);
      }
      if (named3 != null)
      {
        List<Gene> geneList = new List<Gene>();
        foreach (Gene gene in pawn.genes.GenesListForReading)
        {
          if (gene.def.endogeneCategory == EndogeneCategory.HairColor)
            geneList.Add(gene);
        }
        for (int index = 0; index < geneList.Count; ++index)
          pawn.genes.RemoveGene(geneList[index]);
        pawn.genes.AddGene(named3, false);
      }
      pawn.story.SkinColorBase = pawn.story.SkinColorBase;
      if (pawn.story.hairDef != null)
        pawn.Drawer.renderer.SetAllGraphicsDirty();
    }
    pawn.RaceProps.body.AllParts.FirstOrDefault<BodyPartRecord>((Predicate<BodyPartRecord>) (x => x.def == BodyPartDefOf.Head));
    pawn.story.traits.allTraits.Clear();
    pawn.story.traits.GainTrait(new Trait(TraitDefOf.Kind));
    if (pawn.style != null)
    {
      TattooDef faceTattoo = DefDatabase<TattooDef>.GetNamed("SylvieRace_ScarHead", false);
      TattooDef bodyTattoo = DefDatabase<TattooDef>.GetNamed("SylvieRace_ScarBody", false);
      if (faceTattoo != null)
        pawn.style.FaceTattoo = faceTattoo;
      if (bodyTattoo != null)
        pawn.style.BodyTattoo = bodyTattoo;
    }
    GenSpawn.Spawn((Thing) pawn, traderLeader.Position, map);
    pawn.guest.SetGuestStatus(traderLeader.Faction, GuestStatus.Prisoner);
    traderLeader.GetLord()?.AddPawn(pawn);
    Current.Game.GetComponent<SylvieGameComponent>().hasSylvieSpawned = true;
    LetterDef named4 = DefDatabase<LetterDef>.GetNamed("Sylvie_OfferLetter", false);
    if (named4 == null)
    {
      Log.Error("又tm出bug啦");
    }
    else
    {
      IncidentWorker_SylvieTrader.ChoiceLetter_SylvieOffer let = new IncidentWorker_SylvieTrader.ChoiceLetter_SylvieOffer();
      let.def = named4;
      let.Configure(traderLeader, pawn, map);
      Find.LetterStack.ReceiveLetter((Letter) let);
    }
  }

    public class ChoiceLetter_SylvieOffer : ChoiceLetter
    {
        public Pawn sylvie;
        public Map map;
        public int price = 100;

        public void Configure(Pawn trader, Pawn targetPawn, Map targetMap)
        {
            this.sylvie = targetPawn;
            this.map = targetMap;
            this.relatedFaction = trader.Faction;
            this.lookTargets = new LookTargets((Thing)targetPawn);
            this.Label = (TaggedString)"Sylvie_OfferLabel".Translate();
            this.Text = (TaggedString)"Sylvie_OfferText".Translate(trader.Name.ToStringShort);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Pawn>(ref this.sylvie, "sylvie");
            Scribe_References.Look<Map>(ref this.map, "map");
            Scribe_Values.Look<int>(ref this.price, "price", 100);
        }

        public override IEnumerable<DiaOption> Choices
        {
            get
            {
                DiaOption buyOption = new DiaOption(
                    "Sylvie_PayAndTakeHer".Translate(this.price.ToString()));
                int currentSilver = 0;
                List<Thing> silverList = (List<Thing>) null;
                if (this.map != null)
                {
                    silverList = this.map.listerThings.ThingsOfDef(ThingDefOf.Silver);
                    foreach (Thing t in silverList)
                    {
                        if (!t.IsForbidden(Faction.OfPlayer))
                            currentSilver += t.stackCount;
                    }
                }
                if (currentSilver >= this.price)
                {
                    buyOption.action = (Action) (() =>
                    {
                        int price = this.price;
                        if (silverList != null)
                        {
                            foreach (Thing t in silverList.ToList<Thing>())
                            {
                                if (price > 0)
                                {
                                    if (!t.IsForbidden(Faction.OfPlayer))
                                    {
                                        int count = Mathf.Min(t.stackCount, price);
                                        t.SplitOff(count).Destroy();
                                        price -= count;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        if (this.sylvie != null && !this.sylvie.Dead)
                        {
                            if (!this.sylvie.Spawned)
                                GenSpawn.Spawn((Thing)this.sylvie, this.map.Center, this.map);
                            this.sylvie.guest.SetGuestStatus((Faction)null);
                            this.sylvie.SetFaction(Faction.OfPlayer, (Pawn)null);
                            Messages.Message(
                                "Sylvie_TradeSuccess".Translate(),
                                (LookTargets)(Thing)this.sylvie,
                                MessageTypeDefOf.PositiveEvent);
                        }
                        Find.LetterStack.RemoveLetter((Letter) this);
                    });
                    buyOption.resolveTree = true;
                }
                else
                {
                    buyOption.Disable(
                        "Sylvie_NotEnoughSilver".Translate(currentSilver.ToString()));
                }

                yield return buyOption;
                yield return new DiaOption("Sylvie_Refuse".Translate())
                {
                    action = (Action) (() => Find.LetterStack.RemoveLetter((Letter) this)),
                    resolveTree = true
                };
            }
        }
    }
}
