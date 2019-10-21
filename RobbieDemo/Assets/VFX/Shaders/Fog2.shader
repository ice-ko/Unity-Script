Shader "Parts Light Vert"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MainColor("Base Color", Color) = (0.5,0.5,0.5,1)
		_Emission("Emission", Range(0.0,1.0)) = 0.0
	}
	SubShader
	{
		Tags {"LightMode" = "Vertex" "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector" = "True" }
		LOD 100
		Blend One One
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _MainColor;
			half _Emission;
			
			v2f vert (appdata v)
			{
				v2f o;
				fixed4 cl2 = v.color;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.light = UNITY_LIGHTMODEL_AMBIENT.xyz/10;
				o.color = 0;
				//Calculate light
				float3 viewpos = UnityObjectToViewPos(v.vertex.xyz);// mul(UNITY_MATRIX_MV, v.vertex).xyz;
				for (int i = 0; i < 8; i++) {
					half3 toLight = unity_LightPosition[i].xyz - viewpos.xyz * unity_LightPosition[i].w;
					half lengthSq = dot(toLight, toLight);
					half atten = 1.0 / (1.0 + lengthSq * unity_LightAtten[i].z);
 
					//fixed cut = 0.1;
					//fixed4 cl = unity_LightColor[i];
					//cl.r = max(cl.r - cut, -0.1);
					//cl.g = max(cl.g - cut, -0.1);
					//cl.b = max(cl.b - cut, -0.1);
					//cl.a = max(cl.a - cut, -0.1);

					o.color += unity_LightColor[i] * (atten) + _Emission;
				}

				o.color *= _MainColor * cl2;// (max(cl.r-0.05,0), max(cl.g-0.05,0), max(cl.b-0.05,0));

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 li = i.color;
				col.rgb = li.rgb;
				col.a *= li.a;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return (col * col.a);
			}
			ENDCG
		}
	}
}
