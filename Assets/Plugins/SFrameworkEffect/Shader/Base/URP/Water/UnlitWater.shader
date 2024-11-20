Shader "SFramework/Base/URP/UnlitWater"
{
	Properties
	{
		_FoamTex("泡沫贴图(R:海浪泡沫,G:岸边泡沫,B:海浪扰动)", 2D) = "white" {}
		[Normal]_NormalTex("法线贴图", 2D) = "bump" {}
		_WaveMask ("海浪遮罩", 2D) = "white" {}
		_WaveTex("海浪渐变", 2D) = "white" {}
		_Gradient("海水颜色渐变", 2D) = "white" {}
		_Sky("反射天空盒", cube) = "" {}

		[Space]
		_WaveParams ("海浪参数(x:海浪范围,y:海浪偏移,z:海浪扰动,w:浪花泡沫扰动)", vector) = (0,0,0,0)
		_FoamParams("岸边泡沫参数(x:淡入,y:淡出,z:宽度,w:透明度)", vector) = (0,0,0,0)
		_Speed("速度参数(x:风速,y:海浪速度)", vector) = (0,0,0,0)

		[Space]
		_NormalScale ("法线缩放", range(0, 1)) = 1
		_Fresnel("菲涅尔系数", float) = 0
		
		_Specular("Specular", float) = 0
		_Gloss("Gloss", float) = 0
		_FoamColor ("泡沫颜色", color) = (1,1,1,1)
		_SpecColor ("高光颜色", color) = (0.4,0.4,0.4,1)
		_LightDir("光照方向", vector) = (0, 0, 0, 0)
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "true" }
		LOD 100

		Pass
		{
			blend srcalpha oneminussrcalpha
			zwrite off
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma target 3.0
			
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

			struct Attributes
			{
				float4 positionOS : POSITION;
				float2 texcoord : TEXCOORD0;
				float4 color : COLOR;
				float4 tangentOS : TANGENT;
				float3 normalOS : NORMAL;
			};

			struct Varyings
			{
				float2 uv_FoamTex : TEXCOORD0;
				float2 uv_NormalTex : TEXCOORD1;
				float fogCoord : TEXCOORD2;
				float4 TW0:TEXCOORD3;
				float4 TW1:TEXCOORD4;
				float4 TW2:TEXCOORD5;
				float4 positionCS : SV_POSITION;
				float4 color : COLOR;
			};

			TEXTURE2D(_FoamTex);
			SAMPLER(sampler_FoamTex);
			float4 _FoamTex_ST;

			TEXTURE2D(_WaveTex);
			SAMPLER(sampler_WaveTex);
			TEXTURE2D(_WaveMask);
			SAMPLER(sampler_WaveMask);

			half4 _Speed;
			
			half4 _WaveParams;

			half _NormalScale;

			half4 _FoamParams;

			TEXTURE2D(_Gradient);
			SAMPLER(sampler_Gradient);
			TEXTURE2D(_NormalTex);
			SAMPLER(sampler_NormalTex);
			float4 _NormalTex_ST;

			half _Fresnel;

			TEXTURECUBE(_Sky);
			SAMPLER(sampler_Sky);

			half _Specular;
			float _Gloss;

			half4 _LightDir;
			half4 _SpecColor;

			float4 _FoamColor;
			
			Varyings vert(Attributes input)
			{
				Varyings output;
				output.positionCS = TransformObjectToHClip(input.positionOS.xyz);

				output.uv_FoamTex = TRANSFORM_TEX(input.texcoord, _FoamTex);
				output.uv_NormalTex = TRANSFORM_TEX(input.texcoord, _NormalTex);

				output.fogCoord = ComputeFogFactor(output.positionCS.z);

				float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
				float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
				float3 tangentWS = TransformObjectToWorldDir(input.tangentOS.xyz);
				float tangentSign = input.tangentOS.w * GetOddNegativeScale();
				float3 bitangentWS = cross(normalWS, tangentWS) * tangentSign;

				output.TW0 = float4(tangentWS.x, bitangentWS.x, normalWS.x, positionWS.x);
				output.TW1 = float4(tangentWS.y, bitangentWS.y, normalWS.y, positionWS.y);
				output.TW2 = float4(tangentWS.z, bitangentWS.z, normalWS.z, positionWS.z);

				output.color = input.color;

				return output;
			}
			
			half4 frag(Varyings input) : SV_Target
			{
				//采样法线贴图
				float2 normalUV1 = input.uv_NormalTex + float2(_Time.x*_Speed.x, 0);
				float2 normalUV2 = float2(_Time.x*_Speed.x + input.uv_NormalTex.y, input.uv_NormalTex.x);
				half4 normalCol = (SAMPLE_TEXTURE2D(_NormalTex, sampler_NormalTex, normalUV1) + 
								 SAMPLE_TEXTURE2D(_NormalTex, sampler_NormalTex, normalUV2)) * 0.5;
			
				half3 worldNormal = UnpackNormal(normalCol);

				//泡沫使用法线贴图的rg进行扰动
				half3 foam = SAMPLE_TEXTURE2D(_FoamTex, sampler_FoamTex, input.uv_FoamTex + worldNormal.xy*_WaveParams.w).rgb;
				
				worldNormal = lerp(half3(0, 0, 1), worldNormal, _NormalScale);
				worldNormal = normalize(half3(dot(input.TW0.xyz, worldNormal), dot(input.TW1.xyz, worldNormal), dot(input.TW2.xyz, worldNormal)));

				//根据顶点颜色r通道采样海水渐变
				half4 col = SAMPLE_TEXTURE2D(_Gradient, sampler_Gradient, float2(input.color.r, 0.5));
				
				//采样反射天空盒
				half3 worldPos = half3(input.TW0.w, input.TW1.w, input.TW2.w);

				half3 viewDir = normalize(GetWorldSpaceViewDir(worldPos));
				half3 refl = reflect(-viewDir, worldNormal);

				half vdn = saturate(pow(dot(viewDir, worldNormal), _Fresnel));

				col.rgb = lerp(SAMPLE_TEXTURECUBE(_Sky, sampler_Sky, refl).rgb, col.rgb, vdn);

				//计算海浪和岸边泡沫
				float2 waveUV1 = float2(input.color.r + _WaveParams.y + _WaveParams.x*sin(_Time.x*_Speed.y + _WaveParams.z*foam.b), 0);
				float2 waveUV2 = float2(input.color.r + _WaveParams.y + _WaveParams.x*cos(_Time.x*_Speed.y + _WaveParams.z*foam.b), 0);
				float wave1 = SAMPLE_TEXTURE2D(_WaveTex, sampler_WaveTex, waveUV1).r;
				float wave2 = SAMPLE_TEXTURE2D(_WaveTex, sampler_WaveTex, waveUV2).r;
				float waveAlpha = SAMPLE_TEXTURE2D(_WaveMask, sampler_WaveMask, float2(input.color.r, 0)).r;

				float sfadein = 1 - saturate((_FoamParams.x - input.color.r) / _FoamParams.x);
				float sfadeout = 1 - saturate((input.color.r - _FoamParams.y) / _FoamParams.z);

				col+= (_FoamColor - col)* (wave1 + wave2)*waveAlpha*foam.r*input.color.b;
				col += (_FoamColor - col)* sfadein*sfadeout *_FoamParams.w*foam.g*input.color.g;

				//计算高光
				half3 h = normalize(viewDir - normalize(_LightDir.xyz));
				float ndh = max(0, dot(worldNormal, h));

				col += _Gloss*pow(ndh, _Specular*128.0)*_SpecColor;
				
				// apply fog
				col.rgb = MixFog(col.rgb, input.fogCoord);

				//应用顶点透明度
				col.a *= input.color.a;
				return col;
			}
			ENDHLSL
		}
	}
}
