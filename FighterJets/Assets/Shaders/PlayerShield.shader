Shader "Custom/PlayerShield" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MaxEffectBound ("MaxEffectBound", Range (0, 1)) = 0
	_MinEffectBound ("MinEffectBound", Range (0, 1)) = 0
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100

	Alphatest Greater 0
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
	ColorMask RGBA
	Cull Off


	// Non-lightmapped
	Pass {
		Tags { "LightMode" = "Vertex" }
		Material {
			Diffuse [_Color]
			Ambient [_Color]
			Shininess [_Shininess]
			Specular [_SpecColor]
			Emission [_Emission]
		}
		Lighting On
		SeparateSpecular On
		SetTexture [_MainTex] {
			Combine texture * primary DOUBLE, texture * primary
		}

	}
	Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};

			fixed4 _Color;
			fixed4 _BaseColor;
			float _MinEffectBound;
			float _MaxEffectBound;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				float4 hey = IN.vertex;
				OUT.vertex = mul(UNITY_MATRIX_MVP, hey);
				OUT.texcoord = IN.texcoord;

				OUT.color = IN.color * _Color;

				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : COLOR
			{
				if (IN.texcoord.y >= _MinEffectBound && IN.texcoord.y <= _MaxEffectBound ||
					IN.texcoord.y >= _MinEffectBound - .2 && IN.texcoord.y <= _MaxEffectBound - .18 ||
					IN.texcoord.y >= _MinEffectBound - .3 && IN.texcoord.y <= _MaxEffectBound - .29){
					return fixed4(_Color.xyz, .4);
				}
				return fixed4(_Color.xyz, 0);
			}
		ENDCG
		}
}

}
