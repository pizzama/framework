Shader "Debug/VertexColor" {
SubShader {
    Pass {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
    #include "UnityCG.cginc"

        // 顶点输入：位置、颜色
        struct appdata {
            float4 vertex : POSITION;
            fixed4 color : COLOR;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            fixed4 color : COLOR;
        };
        
        v2f vert (appdata v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex );
            o.color = v.color;
            return o;
        }
        
        fixed4 frag (v2f i) : SV_Target { return i.color; }
        ENDCG
    }
}
}