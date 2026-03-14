#nullable enable
using RimWorld;
using System.Linq;
using Verse;

namespace SylvieMod;

/// <summary>
/// Game component for managing Sylvie-related state and events.
/// </summary>
public class SylvieGameComponent : GameComponent
{
    public bool hasSylvieSpawned;
    private Pawn? sylviePawn;
    private int hediffTriggerTick = -1;
    private bool hediffTriggered;

    private const int CheckInterval = SylvieConstants.CheckIntervalTicks;
    private const int InitialEventTick = SylvieConstants.InitialEventDelayTicks;

    public SylvieGameComponent(Game game) { }

    public override void GameComponentTick()
    {
        base.GameComponentTick();

        if (Find.TickManager.TicksGame % CheckInterval != 0)
            return;

        if (!hasSylvieSpawned && Find.TickManager.TicksGame >= InitialEventTick)
        {
            if (CheckForExistingSylvie())
            {
                hasSylvieSpawned = true;
                return;
            }
            TryForceSylvieEvent();
        }

        if (!hediffTriggered && sylviePawn != null && hediffTriggerTick > 0 && Find.TickManager.TicksGame >= hediffTriggerTick)
        {
            if (SylvieHediffManager.TryTriggerHediff(sylviePawn))
            {
                hediffTriggered = true;
            }
        }
    }

    private bool CheckForExistingSylvie()
    {
        foreach (Map map in Find.Maps)
        {
            if (map.IsPlayerHome)
            {
                foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
                {
                    if (SylvieDefNames.IsSylvieRace(pawn))
                    {
                        sylviePawn = pawn;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void TryForceSylvieEvent()
    {
        Map? target = Find.Maps
            .Where(m => m.IsPlayerHome && m.mapPawns.FreeColonistsSpawnedCount > 0)
            .FirstOrDefault();

        if (target == null) return;

        IncidentDef? incidentDef = SylvieDefNames.Incident_ArrivalEventDef;
        if (incidentDef == null) return;

        IncidentParms parms = StorytellerUtility.DefaultParmsNow(incidentDef.category, target);
        if (incidentDef.Worker.TryExecute(parms))
        {
            hasSylvieSpawned = true;
        }
    }

    /// <summary>
    /// Registers a Sylvie pawn and schedules the hediff trigger.
    /// </summary>
    public void RegisterSylviePawn(Pawn pawn)
    {
        sylviePawn = pawn;
        hediffTriggerTick = SylvieHediffManager.CalculateTriggerTick();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref hasSylvieSpawned, "hasSylvieSpawned");
        Scribe_References.Look(ref sylviePawn, "sylviePawn");
        Scribe_Values.Look(ref hediffTriggerTick, "hediffTriggerTick", -1);
        Scribe_Values.Look(ref hediffTriggered, "hediffTriggered");
    }
}
