Shader "Example/Shader_06" {
	Properties 
	{
		_MainTexture("MainTexture (RGB)", 2D)	= "white" {}		//贴图 选项："white","black","gray","bump"中的一个
		_AddTexture("AddTexture",2D)			= "white"{}			//使颜色的亮度变化大
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }								//声明在渲染非透明物体时调用

		LOD 200														//允许100-600之间  这个数值决定了我们能用什么样的Shader
		
		CGPROGRAM													//“Cg/HLSL”语言

		#pragma surface surf Lambert								//Lambert 表示漫反射材质(受光照影响)

		sampler2D		_MainTexture;								//再次声明变量（主纹理）
		sampler2D		_AddTexture;								//再次声明变量（增加的纹理）

		struct Input												//输入的结构体
		{
			float2 uv_MainTexture;
		};

		//Shader 主程序函数（表面着色）
		void surf (Input IN, inout SurfaceOutput o)					//CG规定固定了方法的“签名”
		{
			half4 c		= tex2D(_MainTexture, IN.uv_MainTexture);		
			half4 c2	= tex2D(_AddTexture, IN.uv_MainTexture);

			o.Alpha		= c.a;	

			//透明混合公式：NewColor = SrcColor * (1-Alpha) + Color * Alpha;
			o.Emission = c.rgb*(1 - c2.a) + c2.rgb * c2.a;
		}

		ENDCG
	} 

	FallBack "Diffuse"
}
