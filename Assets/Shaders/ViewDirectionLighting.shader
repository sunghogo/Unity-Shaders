Shader "Custom/ViewDirectionLighting"
{
    Properties {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _materialColor ("Material Color", Color) = (1, 1, 1, 1)
        _ambientIntensity ("Ambient Light Color", Color) = (0, 0, 1, 1)
        _ambientReflection ("Ambient Reflection Constant", Range(0, 1)) = 1
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
        fixed4 _materialColor;
        fixed4 _ambientIntensity;
        half _ambientReflection;
        
        void surf(Input IN, inout SurfaceOutput o) {
            half4 diffuseColor = tex2D(_diffuseMap, IN.uv_diffuseMap);
            o.Albedo = diffuseColor.rgb;
            o.Alpha = diffuseColor.a;
            o.Normal = normalize(UnpackNormal(tex2D(_normalMap, IN.uv_normalMap)).xyz);
        }

        half4 LightingViewDirection(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
            half4 ambientIllumination = _ambientReflection * _ambientIntensity * atten;
            half NdotV = saturate(dot(s.Normal, normalize(viewDir))) * atten;
            return ambientIllumination + NdotV;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
