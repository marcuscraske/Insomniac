uniform extern texture ScreenTexture;	

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;	
};

float4 PixelShader(float2 texCoord: TEXCOORD0) : COLOR
{
	float4 color = tex2D(ScreenS, texCoord);
	color.rgb = (color.r+color.g+color.b)/3.0f;
	return color;
}

technique
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShader();
	}
}