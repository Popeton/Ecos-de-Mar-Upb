Shader "Custom/IridescenceDistortionURP"
{
    Properties
    {
        [Header(Main Textures)]
        _MainTex("Main Texture", 2D) = "white" {}
        _ColorRamp("Color Ramp", 2D) = "white" {}
        _Mask("Mask", 2D) = "white" {}
        _Noise("Noise", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        
        [Header(Controls)]
        _Blend("Blend", Range(0,1)) = 0.5
        _Distortion("Distortion", Float) = 6
        _BumpPower("Bump Power", Range(0.1, 1)) = 0.1

        [Header(HSBC)]
        _Hue("Hue", Range(0, 1)) = 0
        _Saturation("Saturation", Range(0, 1)) = 0.5
        _Brightness("Brightness", Range(0, 1)) = 0.5
        _Contrast("Contrast", Range(0, 1)) = 0.5

        [Header(Material Settings)]
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_ColorRamp); SAMPLER(sampler_ColorRamp);
            TEXTURE2D(_Mask); SAMPLER(sampler_Mask);
            TEXTURE2D(_Noise); SAMPLER(sampler_Noise);
            TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap);

            float _Blend;
            float _Distortion;
            float _BumpPower;
            float _Hue, _Saturation, _Brightness, _Contrast;
            float _Glossiness;
            float _Metallic;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(positionWS);
                OUT.uv = IN.uv;
                OUT.viewDirWS = normalize(_WorldSpaceCameraPos - positionWS);
                OUT.worldPos = positionWS;
                return OUT;
            }

            float3 ApplyHue(float3 color, float hue)
            {
                float angle = radians(hue * 360);
                float3 k = float3(0.57735, 0.57735, 0.57735);
                float cosA = cos(angle);
                float sinA = sin(angle);
                return color * cosA + cross(k, color) * sinA + k * dot(k, color) * (1.0 - cosA);
            }

            float4 ApplyHSBC(float4 color, float4 hsbc)
            {
                float3 result = ApplyHue(color.rgb, hsbc.r);
                result = (result - 0.5) * hsbc.a * 2 + 0.5;  // Contrast
                result += hsbc.b * 2 - 1;                   // Brightness
                float3 intensity = dot(result, float3(0.39, 0.59, 0.11));
                result = lerp(intensity.xxx, result, hsbc.g * 2); // Saturation
                return float4(result, color.a);
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                float4 mainColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                float noise = SAMPLE_TEXTURE2D(_Noise, sampler_Noise, uv).r;
                float4 mask = SAMPLE_TEXTURE2D(_Mask, sampler_Mask, uv);
                float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uv));
                normalTS.z /= _BumpPower;
                float3 normalWS = TransformTangentToWorld(normalTS, float3x3(1,0,0, 0,1,0, 0,0,1)); // Simplified

                float rim = saturate(dot(IN.viewDirWS, normalWS));
                float2 noiseUV = IN.viewDirWS.xy + rim * noise * _Distortion;

                float4 rampColor = SAMPLE_TEXTURE2D(_ColorRamp, sampler_ColorRamp, noiseUV + uv);
                rampColor = max(rampColor * mask, (1 - mask) * mainColor);

                float4 hsbc = float4(_Hue, _Saturation, _Brightness, _Contrast);
                float4 rampHSBC = ApplyHSBC(rampColor, hsbc);

                float4 finalColor = lerp(mainColor, rampHSBC, _Blend);

                return float4(finalColor.rgb, 1.0);
            }

            ENDHLSL
        }
    }

    FallBack Off
}
