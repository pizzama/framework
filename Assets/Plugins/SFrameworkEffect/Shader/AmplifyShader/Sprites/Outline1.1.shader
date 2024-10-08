// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Outline"
{
	Properties
	{
		[HDR]_Color("Color", Color) = (0,0,0,0)
		_Main_Texture("Main_Texture", 2D) = "white" {}
		_Edge_with("Edge_with", Range( 0 , 2)) = 0
		[NoScaleOffset]_Disslove_Texture("Disslove_Texture", 2D) = "white" {}
		_Disslove_tilingoffset("Disslove_tiling&offset", Vector) = (0,0,0,0)
		_Disslove_strength("Disslove_strength", Range( 0 , 1)) = 0
		_Deviation01("Deviation01", Vector) = (0.5,-1,0,0)
		_Deviation02("Deviation02", Vector) = (0.5,1,0,0)
		_Deviation03("Deviation03", Vector) = (-1,0.5,0,0)
		_Deviation04("Deviation04", Vector) = (1,0.5,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform sampler2D _Main_Texture;
			uniform float4 _Main_Texture_TexelSize;
			uniform float _Edge_with;
			uniform float4 _Main_Texture_ST;
			uniform float4 _Color;
			uniform float2 _Deviation01;
			uniform float2 _Deviation02;
			uniform float2 _Deviation04;
			uniform float2 _Deviation03;
			uniform sampler2D _Disslove_Texture;
			uniform float4 _Disslove_tilingoffset;
			uniform float _Disslove_strength;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
#endif
				float2 uv0217 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult209 = (float2(_Main_Texture_TexelSize.x , _Main_Texture_TexelSize.y));
				float2 temp_output_210_0 = ( appendResult209 * _Edge_with );
				float2 _Vector1 = float2(2,-2);
				float2 appendResult205 = (float2(_Vector1.x , _Vector1.y));
				float2 appendResult204 = (float2(_Vector1.x , _Vector1.x));
				float2 appendResult206 = (float2(_Vector1.y , _Vector1.y));
				float2 appendResult207 = (float2(_Vector1.y , _Vector1.y));
				float2 uv_Main_Texture = i.ase_texcoord1.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float2 uv0335 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_360_0 = ( saturate( ( tex2D( _Main_Texture, ( ( ( uv0335 - _Deviation01 ) * 0.99 ) + _Deviation01 ) ).a + tex2D( _Main_Texture, ( ( ( uv0335 - _Deviation02 ) * 0.99 ) + _Deviation02 ) ).a + tex2D( _Main_Texture, ( ( ( uv0335 - _Deviation04 ) * 0.99 ) + _Deviation04 ) ).a + tex2D( _Main_Texture, ( ( ( uv0335 - _Deviation03 ) * 0.99 ) + _Deviation03 ) ).a ) ) - tex2D( _Main_Texture, uv_Main_Texture ).a );
				float2 uv0267 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult381 = (float2(_Disslove_tilingoffset.z , _Disslove_tilingoffset.w));
				float2 appendResult271 = (float2(_Disslove_tilingoffset.x , _Disslove_tilingoffset.y));
				float2 uv0244 = i.ase_texcoord1.xy * appendResult271 + float2( 0,0 );
				float2 panner245 = ( 1.0 * _Time.y * appendResult381 + uv0244);
				float4 tex2DNode237 = tex2D( _Disslove_Texture, ( uv0267 + panner245 ) );
				float temp_output_238_0 = ( tex2DNode237.r + 1.0 + ( _Disslove_strength * -2.0 ) + tex2DNode237.a );
				float temp_output_243_0 = saturate( temp_output_238_0 );
				float4 appendResult378 = (float4(( temp_output_360_0 * _Color * temp_output_243_0 ).rgb , ( temp_output_360_0 * _Color.a * temp_output_243_0 )));
				
				
				finalColor = ( ( ( saturate( ( tex2D( _Main_Texture, ( uv0217 + ( temp_output_210_0 * appendResult205 ) ) ).a + tex2D( _Main_Texture, ( uv0217 + ( temp_output_210_0 * appendResult204 ) ) ).a + tex2D( _Main_Texture, ( uv0217 + ( temp_output_210_0 * appendResult206 ) ) ).a + tex2D( _Main_Texture, ( uv0217 + ( temp_output_210_0 * appendResult207 ) ) ).a ) ) - tex2D( _Main_Texture, uv_Main_Texture ).a ) * _Color ) + appendResult378 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18000
0;0;2048;1091;800.6454;-1880.668;1.3;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;173;-2768,832;Inherit;True;Property;_Main_Texture;Main_Texture;1;0;Create;True;0;0;False;0;None;a70f3d09fd2ec5b4288a6be3b3f459ea;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Vector4Node;270;-1416.618,2371.809;Inherit;False;Property;_Disslove_tilingoffset;Disslove_tiling&offset;4;0;Create;True;0;0;False;0;0,0,0,0;30,5,0,-2;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;346;-1185.264,-50.57145;Inherit;False;Property;_Deviation02;Deviation02;7;0;Create;True;0;0;False;0;0.5,1;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;338;-1192.362,-236.7038;Inherit;False;Property;_Deviation01;Deviation01;6;0;Create;True;0;0;False;0;0.5,-1;0.5,-1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;353;-1177.292,212.2416;Inherit;False;Property;_Deviation03;Deviation03;8;0;Create;True;0;0;False;0;-1,0.5;0,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;358;-1170.114,416.9703;Inherit;False;Property;_Deviation04;Deviation04;9;0;Create;True;0;0;False;0;1,0.5;1,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;335;-1090.297,-524.4922;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexelSizeNode;208;-2400,1024;Inherit;False;-1;1;0;SAMPLER2D;_Sampler0208;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;351;-759.7467,78.93552;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;337;-827.9346,-448.6281;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;340;-956.1346,-219.4279;Inherit;False;Constant;_Float3;Float 3;12;0;Create;True;0;0;False;0;0.99;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;203;-2176,1472;Inherit;False;Constant;_Vector1;Vector 1;3;0;Create;True;0;0;False;0;2,-2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;209;-2096,1024;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;354;-794.4733,441.4806;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;211;-2304,1248;Inherit;False;Property;_Edge_with;Edge_with;2;0;Create;True;0;0;False;0;0;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;271;-1194.618,2397.809;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;343;-740.7913,-182.247;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;244;-1019.891,2345.451;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;205;-1936,1440;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;348;-599.7467,94.93552;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;204;-1936,1312;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;207;-1920,1712;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;381;-1100.354,2545.603;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;339;-644.9346,-438.6281;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;344;-583.1934,-168.0134;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;355;-634.4733,457.4806;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;210;-1888,1104;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;206;-1936,1568;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;356;-426.4732,489.4806;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;245;-676.1097,2368.981;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;214;-1680,1424;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;349;-391.7466,126.9355;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;267;-677.5186,2865.723;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;345;-365.1934,-137.0134;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;341;-426.9347,-407.6281;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;-1696,1520;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;-1696,1296;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-1680,1696;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;217;-1712,992;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;365;-211.6159,514.7125;Inherit;True;Property;_TextureSample8;Texture Sample 8;11;0;Create;True;0;0;False;0;-1;None;6a562f742013d2d4f83e3c212e17aba9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;364;-209.0739,189.3442;Inherit;True;Property;_TextureSample7;Texture Sample 7;11;0;Create;True;0;0;False;0;-1;None;6a562f742013d2d4f83e3c212e17aba9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;363;-167.1319,-129.6694;Inherit;True;Property;_TextureSample6;Texture Sample 6;11;0;Create;True;0;0;False;0;-1;None;6a562f742013d2d4f83e3c212e17aba9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;242;-89.57574,2652.199;Inherit;False;Property;_Disslove_strength;Disslove_strength;5;0;Create;True;0;0;False;0;0;0.861;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;221;-1360,1728;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;268;-340.7232,2746.266;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;220;-1328,1568;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;241;-9.575745,2765.199;Inherit;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;218;-1376,1296;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;219;-1376,1440;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;336;-155.4526,-411.8048;Inherit;True;Property;_TextureSample5;Texture Sample 5;11;0;Create;True;0;0;False;0;-1;None;6a562f742013d2d4f83e3c212e17aba9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;239;151.5341,2463.979;Inherit;False;Constant;_Float2;Float2;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;223;-1072,976;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;240;239.034,2697.28;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;226;-1088,1616;Inherit;True;Property;_TextureSample3;Texture Sample 3;7;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;347;270.5734,-80.06437;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;237;-167.4573,2295.23;Inherit;True;Property;_Disslove_Texture;Disslove_Texture;3;1;[NoScaleOffset];Create;True;0;0;False;0;-1;None;e1b995eb33e478046a679d9c1681dd27;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;224;-1088,1184;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;225;-1088,1408;Inherit;True;Property;_TextureSample2;Texture Sample 2;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;367;539.301,-40.48363;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;227;-572.5857,1351.426;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;238;381.5315,2309.679;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;366;-136.6484,798.9145;Inherit;True;Property;_TextureSample9;Texture Sample 9;11;0;Create;True;0;0;False;0;-1;None;6a562f742013d2d4f83e3c212e17aba9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;231;-239.2118,1352.702;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;360;611.7907,671.6237;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;159;286.2148,1744.67;Inherit;False;Property;_Color;Color;0;1;[HDR];Create;True;0;0;False;0;0,0,0,0;5.656854,0.7067193,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;243;532.0339,2264.636;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;230;-1072,1824;Inherit;True;Property;_TextureSample4;Texture Sample 4;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;228;38.43982,1334.249;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;377;1767.713,1851.687;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;1228.2,1476.113;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;380;955.6425,1730.789;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;378;2310.843,2061.732;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;371;1268.01,2370.542;Inherit;True;Property;_TextureSample10;Texture Sample 10;11;0;Create;True;0;0;False;0;-1;None;b7e5b8bba9fdce24995cc9ff764e8689;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;369;403.2843,2522.542;Inherit;False;Property;_edgewith;edgewith;10;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;379;2469.802,1912.652;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;370;1039.01,2384.542;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SmoothstepOpNode;368;776.0075,2349.651;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;372;1904.443,2197.147;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;373;1646.005,2375.888;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;376;2133.558,2221.742;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;131;2794.302,1805.158;Float;False;True;-1;2;ASEMaterialInspector;100;1;Outline;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;2;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;0
WireConnection;208;0;173;0
WireConnection;351;0;335;0
WireConnection;351;1;353;0
WireConnection;337;0;335;0
WireConnection;337;1;338;0
WireConnection;209;0;208;1
WireConnection;209;1;208;2
WireConnection;354;0;335;0
WireConnection;354;1;358;0
WireConnection;271;0;270;1
WireConnection;271;1;270;2
WireConnection;343;0;335;0
WireConnection;343;1;346;0
WireConnection;244;0;271;0
WireConnection;205;0;203;1
WireConnection;205;1;203;2
WireConnection;348;0;351;0
WireConnection;348;1;340;0
WireConnection;204;0;203;1
WireConnection;204;1;203;1
WireConnection;207;0;203;2
WireConnection;207;1;203;2
WireConnection;381;0;270;3
WireConnection;381;1;270;4
WireConnection;339;0;337;0
WireConnection;339;1;340;0
WireConnection;344;0;343;0
WireConnection;344;1;340;0
WireConnection;355;0;354;0
WireConnection;355;1;340;0
WireConnection;210;0;209;0
WireConnection;210;1;211;0
WireConnection;206;0;203;2
WireConnection;206;1;203;2
WireConnection;356;0;355;0
WireConnection;356;1;358;0
WireConnection;245;0;244;0
WireConnection;245;2;381;0
WireConnection;214;0;210;0
WireConnection;214;1;205;0
WireConnection;349;0;348;0
WireConnection;349;1;353;0
WireConnection;345;0;344;0
WireConnection;345;1;346;0
WireConnection;341;0;339;0
WireConnection;341;1;338;0
WireConnection;215;0;210;0
WireConnection;215;1;206;0
WireConnection;212;0;210;0
WireConnection;212;1;204;0
WireConnection;216;0;210;0
WireConnection;216;1;207;0
WireConnection;365;0;173;0
WireConnection;365;1;356;0
WireConnection;364;0;173;0
WireConnection;364;1;349;0
WireConnection;363;0;173;0
WireConnection;363;1;345;0
WireConnection;221;0;217;0
WireConnection;221;1;216;0
WireConnection;268;0;267;0
WireConnection;268;1;245;0
WireConnection;220;0;217;0
WireConnection;220;1;215;0
WireConnection;218;0;217;0
WireConnection;218;1;212;0
WireConnection;219;0;217;0
WireConnection;219;1;214;0
WireConnection;336;0;173;0
WireConnection;336;1;341;0
WireConnection;223;0;173;0
WireConnection;223;1;218;0
WireConnection;240;0;242;0
WireConnection;240;1;241;0
WireConnection;226;0;173;0
WireConnection;226;1;221;0
WireConnection;347;0;336;4
WireConnection;347;1;363;4
WireConnection;347;2;365;4
WireConnection;347;3;364;4
WireConnection;237;1;268;0
WireConnection;224;0;173;0
WireConnection;224;1;219;0
WireConnection;225;0;173;0
WireConnection;225;1;220;0
WireConnection;367;0;347;0
WireConnection;227;0;224;4
WireConnection;227;1;223;4
WireConnection;227;2;225;4
WireConnection;227;3;226;4
WireConnection;238;0;237;1
WireConnection;238;1;239;0
WireConnection;238;2;240;0
WireConnection;238;3;237;4
WireConnection;366;0;173;0
WireConnection;231;0;227;0
WireConnection;360;0;367;0
WireConnection;360;1;366;4
WireConnection;243;0;238;0
WireConnection;230;0;173;0
WireConnection;228;0;231;0
WireConnection;228;1;230;4
WireConnection;377;0;360;0
WireConnection;377;1;159;4
WireConnection;377;2;243;0
WireConnection;158;0;360;0
WireConnection;158;1;159;0
WireConnection;158;2;243;0
WireConnection;380;0;228;0
WireConnection;380;1;159;0
WireConnection;378;0;158;0
WireConnection;378;3;377;0
WireConnection;371;1;370;0
WireConnection;379;0;380;0
WireConnection;379;1;378;0
WireConnection;370;0;368;0
WireConnection;368;0;238;0
WireConnection;368;2;369;0
WireConnection;372;0;373;0
WireConnection;372;2;371;4
WireConnection;373;0;371;0
WireConnection;376;0;372;0
WireConnection;131;0;379;0
ASEEND*/
//CHKSM=26444F056BC392CFBC7593B86FB8D17F80C164AA