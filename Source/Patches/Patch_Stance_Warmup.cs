#nullable enable
using HarmonyLib;
using Verse;
using RimWorld;
using FacialAnimation;
using System.Collections.Generic;

namespace SylvieMod;

#region Component Registration

/// <summary>
/// Registers Sylvie-specific components when a pawn spawns.
/// </summary>
[HarmonyPatch(typeof(Pawn), nameof(Pawn.SpawnSetup))]
public static class Patch_Pawn_SpawnSetup
{
    /// <summary>
    /// Postfix to register Sylvie components after pawn spawn.
    /// Only processes Sylvie race pawns to avoid unnecessary checks.
    /// </summary>
    public static void Postfix(Pawn __instance)
    {
        // 只处理 Sylvie 种族的 Pawn，避免对其他种族进行不必要的组件注册检查
        if (!SylvieDefNames.IsSylvieRace(__instance))
            return;

        SylvieComponentRegistry.RegisterAllComponents(__instance);
    }
}

#endregion

#region Aiming Animation Patch

/// <summary>
/// Patches FaceAnimation.GetCurrentFrame to provide custom aiming animation frames.
/// Synchronizes animation with weapon warmup and cooldown states.
/// </summary>
[HarmonyPatch(typeof(FaceAnimation), nameof(FaceAnimation.GetCurrentFrame))]
public static class Patch_FaceAnimation_GetCurrentFrame
{
    #region Cached Defs

    private static FaceAnimationDef.AnimationFrame? CachedCooldownFrame;
    private static BrowShapeDef? CachedConfusedBrowDef;
    private static EyeballShapeDef? CachedLookdownEyeballDef;

    #endregion

    #region Cached Def Properties

    private static BrowShapeDef ConfusedBrowDef
    {
        get
        {
            if (CachedConfusedBrowDef == null)
                CachedConfusedBrowDef = DefDatabase<BrowShapeDef>.GetNamedSilentFail("confused");
            return CachedConfusedBrowDef!;
        }
    }

    private static EyeballShapeDef LookdownEyeballDef
    {
        get
        {
            if (CachedLookdownEyeballDef == null)
                CachedLookdownEyeballDef = DefDatabase<EyeballShapeDef>.GetNamedSilentFail("lookdown");
            return CachedLookdownEyeballDef!;
        }
    }

    #endregion

    #region Frame Generation

    /// <summary>
    /// Gets or creates the cached cooldown frame.
    /// </summary>
    private static FaceAnimationDef.AnimationFrame GetCooldownFrame()
    {
        if (CachedCooldownFrame == null)
        {
            CachedCooldownFrame = new FaceAnimationDef.AnimationFrame
            {
                duration = SylvieConstants.DefaultAnimationDuration,
                browShapeDef = ConfusedBrowDef,
                eyeballShapeDef = LookdownEyeballDef
            };
        }
        return CachedCooldownFrame;
    }

    #endregion

    #region Patch Implementation

    /// <summary>
    /// Prefix patch to override animation frame calculation for aiming animations.
    /// </summary>
    public static bool Prefix(FaceAnimation __instance, int tickGame, ref FaceAnimationDef.AnimationFrame? __result)
    {
        // Early exit if not aiming animation
        if (__instance.animationDef.defName != SylvieDefNames.Animation_Aiming)
            return true;

        // Get associated pawn
        Pawn? pawn = SylvieAnimationRegistry.GetAssociatedPawn(__instance);
        if (pawn == null)
            return true;

        // Get animation frames
        var frames = __instance.animationDef.GetSequentialAnimationFrames();
        if (frames == null || frames.Count == 0)
            return true;

        // Get current stance
        Stance? curStance = pawn.stances?.curStance;

        // Handle warmup phase
        if (curStance is Stance_Warmup warmup)
        {
            __result = CalculateWarmupFrame(warmup, frames, pawn);
            return false;
        }

        // Handle cooldown phase
        if (curStance is Stance_Cooldown cooldown)
        {
            __result = CalculateCooldownFrame(cooldown, frames);
            return false;
        }

        // Default: use original method
        return true;
    }

    /// <summary>
    /// Calculates the animation frame during weapon warmup.
    /// </summary>
    private static FaceAnimationDef.AnimationFrame? CalculateWarmupFrame(
        Stance_Warmup warmup,
        List<FaceAnimationDef.AnimationFrame> frames,
        Pawn pawn)
    {
        Verb? verb = warmup.verb;
        if (verb == null)
            return null;

        float warmupTime = verb.verbProps.warmupTime;
        float aimingDelayFactor = pawn.GetStatValue(StatDefOf.AimingDelayFactor);
        int totalWarmupTicks = (warmupTime * aimingDelayFactor).SecondsToTicks();

        if (totalWarmupTicks <= 0)
            return null;

        int frameIndex = CalculateFrameIndexFromProgress(
            warmup.ticksLeft,
            totalWarmupTicks,
            frames.Count
        );

        return frames[frameIndex];
    }

    /// <summary>
    /// Calculates the animation frame during weapon cooldown.
    /// </summary>
    private static FaceAnimationDef.AnimationFrame? CalculateCooldownFrame(
        Stance_Cooldown cooldown,
        List<FaceAnimationDef.AnimationFrame> frames)
    {
        Verb? verb = cooldown.verb;

        // During burst firing, show last frame
        if (verb != null && verb.state == VerbState.Bursting)
        {
            return frames[frames.Count - 1];
        }

        // During cooldown, show confused expression
        return GetCooldownFrame();
    }

    /// <summary>
    /// Calculates frame index based on progress through the animation.
    /// </summary>
    private static int CalculateFrameIndexFromProgress(int ticksLeft, int totalTicks, int frameCount)
    {
        int elapsedTicks = totalTicks - ticksLeft;
        float progress = (float)elapsedTicks / totalTicks;
        int frameIndex = (int)(progress * frameCount);

        // Clamp to valid range
        if (frameIndex < 0) frameIndex = 0;
        if (frameIndex >= frameCount) frameIndex = frameCount - 1;
        return frameIndex;
    }

    #endregion
}

#endregion

#region Animation Registration

/// <summary>
/// Registers Sylvie aiming animations when the facial animation controller initializes.
/// </summary>
[HarmonyPatch(typeof(FacialAnimationControllerComp), "InitializeIfNeed")]
public static class Patch_FacialAnimationControllerComp_InitializeIfNeed
{
    /// <summary>
    /// Postfix to register animations after initialization.
    /// </summary>
    public static void Postfix(
        FacialAnimationControllerComp __instance,
        Pawn ___pawn,
        Dictionary<string, List<FaceAnimation>> ___animationDict)
    {
        // Only process Sylvie race
        if (!SylvieDefNames.IsSylvieRace(___pawn))
            return;

        // Register aiming animations
        SylvieAnimationRegistry.RegisterAnimationsByType(
            ___animationDict,
            ___pawn,
            SylvieDefNames.Animation_Aiming
        );
    }
}

#endregion
