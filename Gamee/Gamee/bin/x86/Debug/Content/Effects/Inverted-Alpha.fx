uniform extern texture ScreenTexture;	

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;	
};

float4 PixelShader(float2 texCoord: TEXCOORD0) : COLOR
{
	float4 color = tex2D(ScreenS, texCoord);
	float1 alpha = color.a;
	color = 1-tex2D( ScreenS, texCoord.xy);
	color.a = alpha;
	return color;
}

technique
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShader();
	}
}