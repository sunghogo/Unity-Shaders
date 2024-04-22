Shader "Custom/NoLighting"
{
    Properties {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader {
        CGPROGRAM
        #pragma surface surf NoLighting

        struct Input {
            float2 uv_diffuseMap;
            float2 uv_normalMap;
            float3 viewDir;
        };

        sampler2D _diffuseMap;
        sampler2D _normalMap;
        fixed4 _color;
        
        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D(_diffuseMap, IN.uv_diffuseMap).rgb * _color.rgb * saturate(dot(IN.viewDir, o.Normal));
            o.Alpha = tex2D(_diffuseMap, IN.uv_diffuseMap).a * _color.a;
            o.Normal = UnpackNormal(tex2D(_normalMap, IN.uv_normalMap));
        }

        half4 LightingNoLighting(SurfaceOutput s, half3 lightDir, half atten) {
            return half4(s.Albedo, s.Alpha);
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
