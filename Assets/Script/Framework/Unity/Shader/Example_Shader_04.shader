Shader "Example/Shader_04" {
	Properties 
	{
		_MainTexture("MainTexture (RGB)", 2D)	= "white" {}		//贴图 选项："white","black","gray","bump"中的一个
		_LightColor("LightColor",Color)			= (0,0,0,0)         //使颜色的亮度变化大
		_ToneColor("ToneColor",Color)			= (0,0,0,0)			//使颜色的色调变换大
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }								//声明在渲染非透明物体时调用

		LOD 200														//允许100-600之间  这个数值决定了我们能用什么样的Shader
		
		CGPROGRAM													//“Cg/HLSL”语言

		#pragma surface surf Lambert								//Lambert 表示漫反射材质(受光照影响)

		sampler2D		_MainTexture;								//再次声明变量（主纹理）
		float4			_LightColor;
		float4			_ToneColor;

		struct Input												//输入的结构体
		{
			float2 uv_MainTexture;
		};

		//Shader 主程序函数（表面着色）
		void surf (Input IN, inout SurfaceOutput o)					//CG规定固定了方法的“签名”
		{
			half4 c		= tex2D (_MainTexture, IN.uv_MainTexture);		
			o.Alpha		= c.a;	

			//o.Emission	= c.rgb;									//表示物体自己发散的颜色（固有颜色）

			//o.Emission  = c.rgb * _LightColor.rgb;					//固有颜色的乘法（亮度变化大）
			o.Emission	= c.rgb + _ToneColor.rgb;				    //固有颜色的加法（色调变化大）
		}

		ENDCG
	} 

	FallBack "Diffuse"
}
