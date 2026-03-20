#nullable enable
using Verse;
using RimWorld;
using UnityEngine;

namespace SylvieMod;

/// <summary>
/// Component that renders cat ear graphics for Sylvie pawns during research animation.
/// Synchronizes ear animation frames with facial animation states.
/// </summary>
public class SylvieCatEarComp : ThingComp
{
    #region Constants

    /// <summary>
    /// Size of the cat ear graphic.
    /// </summary>
    private static readonly Vector2 EarSize = new Vector2(1.5f, 1.5f);

    /// <summary>
    /// Render layer for cat ears (between hair and helmet).
    /// </summary>
    private const float EarLayer = 74f;

    /// <summary>
    /// Frame index for cat ear 1.
    /// </summary>
    private const int EarFrame1 = 0;

    /// <summary>
    /// Frame index for cat ear 2.
    /// </summary>
    private const int EarFrame2 = 1;

    #endregion

    #region Fields

    private Pawn? cachedPawn;
    private Graphic? catEar1Graphic;
    private Graphic? catEar2Graphic;

    /// <summary>
    /// Current ear frame index (0 = ear 1, 1 = ear 2).
    /// </summary>
    private int currentEarFrame = 0;

    /// <summary>
    /// Whether the cat ears should be rendered.
    /// </summary>
    private bool shouldRender = false;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the parent pawn, using cached value if available.
    /// </summary>
    public Pawn Pawn => cachedPawn ??= (parent as Pawn)!;

    /// <summary>
    /// Gets the first cat ear graphic, initializing if necessary.
    /// </summary>
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

    /// <summary>
    /// Gets the second cat ear graphic, initializing if necessary.
    /// </summary>
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

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets the current cat ear frame.
    /// </summary>
    /// <param name="frameIndex">Frame index (0 = ear 1, 1 = ear 2)</param>
    public void SetCurrentEarFrame(int frameIndex)
    {
        currentEarFrame = frameIndex;
    }

    /// <summary>
    /// Sets whether the cat ears should be rendered.
    /// </summary>
    /// <param name="render">True to render, false to hide</param>
    public void SetShouldRender(bool render)
    {
        shouldRender = render;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Gets the current ear graphic based on frame index.
    /// </summary>
    /// <returns>The graphic for the current frame</returns>
    private Graphic GetCurrentEarGraphic()
    {
        return currentEarFrame == EarFrame1 ? CatEar1Graphic : CatEar2Graphic;
    }

    /// <summary>
    /// Calculates the head size factor from the pawn's life stage.
    /// </summary>
    /// <returns>Head size factor (1.0 if not available)</returns>
    private float GetHeadSizeFactor()
    {
        if (ModsConfig.BiotechActive && Pawn.ageTracker.CurLifeStage.headSizeFactor.HasValue)
        {
            return Pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
        }
        return 1f;
    }

    #endregion

    #region Rendering

    /// <summary>
    /// Called after the pawn is drawn. Renders cat ears if enabled.
    /// </summary>
    public override void PostDraw()
    {
        base.PostDraw();

        // Skip if rendering is disabled
        if (!shouldRender)
            return;

        // Only render for Sylvie race
        if (!SylvieDefNames.IsSylvieRace(Pawn))
            return;

        Rot4 rot = Pawn.Rotation;

        float headSizeFactor = GetHeadSizeFactor();

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

    #endregion
}
