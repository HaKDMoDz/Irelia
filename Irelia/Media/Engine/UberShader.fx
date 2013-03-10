#include "TextureSamplers.hlsl"

////////////////////////////////////////////////////////
// Static and Dynamic constants
////////////////////////////////////////////////////////

//#define VS_MODEL vs_2_0
//#define PS_MODEL ps_2_0

//---- Matrices ----
float4x4 m_World	: World < bool UIHidden = true; >;
float4x4 m_WVP		: WorldViewProjection < bool UIHidden = true; >;
float4x4 m_WorldIT	: WorldInverseTranspose < bool UIHidden = true; >;
float4x4 m_ViewInv	: ViewInverse  < bool UIHidden = true; >;
float4x4 m_WorldI	: WorldInverse  < bool UIHidden = true; >;

float3 g_vLightPos : Position <
    string Object = "PointLight0";
    string UIName =  "Lamp 0 Position";
    string Space = "World";
> = {-0.5f,2.0f,1.25f};

float g_fOffsetBias
<
	string UIWidget = "slider";
	string UIMin = "-1.0";
	string UIMax = "1.0";
	string UIStep = "0.01";
	string UIName = "Offset Height";
    string UIHelp = "Height of the Offset Mapping";
> = 0.0f;

// Here you can turn on/off the lighting calculations
bool useNormalMap
<
	string UIName = "Use Normal Mapping?";
> = true;

bool useOffsetMapping
<
	string UIName = "Use Offset Mapping?";
> = true;

float4 specularColor = {0.5, 0.5, 0.5, 1.0};
float specularPower = 30.0;

////////////////////////////////////////
//     Light Functions                //
////////////////////////////////////////
float3 Lambert(float3 N, float3 L)
{
	return max(0.0f, dot(N, L));
}

float3 NormalsTangent(float4 texNormal, float3 Nn, float3 Bn, float3 Tn)
{
	texNormal = texNormal * 2.0f - 1.0f;
	return normalize((texNormal.a * Tn) + (texNormal.g * Bn) + (texNormal.b * Nn));
}

float3 Blinn(float3 L, float3 V, float3 N, float roughness)
{
	float3 H = normalize(L + V);
	float NdH = max(0.f, dot(N, H));
	return smoothstep(-0.1, 0.1, dot(N, L)) * pow(NdH, roughness);
}

////////////////////////////////////////////////////////
// Vertex Shader and Structs
////////////////////////////////////////////////////////

// properties read from each vertex
struct VS_INPUT
{
	float4 Position : POSITION;  // untransformed vertex position
	float2 TexCoord : TEXCOORD0; // vertex texture coordinates
	float3 Normal	: NORMAL0;
	float3 Binormal	: BINORMAL0;
	float3 Tangent	: TANGENT0;
	float4 Color    : COLOR0;    // vertex color
};

// properties output for each vertex
struct VS_OUTPUT
{
	float4 Position			 : POSITION;	// transformed vertex position
	float4 cVert			 : COLOR0;		// vertex color
	float2 uvTexcoord		 : TEXCOORD0;	// final texture coordinates
	float3 vNormalWS		 : TEXCOORD1;
	float3 vBinormalWS		 : TEXCOORD2;
	float3 vTangentWS		 : TEXCOORD3;
	float3 vLightWS			 : TEXCOORD4;
	float3 vViewWS			 : TEXCOORD5;
	float3 vViewTS			 : TEXCOORD6;
};

// Main pass vertex shader
void VS(in VS_INPUT In, out VS_OUTPUT Out)
{
	Out.Position = mul(In.Position, m_WVP);
	Out.uvTexcoord = In.TexCoord;
	Out.cVert = In.Color;

	// World vectors
	Out.vNormalWS = mul(In.Normal, m_WorldIT);
	Out.vBinormalWS = mul(In.Binormal, m_WorldIT);
	Out.vTangentWS = mul(In.Tangent, m_WorldIT);

	// World position
  	float3 worldPosition = mul(In.Position, m_World);

	Out.vLightWS = g_vLightPos - worldPosition;
	Out.vViewWS = m_ViewInv[3] - worldPosition;
	
	// If we use offset mapping, we need the view vector in tangent space
	if (useOffsetMapping) {
	    float3x3 mWorldToTangent = float3x3(Out.vTangentWS, Out.vBinormalWS, Out.vNormalWS);
		Out.vViewTS = mul(mWorldToTangent, Out.vViewWS);
	}
}

////////////////////////////////////////
//     Pixel Shaders                  //
////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////
//     Full Shading
//////////////////////////////////////////////////////////////////////////////////////////////

float4 PSFull(in VS_OUTPUT In) : COLOR0
{
	// These are common vectors for whatever lighting function we use
	float3 V = normalize(In.vViewWS);
	float3 Nn = normalize(In.vNormalWS);
	float3 Bn = normalize(In.vBinormalWS);
	float3 Tn = normalize(In.vTangentWS);
	float3 L = normalize(In.vLightWS);
	float2 texCoord = In.uvTexcoord;

	// If we use offset mapping, we need to replace the existing UV coords
	if (useOffsetMapping == true) {
		float2 height = tex2D(samplerHeightMap, In.uvTexcoord.xy);
		texCoord = In.uvTexcoord + g_fOffsetBias * (height - 1) * normalize(In.vViewTS);
	}

	// Let's sample our maps
	float4 texDiffuse = tex2D(samplerDiffuse, texCoord);
	float4 texSpecular = tex2D(samplerSpecular, texCoord);

    // Figure out N (normal)
    float3	N = Nn;
	if (useNormalMap)
	{
		float4 texNormal = tex2D(samplerNormal, texCoord);
		N = NormalsTangent(texNormal, Nn, Bn, Tn);
	}
	
	// Ambient, either a standard ambient cube or one lerped with a reflection mask
	float3 ambientColor;
	ambientColor = float3(0, 0, 0);

	// Figure out our diffuse lighting
	float3 diffuseColor = texDiffuse * Lambert(N, L);

	// Specular lighting
	float3 specular = Blinn(L, V, N, specularPower) * specularColor.rgb;

    // Put it all together
	float3 lightComp = ambientColor + diffuseColor + specular;
	return float4(lightComp, 1);
}

//////////////////////////////////////////////////////////////////////////////////////////////
//     Vertex Colors Only
//////////////////////////////////////////////////////////////////////////////////////////////

float4 PSVertexColor(in VS_OUTPUT In) : COLOR0
{
	return In.cVert;
}

////////////////////////////////////////////////////////
// Techniques
////////////////////////////////////////////////////////

technique SimpleEffect
{
	pass P0
	{
		Vertexshader = compile vs_3_0 VS();
		alphablendenable = true;
    	srcblend = srcalpha;
    	destblend = invsrcalpha;
        CullMode = ccw;
		PixelShader = compile ps_3_0 PSFull();
	}
}

technique VertexColor
{
	pass P0
	{
		Vertexshader = compile vs_3_0 VS();
		alphablendenable = true;
    	srcblend = srcalpha;
    	destblend = invsrcalpha;
        CullMode = ccw;
		PixelShader = compile ps_3_0 PSVertexColor();
	}
}