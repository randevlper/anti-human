Shader "Custom/BulletShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_OutlineWidth ("Outline Width", float) = 1
		_EmissionColor ("Emission Color", Color) = (1,1,1,1)
		_EmissionMagnitude("Emission Magnitude", Float) = 1.0
		_Emission ("Emission", 2D) = "black" {}

	}

	CGINCLUDE
	#include "UnityCG.cginc"
	struct appdata{
		float4 vertex : POSITION;
	};

	struct v2f{
		float4 pos : POSITION;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;
	sampler2D _MainTex;
	fixed4 _OutlineColor;
	float _OutlineWidth;

	sampler2D _Emission;
	float _EmissionMagnitude;
	fixed4 _EmissionColor;
	ENDCG


	SubShader {
		Tags{"Queue" = "Transparent" "Ignore Projector" = "True"}
		Pass
		{
			ZWrite Off
			Cull Back
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			v2f vert(appdata v){
				appdata original = v;
				v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			half4 frag(v2f i) : Color {
				return _OutlineColor;
			}
			ENDCG
		}

		Tags{"Queue" = "Geometry"}
		CGPROGRAM
		#pragma surface surf Standard
		struct Input{
			float2 uv_MainTex;
			float2 uv_Emission;
		};

		void surf(Input IN, inout SurfaceOutputStandard o){
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Smoothness = _Glossiness;
			o.Metallic = _Metallic;

			fixed4 e = tex2D (_Emission, IN.uv_Emission) * _EmissionColor;
			o.Emission = e * _EmissionMagnitude;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
