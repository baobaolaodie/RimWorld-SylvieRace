#nullable enable
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace SylvieMod;

public class SylvieCooldownTracker : ThingComp
{
    private Pawn? cachedPawn;
    private int cooldownStartTick = -1;
    private int totalCooldownTicks = 0;
    private Verb? lastVerb;
    
    public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;
    
    private static Dictionary<Pawn, SylvieCooldownTracker> trackers = new Dictionary<Pawn, SylvieCooldownTracker>();
    
    public static SylvieCooldownTracker? GetTracker(Pawn pawn)
    {
        if (trackers.TryGetValue(pawn, out var tracker))
            return tracker;
        tracker = pawn.GetComp<SylvieCooldownTracker>();
        if (tracker != null)
            trackers[pawn] = tracker;
        return tracker;
    }
    
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
    
    public int GetSweatFrame()
    {
        float progress = CooldownProgress;
        if (progress < 0.33f) return 1;
        if (progress < 0.66f) return 2;
        return 3;
    }
    
    public (int insertFrame, int bulletCount) GetBulletAnimationState()
    {
        float progress = CooldownProgress;
        
        int cycle = (int)(progress * 5);
        if (cycle > 4) cycle = 4;
        
        float cycleProgress = (progress * 5) - cycle;
        
        int insertFrame;
        if (cycleProgress < 0.25f) insertFrame = 1;
        else if (cycleProgress < 0.5f) insertFrame = 2;
        else if (cycleProgress < 0.75f) insertFrame = 3;
        else insertFrame = 0;
        
        int bulletCount = cycle + 1;
        if (bulletCount > 5) bulletCount = 5;
        
        return (insertFrame, bulletCount);
    }
    
    public override void CompTick()
    {
        base.CompTick();
        
        if (!IsInRangedCooldown)
        {
            cooldownStartTick = -1;
            lastVerb = null;
        }
    }
}
