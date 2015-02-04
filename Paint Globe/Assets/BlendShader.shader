Shader "Custom/BlendShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Tex1 ("Base (RGB)", 2D) = "white" {}
		_Tex2 ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _Tex1;
		sampler2D _Tex2;
		
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 b = tex2D (_Tex1, IN.uv_MainTex * 10.0);
			half4 d = tex2D (_Tex2, IN.uv_MainTex * 10.0);
			
			float v1 = 1.0 - c.r;
			float v2 = c.r;
			
			half4 final = b * v1 + d * v2;
			
			o.Albedo = final.rgb;
			o.Alpha = 1.0;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
