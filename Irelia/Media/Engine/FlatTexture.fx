float4x4 gWvpXf : WorldViewProjection;

texture gTexture : DIFFUSE;
sampler2D gColorSampler = sampler_state
{
	Texture = <gTexture>;
	AddressU = Wrap;
	AddressV = Wrap;
};

struct AppData
{
	float3 pos : POSITION;
	float2 uv  : TEXCOORD0;
};

struct VertexOutput
{
	float4 hpos : POSITION;
	float2 uv	: TEXCOORD0;
};

VertexOutput flatVS(AppData IN,
					uniform float4x4 wvpXf)
{
	VertexOutput OUT = (VertexOutput)0;
	float4 po = float4(IN.pos, 1);
	OUT.hpos = mul(po, wvpXf);
	OUT.uv = IN.uv;
	return OUT;
}

float4 flatPS(VertexOutput IN, 
			  uniform sampler2D colorSampler) : COLOR
{
	float3 color = tex2D(colorSampler, IN.uv).rgb;
	return float4(color.rgb, 1.0);
}

technique SimpleEffect
{
	pass P0 
	{
		CullMode = None;
		VertexShader = compile vs_3_0 flatVS(gWvpXf);
		PixelShader = compile ps_3_0 flatPS(gColorSampler);
	}
}
