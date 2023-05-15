Shader "Hidden/Edanoue/Debug/RuntimeConsolePanel"
{
    Properties
    {
        [MainColor] _Color("Color", Color) = (0.5, 0.5, 0.5, 1)
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

        Fog
        {
            Mode Off
        }
        
        Cull Off
        Blend DstColor Zero, Zero One
        ZWrite Off
        ZTest Always

        Pass
        {
            Name "Unlit"

            HLSLPROGRAM
            #pragma target 2.0

            // Pragmas
            #pragma multi_compile_instancing
            #pragma vertex vert
            #pragma fragment frag

            // Includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Properties
            CBUFFER_START(UnityPerMaterial)
            half4 _Color;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
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
                output.color = _Color;
                return output;
            }

            void frag(
                Varyings input, out half4 outColor : SV_Target0
            )
            {
                // Outputs
                half4 finalColor = lerp(half4(1.0, 1.0, 1.0, 1.0), _Color, _Color.a); // AlphaModulate
                finalColor.a = _Color.a;
                outColor = finalColor;
            }
            ENDHLSL
        }
    }

    Fallback "Hidden/Universal Render Pipeline/FallbackError"
}