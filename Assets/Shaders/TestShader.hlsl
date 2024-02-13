
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

float4 _ColorTint;
TEXTURE2D(_ColorMap); SAMPLER(sampler_ColorMap);

float4 _ColorMap_ST; // (tilings, offsets)
float _Smoothness;

struct Attributes {
	float3 position: POSITION;
	float3 normalOS: NORMAL;
	float2 uv: TEXCOORD0;
};

struct Interpolators {
	float4 positionCS: SV_POSITION;

	float2 uv: TEXCOORD0;
	float3 positionWS: TEXCOORD1;
	float3 normalWS: TEXCOORD2;
};

Interpolators Vertex(Attributes input) {
	Interpolators output;
	VertexPositionInputs positionInputs = GetVertexPositionInputs(input.position);
	VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS);

	output.positionCS = positionInputs.positionCS;
	output.normalWS = normalInputs.normalWS;
	output.positionWS = positionInputs.positionWS;
	output.uv = TRANSFORM_TEX(input.uv, _ColorMap);
	return output;
}

float4 Fragment(Interpolators input) : SV_TARGET{
	float4 colorSample = SAMPLE_TEXTURE2D(_ColorMap, sampler_ColorMap, input.uv);

	InputData lightingInput = (InputData)0;
	lightingInput.positionWS = input.positionWS;
	lightingInput.normalWS = normalize(input.normalWS);
	lightingInput.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
	
	SurfaceData surfaceInput = (SurfaceData)0;
	surfaceInput.albedo = colorSample.rgb;
	surfaceInput.alpha = colorSample.a;
	surfaceInput.specular = 1;
	surfaceInput.smoothness = _Smoothness;
	
	return UniversalFragmentBlinnPhong(lightingInput, surfaceInput);
}
