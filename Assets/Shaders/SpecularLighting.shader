Shader "Custom/SpecularLighting"
{
    Properties
    {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _modelColor ("Model Color", Color) = (1, 1, 1, 1)
        _specularColor ("Specular Light Color", Color) = (1, 0, 0, 1)
        _specularPower ("Specular Power", Range(0, 96)) = 48
    }

    SubShader
    {
        Tags { 
            "Queue" = "Geometry" 
        }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf SpecularLighting

        struct Input
        {
            float2 uv_diffuseMap;
            float2 uv_normalMap;
        };

        sampler2D _diffuseMap;
        sampler2D _normalMap;
        fixed4 _modelColor;
        fixed4 _specularColor;
        half _specularPower;


        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_diffuseMap, IN.uv_diffuseMap).rgb * _modelColor.rgb;
            o.Alpha = tex2D(_diffuseMap, IN.uv_diffuseMap).a * _modelColor.a;
            o.Normal = normalize(UnpackNormal(tex2D(_normalMap, IN.uv_normalMap)).xyz);
        }

        half4 LightingSpecularLighting(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
            half3 lightDirNorm = normalize(lightDir);

            // Reflection vector: R = L - 2 * (dot(l, N)) x N where L is light vector reflecting off the surface
            // Can opt for reflect(L, N) next time;
            half3 reflection = normalize(-lightDirNorm) - 2 * dot(-lightDirNorm, s.Normal) * s.Normal;
            half VdotR = saturate(dot(normalize(viewDir), reflection));
            half specular = pow(VdotR, _specularPower);
            
            return half4(s.Albedo * _specularColor * specular * atten, s.Alpha);
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
