Shader "Universal Render Pipeline/Custom/AtlasDotsInstancing"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _BaseColor ("Base Colour", Color) = (1, 1, 1, 1)
        _TileParams ("Tile Parameters", Vector) = (1, 1, 0, 0) // XY: Tiling, ZW: Offset
        _Size ("Size", Vector) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="AlphaTest"  // 使用剪切透明的渲染队列
            "RenderType"="TransparentCutout"
        }

        Pass
        {
            Name "Forward"
            Tags
            {
                "LightMode"="UniversalForward"
            }

            ZWrite On
            ZTest LEqual
            Cull Back
            Blend Off  // 不做透明混合（已剪切）

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5
            #pragma vertex UnlitPassVertex
            #pragma fragment UnlitPassFragment
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float4 _BaseColor;
            float4 _TileParams; // x: Tiling X, y: Tiling Y, z: Offset X, w: Offset Y
            float4 _Size;
            CBUFFER_END

            #ifdef UNITY_DOTS_INSTANCING_ENABLED
                UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
                    UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
                    UNITY_DOTS_INSTANCED_PROP(float4, _TileParams)
                    UNITY_DOTS_INSTANCED_PROP(float4, _Size)
                UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)
                #define _BaseColor UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _BaseColor)
                #define _TileParams UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _TileParams)
                #define _Size UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _Size)
            #endif

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            Varyings UnlitPassVertex(Attributes input)
            {
                Varyings output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                float3 scaledPosition = input.positionOS.xyz;
                scaledPosition.xy *= _Size.xy;

                VertexPositionInputs positionInputs = GetVertexPositionInputs(scaledPosition);
                output.positionCS = positionInputs.positionCS;

                float2 scaledUV = input.uv * _TileParams.xy;
                output.uv = scaledUV + _TileParams.zw;

                output.color = input.color;
                return output;
            }

            half4 UnlitPassFragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                #if defined(UNITY_NO_TEXTURE_2D_ARRAY)
                input.uv = frac(input.uv);
                #endif

                half4 baseMap = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);

                // Alpha Clip 剪裁透明区域
                clip(baseMap.a - 0.1);

                return baseMap * _BaseColor * input.color;
            }
            ENDHLSL
        }
    }
}
