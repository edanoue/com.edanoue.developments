Shader "Hidden/Edanoue/Debug/DebugLineZTestEnable"
{
    Properties
    {
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType"="Opaque"
            "IgnoreProjector" = "True"
        }
        LOD 100

        // Render State
        ZWrite On
        Cull Back

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

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                output.color = input.color;

                // For VR
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                // Outputs
                const VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                return output;
            }

            void frag(
                Varyings input, out half4 outColor : SV_Target0
            )
            {
                // Outputs
                half4 finalColor = input.color;
                finalColor.a = 1.0f;
                outColor = finalColor;
            }
            ENDHLSL
        }
    }

    Fallback "Hidden/Universal Render Pipeline/FallbackError"
}