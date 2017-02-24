Shader "Example/Shader_03" {
	Properties 
	{
		_MainTexture("MainTexture (RGB)", 2D) = "white" {}		//贴图 选项："white","black","gray","bump"中的一个
		_VexRange("VexRange", Range(-1,1))	  = 0.5				//自定义顶点的控制范围
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }							//声明在渲染非透明物体时调用

		LOD 200													//允许100-600之间  这个数值决定了我们能用什么样的Shader
		
		CGPROGRAM												//“Cg/HLSL”语言

		#pragma surface surf Lambert vertex:vert 				//Lambert 表示漫反射材质(受光照影响),同时自定义顶点控制函数

		sampler2D		_MainTexture;							//再次声明变量（主纹理）
		float			_VexRange;								//顶点的变换范围

		struct Input											//输入的结构体
		{
			float2 uv_MainTexture;
		};

		//Shader 主程序函数（表面着色）
		void surf (Input IN, inout SurfaceOutput o)				//CG规定固定了方法的“签名”
		{
			half4 c		= tex2D (_MainTexture, IN.uv_MainTexture);		
			o.Albedo	= c.rgb;
			o.Alpha		= c.a;	
		}

		//自定义顶点控制函数
		void vert(inout appdata_full v) 
		{
			v.vertex.xyz += v.normal * _VexRange * sin(_Time.y);
		}

		ENDCG
	} 

	FallBack "Diffuse"
}
