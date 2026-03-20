#nullable enable
using System.Collections.Generic;
using Verse;

namespace SylvieMod;

/// <summary>
/// Tracks aiming state for Sylvie pawns to synchronize facial animations.
/// </summary>
public class SylvieAimingTracker : ThingComp
{
    #region Fields

    private Pawn? cachedPawn;
    private static readonly Dictionary<Pawn, SylvieAimingTracker> Trackers = new();

    #endregion

    #region Properties

    /// <summary>
    /// Gets the parent pawn, using cached value if available.
    /// </summary>
    public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;

    #endregion

    #region Static Methods

    /// <summary>
    /// Gets the tracker for the specified pawn, creating it if necessary.
    /// </summary>
    /// <param name="pawn">The pawn to get tracker for</param>
    /// <returns>The tracker instance, or null if not found</returns>
    public static SylvieAimingTracker? GetTracker(Pawn pawn)
    {
        if (pawn == null) return null;

        if (Trackers.TryGetValue(pawn, out var tracker))
            return tracker;

        tracker = pawn.GetComp<SylvieAimingTracker>();
        if (tracker != null)
            Trackers[pawn] = tracker;

        return tracker;
    }

    /// <summary>
    /// Clears the tracker cache. Should be called on game reset.
    /// </summary>
    public static void ClearCache()
    {
        Trackers.Clear();
    }

    #endregion
}
