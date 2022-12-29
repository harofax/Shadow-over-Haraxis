Shader "dither/Dither Render Quad"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DitherPattern ("Dither Pattern", 2D) = "black" {}
        _Tint ("Tint", Color) = (1,1,1,1)
        _DitherSize("Dither size", Float) = 1
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

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _DitherPattern;

            float4 _MainTex_TexelSize;
            float4 _DitherPattern_TexelSize;
            float _DitherSize;

            fixed4 _Tint;

            Interpolators vert (MeshData v)
            {
                Interpolators i;
                i.vertex = UnityObjectToClipPos(v.vertex);
                i.uv = v.uv;
                
                return i;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
           
                float2 ditherCoordinate = i.uv * _ScreenParams.xy * (_DitherPattern_TexelSize.xy * _DitherSize);
                float ditherVal = tex2D(_DitherPattern, ditherCoordinate);
                
                float dither_threshold = step(ditherVal, col);
                float4 dithered_dark = ditherVal * col * 3;
                
                
                return lerp(dithered_dark, col, dither_threshold);
            }
            ENDCG
        }
    }
}
