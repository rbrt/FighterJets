Shader "Custom/WaterSurface" {
	Properties {
		_Color ("Color", Color) = (0,0,1,0)
		_Lambda ("Lambda", Float) = 0
		_WaveVector ("Wave Vector", Float) = 0
		_Frequency ("Frequency", Float) = 0
		_Amplitude ("Amplitude", Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma target 3.0

		struct Input {
			float3 pos : SV_POSITION;
            float3 normal : TEXCOORD;
        };

		float _Amplitude;
		float _Frequency;
		float _WaveVector;
		float _Lambda;
		fixed4 _Color;

		fixed2 gerstnerSumXZ(fixed2 x0, fixed2 waveVector, float freq, float amplitude, float phase){
			float lambda = _Lambda;
			float g = 9.8;
			float frequency = sqrt(g * freq);
			float k = 2 * 3.14159 / lambda;
			float t = _Time.y;
			fixed2 wave;
			float amp = amplitude * _Amplitude;

			wave.xy = (waveVector / k) * amp * sin(dot(waveVector, x0) - frequency * t + phase);

			return wave;
		}

		float gerstnerSumY(fixed2 x0, fixed2 waveVector, float freq, float amplitude, float phase){
			float lambda = _Lambda;
			float g = 9.8;
			float frequency = sqrt(g * freq);
			float k = 2 * 3.14159 / lambda;
			float t = _Time.y;
			fixed3 wave;

			float amp = amplitude * _Amplitude;

			return amp * cos(dot(waveVector, x0) - frequency * t + phase);
		}

		fixed3 gerstnerSumGenerator(fixed2 x0){
			fixed3 wave;

			wave.xz = gerstnerSumXZ(x0, fixed2(1,1), .2, .2, .2);
			wave.y += gerstnerSumY(x0, fixed2(1,1), .2, .2, .2);
			wave.xz += gerstnerSumXZ(x0, fixed2(1,-1), 1, .5, .1);
			wave.y += gerstnerSumY(x0, fixed2(1,-1), 1, .5, .1);
			//wave.xz = gerstnerSumXZ(x0, fixed2(-1,1), .3, 1, .5);
			//wave.y += gerstnerSumY(x0, fixed2(-1,1), .3, 1, .5);
			//wave.xz += gerstnerSumXZ(x0, fixed2(-1,-1), 1, .5, .2);
			//wave.y += gerstnerSumY(x0, fixed2(-1,-1), 1, .5, .2);

			wave.xz = x0 - wave.xz;

			return wave;
		}

		void vert (inout appdata_full v, out Input o){
			float4 p = v.vertex;
			p.xyz = gerstnerSumGenerator(v.vertex.xz);
			v.vertex = p;
			o.pos = mul(UNITY_MATRIX_MVP, p);
			o.normal = v.vertex.xyz;
			v.normal = v.vertex.xyz;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
