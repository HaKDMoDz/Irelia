float2 windowSize : ViewportPixelSize;

// Render-to-Texture
texture sceneMapTexture : RenderColorTarget;
sampler samplerSceneMap = sampler_state
{
	Texture = <sceneMapTexture>;
	AddressU = Clamp;
	AddressV = Clamp;
	AddressW = Clamp;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = None;
};

// For the last pass we add this screen border fadeout map to darken the borders
texture screenBorderFadeoutMapTexture : Diffuse;
sampler samplerScreenBorderFadeoutMapSampler = sampler_state
{
	Texture = <screenBorderFadeoutMapTexture>;
	AddressU = Clamp;
	AddressV = Clamp;
	AddressW = Clamp;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = None;
};

// Returns luminance value of color to convert color to grayscale
float Luminance(float3 color)
{
	return dot(color, float3(0.3, 0.59, 0.11));
}

struct VS_INPUT
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
};

struct VS_OUTPUT
{
	float4 Position : POSITION;
	float2 TexCoord[2] : TEXCOORD0;
};

void VS(in VS_INPUT In, out VS_OUTPUT Out)
{
	Out.Position = In.Position;

	// Don't use bilinear filtering
	float2 texelSize = 1.0 / windowSize;
	Out.TexCoord[0] = In.TexCoord + texelSize * 0.5;
	Out.TexCoord[1] = In.TexCoord + texelSize * 0.5;
}

float4 PS(in VS_OUTPUT In) : COLOR
{
	float4 orig = tex2D(samplerSceneMap, In.TexCoord[0]);
	float4 screenBorderFadeout = tex2D(samplerScreenBorderFadeoutMapSampler, In.TexCoord[1]);
	//float4 ret = Luminance(orig);
	float4 ret = orig;
	ret.rgb *= screenBorderFadeout;
	return ret;
}

technique ScreenDarkenBorder
{
	pass DarkenBorder
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader = compile ps_3_0 PS();
	}
}