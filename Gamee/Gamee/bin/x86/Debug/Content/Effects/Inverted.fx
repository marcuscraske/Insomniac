uniform extern texture ScreenTexture;	

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;	
};

float4 PixelShader(float2 texCoord: TEXCOORD0) : COLOR
{
	float4 color = tex2D(ScreenS, texCoord);
	color = 1-tex2D( ScreenS, texCoord.xy);
	color.a = 1.0f;
	return color;
}

technique
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShader();
	}
}