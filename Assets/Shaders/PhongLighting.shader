Shader "Custom/PhongLighting"
{
    Properties
    {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _materialColor ("Material Color", Color) = (0.5, 0.5, 0.5, 1)
        _ambientIntensity ("Ambient Light Color", Color) = (0, 0, 1, 1)
        _ambientReflection ("Ambient Reflection Constant", Range(0, 1)) = 1
        _diffuseIntensity ("Diffuse Light Color", Color) = (0, 0, 1, 1)
        _diffuseReflection ("Diffuse Reflection Constant", Range(0, 1)) = 1
        _specularIntensity ("Specular Light Color", Color) = (0, 1, 0, 1)
        _specularReflection ("Specular Reflection Constant", Range(0, 1)) = 1
        _specularPower ("Specular Power", Range(0, 96)) = 48
    }

    SubShader
    {
        Tags { 
            "Queue" = "Geometry" 
        }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf PhongLighting

        struct Input
        {
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
        fixed4 _specularIntensity;
        half _specularReflection;
        half _specularPower;


        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 diffuseColor = tex2D(_diffuseMap, IN.uv_diffuseMap);
            o.Albedo = diffuseColor.rgb * _materialColor.rgb;
            o.Alpha = diffuseColor.a * _materialColor.a;
            o.Normal = normalize(UnpackNormal(tex2D(_normalMap, IN.uv_normalMap)).xyz);
        }

        half4 LightingPhongLighting(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
            half4 ambient = half4(_ambientReflection * _ambientIntensity.rgb * atten, _ambientIntensity.a);

            half3 lightDirNorm = normalize(lightDir);
            half LdotN = saturate(dot(lightDirNorm, s.Normal));
            half4 diffuse = half4(_diffuseReflection * s.Albedo * LdotN * _diffuseIntensity * atten, s.Alpha);

            // Reflection vector: R = 2 * dot(L, N) * N - L, where L's direction is from surface to light source
            // Can opt for reflect(L, N) next time;
            half3 reflection = 2 * dot(lightDirNorm, s.Normal) * s.Normal - lightDirNorm;
            half VdotR = saturate(dot(normalize(viewDir), reflection));
            half specularStrength = pow(VdotR, _specularPower);
            half4 specular = half4(_specularReflection * specularStrength * _specularIntensity.rgb * atten, _specularIntensity.a);
            
            return diffuse + ambient + specular;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
