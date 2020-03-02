﻿Shader "Unlit/waterDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_SurfaceDistortion("Surface Distortion", 2D) = "white" {}
		_SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27
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
				float2 distortUV : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			sampler2D _SurfaceDistortion;
			float4 _SurfaceDistortion_ST;

			float _SurfaceDistortionAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV ).xy * 2 - 1) * _SurfaceDistortionAmount;
				distortSample.y += _Time;
                fixed4 col = tex2D(_MainTex, i.uv + distortSample);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
