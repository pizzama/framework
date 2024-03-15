// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "UISoftDissolve"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_DissolveTexture("DissolveTexture", 2D) = "white" {}
		_DissolveStrength("DissolveStrength", Range( 0 , 1.1)) = 0
		_EdgeWidth("EdgeWidth", Float) = 0
		[HDR]_EdgeColor("EdgeColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			Comp [_StencilComp]
			Pass [_StencilOp]
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
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#define ASE_NEEDS_FRAG_COLOR

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _DissolveStrength;
			uniform sampler2D _DissolveTexture;
			uniform float4 _DissolveTexture_ST;
			uniform float _EdgeWidth;
			uniform float4 _EdgeColor;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode2 = tex2D( _MainTex, uv_MainTex );
				float2 uv_DissolveTexture = IN.texcoord.xy * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
				float4 tex2DNode3 = tex2D( _DissolveTexture, uv_DissolveTexture );
				float temp_output_8_0 = step( _DissolveStrength , ( tex2DNode3.g + _EdgeWidth ) );
				float temp_output_10_0 = ( temp_output_8_0 - step( _DissolveStrength , tex2DNode3.g ) );
				float4 lerpResult15 = lerp( ( IN.color * tex2DNode2 ) , ( tex2DNode2.a * temp_output_10_0 * _EdgeColor ) , temp_output_10_0);
				float4 appendResult17 = (float4((lerpResult15).rgb , ( tex2DNode2.a * temp_output_8_0 )));
				
				half4 color = appendResult17;
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19108
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-121.4,-723.5;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;132.0818,-717.9419;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;8;71.36606,158.4114;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-516.7,-180.5;Inherit;False;Property;_DissolveStrength;DissolveStrength;1;0;Create;True;0;0;0;False;0;False;0;0;0;1.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-537.6997,-97.80001;Inherit;True;Property;_DissolveTexture;DissolveTexture;0;0;Create;True;0;0;0;False;0;False;-1;2801d9b073430c6459b77309cec7683e;2801d9b073430c6459b77309cec7683e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-434.8691,102.5491;Inherit;False;Property;_EdgeWidth;EdgeWidth;2;0;Create;True;0;0;0;False;0;False;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-207.5224,114.7633;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;5;-49.48108,-148.0551;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;10;297.1024,-132.1215;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;1203.221,-81.16452;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;579.2468,-38.09832;Inherit;False;Property;_EdgeColor;EdgeColor;3;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,1.638333,5.047121,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;829.431,-204.5508;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;16;1364.332,-628.6506;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;17;1504.838,-433.5796;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1707.8,-435.7;Float;False;True;-1;2;ASEMaterialInspector;0;3;UISoftDissolve;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;False;True;2;5;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;True;0;True;_ColorMask;False;False;False;False;False;False;False;True;True;0;True;_Stencil;255;True;_StencilReadMask;255;True;_StencilWriteMask;0;True;_StencilComp;0;True;_StencilOp;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;0;True;unity_GUIZTestMode;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.VertexColorNode;14;239.1029,-891.5936;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;478.2099,-761.5895;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;15;1055.655,-765.1743;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;23;1117.41,169.3298;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
WireConnection;2;0;1;0
WireConnection;8;0;4;0
WireConnection;8;1;9;0
WireConnection;9;0;3;2
WireConnection;9;1;6;0
WireConnection;5;0;4;0
WireConnection;5;1;3;2
WireConnection;10;0;8;0
WireConnection;10;1;5;0
WireConnection;18;0;2;4
WireConnection;18;1;23;0
WireConnection;11;0;2;4
WireConnection;11;1;10;0
WireConnection;11;2;21;0
WireConnection;16;0;15;0
WireConnection;17;0;16;0
WireConnection;17;3;18;0
WireConnection;0;0;17;0
WireConnection;22;0;14;0
WireConnection;22;1;2;0
WireConnection;15;0;22;0
WireConnection;15;1;11;0
WireConnection;15;2;10;0
WireConnection;23;0;8;0
ASEEND*/
//CHKSM=5AAD03F36E5106ED92A6E1562B2E10B33C0716B8