// Alpha holds opacity
texture DiffuseTexture : DiffuseMaterialTexture
<
  string ResourceName = "/Textures/Samples/DiginiTestCard.vtf";
  string UIName = "Diffuse Texture";
  string UIHelp = "Diffuse Surface Texture";
  string ResourceType = "2D";
>;

sampler2D samplerDiffuse = sampler_state
{
	Texture = (DiffuseTexture);
	AddressU = Wrap;
	AddressV = Wrap;
	AddressW = Clamp;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;	
};

// Normal
texture NormalTexture : NormalMaterialTexture
<
  string ResourceName = "/Textures/Samples/DiginiTestCard.vtf";
  string UIName = "Normal Texture";
  string UIHelp = "Normal Surface Texture";
  string ResourceType = "2D";
>;

sampler2D samplerNormal = sampler_state
{
	Texture = (NormalTexture);
	AddressU = Wrap;
	AddressV = Wrap;
	AddressW = Clamp;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;	
};

// Alpha is glossiness factor
texture SpecularTexture : SpecularMaterialTexture
<
  string ResourceName = "/Textures/Samples/DiginiTestCard.vtf";
  string UIName = "Specular Texture";
  string UIHelp = "Specular Surface Texture";
  string ResourceType = "2D";
>;

sampler2D samplerSpecular = sampler_state
{
	Texture = (SpecularTexture);
	AddressU = Wrap;
	AddressV = Wrap;
	AddressW = Clamp;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;	
};

// Heightmap
texture HeightMapTexture
<
  string ResourceName = "/Textures/Samples/DiginiTestCard.vtf";
  string UIName = "Height Map Texture";
  string UIHelp = "Height Map Texture";
  string ResourceType = "2D";
>;

sampler2D samplerHeightMap = sampler_state
{
	Texture = (HeightMapTexture);
	AddressU = Wrap;
	AddressV = Wrap;
	AddressW = Clamp;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;	
};