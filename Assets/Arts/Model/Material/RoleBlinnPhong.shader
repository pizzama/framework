Shader "Game/Base/RoleBlinnPhong" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		//环境光
		_AmbientColor("Ambient Color", Color) = (0.1, 0.1, 0.1, 1.0)
		//高光
		_SpecularColor("Specular Color", Color) = (0.12, 0.31, 0.47, 1.0)
		//平滑       
		_Glossiness("Gloss", Range(1.0,512.0)) = 80.0
		_Detail("Detail", 2D) = "gray" {}

		_RimColor("Rim Color", Color) = (0.12, 0.31, 0.47, 1.0)
		_RimPower("Rim Power", Range(0.5, 8.0)) = 3.0
		_Ramp("Shading Ramp", 2D) = "gray" {}

		_HurtTex("HurtTex", 2D) = "white" {}
		//自发光
		// _intensity("_intensity",Range(0,50))=0

	}
		SubShader {
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf CustomBlinnPhong

		//float _intensity;
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Ramp;
		sampler2D _Detail;
		sampler2D _HurtTex;

		fixed4 _AmbientColor;
		fixed4 _SpecularColor;
		half _Glossiness;

		fixed4 _RimColor;
		half _RimPower;

		struct Input {
		   float2 uv_MainTex;
		   float2 uv_BumpMap;
		   half3 viewDir;
		   float2 uv_Detail;
		   float2 uv_HurtTex;
		};

		inline fixed4 LightingCustomBlinnPhong(SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten) {
			fixed3 ambient = s.Albedo * _AmbientColor.rgb;

			fixed NdotL = saturate(dot(s.Normal, lightDir));
			fixed3 diffuse = s.Albedo * _LightColor0.rgb * NdotL;

			fixed3 h = normalize(lightDir + viewDir);
			float nh = saturate(dot(s.Normal, h));
			float specPower = pow(nh, _Glossiness);
			fixed3 specular = _LightColor0.rgb * specPower * _SpecularColor.rgb;

			fixed4 c;
			c.rgb = (ambient + diffuse + specular) * (atten);
			c.a = s.Alpha + (_LightColor0.a * _SpecularColor.a * specPower * atten);
			return c;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c_hurt = tex2D(_HurtTex, IN.uv_HurtTex);

			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;

			fixed3 c_detail = tex2D(_Detail, IN.uv_Detail).rgb * 2;
			o.Albedo *= c_detail;

			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			fixed rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = (_RimColor.rgb * pow(rim, _RimPower));
		}
		ENDCG
	}
	FallBack "Diffuse"
}
