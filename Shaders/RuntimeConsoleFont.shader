﻿Shader "Hidden/Edanoue/Debug/RuntimeConsoleFont"
{
    Properties
    {
        [MainTexture] _MainTex ("Font Texture", 2D) = "white" {}
        [MainColor] _Color("Text Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "IgnoreProjector" = "True"
            "Queue"="Transparent"
        }
        
        LOD 100

        Pass
        {
            Name "Unlit"
            Tags
            {
                "LightMode" = "UniversalForwardOnly"
            }

            // Render State
            ZWrite Off   // ZBuffer に書き込まない
            ZTest Always // Z を無視して常に描写する
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha
            
            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
            #pragma vertex vert
            #pragma fragment frag

            // GPU Instancing
            // #pragma multi_compile_instancing
            
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Properties
            CBUFFER_START(UnityPerMaterial)
            half4 _Color;
            CBUFFER_END

            sampler2D _MainTex;
            uniform float4 _MainTex_ST;

            struct Attributes
            {
                float4 positionOS : POSITION;
                half4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
                half4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                // For VR
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                // Outputs
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.color = input.color * _Color;
                output.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
                return output;
            }

            void frag(Varyings input, out half4 outColor : SV_Target0)
            {
                // Outputs
                half4 finalColor = input.color;
                finalColor.a *= tex2D(_MainTex, input.uv).a;
                outColor = finalColor;
            }
            
            ENDHLSL
        }
    }

    Fallback "Hidden/Universal Render Pipeline/FallbackError"
}