Shader "Loop/TestShader"
{
    Properties
    {
        _ColorMap("Texture", 2D) = "white" {}
        _ColorTint("Tint", Color) = (1,1,1,1)
        _Smoothness("Smoothness", float) = 1.0
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        //LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}
            HLSLPROGRAM
            #define _SPECULAR_COLOR
            
            #pragma vertex Vertex
            #pragma fragment Fragment
            
            #include "TestShader.hlsl"
            ENDHLSL
        }
    }
}
