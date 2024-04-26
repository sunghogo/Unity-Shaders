Shader "Custom/ViewDirectionShading"
{
    Properties {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader {
        Tags { 
            "Queue" = "Geometry" 
        }

        CGPROGRAM
        #pragma surface surf ViewDirection

        struct Input {
            float2 uv_diffuseMap;
            float2 uv_normalMap;
            float3 viewDir;
        };

        sampler2D _diffuseMap;
        sampler2D _normalMap;
        fixed4 _color;
        
        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D(_diffuseMap, IN.uv_diffuseMap).rgb * _color.rgb * saturate(dot(normalize(IN.viewDir), normalize(o.Normal)));
            o.Alpha = tex2D(_diffuseMap, IN.uv_diffuseMap).a * _color.a;
            o.Normal = normalize(UnpackNormal(tex2D(_normalMap, IN.uv_normalMap)).xyz);
        }

        // Experimenting with fixed4 (11-bit precision)
        fixed4 LightingViewDirection(SurfaceOutput s, half3 lightDir, half atten) {
            return half4(s.Albedo, s.Alpha);
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
