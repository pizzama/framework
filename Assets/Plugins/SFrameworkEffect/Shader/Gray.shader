﻿Shader "SFramework/Base/Gray"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _ToneColor("ToneColor",Color) = (1,1,1,1)
        _ToneIntensity("ToneIntensity", Range(-1,1)) = 0
        _Saturation("Saturation",Range(0,1)) = 1
        _Contrast("Contrast",Float) = 1


        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp] 
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_ALPHACLIP
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };
            
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.worldPosition = IN.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = IN.texcoord;
                
                #ifdef UNITY_HALF_TEXEL_OFFSET
                OUT.vertex.xy += (_ScreenParams.zw-1.0) * float2(-1,1) * OUT.vertex.w;
                #endif
                
                OUT.color = IN.color * _Color;
                return OUT;
            }

            sampler2D _MainTex;
            half _ToneIntensity;
            half _Saturation;
            half _Contrast;
            fixed3 _ToneColor;

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 renderTex = tex2D(_MainTex, IN.texcoord);
                float gray = dot(renderTex.rgb, float3(0.2154,0.7154,0.0721));
				renderTex.rgb = float3(gray, gray, gray);
                fixed4 color = (renderTex + _TextureSampleAdd) * IN.color;
                fixed3 finalColor;
                if(_ToneIntensity<0)
                {
                    finalColor = color + color*_ToneIntensity;
                }
                else
                {
                    finalColor = color + (_ToneColor-color)*_ToneIntensity;
                }
                //fixed gray = dot(fixed3(0.2154,0.7154,0.0721),finalColor);
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
               
                finalColor = lerp(gray,finalColor,_Saturation);
                finalColor = lerp(0.5,finalColor,_Contrast);
                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return fixed4(finalColor, color.a);  
            }
        ENDCG
        }
    }
}
