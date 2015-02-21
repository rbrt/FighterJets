Shader "Custom/WaterSurface" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
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

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		float _Amplitude;
		float _Frequency;
		float _WaveVector;
		float _Lambda;

		fixed3 gerstner(fixed2 x0){
			float lambda = _Lambda;
			fixed2 waveVector = fixed2(_WaveVector, _WaveVector);
			float g = 9.8;
			float frequency = sqrt(g * _Frequency);
			float amplitude = _Amplitude;
			float k = 2 * 3.14159 / lambda;
			float t = _Time.y;
			fixed3 wave;

			wave.xz = x0 - (waveVector / k) * amplitude * sin(dot(waveVector, x0) - frequency * t);
			wave.y = amplitude * cos(dot(waveVector, x0) - frequency * t);

			return wave;
		}

		fixed2 gerstnerSumXZ(fixed2 x0, fixed2 waveVector, float freq, float amplitude, float phase){
			float lambda = _Lambda;
			float g = 9.8;
			float frequency = sqrt(g * freq);
			float k = 2 * 3.14159 / lambda;
			float t = _Time.y;
			fixed2 wave;

			wave.xy = (waveVector / k) * amplitude * sin(dot(waveVector, x0) - frequency * t + phase);

			return wave;
		}

		float gerstnerSumY(fixed2 x0, fixed2 waveVector, float freq, float amplitude, float phase){
			float lambda = _Lambda;
			float g = 9.8;
			float frequency = sqrt(g * freq);
			float k = 2 * 3.14159 / lambda;
			float t = _Time.y;
			fixed3 wave;

			return amplitude * cos(dot(waveVector, x0) - frequency * t + phase);
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

		void vert (inout appdata_full v){
			v.vertex.xyz = gerstnerSumGenerator(v.vertex.xz);
			v.normal = v.vertex;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
