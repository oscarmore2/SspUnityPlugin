Shader "CustomRenderTexture/RT_YUV2RGBA"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Tex("InputTex", 2D) = "white" {}
        _YTex("Y channel", 2D) = "black" {}
		_UTex("U channel", 2D) = "gray" {}
		_VTex("V channel", 2D) = "gray" {}
     }

     SubShader
     {
        Lighting Off
        Blend One Zero

        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
             #pragma target 3.0

            float4      _Color;
            sampler2D   _Tex;

            sampler2D _YTex;
			sampler2D _UTex;
			sampler2D _VTex;

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
            	float2 inv_uv = float2(IN.localTexcoord.x, 1.0 - IN.localTexcoord.y);
				float ych = tex2D(_YTex, inv_uv).a;
				float uch = tex2D(_UTex, inv_uv).a * 0.872 - 0.436;		//	Scale from 0 ~ 1 to -0.436 ~ +0.436
				float vch = tex2D(_VTex, inv_uv).a * 1.230 - 0.615;		//	Scale from 0 ~ 1 to -0.615 ~ +0.615
				/*	BT.601	*/
				float rch = clamp(ych + 1.13983 * vch, 0.0, 1.0);
				float gch = clamp(ych - 0.39465 * uch - 0.58060 * vch, 0.0, 1.0);
				float bch = clamp(ych + 2.03211 * uch, 0.0, 1.0);
				
				fixed4 col = fixed4(rch, gch, bch, 1.0);

				return col;
            }
            ENDCG
                    }
    }
}