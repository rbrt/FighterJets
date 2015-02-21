Shader "Custom/Water" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Lambda ("Lambda", Float) = 0
	_WaveVector ("Wave Vector", Float) = 0
	_Frequency ("Frequency", Float) = 0
	_Amplitude ("Amplitude", Float) = 0
}

SubShader {
	LOD 100

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

			float _Amplitude;
			float _Frequency;
			float _WaveVector;
			float _Lambda;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				float4 vertex = IN.vertex;
				float4 vertexInWorld = mul(_Object2World, IN.vertex);

				float lambda = _Lambda;
				fixed2 waveVector = fixed2(_WaveVector, _WaveVector);
				//float g = 9.8;
				float g = 9.8;
				float frequency = sqrt(g * _Frequency);
				float amplitude = _Amplitude;
				float k = 2 * 3.14159 / lambda;
				float t = _Time.y;
				fixed2 x0 = vertexInWorld.xz;

				fixed3 wave;

				wave.xz = x0 - (waveVector / k) * amplitude * sin(dot(waveVector, x0) - frequency * t);
				wave.y = amplitude * cos(dot(waveVector, x0) - frequency * t);

				vertex.xyz = wave;
				OUT.vertex = mul(UNITY_MATRIX_MVP, vertex);
				//OUT.vertex.y = wave.y;

				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;

				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : COLOR
			{
				return fixed4(_Color);
			}
		ENDCG
		}
}

}
