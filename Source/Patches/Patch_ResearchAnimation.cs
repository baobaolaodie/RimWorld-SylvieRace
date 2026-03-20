#nullable enable
using HarmonyLib;
using Verse;
using RimWorld;
using FacialAnimation;
using System.Collections.Generic;

namespace SylvieMod;

/// <summary>
/// Patches FaceAnimation.GetCurrentFrame for Sylvie research animation.
/// Synchronizes cat ear animation with facial animation states.
/// </summary>
[HarmonyPatch(typeof(FaceAnimation), nameof(FaceAnimation.GetCurrentFrame))]
public static class Patch_FaceAnimation_GetCurrentFrame_Research
{
    #region Constants

    /// <summary>
    /// Research animation timeout in ticks (1 second @ 60 TPS).
    /// </summary>
    private const int ResearchTimeoutTicks = 60;

    /// <summary>
    /// Cat ear frame indices.
    /// </summary>
    private const int EarFrame1 = 0;
    private const int EarFrame2 = 1;

    #endregion

    #region State Tracking

    private static readonly Dictionary<Pawn, bool> WasResearchActive = new();
    private static readonly Dictionary<Pawn, int> LastResearchTick = new();

    #endregion

    #region Patch Implementation

    /// <summary>
    /// Prefix patch to handle research animation frames and cat ear synchronization.
    /// </summary>
    public static bool Prefix(FaceAnimation __instance, int tickGame, ref FaceAnimationDef.AnimationFrame? __result)
    {
        // Defensive check: validate animation
        if (!SylvieAnimationHelper.IsValidAnimation(__instance))
        {
            return true;
        }

        // Handle non-research animations
        if (__instance.animationDef.defName != SylvieDefNames.Animation_Research)
        {
            HandleNonResearchAnimation(__instance);
            return true;
        }

        // Get associated pawn
        Pawn? pawn = SylvieAnimationRegistry.GetAssociatedPawn(__instance);
        if (pawn == null)
            return true;

        // Validate Sylvie race
        if (!SylvieDefNames.IsSylvieRace(pawn))
        {
            EnsureCatEarHidden(pawn);
            return true;
        }

        // Check if actually researching
        if (!IsActuallyResearching(pawn))
        {
            EnsureCatEarHidden(pawn);
            return true;
        }

        // Get and validate frames
        var frames = SylvieAnimationHelper.GetAnimationFrames(__instance.animationDef);
        if (!SylvieAnimationHelper.HasValidFrames(frames))
        {
            EnsureCatEarHidden(pawn);
            return true;
        }

        // Calculate frame index
        int startTick = SylvieAnimationHelper.GetStartTick(__instance);
        int elapsedTicks = tickGame - startTick;
        int frameIndex = SylvieAnimationHelper.CalculateFrameIndex(elapsedTicks, frames!.Count);

        __result = frames[frameIndex];

        // Update cat ear animation
        UpdateCatEarAnimation(pawn, __instance.animationDef, frameIndex);

        return false;
    }

    #endregion

    #region Cat Ear Management

    /// <summary>
    /// Updates cat ear animation based on current frame.
    /// </summary>
    private static void UpdateCatEarAnimation(Pawn pawn, FaceAnimationDef animationDef, int frameIndex)
    {
        var catEarComp = pawn.GetComp<SylvieCatEarComp>();
        if (catEarComp == null) return;

        int originalFrameIndex = CalculateOriginalFrameIndex(animationDef, frameIndex);
        int earFrame = MapToEarFrame(originalFrameIndex);

        catEarComp.SetCurrentEarFrame(earFrame);
        catEarComp.SetShouldRender(true);
    }

    /// <summary>
    /// Calculates original animation frame index from expanded frame index.
    /// </summary>
    private static int CalculateOriginalFrameIndex(FaceAnimationDef animationDef, int expandedFrameIndex)
    {
        var frames = SylvieAnimationHelper.GetAnimationFrames(animationDef);
        if (frames == null || frames.Count == 0) return 0;

        // Get duration per frame (use first frame as baseline)
        int durationPerFrame = System.Math.Max(1, frames[0].duration);
        int originalFrameIndex = expandedFrameIndex / durationPerFrame;

        return System.Math.Min(originalFrameIndex, frames.Count - 1);
    }

