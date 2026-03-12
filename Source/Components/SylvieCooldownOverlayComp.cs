#nullable enable
using Verse;
using RimWorld;
using UnityEngine;

namespace SylvieMod;

public class SylvieCooldownOverlayComp : ThingComp
{
    private Pawn? cachedPawn;
    private Graphic[]? sweatGraphics;
    private Graphic? magazineGraphic;
    private Graphic[]? bulletInsertGraphics;
    private Graphic[]? bulletCountGraphics;
    
    private static readonly string[] SweatTextureNames = { "sweat1", "sweat2", "sweat3" };
    private static readonly string[] BulletInsertTextureNames = { "bullet_insert1", "bullet_insert2", "bullet_insert3" };
    private static readonly string[] BulletCountTextureNames = { "bullet1", "bullet2", "bullet3", "bullet4", "bullet5" };
    
    public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;
    
    private static readonly Vector2 OverlaySize = new Vector2(1.5f, 1.5f);
    private static readonly Vector3 DrawScale = new Vector3(1.5f, 1f, 1.5f);
    
    private Graphic[] SweatGraphics
    {
        get
        {
            if (sweatGraphics == null)
            {
                sweatGraphics = new Graphic[3];
                for (int i = 0; i < 3; i++)
                {
                    sweatGraphics[i] = GraphicDatabase.Get<Graphic_Multi>(
                        $"Things/Pawn/Sylvie/CooldownOverlay/Normal/Unisex/{SweatTextureNames[i]}",
                        ShaderDatabase.Transparent,
                        OverlaySize,
                        Color.white);
                }
            }
            return sweatGraphics;
        }
    }
    
    private Graphic MagazineGraphic
    {
        get
        {
            if (magazineGraphic == null)
            {
                magazineGraphic = GraphicDatabase.Get<Graphic_Multi>(
                    "Things/Pawn/Sylvie/CooldownOverlay/Normal/Unisex/magazine",
                    ShaderDatabase.Transparent,
                    OverlaySize,
                    Color.white);
            }
            return magazineGraphic;
        }
    }
    
    private Graphic[] BulletInsertGraphics
    {
        get
        {
            if (bulletInsertGraphics == null)
            {
                bulletInsertGraphics = new Graphic[3];
                for (int i = 0; i < 3; i++)
                {
                    bulletInsertGraphics[i] = GraphicDatabase.Get<Graphic_Multi>(
                        $"Things/Pawn/Sylvie/CooldownOverlay/Normal/Unisex/{BulletInsertTextureNames[i]}",
                        ShaderDatabase.Transparent,
                        OverlaySize,
                        Color.white);
                }
            }
            return bulletInsertGraphics;
        }
    }
    
    private Graphic[] BulletCountGraphics
    {
        get
        {
            if (bulletCountGraphics == null)
            {
                bulletCountGraphics = new Graphic[5];
                for (int i = 0; i < 5; i++)
                {
                    bulletCountGraphics[i] = GraphicDatabase.Get<Graphic_Multi>(
                        $"Things/Pawn/Sylvie/CooldownOverlay/Normal/Unisex/{BulletCountTextureNames[i]}",
                        ShaderDatabase.Transparent,
                        OverlaySize,
                        Color.white);
                }
            }
            return bulletCountGraphics;
        }
    }
    
    private static readonly Vector3[] FaceOffsets = new Vector3[]
    {
        new Vector3(0f, 1f, 0.33f),
        new Vector3(0f, 1f, 0.33f),
        new Vector3(0f, 1f, 0.33f),
        new Vector3(0f, 1f, 0.33f)
    };
    
    private Vector3 GetFaceDrawOffset()
    {
        Rot4 rot = Pawn.Rotation;
        return FaceOffsets[rot.AsInt];
    }
    
    public override void PostDraw()
    {
        base.PostDraw();
        
        if (Pawn.def.defName != "Sylvie_Race")
            return;
        
        var tracker = SylvieCooldownTracker.GetTracker(Pawn);
        if (tracker == null || !tracker.IsInRangedCooldown)
            return;
        
        Vector3 faceOffset = GetFaceDrawOffset();
        Vector3 drawPos = Pawn.DrawPos + faceOffset;
        drawPos.y += 0.01f;
        
        Rot4 rot = Pawn.Rotation;
        
        int sweatFrame = tracker.GetSweatFrame();
        if (sweatFrame >= 1 && sweatFrame <= 3)
        {
            Graphic sweatGraphic = SweatGraphics[sweatFrame - 1];
            Material mat = sweatGraphic.MatAt(rot);
            if (mat != null)
            {
                Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, DrawScale);
                Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0);
            }
        }
        
        Material magazineMat = MagazineGraphic.MatAt(rot);
        if (magazineMat != null)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, DrawScale);
            Graphics.DrawMesh(MeshPool.plane10, matrix, magazineMat, 0);
        }
        
        var (insertFrame, bulletCount) = tracker.GetBulletAnimationState();
        
        if (insertFrame >= 1 && insertFrame <= 3)
        {
            Graphic insertGraphic = BulletInsertGraphics[insertFrame - 1];
            Material insertMat = insertGraphic.MatAt(rot);
            if (insertMat != null)
            {
                Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, DrawScale);
                Graphics.DrawMesh(MeshPool.plane10, matrix, insertMat, 0);
            }
        }
        
        if (bulletCount >= 1 && bulletCount <= 5)
        {
            Graphic bulletGraphic = BulletCountGraphics[bulletCount - 1];
            Material bulletMat = bulletGraphic.MatAt(rot);
            if (bulletMat != null)
            {
                Matrix4x4 matrix = Matrix4x4.TRS(drawPos, Quaternion.identity, DrawScale);
                Graphics.DrawMesh(MeshPool.plane10, matrix, bulletMat, 0);
            }
        }
    }
}
