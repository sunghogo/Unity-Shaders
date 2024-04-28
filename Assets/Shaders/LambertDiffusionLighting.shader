Shader "Custom/LambertDiffusionLighting"
{
    Properties {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _materialColor ("Material Color", Color) = (0.5, 0.5, 0.5, 1)
        _ambientIntensity ("Ambient Light Color", Color) = (0, 0, 1, 1)
        _ambientReflection ("Ambient Reflection Constant", Range(0, 1)) = 1
        _diffuseIntensity ("Diffuse Light Color", Color) = (0, 0, 1, 1)
        _diffuseReflection ("Diffuse Reflection Constant", Range(0, 1)) = 1

    }

    SubShader {
        Tags { 
            "Queue" = "Geometry" 
        }

        CGPROGRAM
        #pragma surface surf LambertDiffusion

        struct Input {
            float2 uv_diffuseMap;
            float2 uv_normalMap;
        };

        sampler2D _diffuseMap;
        sampler2D _normalMap;
        fixed4 _materialColor;
        fixed4 _ambientIntensity;
        half _ambientReflection;
        fixed4 _diffuseIntensity;
        half _diffuseReflection;


        void surf(Input IN, inout SurfaceOutput o) {
            half4 diffuseColor = tex2D(_diffuseMap, IN.uv_diffuseMap);
            o.Albedo = diffuseColor.rgb * _materialColor.rgb;
            o.Alpha = diffuseColor.a * _materialColor.a;
            o.Normal = normalize(UnpackNormal(tex2D(_normalMap, IN.uv_normalMap)).xyz);
        }

        // Experimenting with fixed4 (11-bit precision)
        half4 LightingLambertDiffusion(SurfaceOutput s, half3 lightDir, half atten) {
            half4 ambient = half4(_ambientReflection * _ambientIntensity.rgb * atten, _ambientIntensity.a);

            half3 lightDirNorm = normalize(lightDir);
            half LdotN = saturate(dot(lightDirNorm, s.Normal));
            half4 diffuse = half4(_diffuseReflection * s.Albedo * LdotN * _diffuseIntensity * atten, s.Alpha);

            return diffuse + ambient;
        }

        ENDCG
    }
    
    FallBack "Diffuse"
}
