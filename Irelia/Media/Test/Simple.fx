//--------------------------------------------------------------------------------------
// Shader parameters
//--------------------------------------------------------------------------------------
float4x4 gWorldXf		: World;
float4x4 gWorldITXf		: WorldInverseTranspose;
float4x4 gWvpXf			: WorldViewProjection;
float4	 gLamp0Color	: SPECULAR;
texture  gTexture		: DIFFUSE;
bool	 gBool;
float	 gFloat;
float2   gFloat2;

//--------------------------------------------------------------------------------------
// Vertex shader output structure
//--------------------------------------------------------------------------------------
struct VS_OUTPUT
{
    float4 position   : POSITION;   // vertex position
	float4 color	  : COLOR;		// color
};


//--------------------------------------------------------------------------------------
// Vertex shader
//--------------------------------------------------------------------------------------
VS_OUTPUT VS(float3 pos : POSITION, float4 color : COLOR, float4 normal	: NORMAL)
{
	VS_OUTPUT output;
	
    output.position = float4(pos.xyz, 1);
	output.color = color;
	return output;
}


//--------------------------------------------------------------------------------------
// Pixel shader
//--------------------------------------------------------------------------------------
float4 PS(VS_OUTPUT In) : COLOR
{
	return In.color;
}


technique SimpleEffect
{
	pass P0
	{
		VertexShader = compile vs_1_1 VS();
		PixelShader	 = compile ps_1_1 PS();
	}
}