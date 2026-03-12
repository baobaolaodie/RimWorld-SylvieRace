#nullable enable
using HarmonyLib;
using Verse;
using RimWorld;
using FacialAnimation;
using System.Collections.Generic;

namespace SylvieMod;

public class SylvieAimingTracker : ThingComp
{
    private Pawn? cachedPawn;
    
    public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;
    
    private static Dictionary<Pawn, SylvieAimingTracker> trackers = new Dictionary<Pawn, SylvieAimingTracker>();
    
    public static SylvieAimingTracker? GetTracker(Pawn pawn)
    {
        if (trackers.TryGetValue(pawn, out var tracker))
            return tracker;
        tracker = pawn.GetComp<SylvieAimingTracker>();
        if (tracker != null)
            trackers[pawn] = tracker;
        return tracker;
    }
}

[HarmonyPatch(typeof(Pawn), nameof(Pawn.SpawnSetup))]
public static class Patch_Pawn_SpawnSetup
{
    public static void Postfix(Pawn __instance)
    {
        if (__instance.def.defName == "Sylvie_Race")
        {
            if (__instance.GetComp<SylvieAimingTracker>() == null)
            {
                __instance.AllComps.Add(new SylvieAimingTracker
                {
                    parent = __instance
                });
            }
            if (__instance.GetComp<SylvieCooldownTracker>() == null)
            {
                __instance.AllComps.Add(new SylvieCooldownTracker
                {
                    parent = __instance
                });
            }
            if (__instance.GetComp<SylvieCooldownOverlayComp>() == null)
            {
                __instance.AllComps.Add(new SylvieCooldownOverlayComp
                {
                    parent = __instance
                });
            }
        }
    }
}

[HarmonyPatch(typeof(FaceAnimation), nameof(FaceAnimation.GetCurrentFrame))]
public static class Patch_FaceAnimation_GetCurrentFrame
{
    private static Dictionary<FaceAnimation, Pawn> animationToPawn = new Dictionary<FaceAnimation, Pawn>();
    private static FaceAnimationDef.AnimationFrame? cachedCooldownFrame;
    private static BrowShapeDef? confusedBrowDef;
    
    public static void RegisterAnimation(FaceAnimation animation, Pawn pawn)
    {
        animationToPawn[animation] = pawn;
    }
    
    private static BrowShapeDef ConfusedBrowDef
    {
        get
        {
            if (confusedBrowDef == null)
                confusedBrowDef = DefDatabase<BrowShapeDef>.GetNamedSilentFail("confused");
            return confusedBrowDef!;
        }
    }
    
    private static EyeballShapeDef? lookdownEyeballDef;
    
    private static EyeballShapeDef LookdownEyeballDef
    {
        get
        {
            if (lookdownEyeballDef == null)
                lookdownEyeballDef = DefDatabase<EyeballShapeDef>.GetNamedSilentFail("lookdown");
            return lookdownEyeballDef!;
        }
    }
    
    private static FaceAnimationDef.AnimationFrame GetCooldownFrame()
    {
        if (cachedCooldownFrame == null)
        {
            cachedCooldownFrame = new FaceAnimationDef.AnimationFrame
            {
                duration = 30,
                browShapeDef = ConfusedBrowDef,
                eyeballShapeDef = LookdownEyeballDef
            };
        }
        return cachedCooldownFrame;
    }
    
    public static bool Prefix(FaceAnimation __instance, int tickGame, ref FaceAnimationDef.AnimationFrame? __result)
    {
        if (__instance.animationDef.defName != "Sylvie_AimingAnimation") return true;
        
        if (!animationToPawn.TryGetValue(__instance, out var pawn))
            return true;
        
        var frames = __instance.animationDef.GetSequentialAnimationFrames();
        if (frames == null || frames.Count == 0) return true;
        
        Stance? curStance = pawn.stances?.curStance;
        
        if (curStance is Stance_Warmup warmup)
        {
            Verb? verb = warmup.verb;
            if (verb == null) return true;
            
            float warmupTime = verb.verbProps.warmupTime;
            float aimingDelayFactor = pawn.GetStatValue(StatDefOf.AimingDelayFactor);
            int totalWarmupTicks = (warmupTime * aimingDelayFactor).SecondsToTicks();
            
            if (totalWarmupTicks <= 0) return true;
            
            int ticksLeft = warmup.ticksLeft;
            int elapsedTicks = totalWarmupTicks - ticksLeft;
            
            int totalFrames = frames.Count;
            float progress = (float)elapsedTicks / totalWarmupTicks;
            int frameIndex = (int)(progress * totalFrames);
            
            if (frameIndex >= totalFrames)
                frameIndex = totalFrames - 1;
            if (frameIndex < 0)
                frameIndex = 0;
            
            __result = frames[frameIndex];
            return false;
        }
        else if (curStance is Stance_Cooldown cooldown)
        {
            Verb? verb = cooldown.verb;
            if (verb != null && verb.state == VerbState.Bursting)
            {
                __result = frames[frames.Count - 1];
                return false;
            }
            
            __result = GetCooldownFrame();
            return false;
        }
        
        return true;
    }
}

[HarmonyPatch(typeof(FacialAnimationControllerComp), "InitializeIfNeed")]
public static class Patch_FacialAnimationControllerComp_InitializeIfNeed
{
    public static void Postfix(FacialAnimationControllerComp __instance, Pawn ___pawn, Dictionary<string, List<FaceAnimation>> ___animationDict)
    {
        if (___pawn == null || ___pawn.def.defName != "Sylvie_Race") return;
        
        foreach (var kvp in ___animationDict)
        {
            foreach (var animation in kvp.Value)
            {
                if (animation.animationDef.defName == "Sylvie_AimingAnimation")
                {
                    Patch_FaceAnimation_GetCurrentFrame.RegisterAnimation(animation, ___pawn);
                }
            }
        }
    }
}
