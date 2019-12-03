Shader "Custom/LavaShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	    _DisplacementFactor("Facteur de déplacement", Range(0,1)) = 0.0
		_LavaTexture("Texture de lave", 2D) = "white" {}
		_DispTexture("Texture de deplacement", 2D) = "white" {}
		_RockTexture("Texture de roche", 2D) = "white" {}
		_SpeedScroll("scroll speed", Range(0,100)) = 3
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:disp

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _LavaTexture;
		sampler2D _DispTexture;
		sampler2D _RockTexture;

		struct appdata{
			float4 vertex : POSITION;
			float4 tangent: TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		struct Input {
			float2 uv_LavaTexture;
			float2 uv_DispTexture;
			float2 uv_RockTexture;
		};

		half _Glossiness;
		half _Metallic;
		half _SpeedScroll;
		half _DisplacementFactor;

		void disp(inout appdata v) {
			float splat = tex2Dlod(_DispTexture, float4 (v.texcoord.xy, 0, 0)).r;
			v.vertex.xyz -= v.normal * splat;
			v.vertex.xyz += v.normal * _DisplacementFactor;
		}




		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed2 scrolledUV = IN.uv_LavaTexture;
			fixed2 scrolledUVDips = IN.uv_DispTexture;
			fixed ScrollValue = _Time * _SpeedScroll;

			scrolledUV += fixed2(ScrollValue, ScrollValue);
			ScrollValue = _Time * _SpeedScroll / 4;

			fixed4 c = tex2D(_LavaTexture, scrolledUV);
			fixed4 b = tex2D(_RockTexture, scrolledUV);




			// Albedo comes from a texture tinted by color
		
			o.Albedo = c.rgb - b.rgb * 2.5;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
