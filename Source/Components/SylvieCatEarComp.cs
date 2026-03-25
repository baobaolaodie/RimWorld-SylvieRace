#nullable enable
using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;

namespace SylvieMod;

/// <summary>
/// Component that renders cat ear graphics for Sylvie pawns during research animation.
/// Uses PawnRenderNode system for perfect synchronization with animation mods like Yayo's Animation.
/// </summary>
public class SylvieCatEarComp : ThingComp
{
    #region Constants

    /// <summary>
    /// Render layer for cat ears (between hair and helmet).
    /// </summary>
    private const float EarLayer = SylvieConstants.CatEarRenderLayer;

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
    /// Gets the current ear frame index.
    /// </summary>
    public int CurrentEarFrame => currentEarFrame;

    /// <summary>
    /// Gets whether the cat ears should be rendered.
    /// </summary>
    public bool ShouldRender => shouldRender;

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

    /// <summary>
    /// Returns render nodes for the cat ears.
    /// This integrates with RimWorld's PawnRenderNode system, ensuring perfect
    /// synchronization with animation mods like Yayo's Animation.
    /// </summary>
    /// <returns>List of render nodes, or null if not applicable</returns>
    public override List<PawnRenderNode>? CompRenderNodes()
    {
        // Only create nodes for Sylvie race
        if (!SylvieDefNames.IsSylvieRace(Pawn))
        {
            return null;
        }

        // Check if render tree is available
        if (Pawn.Drawer?.renderer?.renderTree == null)
        {
            return null;
        }

        // Create the render node with properties
        var props = new PawnRenderNodeProperties
        {
            debugLabel = "SylvieCatEar",
            workerClass = typeof(SylvieCatEarNodeWorker),
            baseLayer = EarLayer,
            parentTagDef = PawnRenderNodeTagDefOf.Head
        };

        var node = new SylvieCatEarRenderNode(Pawn, props, Pawn.Drawer.renderer.renderTree, this);

        return new List<PawnRenderNode> { node };
    }

    #endregion

    #region Save Compatibility

    /// <summary>
    /// 序列化/反序列化组件数据。
    /// </summary>
    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref currentEarFrame, "currentEarFrame", 0);
        Scribe_Values.Look(ref shouldRender, "shouldRender", false);
    }

    #endregion
}
