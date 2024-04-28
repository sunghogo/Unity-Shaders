Shader "Custom/RimLighting"
{
    Properties {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _materialColor ("Material Color", Color) = (0.5, 0.5, 0.5, 1)
        _ambientIntensity ("Ambient Light Color", Color) = (0, 0, 1, 1)
        _ambientReflection ("Ambient Reflection Constant", Range(0, 1)) = 1
        _rimIntensity ("Rim Light Color", Color) = (0, 1, 0, 1)
        _rimReflection ("Rim Reflection Constant", Range(0, 1)) = 3
        _rimPower ("Rim Power", Range(0,10)) = 1
    }

    SubShader {
        Tags { 
            "Queue" = "Geometry" 
        }

        CGPROGRAM
        #pragma surface surf NoLighting

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
        fixed4 _rimIntensity;
        half _rimReflection;
        half _rimPower;

        void surf(Input IN, inout SurfaceOutput o) {
            half4 diffuseColor = tex2D(_diffuseMap, IN.uv_diffuseMap);
            o.Albedo = diffuseColor.rgb * _materialColor.rgb;
            o.Alpha = diffuseColor.a * _materialColor.a;
            o.Normal = normalize(UnpackNormal(tex2D(_normalMap, IN.uv_normalMap)).xyz);
        }

        half4 LightingNoLighting(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
            half4 ambient = half4(_ambientReflection * _ambientIntensity.rgb * atten, _ambientIntensity.a);

            half4 diffuse = half4(s.Albedo * atten, s.Alpha);

            half NdotV = saturate(dot(s.Normal, normalize(viewDir)));
            half InvertedNdotV = 1 - NdotV;
            half rimStrength = pow(InvertedNdotV, _rimPower);
            half4 rim = half4(_rimReflection * rimStrength * _rimIntensity.rgb * atten, _rimIntensity.a);

            return diffuse + ambient + rim;
        }

        ENDCG
    }
    
    FallBack "Diffuse"
}
