
Shader "Example/Shader_10" {
	Properties 
	{
		_MainTexture("MainTexture (RGB)", 2D)	 = "white" {}		//贴图 选项："white","black","gray","bump"中的一个
		_Ramp("Shading Ramp", 2D) = "gray" {}						//渐变纹理
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }								//声明在渲染非透明物体时调用

		LOD 200														//允许100-600之间  这个数值决定了我们能用什么样的Shader
		
		CGPROGRAM													//“Cg/HLSL”语言

		#pragma surface surf SelfLambert								//Lambert 表示漫反射材质(受光照影响)

		sampler2D		_MainTexture;								//再次声明变量（主纹理）
		sampler2D		_Ramp;

		struct Input												//输入的结构体
		{
			//主纹理
			float2 uv_MainTexture;
		};

		//Shader 主程序函数（表面着色）
		void surf (Input IN, inout SurfaceOutput o)					//CG规定固定了方法的“签名”
		{
			half4 c		= tex2D(_MainTexture, IN.uv_MainTexture);		

			o.Albedo	= c.rgb;
			o.Alpha		= c.a;
		}

		//实现自定义的光照模式  
		half4 LightingSelfLambert(SurfaceOutput s, half3 lightDir, half atten)
		{
			//点乘反射光线法线和光线方向  
			half NdotL = dot(s.Normal, lightDir);

			//在兰伯特光照的基础上加上这句，增加光强  
			float diff = NdotL * 0.5 + 0.5;

			//从纹理中定义渐变效果  
			half3 ramp = tex2D(_Ramp, float2(diff, diff)).rgb;

			//计算出最终结果  
			half4 color;
			color.rgb	= s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
			color.a		= s.Alpha;
			return color;
		}

		ENDCG
	} 

	FallBack "Diffuse"
}
