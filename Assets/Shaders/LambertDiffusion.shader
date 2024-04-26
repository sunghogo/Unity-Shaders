Shader "Custom/LambertDiffusion"
{
    Properties {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _color ("Color", Color) = (1, 1, 1, 1)
        _bump ("Bump", Range(0,10)) = 1
    }

    SubShader {
        CGPROGRAM
        #pragma surface surf LambertDiffusion

        struct Input {
            float2 uv_diffuseMap;
            float2 uv_normalMap;
        };

        sampler2D _diffuseMap;
        sampler2D _normalMap;
        fixed4 _color;
        half _bump; 

        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D(_diffuseMap, IN.uv_diffuseMap).rgb * _color.rgb;
            o.Alpha = tex2D(_diffuseMap, IN.uv_diffuseMap).a * _color.a;
            o.Normal = UnpackNormal(tex2D(_normalMap, IN.uv_normalMap));
            o.Normal *= float3(_bump, _bump, 1);
        }

        // Experimenting with fixed4 (11-bit precision)
        fixed4 LightingLambertDiffusion(SurfaceOutput s, half3 lightDir, half atten) {
            fixed lambert = saturate(dot(s.Normal, lightDir));
            return fixed4(s.Albedo * lambert * atten, s.Alpha);
        }

        ENDCG
    }
    
    FallBack "Diffuse"
}
