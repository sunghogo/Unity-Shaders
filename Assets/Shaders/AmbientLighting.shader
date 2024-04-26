Shader "Custom/Ambient"
{
    Properties {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _modelColor ("Model Color", Color) = (1, 1, 1, 1)
        _ambientColor ("Ambient Light Color", Color) = (0, 0, 1, 1)
    }

    SubShader {
        Tags { 
            "Queue" = "Geometry" 
        }

        CGPROGRAM
        #pragma surface surf AmbientDiffusion

        struct Input {
            float2 uv_diffuseMap;
            float2 uv_normalMap;
        };

        sampler2D _diffuseMap;
        sampler2D _normalMap;
        fixed4 _modelColor;
        fixed4 _ambientColor;

        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D(_diffuseMap, IN.uv_diffuseMap).rgb * _modelColor.rgb;
            o.Alpha = tex2D(_diffuseMap, IN.uv_diffuseMap).a * _modelColor.a;
            o.Normal = normalize(UnpackNormal(tex2D(_normalMap, IN.uv_normalMap)).xyz);
        }

        // Experimenting with fixed4 (11-bit precision)
        half4 LightingAmbientDiffusion(SurfaceOutput s, half3 lightDir, half atten) {
            return half4(s.Albedo * _ambientColor.rgb * atten, s.Alpha);
        }

        ENDCG
    }
    
    FallBack "Diffuse"
}
