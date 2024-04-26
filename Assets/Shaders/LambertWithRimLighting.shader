Shader "Custom/LambertWithRimLighting"
{
    Properties {
        _diffuseMap ("Texture", 2D) = "white" {}
        _normalMap ("Normal", 2D) = "bump" {}
        _modelColor ("Model Color", Color) = (1, 1, 1, 1)
        _ambientColor ("Ambient Light Color", Color) = (0, 0, 1, 1)
        _rimPower ("Rim Power", Range(0,10)) = 1
        _rimColor ("Rim Light Color", Color) = (1, 0, 0, 1)
    }

    SubShader {
        Tags { 
            "Queue" = "Geometry" 
        }

        CGPROGRAM
        #pragma surface surf LambertWithRim

        struct Input {
            float2 uv_diffuseMap;
            float2 uv_normalMap;
            float3 viewDir;
        };

        sampler2D _diffuseMap;
        sampler2D _normalMap;
        fixed4 _modelColor;
        fixed4 _ambientColor;
        half _rimPower;
        fixed4 _rimColor;

        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D(_diffuseMap, IN.uv_diffuseMap).rgb * _modelColor.rgb;
            o.Alpha = tex2D(_diffuseMap, IN.uv_diffuseMap).a * _modelColor.a;
            o.Normal = normalize(UnpackNormal(tex2D(_normalMap, IN.uv_normalMap)).xyz);
            
            half NdotV = saturate(dot(normalize(o.Normal), normalize(IN.viewDir)));
            half rim = 1 - NdotV;
            o.Emission = _rimColor.rgb * pow(rim, _rimPower);

        }

        half4 LightingLambertWithRim(SurfaceOutput s, half3 lightDir, half atten) {
            half LdotN = saturate(dot(normalize(lightDir), normalize(s.Normal)));
            half diffuse = LdotN;
            return half4(s.Albedo * _ambientColor.rgb * diffuse * atten, s.Alpha);
        }

        ENDCG
    }
    
    FallBack "Diffuse"
}