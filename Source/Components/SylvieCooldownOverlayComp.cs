#nullable enable
using Verse;
using RimWorld;
using UnityEngine;

namespace SylvieMod;

/// <summary>
/// Component that renders cooldown overlay graphics for Sylvie pawns.
/// Displays sweat, magazine, and bullet animations during ranged weapon cooldown.
/// </summary>
public class SylvieCooldownOverlayComp : ThingComp
{
    #region Constants

    private static readonly Vector2 OverlaySize = new Vector2(1.5f, 1.5f);
    private static readonly Vector3 DrawScale = Vector3.one;
    private const float SweatLayer = 61f;

    private static readonly string[] SweatTextureNames = { "sweat1", "sweat2", "sweat3" };
    private static readonly string[] BulletInsertTextureNames = { "bullet_insert1", "bullet_insert2", "bullet_insert3" };
    private static readonly string[] BulletCountTextureNames = { "bullet1", "bullet2", "bullet3", "bullet4", "bullet5" };

    #endregion

    #region Fields

    private Pawn? cachedPawn;
    private Graphic[]? sweatGraphics;
    private Graphic? magazineGraphic;
    private Graphic[]? bulletInsertGraphics;
    private Graphic[]? bulletCountGraphics;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the parent pawn, using cached value if available.
    /// </summary>
    public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;

    /// <summary>
    /// Gets the sweat graphics array, initializing if necessary.
    /// </summary>
    private Graphic[] SweatGraphics => sweatGraphics ??= InitializeGraphics(SweatTextureNames);

    /// <summary>
    /// Gets the magazine graphic, initializing if necessary.
    /// </summary>
    private Graphic MagazineGraphic => magazineGraphic ??= InitializeGraphic("magazine");

    /// <summary>
    /// Gets the bullet insert graphics array, initializing if necessary.
    /// </summary>
    private Graphic[] BulletInsertGraphics => bulletInsertGraphics ??= InitializeGraphics(BulletInsertTextureNames);

    /// <summary>
    /// Gets the bullet count graphics array, initializing if necessary.
    /// </summary>
    private Graphic[] BulletCountGraphics => bulletCountGraphics ??= InitializeGraphics(BulletCountTextureNames);

    #endregion

    #region Graphic Initialization

    /// <summary>
    /// Initializes a single graphic from the cooldown overlay texture path.
    /// </summary>
    /// <param name="textureName">The texture name without path prefix</param>
    /// <returns>The initialized graphic</returns>
    private Graphic InitializeGraphic(string textureName)
    {
        return GraphicDatabase.Get<Graphic_Multi>(
            $"Things/Pawn/Sylvie/CooldownOverlay/Normal/Unisex/{textureName}",
            ShaderDatabase.Transparent,
            OverlaySize,
            Color.white);
    }

    /// <summary>
    /// Initializes an array of graphics from texture names.
    /// </summary>
    /// <param name="textureNames">Array of texture names</param>
    /// <returns>Array of initialized graphics</returns>
    private Graphic[] InitializeGraphics(string[] textureNames)
    {
        var graphics = new Graphic[textureNames.Length];
        for (int i = 0; i < textureNames.Length; i++)
        {
            graphics[i] = InitializeGraphic(textureNames[i]);
        }
        return graphics;
    }

    #endregion

    #region Rendering

    /// <summary>
    /// Called after the pawn is drawn. Renders cooldown overlay if in ranged cooldown.
    /// </summary>
    public override void PostDraw()
    {
        base.PostDraw();

        // Only render for Sylvie race
        if (!SylvieDefNames.IsSylvieRace(Pawn))
            return;

        // Check if in cooldown
        var tracker = SylvieCooldownTracker.GetTracker(Pawn);
        if (tracker == null || !tracker.IsInRangedCooldown)
            return;

        // Get render parameters
        Rot4 rotation = Pawn.Rotation;
        Vector3 drawPosition = CalculateDrawPosition(rotation);
        Vector3 drawScale = CalculateDrawScale();

        // Render components
        RenderSweat(tracker, rotation, drawPosition, drawScale);
        RenderMagazine(rotation, drawPosition, drawScale);
        
        var (insertFrame, bulletCount) = tracker.GetBulletAnimationState();
        RenderBulletInsert(insertFrame, rotation, drawPosition, drawScale);
        RenderBulletCount(bulletCount, rotation, drawPosition, drawScale);
    }

