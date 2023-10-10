
Shader "Custom/CircleFadeShader" 
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Radius ("Radius", Range(0.1, 1)) = 0.5
        _Tiling ("Tiling", Vector) = (1, 1, 0, 0)
        _FadeIntensity ("Fade Intensity", Range(0.01, 1)) = 0.1
    }
    SubShader {
        Tags { "Queue"="Transparent+500" "RenderType"="Transparent" }
        LOD 100

        Pass {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Radius;
            float4 _Tiling;
            float _FadeIntensity;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.uv - float2(0.5, 0.5);
                float dist = length(uv);
                float alpha = smoothstep(_Radius, _Radius - _FadeIntensity, dist);
                fixed4 col = tex2D(_MainTex, i.uv/_Tiling.xy + _Tiling.zw);
                return col * _Color * alpha;
            }
            ENDCG
        }
    }
}

