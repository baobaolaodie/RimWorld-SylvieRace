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
    // 存档版本控制
    private const int CURRENT_SAVE_VERSION = 1;
    private int saveDataVersion = 1;

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

        // 检查 Sylvie 是否仍然有效
        if (sylviePawn != null && (sylviePawn.Dead || sylviePawn.Destroyed))
        {
            Log.Message("[SylvieMod] Sylvie is no longer alive, clearing reference");
            sylviePawn = null;
            hediffTriggered = true; // 防止继续触发 Hediff
        }

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

        // 存档版本控制
        Scribe_Values.Look(ref saveDataVersion, "saveDataVersion", 1);

        // 数据迁移
        if (Scribe.mode == LoadSaveMode.LoadingVars && saveDataVersion < CURRENT_SAVE_VERSION)
        {
            PerformDataMigration();
            saveDataVersion = CURRENT_SAVE_VERSION;
        }

        Scribe_Values.Look(ref hasSylvieSpawned, "hasSylvieSpawned");
        Scribe_References.Look(ref sylviePawn, "sylviePawn");
        Scribe_Values.Look(ref hediffTriggerTick, "hediffTriggerTick", -1);
        Scribe_Values.Look(ref hediffTriggered, "hediffTriggered");
    }

    /// <summary>
    /// 执行数据迁移。
    /// </summary>
    private void PerformDataMigration()
    {
        Log.Message($"[SylvieMod] Migrating save data from v{saveDataVersion} to v{CURRENT_SAVE_VERSION}");
        // 当前版本无需特殊迁移逻辑，未来版本可在此添加
    }

    /// <summary>
    /// 游戏加载完成后调用，支持存档中途加入。
    /// </summary>
    public override void LoadedGame()
    {
        base.LoadedGame();

        // 延迟一帧执行组件注册，确保所有 Pawn 已完全初始化
        LongEventHandler.ExecuteWhenFinished(() =>
        {
            RegisterComponentsForExistingSylvie();
        });
    }

    /// <summary>
    /// 为已存在的 Sylvie Pawn 注册组件。
    /// </summary>
    private void RegisterComponentsForExistingSylvie()
    {
        int registeredCount = 0;
        
        foreach (Map map in Find.Maps)
        {
            foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
            {
                if (SylvieDefNames.IsSylvieRace(pawn))
                {
                    SylvieComponentRegistry.RegisterAllComponents(pawn);
                    registeredCount++;
                    Log.Message($"[SylvieMod] Registered components for existing Sylvie: {pawn.LabelShort}");
                }
            }
        }
        
        if (registeredCount > 0)
        {
            Log.Message($"[SylvieMod] Total Sylvie pawns registered: {registeredCount}");
        }
    }
}
