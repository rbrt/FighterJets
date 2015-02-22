﻿Shader "Custom/WaterSurface" {
	Properties {
		_Color ("Color", Color) = (0,0,1,0)
		_Lambda ("Lambda", Float) = 0
		_Frequency ("Frequency", Float) = 0
		_Amplitude ("Amplitude", Float) = 0
		_PlayerPosition("Player Position", Vector) = (0,0,0,0)
		_PlayerForward("Player Forward", Vector) = (0,0,0,0)
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
			float4 color : COLOR;
			float3 worldPos;
        };

		float _Amplitude;
		float _Frequency;
		float _Lambda;
		fixed4 _Color;

		fixed4 _PlayerPosition;
		fixed4 _PlayerForward;

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
			wave.y = gerstnerSumY(x0, fixed2(1,1), .2, .2, .2);
			wave.xz += gerstnerSumXZ(x0, fixed2(1,-1), 1, .5, .1);
			wave.y += gerstnerSumY(x0, fixed2(1,-1), 1, .5, .1);
			//wave.xz = gerstnerSumXZ(x0, fixed2(-1,1), .3, 1, .5);
			//wave.y += gerstnerSumY(x0, fixed2(-1,1), .3, 1, .5);
			//wave.xz += gerstnerSumXZ(x0, fixed2(-1,-1), 1, .5, .2);
			//wave.y += gerstnerSumY(x0, fixed2(-1,-1), 1, .5, .2);

			wave.xz = x0 - wave.xz;

			return wave;
		}

		float sign (float2 p1, float2 p2, float2 p3){
		    return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
		}

		bool PointInTriangle (float2 pt, float2 v1, float2 v2, float2 v3){
		    bool b1, b2, b3;

		    b1 = sign(pt, v1, v2) < 0.0f;
		    b2 = sign(pt, v2, v3) < 0.0f;
		    b3 = sign(pt, v3, v1) < 0.0f;

		    return ((b1 == b2) && (b2 == b3));
		}

		void vert (inout appdata_full v, out Input o){
			float4 p = v.vertex;
			p.xyz = gerstnerSumGenerator(v.vertex.xz);
			v.vertex = p;
			//o.color.xyz = mul(, v.vertex);
			//o.normal = v.vertex.xyz;
			//v.normal = v.vertex.xyz;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			float deltaY = 100;
			float scaleWidth = 50;
			float4 forwardPoint = _PlayerPosition + _PlayerForward * 30;
			float4 rearPoint = _PlayerPosition - _PlayerForward * deltaY;
			float4 proxyVector = forwardPoint + float4(0,.1,0,0);

			// Will want to multiply these by some value
			float3 rightPoint = rearPoint + cross(proxyVector.xyz, forwardPoint.xyz) * scaleWidth;
			float3 leftPoint = rearPoint + cross(forwardPoint.xyz, proxyVector.xyz) * scaleWidth;

			//if (distance(IN.worldPos, leftPoint) < 50){
			if (PointInTriangle(IN.worldPos.xz, rightPoint.xz, leftPoint.xz, forwardPoint.xz)){
				o.Albedo = fixed4(1,0,0,.5);
			}
			else{
				o.Albedo = _Color;
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
