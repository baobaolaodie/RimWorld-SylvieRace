#nullable enable
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace SylvieMod;

/// <summary>
/// Game component for tracking Sylvie petting cooldowns.
/// Manages the timing of when each Sylvie was last petted.
/// </summary>
public class SylvieSeekPettingTracker : GameComponent
{
    private Dictionary<Pawn, int> lastPettingTicks = new Dictionary<Pawn, int>();
    private List<Pawn> pawnKeys = new List<Pawn>();
    private List<int> tickValues = new List<int>();

    /// <summary>
    /// Cooldown duration in ticks (6 hours = 15000 ticks, where 1 hour = 2500 ticks).
    /// </summary>
    public const int CooldownTicks = 15000; // 6 hours * 2500 ticks/hour

    public SylvieSeekPettingTracker(Game game) { }

    /// <summary>
    /// Gets the last tick when the specified pawn was petted.
    /// Returns -1 if the pawn has never been petted.
    /// </summary>
    public int GetLastPettingTick(Pawn pawn)
    {
        return lastPettingTicks.TryGetValue(pawn, out int tick) ? tick : -1;
    }

    /// <summary>
    /// Sets the last petting tick for the specified pawn.
    /// </summary>
    public void SetLastPettingTick(Pawn pawn, int tick)
    {
        lastPettingTicks[pawn] = tick;
    }

    /// <summary>
    /// Checks if the specified pawn is currently in petting cooldown.
    /// </summary>
    public bool IsInCooldown(Pawn pawn)
    {
        int lastTick = GetLastPettingTick(pawn);
        if (lastTick < 0) return false;
        return Find.TickManager.TicksGame - lastTick < CooldownTicks;
    }

    /// <summary>
    /// Gets the remaining cooldown time in ticks for the specified pawn.
    /// Returns 0 if not in cooldown.
    /// </summary>
    public int GetCooldownRemaining(Pawn pawn)
    {
        int lastTick = GetLastPettingTick(pawn);
        if (lastTick < 0) return 0;

        int elapsed = Find.TickManager.TicksGame - lastTick;
        int remaining = CooldownTicks - elapsed;
        return remaining > 0 ? remaining : 0;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref lastPettingTicks, "lastPettingTicks", LookMode.Reference, LookMode.Value, ref pawnKeys, ref tickValues);
    }
}
