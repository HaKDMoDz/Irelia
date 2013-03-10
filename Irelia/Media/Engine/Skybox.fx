float4x4 m_ViewInv	: ViewInverse;
float4 ambientColor : Ambient;
float scale;
float aspectRatio;

texture diffuseTexture : Environment;
samplerCUBE samplerDiffuse = sampler_state
{
	Texture = <diffuseTexture>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Point;
	AddressU = Wrap;
	AddressV = Wrap;
	AddressW = Wrap;
};

struct VS_INPUT
{
	float3 Position : POSITION;	// 2D screen position
};

struct VS_OUTPUT
{
	float4 Position   : POSITION;
	float3 uvTexcoord : TEXCOORD0;
};

void VS(in VS_INPUT In, out VS_OUTPUT Out)
{
	Out.Position = float4(In.Position.xy, 1, 1);
	Out.uvTexcoord = float4(In.Position.x * aspectRatio, In.Position.y, scale, 0);
	Out.uvTexcoord = mul(Out.uvTexcoord, m_ViewInv).xyz;
}

float4 PS(in VS_OUTPUT In) : COLOR0
{
	return ambientColor * texCUBE(samplerDiffuse, In.uvTexcoord);
}

technique Skybox
{
	pass P0
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader = compile ps_3_0 PS();
		ZEnable = false;
	}
}