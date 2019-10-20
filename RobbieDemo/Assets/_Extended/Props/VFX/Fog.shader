Shader "Particles Lighted" {
	Properties{
		_MainColor("Base Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	_Emission("Emission", Range(0.0,1.0)) = 0.0
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200

		CGPROGRAM
		// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
#pragma exclude_renderers gles
#pragma surface surf WrapLambert alpha
#pragma vertex vert

		half4 LightingWrapLambert(SurfaceOutput s, half3 lightDir, half atten)
	{
		//half NdotL = dot(s.Normal, lightDir);
		//half diff = NdotL * 0.3 + 0.7;
		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * (atten);
		c.a = s.Alpha;
		return c;
	}

	fixed4 _MainColor;
	sampler2D _MainTex;
	half _Emission;

	struct Input
	{
		float2 uv_MainTex;
		float3 viewDir;
		float4 color : COLOR;
	};

	void vert(inout appdata_full v, out Input o)
	{
		UNITY_INITIALIZE_OUTPUT(Input,o);
		o.color = v.color;
	}

	void surf(Input IN, inout SurfaceOutput o)
	{
		//float f = pow((saturate(dot(normalize(IN.viewDir),o.Normal) + _FresnelO)),_FresnelP);
		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _MainColor * IN.color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
		o.Emission = _Emission;
	}
	ENDCG
	}
		FallBack "Diffuse"
}