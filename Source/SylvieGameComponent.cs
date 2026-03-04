using RimWorld;
using System;
using System.Linq;
using Verse;

#nullable disable
namespace SylvieMod;

public class SylvieGameComponent : GameComponent
{
  public bool hasSylvieSpawned = false;
  public Pawn sylviePawn = null;
  public int hediffTriggerTick = -1;
  public bool hediffTriggered = false;
  private int checkInterval = 2500;
  private const int TargetTick = 5000;
  private const int HediffDelayTicks = 300000;

  public SylvieGameComponent(Game game)
  {
  }

  public override void GameComponentTick()
  {
    base.GameComponentTick();
    if (Find.TickManager.TicksGame % this.checkInterval != 0)
      return;
    if (!this.hasSylvieSpawned && Find.TickManager.TicksGame >= 5000)
      this.TryForceSylvieEvent();
    if (!this.hediffTriggered && this.sylviePawn != null && this.hediffTriggerTick > 0 && Find.TickManager.TicksGame >= this.hediffTriggerTick)
      this.TriggerHediff();
  }

  private void TryForceSylvieEvent()
  {
    Map target = Find.Maps.Where<Map>((Func<Map, bool>) (m => m.IsPlayerHome && m.mapPawns.FreeColonistsSpawnedCount > 0)).FirstOrDefault<Map>();
    if (target == null)
      return;
    IncidentDef incidentDef = IncidentDef.Named("Sylvie_ArrivalEvent");
    IncidentParms parms = StorytellerUtility.DefaultParmsNow(incidentDef.category, (IIncidentTarget) target);
    if (incidentDef.Worker.TryExecute(parms))
      this.hasSylvieSpawned = true;
  }

  public void SetSylviePawn(Pawn pawn)
  {
    this.sylviePawn = pawn;
    this.hediffTriggerTick = Find.TickManager.TicksGame + HediffDelayTicks;
  }

  private void TriggerHediff()
  {
    if (this.sylviePawn == null || this.sylviePawn.Dead)
    {
      this.hediffTriggered = true;
      return;
    }
    HediffDef hediffDef = HediffDef.Named("SylvieRace_InitialTrauma");
    if (hediffDef == null)
    {
      Log.Error("SylvieRace: Could not find SylvieRace_InitialTrauma HediffDef");
      this.hediffTriggered = true;
      return;
    }
    Hediff hediff = HediffMaker.MakeHediff(hediffDef, this.sylviePawn);
    hediff.Severity = 1.0f;
    this.sylviePawn.health.AddHediff(hediff);
    this.hediffTriggered = true;
    this.SendHediffLetter();
  }

  private void SendHediffLetter()
  {
    if (this.sylviePawn == null)
      return;
    TaggedString label = "SylvieRace_HediffLetterLabel".Translate();
    TaggedString text = "SylvieRace_HediffLetterText".Translate(this.sylviePawn.LabelShort);
    Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, new LookTargets(this.sylviePawn));
  }

  public override void ExposeData()
  {
    base.ExposeData();
    Scribe_Values.Look<bool>(ref this.hasSylvieSpawned, "hasSylvieSpawned");
    Scribe_References.Look<Pawn>(ref this.sylviePawn, "sylviePawn");
    Scribe_Values.Look<int>(ref this.hediffTriggerTick, "hediffTriggerTick", -1);
    Scribe_Values.Look<bool>(ref this.hediffTriggered, "hediffTriggered");
  }
}
