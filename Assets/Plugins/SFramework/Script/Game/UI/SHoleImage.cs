using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/*
 * 带空的图，用于做新手引导
 */
public class SHoleImage : Image {
    public override Material GetModifiedMaterial(Material baseMaterial) {
        var toUse = base.GetModifiedMaterial(baseMaterial);  //baseMaterial;

        if (m_ShouldRecalculateStencil) {
            var rootCanvas = MaskUtilities.FindRootSortOverrideCanvas(transform);
            m_StencilValue = maskable ? MaskUtilities.GetStencilDepth(transform, rootCanvas) : 0;
            m_ShouldRecalculateStencil = false;
        }

        // if we have a enabled Mask component then it will
        // generate the mask material. This is an optimisation
        // it adds some coupling between components though :(
        Mask maskComponent = GetComponent<Mask>();
        if (m_StencilValue > 0 && (maskComponent == null || !maskComponent.IsActive())) {
            var maskMat = StencilMaterial.Add(toUse, (1 << m_StencilValue) - 1, StencilOp.Keep, CompareFunction.NotEqual, ColorWriteMask.All, (1 << m_StencilValue) - 1, 0);
            StencilMaterial.Remove(m_MaskMaterial);
            m_MaskMaterial = maskMat;
            toUse = m_MaskMaterial;
        }
        return toUse;
    }

    public virtual bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera) {
        if (!isActiveAndEnabled)
            return true;
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, sp, eventCamera);
    }
}