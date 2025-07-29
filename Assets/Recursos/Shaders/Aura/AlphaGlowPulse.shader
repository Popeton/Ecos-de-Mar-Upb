// Shaders/AlphaGlowPulse.shader
Shader "Custom/AlphaGlowPulse" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (1,0,0,1)
        _GlowWidth ("Glow Width", Range(0, 0.2)) = 0.05
        _FresnelPower ("Fresnel Power", Range(0, 5)) = 3.0
        _PulseSpeed ("Pulse Speed", Float) = 2.0
        _PulseMin ("Pulse Min", Float) = 0.5
        _PulseMax ("Pulse Max", Float) = 2.0
    }

    SubShader {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
        }

        // Passo 1: Renderizar el glow basado en bordes alpha (2D) o fresnel (3D)
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ IS_2D

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD1;
                float3 normal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _GlowColor;
            float _GlowWidth;
            float _FresnelPower;
            float _PulseSpeed;
            float _PulseMin;
            float _PulseMax;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Base texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Efecto de pulso
                float pulse = lerp(_PulseMin, _PulseMax, (sin(_Time.y * _PulseSpeed) + 1) * 0.5);

                #if IS_2D
                    // Outline para 2D: Detecta bordes basados en alpha
                    float alpha = col.a;
                    float glow = 0;

                    // Muestrea vecinos para detectar bordes
                    float2 offsets[] = { float2(0, _GlowWidth), float2(_GlowWidth, 0), float2(0, -_GlowWidth), float2(-_GlowWidth, 0) };
                    for (int j = 0; j < 4; j++) {
                        float2 uvOffset = i.uv + offsets[j];
                        float neighborAlpha = tex2D(_MainTex, uvOffset).a;
                        glow += step(alpha, 0.1) * neighborAlpha;
                    }

                    glow = saturate(glow * pulse);
                    return _GlowColor * glow + col * alpha;
                #else
                    // Fresnel para 3D
                    float fresnel = pow(1 - saturate(dot(i.normal, i.viewDir)), _FresnelPower);
                    fresnel *= pulse;
                    return col + _GlowColor * fresnel;
                #endif
            }
            ENDCG
        }
    }
}