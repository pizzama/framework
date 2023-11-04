Shader "Custom/UI/DotLine"
{
    Properties
    {
        _Color("Tint", Color) = (1,1,1,1)
        _Cnt("Cnt",float) = 0
        _MainTex("Texture", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.3
        _Ratio("Ratio", Range(0, 1.0)) = 0.5//线宽
        _Width("Width",float) = 0.01//线段高度
        _Speed("Speed",float) = 3//线段的移动速度
        [Toggle(DIR)] _Y("Y？", float) = 0//线段的运动方向  

    }

        SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Blend One OneMinusSrcAlpha

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile __ DIR
            #pragma multi_compile_instancing


            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord  : TEXCOORD0;
                float w : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            fixed4 _Color;
            fixed _Cutoff;
            sampler2D _MainTex;

            fixed _Ratio;
            float _Width;
            float _Speed;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, _Cnt)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, OUT);

                v.vertex.y *= _Width;//约束Y轴的尺寸

                float w = UnityObjectToClipPos(v.vertex).w;
                v.vertex.y *= w;//w值是影响近大远小的因素，我们这里反向乘回去，就可以忽略近大远小的问题
                _Speed = _Speed* w;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = v.texcoord;
                OUT.w = w;//把这个因子放到片段着色器，让线段个数随相机的距离变化而变化

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {

                UNITY_SETUP_INSTANCE_ID(IN);

                float dir = 1;
                #if DIR
                dir = 1;
                #else
                dir = -1;
                #endif

                fixed2 uv = IN.texcoord;
                uv.y *= dir / _Ratio;
                uv.y += _Time.y * _Speed;
                fixed4 color = tex2D(_MainTex, uv) * _Color;
                clip(color.a - _Cutoff);
                float x = _Time.y * _Speed + (dir * IN.texcoord.x * UNITY_ACCESS_INSTANCED_PROP(Props, _Cnt) / clamp(IN.w,2,100));//限制w值，否则一条线中虚线由两边往中间走的错误效果
                int intX = int(x);
                color.a *= step(x - intX, _Ratio);
                color.rgb *= _Color.a;
                
                return color;
            }
        ENDCG
        }
    }
}