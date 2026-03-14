#nullable enable
using Verse;
using RimWorld;
using UnityEngine;

namespace SylvieMod;

public class SylvieCatEarComp : ThingComp
{
    private Pawn? cachedPawn;
    private Graphic? catEar1Graphic;
    private Graphic? catEar2Graphic;
    
    // 0 = 猫耳1, 1 = 猫耳2
    private int currentEarFrame = 0;
    private bool shouldRender = false;
    
    public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;
    
    private static readonly Vector2 EarSize = new Vector2(1.5f, 1.5f);
    private const float EarLayer = 74f;
    
    private Graphic CatEar1Graphic
    {
        get
        {
            if (catEar1Graphic == null)
            {
                catEar1Graphic = GraphicDatabase.Get<Graphic_Multi>(
                    "Things/Pawn/Sylvie/CatEars/Normal/Unisex/catEar1",
                    ShaderDatabase.Transparent,
                    EarSize,
                    Color.white);
            }
            return catEar1Graphic;
        }
    }
    
    private Graphic CatEar2Graphic
    {
        get
        {
            if (catEar2Graphic == null)
            {
                catEar2Graphic = GraphicDatabase.Get<Graphic_Multi>(
                    "Things/Pawn/Sylvie/CatEars/Normal/Unisex/catEar2",
                    ShaderDatabase.Transparent,
                    EarSize,
                    Color.white);
            }
            return catEar2Graphic;
        }
    }
    
    /// <summary>
    /// 设置当前猫耳帧
    /// </summary>
    /// <param name="frameIndex">0=猫耳1, 1=猫耳2</param>
    public void SetCurrentEarFrame(int frameIndex)
    {
        currentEarFrame = frameIndex;
    }
    
    public void SetShouldRender(bool render)
    {
        shouldRender = render;
    }
    
    private Graphic GetCurrentEarGraphic()
    {
        // 0 = 猫耳1, 1 = 猫耳2
        return currentEarFrame == 0 ? CatEar1Graphic : CatEar2Graphic;
    }
    
    public override void PostDraw()
    {
        base.PostDraw();
        
        if (!shouldRender)
            return;
        
        if (!SylvieDefNames.IsSylvieRace(Pawn))
            return;
        
        Rot4 rot = Pawn.Rotation;
        
        float headSizeFactor = 1f;
        if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
        {
            headSizeFactor = Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
        }
        
        Vector3 headOffset = Pawn.Drawer.renderer.BaseHeadOffsetAt(rot);
        Vector3 drawPos = Pawn.DrawPos + headOffset;
        drawPos.y = Pawn.DrawPos.y + PawnRenderUtility.AltitudeForLayer((int)EarLayer);
        
        Vector3 drawScale = Vector3.one * headSizeFactor;
        
        Graphic earGraphic = GetCurrentEarGraphic();
        Material mat = earGraphic.MatAt(rot);
        Mesh mesh = earGraphic.MeshAt(rot);
        
        if (mat != null)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, drawScale);
            Graphics.DrawMesh(mesh, matrix, mat, 0);
        }
    }
}
