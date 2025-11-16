#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_5_0
	#define PS_SHADERMODEL ps_5_0
#endif

matrix MatrixTransform;
float4 FogColor;
float2 PlayerPosPixels;
float Radius;
float Softness;
float TextureWidth;
float TextureHeight;
float Zoom;
// ====== PS ====== 

float4 DrawFogOfWar(float2 coords : TEXCOORD0 ) : COLOR0
{
    float2 pixelCoords = coords * float2(TextureWidth, TextureHeight);
    float2 delta = pixelCoords - PlayerPosPixels;
    float dist = length(delta);  
    float alpha = smoothstep(Radius, Radius + Softness, dist / Zoom); // 0 wewnątrz promienia, 1 poza promieniem+Softness  
    return float4(FogColor.rgb, alpha); // Można zignorować teksturę, używamy tylko koloru mgły
}

technique SpriteDrawing
{
	pass P0
	{	    
		PixelShader = compile PS_SHADERMODEL DrawFogOfWar();
	}
};