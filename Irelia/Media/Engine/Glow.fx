float2 windowSize : ViewportPixelSize;
float radialBlurScaleFactor;
const float downsampleMultiplicator = 0.25f;
const float downsampleScale = 0.25f;
float blurWidth = 8.0f;
float glowIntensity = 0.7f;
float highlightIntensity = 0.4f;

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

texture radialSceneMapTexture : RenderColorTarget;
sampler samplerRadialSceneMap = sampler_state
{
	Texture = <radialSceneMapTexture>;
    AddressU  = Clamp;
    AddressV  = Clamp;
    AddressW  = Clamp;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = None;
};

texture downsampleMapTexture : RenderColorTarget;
sampler samplerDownsampleMap = sampler_state
{
	Texture = <downsampleMapTexture>;
    AddressU  = Clamp;
    AddressV  = Clamp;
    AddressW  = Clamp;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = None;
};

texture blurMap1Texture : RenderColorTarget;
sampler samplerBlurMap1 = sampler_state
{
	Texture = <blurMap1Texture>;
    AddressU  = Clamp;
    AddressV  = Clamp;
    AddressW  = Clamp;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = None;
};

texture blurMap2Texture : RenderColorTarget;
sampler samplerBlurMap2 = sampler_state
{
	Texture = <blurMap2Texture>;
    AddressU  = Clamp;
    AddressV  = Clamp;
    AddressW  = Clamp;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = None;
};

struct VS_INPUT
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
};

struct VS_OUTPUT_POS_3TEXCOORDS
{
	float4 Position		: POSITION;
	float2 TexCoord[3]	: TEXCOORD0;
};

struct VS_OUTPUT_POS_4TEXCOORDS
{
	float4 Position		: POSITION;
	float2 TexCoord[4]	: TEXCOORD0;
};

struct VS_OUTPUT_POS_7TEXCOORDS
{
	float4 Position		: POSITION;
	float2 TexCoord[7]	: TEXCOORD0;
};

struct VS_OUTPUT_POS_8TEXCOORDS
{
	float4 Position		: POSITION;
	float2 TexCoord[8]	: TEXCOORD0;
};

// Returns luminance value of color to convert color to grayscale
float Luminance(float3 color)
{
	return dot(color, float3(0.3, 0.59, 0.11));
}

// Radial Blur
void VS_RadialBlur(in VS_INPUT In, out VS_OUTPUT_POS_8TEXCOORDS Out)
{
	Out.Position = In.Position;

	// Don't use bilinear filtering, correct pixel locations
	float2 texelSize = 1.0 / windowSize;
	Out.TexCoord[0] = In.TexCoord + texelSize * 0.5f;

	// For all radial blur steps scale the finalSceneMap
	float2 texCentered = (In.TexCoord - float2(0.5f, 0.5f)) * 2.0f;

	// now apply formular to nicely increase blur factor to the borders
	for (int i = 1; i < 8; ++i)
	{
		texCentered = texCentered + radialBlurScaleFactor * (0.5f + i * 0.15f) * texCentered * abs(texCentered);
		Out.TexCoord[i] = (texCentered + float2(1.0f, 1.0f)) / 2.0f + texelSize * 0.5;
	}
}

float4 PS_RadialBlur(in VS_OUTPUT_POS_8TEXCOORDS In, uniform sampler2D samplerOrigScene) : Color
{
	float4 radialBlur = tex2D(samplerOrigScene, In.TexCoord[0]);
	for (int i = 1; i < 8; ++i)
		radialBlur += tex2D(samplerOrigScene, In.TexCoord[i]);
	return radialBlur / 8;
}

// Down Sample
void VS_DownSample(in VS_INPUT In, out VS_OUTPUT_POS_4TEXCOORDS Out)
{
	Out.Position = In.Position;

	float2 texelSize = downsampleMultiplicator / (windowSize * downsampleScale);
	float2 s = In.TexCoord;
	Out.TexCoord[0] = s - float2(-1, -1) * texelSize;
	Out.TexCoord[1] = s - float2(+1, +1) * texelSize;
	Out.TexCoord[2] = s - float2(+1, -1) * texelSize;
	Out.TexCoord[3] = s - float2(+1, +1) * texelSize;
}

