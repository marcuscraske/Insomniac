uniform extern texture ScreenTexture;	

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;	
};

float4 PixelShader(float2 texCoord: TEXCOORD0) : COLOR
{
	float4 color = tex2D(ScreenS, texCoord);
	if(color.r > 0.9 || color.g > 0.9 || color.b > 0.9)
	{
		color = color * 1.5;
	}
	return color;
}

technique
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShader();
	}
}