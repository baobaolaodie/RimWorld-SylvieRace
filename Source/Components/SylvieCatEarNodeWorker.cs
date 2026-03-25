#nullable enable
using UnityEngine;
using Verse;

namespace SylvieMod;

/// <summary>
/// Worker class for Sylvie cat ear render node.
/// Handles offset calculation and custom rendering logic.
/// Inheriting from PawnRenderNodeWorker allows the node to automatically
/// inherit transformations from its parent (Head node).
/// </summary>
public class SylvieCatEarNodeWorker : PawnRenderNodeWorker
{
    /// <summary>
    /// Determines whether this node can be drawn now.
    /// Returns false when rendering is disabled (e.g., not in research animation).
    /// </summary>
    /// <param name="node">The render node</param>
    /// <param name="parms">Draw parameters</param>
    /// <returns>True if the node should be drawn, false otherwise</returns>
    public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
    {
        // Check base conditions first
        if (!base.CanDrawNow(node, parms))
        {
            return false;
        }

        // Check if this is our cat ear node
        if (node is SylvieCatEarRenderNode catEarNode)
        {
            // Get the ear comp to check if rendering is enabled
            // Directly access the comp from the pawn
            var comp = parms.pawn.GetComp<SylvieCatEarComp>();
            if (comp == null || !comp.ShouldRender)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Calculates the offset for the cat ear relative to its parent (Head).
    /// This offset is applied on top of the parent's transform matrix,
    /// ensuring the cat ear maintains correct relative position during animations.
    /// </summary>
    /// <param name="node">The render node</param>
    /// <param name="parms">Draw parameters</param>
    /// <param name="pivot">Output pivot point</param>
    /// <returns>The offset vector</returns>
    public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
    {
        // Get the base offset from the parent implementation
        Vector3 offset = base.OffsetFor(node, parms, out pivot);

        // No additional offset needed - the cat ear graphics are positioned correctly
        // relative to the head by default. The parent Head node's transform matrix
        // (which includes Yayo's Animation offsets) is automatically applied.

        return offset;
    }

    /// <summary>
    /// Appends draw requests for this node.
    /// Override to customize how the cat ear is drawn.
    /// </summary>
    /// <param name="node">The render node</param>
    /// <param name="parms">Draw parameters</param>
    /// <param name="requests">List to append draw requests to</param>
    public override void AppendDrawRequests(PawnRenderNode node, PawnDrawParms parms, System.Collections.Generic.List<PawnGraphicDrawRequest> requests)
    {
        // CanDrawNow check is already performed by AppendRequests before calling this method
        // But we double-check here for safety
        if (!CanDrawNow(node, parms))
        {
            return;
        }

        // Get the material for the current frame
        Material? mat = GetFinalizedMaterial(node, parms);

        // If no valid material, don't add request
        if (mat == null)
        {
            return;
        }

        // Get the mesh for the current facing
        Mesh? mesh = node.GetMesh(parms);

        // If no valid mesh, don't add request
        if (mesh == null)
        {
            return;
        }

        // Add the draw request
        // The PawnRenderNode system will automatically apply the parent Head node's
        // transform matrix (including position, rotation, and any animation offsets)
        requests.Add(new PawnGraphicDrawRequest(node, mesh, mat));
    }

    /// <summary>
    /// Gets the finalized material for the cat ear.
    /// </summary>
    /// <param name="node">The render node</param>
    /// <param name="parms">Draw parameters</param>
    /// <returns>The material for the current frame and facing</returns>
    public override Material? GetFinalizedMaterial(PawnRenderNode node, PawnDrawParms parms)
    {
        // Get the graphic for the current frame
        Graphic graphic = node.GraphicFor(parms.pawn);

        // Get the material for the current facing
        return graphic.MatAt(parms.facing);
    }
}
