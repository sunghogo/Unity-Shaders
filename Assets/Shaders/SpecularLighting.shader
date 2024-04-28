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
            fixed4 darken = fixed4(0.1, 0.1, 0.1, 1);
            o.Albedo = tex2D(_diffuseMap, IN.uv_diffuseMap).rgb * _modelColor.rgb * darken.rgb;
            o.Alpha = tex2D(_diffuseMap, IN.uv_diffuseMap).a * _modelColor.a;
            o.Normal = normalize(UnpackNormal(tex2D(_normalMap, IN.uv_normalMap)).xyz);
        }

        half4 LightingSpecularLighting(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
            half3 lightDirNorm = normalize(lightDir);

            // Reflection vector: R = 2 * dot(L, N) * N - L, where L's direction is from surface to light source
            // Can opt for reflect(L, N) next time;
            half3 reflection = 2 * dot(lightDirNorm, s.Normal) * s.Normal - lightDirNorm;
            half VdotR = saturate(dot(normalize(viewDir), reflection));
            half specularStrength = pow(VdotR, _specularPower);
            half3 specular = _specularColor * atten * specularStrength;
            
            return half4(s.Albedo + specular, s.Alpha);
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
