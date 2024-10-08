using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class ParticleShaderGUI : ShaderGUI
{
    public enum PBlendMode
    {
        Additive,
        AlphaBlend,
        Brighter,
        Alpha,
    }

    public enum PRenderMode
    {
        Normal,
        UVAnimation,
        UV2Animation,
        FrameAnimation,
        UVRotation,
    }

    public enum LookAtType
    {
        LookCameraTypeA,
        LookCameraTypeB,
        Off,
    }

    static readonly string[] renderModeKeywords = { "UV_ANIM", "UV2_ANIM", "FRAME_ANIM", "UV_ROT_ANIM" };

    private static class Styles
    {
        public static GUIStyle optionsButton = "PaneOptions";
        public static string emptyTootip = "";
        public static GUIContent mainTexText = new GUIContent("MainTex", "Main Texture (RGBA)");
        public static GUIContent maskTexText = new GUIContent("MaskTex", "Mask Texture (RGBA)");
        public static GUIContent dissolveTexText = new GUIContent("DissolveTex", "Dissolve Texture (A)");

        public static string whiteSpaceString = " ";
        public static string blendingMode = "Blending Mode";
        public static string renderingMode = "Rendering Mode";
        public static string cullMode = "Cull Mode";
        public static string planeType = "Look At Camera Type";
        public static readonly string[] blendNames = System.Enum.GetNames(typeof(PBlendMode));
        public static readonly string[] renderNames = System.Enum.GetNames(typeof(PRenderMode));
        public static readonly string[] cullNames = System.Enum.GetNames(typeof(CullMode));
        public static readonly string[] planeNames = System.Enum.GetNames(typeof(LookAtType));
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        Material material = materialEditor.target as Material;
        MaterialProperty blendMode = FindProperty("_BlendMode", properties);
        MaterialProperty renderMode = FindProperty("_RenderMode", properties);
        MaterialProperty mainTex = FindProperty("_MainTex", properties);
        MaterialProperty tintColor = FindProperty("_TintColor", properties);
        MaterialProperty maskTex = FindProperty("_MaskTex", properties);
        MaterialProperty dissolveTex = FindProperty("_DissolveTex", properties);
        MaterialProperty dissolve = FindProperty("_Dissolve", properties);
        MaterialProperty dissolveEdge = FindProperty("_DissolveEdge", properties);
        MaterialProperty edgeColor = FindProperty("_EdgeColor", properties);
        MaterialProperty offset = FindProperty("_QueueOffset", properties);
        MaterialProperty texTiles = FindProperty("_NumTexTiles", properties);
        MaterialProperty fps = FindProperty("_ReplaySpeed", properties);
        MaterialProperty scroll = FindProperty("_Scrolls", properties);
        MaterialProperty tiling = FindProperty("_Tiling", properties);
        MaterialProperty Flag = FindProperty("_Flag", properties);
        MaterialProperty colorFactor = FindProperty("_ColorFactor", properties);
        MaterialProperty textureColorFactor = FindProperty("_TextureColorFactor", properties);
        MaterialProperty selfAlpha = FindProperty("_SelfAlpha", properties);
        EditorGUI.BeginChangeCheck();
        {
            EditorGUI.showMixedValue = blendMode.hasMixedValue;
            PBlendMode mode = (PBlendMode)blendMode.floatValue;
            EditorGUI.BeginChangeCheck();
            mode = (PBlendMode)EditorGUILayout.Popup(Styles.blendingMode, (int)mode, Styles.blendNames);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo("Blending Mode");
                blendMode.floatValue = (float)mode;
            }

            int rm = 0;
            for (int i = 0; i < renderModeKeywords.Length; ++i)
            {
                if (material.IsKeywordEnabled(renderModeKeywords[i]))
                    rm = i + 1;
            }
            EditorGUI.BeginChangeCheck();
            rm = EditorGUILayout.Popup(Styles.renderingMode, rm, Styles.renderNames);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo("Rendering Mode");
                for (int i = 0; i < materialEditor.targets.Length; ++i)
                {
                    Material m = materialEditor.targets[i] as Material;
                    for (int k = 0; k < renderModeKeywords.Length; ++k)
                    {
                        if (rm == k + 1)
                            m.EnableKeyword(renderModeKeywords[k]);
                        else
                            m.DisableKeyword(renderModeKeywords[k]);
                    }
                }
            }

            EditorGUI.BeginChangeCheck();
            int cullMode = EditorGUILayout.Popup(Styles.cullMode, material.GetInt("_Cull"), Styles.cullNames);
            if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < materialEditor.targets.Length; ++i)
                {
                    Material m = materialEditor.targets[i] as Material;
                    m.SetInt("_Cull", cullMode);
                }
            }

            EditorGUI.BeginChangeCheck();
            bool zwrite = EditorGUILayout.Toggle("Z Write", material.GetInt("_ZWrite") == 1);
            if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < materialEditor.targets.Length; ++i)
                {
                    Material m = materialEditor.targets[i] as Material;
                    m.SetInt("_ZWrite", zwrite ? 1 : 0);
                }
            }
            EditorGUI.showMixedValue = false;

            EditorGUI.BeginChangeCheck();
            bool closer = EditorGUILayout.Toggle("At Late Queue", offset.floatValue > 0.0);
            if (EditorGUI.EndChangeCheck())
            {
                offset.floatValue = closer ? 1f : 0f;
            }
            if (closer)
                EditorGUILayout.HelpBox("The Render Queue now is Transparent+1 !", MessageType.Info);

            materialEditor.RangeProperty(selfAlpha, "SelfAlpha");
            materialEditor.RangeProperty(colorFactor, "ColorFactor");
            materialEditor.RangeProperty(textureColorFactor, "TextureColorFactor");
            materialEditor.ColorProperty(tintColor, "Tint Color");
            materialEditor.TextureProperty(mainTex, "Main Texture", true);
            materialEditor.TextureProperty(maskTex, "Mask Texture", true);
            materialEditor.TextureProperty(dissolveTex, "DissolveTex", true);
            if (dissolveTex.textureValue != null)
            {
                materialEditor.RangeProperty(dissolve, "Dissolve");
                materialEditor.RangeProperty(dissolveEdge, "Dissolve Edge Size");
                materialEditor.ColorProperty(edgeColor, "Dissolve Edge Color");
            }

            if (rm == (int)PRenderMode.FrameAnimation || rm == (int)PRenderMode.UVRotation)
            {
                materialEditor.ShaderProperty(texTiles, "Number of Tiles (XY)");
                materialEditor.ShaderProperty(fps, "Frames per Second");
            }
            else if (rm == (int)PRenderMode.UVAnimation || rm == (int)PRenderMode.UV2Animation)
            {
                materialEditor.ShaderProperty(scroll, "Scroll Speed (XY), Offset(ZW)");
                materialEditor.ShaderProperty(tiling, "Tiling (XY)");
            }
            materialEditor.ShaderProperty(Flag, "是否开启色变");
        }
        if (EditorGUI.EndChangeCheck())
        {
            for (int i = 0; i < blendMode.targets.Length; ++i)
                MaterialChange(blendMode.targets[i] as Material);
        }
        materialEditor.RenderQueueField();
    }

    public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
    {
        base.AssignNewShaderToMaterial(material, oldShader, newShader);
        if (Application.isPlaying)
            return;
        for (int i = 0; i < material.shaderKeywords.Length; ++i)
            material.DisableKeyword(material.shaderKeywords[i]);
        material.renderQueue = -1;
        if (oldShader == null)
            return;

        if (oldShader.name.Contains("Artist/Dissolve/") || oldShader.name.Contains("Artist/Particle/"))
        {
            PBlendMode blendMode = PBlendMode.Additive;
            if (oldShader.name.Contains("Alpha Blended"))
                blendMode = PBlendMode.AlphaBlend;
            else if (oldShader.name.Contains("Brighter"))
                blendMode = PBlendMode.Brighter;
            material.SetFloat("_BlendMode", (float)blendMode);

            PRenderMode renderMode = PRenderMode.Normal;
            if (oldShader.name.Contains("Frame Animation"))
                renderMode = PRenderMode.FrameAnimation;
            else if (oldShader.name.Contains("UV Animation"))
                renderMode = PRenderMode.UVAnimation;
            material.SetFloat("_RenderMode", (float)renderMode);

            if (oldShader.name.Contains("+1"))
                material.SetFloat("_QueueOffset", 1.0f);
            else
                material.SetFloat("_QueueOffset", 0.0f);

            if (!oldShader.name.Contains("Mask"))
                material.SetTexture("_MaskTex", null);

            if (!oldShader.name.Contains("Dissolve"))
                material.SetTexture("_DissolveTex", null);
        }

        MaterialChange(material);
    }

    void MaterialChange(Material material)
    {
        if (material.GetTexture("_MaskTex"))
            material.EnableKeyword("_USE_MASK");
        else
            material.DisableKeyword("_USE_MASK");

        if (material.GetTexture("_DissolveTex"))
        {
            if (material.GetFloat("_DissolveEdge") > 0f)
            {
                material.EnableKeyword("_DISSOLVE_RIM");
                material.DisableKeyword("_DISSOLVE");
            }
            else
            {
                material.EnableKeyword("_DISSOLVE");
                material.DisableKeyword("_DISSOLVE_RIM");
            }
        }
        else
        {
            material.DisableKeyword("_DISSOLVE");
            material.DisableKeyword("_DISSOLVE_RIM");
        }

        if (!Application.isPlaying)
            material.renderQueue = material.GetInt("_QueueOffset") == 0 ? -1 : 3001;

        SetBlendMode(material, (PBlendMode)material.GetFloat("_BlendMode"));
    }

    void SetBlendMode(Material material, PBlendMode mode)
    {
        // 默认禁用SelfAlpha关键字
        material.DisableKeyword("SELF_ALPHA");
        switch (mode)
        {
            case PBlendMode.Additive:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_BlendOp", -1);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            case PBlendMode.AlphaBlend:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_BlendOp", -1);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            case PBlendMode.Brighter:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_BlendOp", (int)UnityEngine.Rendering.BlendOp.Max);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            case PBlendMode.Alpha:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_BlendOp", -1);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("SELF_ALPHA");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
        }
        
    }
}