float4 PS_DownSample(in VS_OUTPUT_POS_4TEXCOORDS In, in uniform sampler2D tex) : Color
{
	float4 c;

	// Box filter
	c = tex2D(tex, In.TexCoord[0]) / 4;
	c += tex2D(tex, In.TexCoord[1]) / 4;
	c += tex2D(tex, In.TexCoord[2]) / 4;
	c += tex2D(tex, In.TexCoord[3]) / 4;

	// Store highlights in alpha
	if (Luminance(c.rgb) < 0.75f)
		c.a = 0.1f;
	else
		c.a = 1;
	return c;
}

// Blur downsampled map
void VS_Blur(in VS_INPUT In, in uniform float2 direction, out VS_OUTPUT_POS_7TEXCOORDS Out)
{
	Out.Position = In.Position;

	float2 texelSize = blurWidth / windowSize;
	float2 s = In.TexCoord - texelSize * (7 - 1) * 0.5 * direction;
	for (int i = 0; i < 7; ++i)
	{
		Out.TexCoord[i] = s + texelSize * i * direction;
	}
}

// Blur filter weights
const half weights7[7] =
{
	0.05,
	0.1,
	0.2,
	0.3,
	0.2,
	0.1,
	0.05,
};

float4 PS_Blur(in VS_OUTPUT_POS_7TEXCOORDS In, uniform sampler2D tex) : Color
{
	float4 c = 0;
	for (int i = 0; i < 7; ++i)
	{
		c += tex2D(tex, In.TexCoord[i]) * weights7[i];
	}
	return c;
}

// Compose final scene
void VS_ScreenQuadSampleUp(in VS_INPUT In, out VS_OUTPUT_POS_3TEXCOORDS Out)
{
	Out.Position = In.Position;

	float2 texelSize = 1.0f / windowSize;

	Out.TexCoord[0] = In.TexCoord + texelSize * 0.5f;
	Out.TexCoord[1] = In.TexCoord + texelSize * 0.5f / downsampleScale;
	Out.TexCoord[2] = In.TexCoord + (1.0 / 128.0) * 0.5f;
}

float4 PS_ComposeFinalImage(in VS_OUTPUT_POS_3TEXCOORDS In, 
							in uniform sampler2D samplerScene,
							in uniform sampler2D samplerBlurredScene) : Color
{
	float4 orig = tex2D(samplerScene, In.TexCoord[0]);
	float4 blur = tex2D(samplerBlurredScene, In.TexCoord[1]);
	float4 ret = 0.75f * orig + glowIntensity * blur + highlightIntensity * blur.a;

	// Change colors a bit, sub 20% red and add 25% blue (photoshop values)
	// Here the values are -4% and +5%
	ret.rgb = float3(ret.r + 0.054f / 2, ret.g - 0.021f / 2, ret.b - 0.035f / 2);
	
	// Change brightness -5% and contrast +10%
	ret.rgb = ret.rgb * 0.975f;
	ret.rgb = (ret.rgb - float3(0.5, 0.5, 0.5)) * 1.05f + float3(0.5, 0.5, 0.5);

	return ret;
}

technique ScreenGlow
{
	pass RadialBlur
	{
		VertexShader = compile vs_3_0 VS_RadialBlur();
		PixelShader = compile ps_3_0 PS_RadialBlur(samplerSceneMap);
	}

	pass DownSample
	{
		VertexShader = compile vs_3_0 VS_DownSample();
		PixelShader = compile ps_3_0 PS_DownSample(samplerRadialSceneMap);
	}

	pass GlowBlur1
	{
		VertexShader = compile vs_3_0 VS_Blur(float2(1, 0));
		PixelShader = compile ps_3_0 PS_Blur(samplerDownsampleMap);
	}

	pass GlowBlur2
	{
		VertexShader = compile vs_3_0 VS_Blur(float2(0, 1));
		PixelShader = compile ps_3_0 PS_Blur(samplerBlurMap1);
	}

	pass ComposeFinalScene
	{
		VertexShader = compile vs_3_0 VS_ScreenQuadSampleUp();
		PixelShader = compile ps_3_0 PS_ComposeFinalImage(samplerRadialSceneMap, samplerBlurMap2);
	}
}