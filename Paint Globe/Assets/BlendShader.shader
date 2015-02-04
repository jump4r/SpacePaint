Shader "Custom/BlendShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Tex1 ("Base (RGB)", 2D) = "white" {}
		_Tex2 ("Base (RGB)", 2D) = "white" {}
		_Tex3 ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _Tex1;
		sampler2D _Tex2;
		sampler2D _Tex3;
		
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 b = tex2D (_Tex1, IN.uv_MainTex * 10.0);
			half4 d = tex2D (_Tex2, IN.uv_MainTex * 10.0);
			half4 e = tex2D (_Tex3, IN.uv_MainTex * 1.0);
			
			half4 final = b * c.r + d * c.g + e * c.b;
			
			o.Albedo = final.rgb;
			o.Alpha = 1.0;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
