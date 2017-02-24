Shader "Self/Katong" {
	
	//--------------------------------【属性】----------------------------------------  
	Properties
	{
		_MainTex("【主纹理】Texture", 2D)	= "white" {}				//贴图 选项："white","black","gray","bump"中的一个
	}

	//--------------------------------【子着色器】----------------------------------  
	SubShader
	{
		//-----------子着色器标签----------  
		Tags{ "RenderType" = "Opaque" }

		LOD 200
		Cull Back	//使用背面剔除
		//-------------------开始CG着色器编程语言段-----------------   
		CGPROGRAM

		//光照模式声明：使用自制的光照模式（控制最终的颜色输出）  LightingToonyGooch
		#pragma surface surf ToonyGooch 

		//输入结构,使可以在surf函数中使用
		struct Input
		{
			//纹理的uv值  
			float2 uv_MainTex;
		};

		//变量声明  
		sampler2D		_MainTex;			//主纹理

		//表面着色函数的编写  
		void surf(Input IN, inout SurfaceOutput o)
		{
			//从主纹理获取rgb颜色值  
			fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo		= texColor.rgb;
			o.Alpha			= texColor.a;
		}

		//实现自定义的光照输出模式  
		half4 LightingToonyGooch(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			fixed4 c;
			c.rgb	= s.Albedo * _LightColor0.rgb;		//最终输出的颜色受光照影响
			c.a		= s.Alpha;
			return c;
		}

		//-------------------结束CG着色器编程语言段------------------    
		ENDCG
	}

	FallBack "Diffuse"
}
