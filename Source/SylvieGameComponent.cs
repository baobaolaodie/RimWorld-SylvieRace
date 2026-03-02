using RimWorld;
using System;
using System.Linq;
using Verse;

#nullable disable
namespace SylvieMod;

public class SylvieGameComponent : GameComponent
{
  public bool hasSylvieSpawned = false;
  private int checkInterval = 2500;
  private const int TargetTick = 5000;

  public SylvieGameComponent(Game game)
  {
  }

  public override void GameComponentTick()
  {
    base.GameComponentTick();
    if (this.hasSylvieSpawned || Find.TickManager.TicksGame % this.checkInterval != 0 || Find.TickManager.TicksGame < 5000)
      return;
    this.TryForceSylvieEvent();
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

  public override void ExposeData()
  {
    base.ExposeData();
    Scribe_Values.Look<bool>(ref this.hasSylvieSpawned, "hasSylvieSpawned");
  }
}