    /// <summary>
    /// Maps original frame index to ear frame.
    /// Frames 0, 2, 4 -> Ear 1; Others -> Ear 2
    /// </summary>
    private static int MapToEarFrame(int originalFrameIndex)
    {
        // Frames 0, 2, 4 use ear 1; others use ear 2
        return (originalFrameIndex == 0 || originalFrameIndex == 2 || originalFrameIndex == 4)
            ? EarFrame1
            : EarFrame2;
    }

    /// <summary>
    /// Ensures cat ears are hidden for the specified pawn.
    /// </summary>
    private static void EnsureCatEarHidden(Pawn pawn)
    {
        var catEarComp = pawn.GetComp<SylvieCatEarComp>();
        catEarComp?.SetShouldRender(false);
    }

    /// <summary>
    /// Handles cleanup for non-research animations.
    /// </summary>
    private static void HandleNonResearchAnimation(FaceAnimation animation)
    {
        if (SylvieAnimationRegistry.IsAnimationRegistered(animation))
        {
            Pawn? registeredPawn = SylvieAnimationRegistry.GetAssociatedPawn(animation);
            if (registeredPawn != null)
            {
                EnsureCatEarHidden(registeredPawn);
            }
        }
    }

    #endregion

    #region State Validation

    /// <summary>
    /// Checks if pawn is actually performing research.
    /// </summary>
    private static bool IsActuallyResearching(Pawn pawn)
    {
        // Check job definition
        if (pawn.CurJobDef?.defName != SylvieDefNames.Job_Research)
            return false;

        // Check job driver is active
        if (pawn.jobs?.curDriver == null)
            return false;

        // Verify job instance
        return pawn.CurJob?.def?.defName == SylvieDefNames.Job_Research;
    }

    #endregion
}

/// <summary>
/// Tracks research state and manages cat ear visibility timeout.
/// </summary>
[HarmonyPatch(typeof(FacialAnimationControllerComp), nameof(FacialAnimationControllerComp.CompTick))]
public static class Patch_FacialAnimationControllerComp_CompTick
{
    private static readonly Dictionary<Pawn, bool> WasResearchActive = new();
    private static readonly Dictionary<Pawn, int> LastResearchTick = new();
    private const int ResearchTimeoutTicks = 60;

    public static void Postfix(FacialAnimationControllerComp __instance)
    {
        Pawn? pawn = __instance.parent as Pawn;
        if (pawn == null) return;
        if (!SylvieDefNames.IsSylvieRace(pawn)) return;

        var catEarComp = pawn.GetComp<SylvieCatEarComp>();
        if (catEarComp == null) return;

        bool isResearchNow = IsActuallyResearching(pawn);
        WasResearchActive.TryGetValue(pawn, out bool wasResearchBefore);

        // Update last research tick
        if (isResearchNow)
        {
            LastResearchTick[pawn] = Find.TickManager.TicksGame;
        }

        // Handle transition from research to non-research
        if (wasResearchBefore && !isResearchNow)
        {
            catEarComp.SetShouldRender(false);
        }

        // Timeout check
        if (wasResearchBefore && !isResearchNow)
        {
            if (LastResearchTick.TryGetValue(pawn, out int lastTick))
            {
                int currentTick = Find.TickManager.TicksGame;
                if (currentTick - lastTick > ResearchTimeoutTicks)
                {
                    catEarComp.SetShouldRender(false);
                    WasResearchActive[pawn] = false;
                }
            }
        }

        WasResearchActive[pawn] = isResearchNow;
    }

    private static bool IsActuallyResearching(Pawn pawn)
    {
        if (pawn.CurJobDef?.defName != SylvieDefNames.Job_Research)
            return false;

        if (pawn.jobs?.curDriver == null)
            return false;

        return pawn.CurJob?.def?.defName == SylvieDefNames.Job_Research;
    }
}

/// <summary>
/// Registers research animations during controller initialization.
/// </summary>
[HarmonyPatch(typeof(FacialAnimationControllerComp), "InitializeIfNeed")]
public static class Patch_FacialAnimationControllerComp_InitializeIfNeed_Research
{
    public static void Postfix(
        FacialAnimationControllerComp __instance,
        Pawn ___pawn,
        Dictionary<string, List<FaceAnimation>> ___animationDict)
    {
        if (!SylvieDefNames.IsSylvieRace(___pawn)) return;

        SylvieAnimationRegistry.RegisterAnimationsByType(
            ___animationDict,
            ___pawn,
            SylvieDefNames.Animation_Research
        );
    }
}