    /// <summary>
    /// Calculates the draw position based on pawn position and head offset.
    /// </summary>
    private Vector3 CalculateDrawPosition(Rot4 rotation)
    {
        Vector3 headOffset = Pawn.Drawer.renderer.BaseHeadOffsetAt(rotation);
        Vector3 drawPos = Pawn.DrawPos + headOffset;
        drawPos.y += 0.01f;
        return drawPos;
    }

    /// <summary>
    /// Calculates the draw scale based on head size factor.
    /// </summary>
    private Vector3 CalculateDrawScale()
    {
        float headSizeFactor = GetHeadSizeFactor();
        return DrawScale * headSizeFactor;
    }

    /// <summary>
    /// Gets the head size factor from the pawn's life stage.
    /// </summary>
    private float GetHeadSizeFactor()
    {
        if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
        {
            return Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
        }
        return 1f;
    }

    #endregion

    #region Component Rendering

    /// <summary>
    /// Renders the sweat overlay based on cooldown progress.
    /// </summary>
    private void RenderSweat(SylvieCooldownTracker tracker, Rot4 rotation, Vector3 drawPosition, Vector3 drawScale)
    {
        // Skip sweat for north rotation (no north texture)
        if (rotation == Rot4.North)
            return;

        int sweatFrame = tracker.GetSweatFrame();
        if (sweatFrame < 1 || sweatFrame > 3)
            return;

        Graphic sweatGraphic = SweatGraphics[sweatFrame - 1];
        RenderGraphic(sweatGraphic, rotation, drawPosition, drawScale, SweatLayer);
    }

    /// <summary>
    /// Renders the magazine overlay.
    /// </summary>
    private void RenderMagazine(Rot4 rotation, Vector3 drawPosition, Vector3 drawScale)
    {
        RenderGraphic(MagazineGraphic, rotation, drawPosition, drawScale);
    }

    /// <summary>
    /// Renders the bullet insert animation frame.
    /// </summary>
    private void RenderBulletInsert(int insertFrame, Rot4 rotation, Vector3 drawPosition, Vector3 drawScale)
    {
        if (insertFrame < 1 || insertFrame > 3)
            return;

        Graphic insertGraphic = BulletInsertGraphics[insertFrame - 1];
        RenderGraphic(insertGraphic, rotation, drawPosition, drawScale);
    }

    /// <summary>
    /// Renders the bullet count indicator.
    /// </summary>
    private void RenderBulletCount(int bulletCount, Rot4 rotation, Vector3 drawPosition, Vector3 drawScale)
    {
        if (bulletCount < 1 || bulletCount > 5)
            return;

        Graphic bulletGraphic = BulletCountGraphics[bulletCount - 1];
        RenderGraphic(bulletGraphic, rotation, drawPosition, drawScale);
    }

    #endregion

    #region Rendering Utilities

    /// <summary>
    /// Renders a graphic at the specified position with the given rotation.
    /// </summary>
    private void RenderGraphic(Graphic graphic, Rot4 rotation, Vector3 position, Vector3 scale, float? layerOverride = null)
    {
        Material mat = graphic.MatAt(rotation);
        Mesh mesh = graphic.MeshAt(rotation);

        if (mat == null)
            return;

        Vector3 renderPos = position;
        if (layerOverride.HasValue)
        {
            renderPos.y = Pawn.DrawPos.y + PawnRenderUtility.AltitudeForLayer(layerOverride.Value);
        }

        Matrix4x4 matrix = Matrix4x4.TRS(renderPos, Quaternion.identity, scale);
        Graphics.DrawMesh(mesh, matrix, mat, 0);
    }

    #endregion
}
