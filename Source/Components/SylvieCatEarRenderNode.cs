#nullable enable
using System;
using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace SylvieMod;

/// <summary>
/// Render node for Sylvie cat ears.
/// Inherits from PawnRenderNode to integrate with the native rendering system,
/// ensuring perfect synchronization with animation mods like Yayo's Animation.
/// </summary>
public class SylvieCatEarRenderNode : PawnRenderNode
{
    /// <summary>
    /// Reference to the parent component for accessing frame state.
    /// </summary>
    private readonly SylvieCatEarComp? earComp;

    /// <summary>
    /// Size of the cat ear graphic.
    /// </summary>
    private static readonly Vector2 EarSize = new Vector2(1.5f, 1.5f);

    /// <summary>
    /// Cached graphics for each frame.
    /// </summary>
    private Graphic? catEar1Graphic;
    private Graphic? catEar2Graphic;

    /// <summary>
    /// Last frame index for tracking changes.
    /// </summary>
    private int lastFrameIndex = -1;

    /// <summary>
    /// Creates a new cat ear render node.
    /// </summary>
    /// <param name="pawn">The pawn this node belongs to</param>
    /// <param name="props">Render node properties</param>
    /// <param name="tree">The render tree</param>
    /// <param name="comp">The parent component for frame state</param>
    public SylvieCatEarRenderNode(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree, SylvieCatEarComp comp)
        : base(pawn, props, tree)
    {
        earComp = comp;
    }

    /// <summary>
    /// Override to enable material check every draw request when we have multiple graphics.
    /// This allows dynamic frame switching.
    /// </summary>
    protected override bool EnsureInitializationWithoutRecache => true;

    /// <summary>
    /// Returns all graphics for this node (both ear frames).
    /// This ensures both graphics are initialized and available.
    /// </summary>
    protected override IEnumerable<Graphic> GraphicsFor(Pawn pawn)
    {
        // Return both graphics so they are both initialized
        yield return GetCatEar1Graphic();
        yield return GetCatEar2Graphic();
    }

    /// <summary>
    /// Gets the graphic for the current frame.
    /// Called by the rendering system to determine what to draw.
    /// Always returns a valid graphic to avoid null reference exceptions.
    /// </summary>
    /// <param name="pawn">The pawn being rendered</param>
    /// <returns>The graphic for the current ear frame</returns>
    public override Graphic GraphicFor(Pawn pawn)
    {
        // Always return a valid graphic to avoid null reference exceptions
        // The actual visibility is controlled by CanDrawNow in the worker
        int frameIndex = earComp?.CurrentEarFrame ?? 0;
        
        // Track frame changes to trigger recache when needed
        if (frameIndex != lastFrameIndex)
        {
            lastFrameIndex = frameIndex;
            // Request recache to ensure the correct graphic is used
            RequestRecache();
        }
        
        return frameIndex == 0 ? GetCatEar1Graphic() : GetCatEar2Graphic();
    }

    /// <summary>
    /// Requests a recache of this node.
    /// This ensures the render tree re-evaluates which graphic to use.
    /// </summary>
    private void RequestRecache()
    {
        try
        {
            // Set the requestRecache field to true to trigger re-initialization
            // This field is checked in EnsureInitialized via RecacheRequested property
            var field = typeof(PawnRenderNode).GetField("requestRecache", 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance);
            field?.SetValue(this, true);
        }
        catch (Exception ex)
        {
            Log.Warning($"[SylvieMod] Failed to request recache for cat ear node: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets the first cat ear graphic, initializing if necessary.
    /// </summary>
    private Graphic GetCatEar1Graphic()
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

    /// <summary>
    /// Gets the second cat ear graphic, initializing if necessary.
    /// </summary>
    private Graphic GetCatEar2Graphic()
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
