#nullable enable
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace SylvieMod;

/// <summary>
/// Component that tracks ranged weapon cooldown state for Sylvie pawns.
/// Provides cooldown progress tracking and animation state calculation.
/// </summary>
public class SylvieCooldownTracker : ThingComp
{
    #region Constants

    /// <summary>
    /// Threshold for sweat frame 1 (0-33% progress).
    /// </summary>
    private const float SweatFrame1Threshold = 0.33f;

    /// <summary>
    /// Threshold for sweat frame 2 (33-66% progress).
    /// </summary>
    private const float SweatFrame2Threshold = 0.66f;

    /// <summary>
    /// Number of bullet animation cycles.
    /// </summary>
    private const int BulletAnimationCycles = 5;

    /// <summary>
    /// Threshold for bullet insert frame 1 (0-25% of cycle).
    /// </summary>
    private const float InsertFrame1Threshold = 0.25f;

    /// <summary>
    /// Threshold for bullet insert frame 2 (25-50% of cycle).
    /// </summary>
    private const float InsertFrame2Threshold = 0.50f;

    /// <summary>
    /// Threshold for bullet insert frame 3 (50-75% of cycle).
    /// </summary>
    private const float InsertFrame3Threshold = 0.75f;

    /// <summary>
    /// Maximum bullet count to display.
    /// </summary>
    private const int MaxBulletCount = 5;

    #endregion

    #region Fields

    private Pawn? cachedPawn;
    private int cooldownStartTick = -1;
    private int totalCooldownTicks = 0;
    private Verb? lastVerb;

    private static readonly Dictionary<Pawn, SylvieCooldownTracker> Trackers = new Dictionary<Pawn, SylvieCooldownTracker>();

    #endregion

    #region Properties

    /// <summary>
    /// Gets the parent pawn, using cached value if available.
    /// </summary>
    public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;

    /// <summary>
    /// Checks if the pawn is currently in ranged weapon cooldown.
    /// </summary>
    public bool IsInRangedCooldown
    {
        get
        {
            var stance = Pawn.stances?.curStance as Stance_Cooldown;
            if (stance == null) return false;

            Verb? verb = stance.verb;
            if (verb == null) return false;

            if (verb.state == VerbState.Bursting) return false;

            if (verb.verbProps.IsMeleeAttack) return false;

            return true;
        }
    }

    /// <summary>
    /// Gets the current cooldown progress (0.0 to 1.0).
    /// </summary>
    public float CooldownProgress
    {
        get
        {
            var stance = Pawn.stances?.curStance as Stance_Cooldown;
            if (stance == null || stance.verb == null)
                return 0f;

            Verb verb = stance.verb;

            if (lastVerb != verb)
            {
                lastVerb = verb;
                cooldownStartTick = Find.TickManager.TicksGame;
                totalCooldownTicks = stance.ticksLeft;
            }

            if (totalCooldownTicks <= 0)
                return 1f;

            int ticksLeft = stance.ticksLeft;
            int elapsedTicks = totalCooldownTicks - ticksLeft;

            return (float)elapsedTicks / totalCooldownTicks;
        }
    }

    #endregion

    #region Static Methods

    /// <summary>
    /// Gets the tracker for the specified pawn.
    /// </summary>
    /// <param name="pawn">The pawn to get tracker for</param>
    /// <returns>The tracker instance, or null if not found</returns>
    public static SylvieCooldownTracker? GetTracker(Pawn pawn)
    {
        if (Trackers.TryGetValue(pawn, out var tracker))
            return tracker;

        tracker = pawn.GetComp<SylvieCooldownTracker>();
        if (tracker != null)
            Trackers[pawn] = tracker;

        return tracker;
    }

    #endregion

    #region Animation State Methods

    /// <summary>
    /// Gets the current sweat animation frame (1-3) based on cooldown progress.
    /// </summary>
    /// <returns>Sweat frame number (1, 2, or 3)</returns>
    public int GetSweatFrame()
    {
        float progress = CooldownProgress;
        if (progress < SweatFrame1Threshold) return 1;
        if (progress < SweatFrame2Threshold) return 2;
        return 3;
    }

    /// <summary>
    /// Gets the current bullet animation state.
    /// </summary>
    /// <returns>Tuple containing insert frame (0-3) and bullet count (1-5)</returns>
    public (int insertFrame, int bulletCount) GetBulletAnimationState()
    {
        float progress = CooldownProgress;

        int cycle = (int)(progress * BulletAnimationCycles);
        if (cycle >= BulletAnimationCycles) cycle = BulletAnimationCycles - 1;

        float cycleProgress = (progress * BulletAnimationCycles) - cycle;

        int insertFrame = CalculateInsertFrame(cycleProgress);
        int bulletCount = CalculateBulletCount(cycle);

        return (insertFrame, bulletCount);
    }

    /// <summary>
    /// Calculates the insert frame based on cycle progress.
    /// </summary>
    private int CalculateInsertFrame(float cycleProgress)
    {
        if (cycleProgress < InsertFrame1Threshold) return 1;
        if (cycleProgress < InsertFrame2Threshold) return 2;
        if (cycleProgress < InsertFrame3Threshold) return 3;
        return 0;
    }

    /// <summary>
    /// Calculates the bullet count based on current cycle.
    /// </summary>
    private int CalculateBulletCount(int cycle)
    {
        int bulletCount = cycle + 1;
        if (bulletCount > MaxBulletCount) bulletCount = MaxBulletCount;
        return bulletCount;
    }

    #endregion

    #region Component Lifecycle

    /// <summary>
    /// Called every tick. Resets cooldown state when not in cooldown.
    /// </summary>
    public override void CompTick()
    {
        base.CompTick();

        if (!IsInRangedCooldown)
        {
            cooldownStartTick = -1;
            lastVerb = null;
        }
    }

    #endregion
}
