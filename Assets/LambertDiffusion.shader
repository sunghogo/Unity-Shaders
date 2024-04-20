Shader "Custom/LambertDiffusion"
{
    Properties
    {
        _color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        CGPROGRAM
        #pragma surface surf Lambert
        
        struct Input {
            float2 uv_mainTex;
        };

        float4 _color;

        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = _color;
        }

        ENDCG
    }

    FallBack "Diffuse"
}
