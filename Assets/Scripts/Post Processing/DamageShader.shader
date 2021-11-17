Shader "Custom/DamageShader"
{ 
    HLSLINCLUDE
    #include "Packages/com.yetman.render-pipelines.universal.postprocess/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

    TEXTURE2D_X(_MainTex);

    float _Intensity;
    float _Shift;

    float3 hueShift(float3 color, float hue) {
        const float3 k = float3(0.57735, 0.57735, 0.57735);
        float cosAngle = cos(hue);
        return float3(color * cosAngle + cross(k, color) * sin(hue) + k * dot(k, color) * (1.0 - cosAngle));
    }

    float4 GrayscaleFragmentProgram (PostProcessVaryings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord);
        float4 color = LOAD_TEXTURE2D_X(_MainTex, uv * _ScreenSize.xy);

        float redShift = 5;

		//color = float4(floor(uv.y*lines)/lines, floor(uv.y*lines)/lines, floor(uv.y*lines)/lines, 1);

        /*
        return fixed4(
        tex.r-_Color.r*(2*tex.r-tex.g-tex.b),
        tex.g-_Color.g*(2*tex.g-tex.r-tex.b),
        tex.b-_Color.b*(2*tex.b-tex.r-tex.g),tex.a);
        }
        */
        color = color * (1 - _Intensity) + float4(hueShift(color, _Shift * 3.14 / 180), 1) * _Intensity;


        return color;
    }
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex FullScreenTrianglePostProcessVertexProgram
            #pragma fragment GrayscaleFragmentProgram
            ENDHLSL
        }
    }
    Fallback Off
}
