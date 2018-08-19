Shader "Transition/FadeInOutTransition"
{
	Properties
	{
		PVW ("PVW", 2D) = "white" {}
		PGM ("PGM", 2D) = "white" {}
		AlphaKey ("AlphaKey", Float) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _PVW;
			sampler2D _PGM;
			Float _AlphaKey;
			float4 _PVW_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _PVW);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 colPVW = tex2D(_PVW, i.uv);
				fixed4 colPGM = tex2D(_PGM, i.uv);
				fixed4 col = colPVW * (1 - _AlphaKey) + colPGM * _AlphaKey;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
