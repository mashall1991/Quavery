Shader "Example/Shader_02" {
	Properties 
	{
		_MainTexture("MainTexture (RGB)", 2D) = "white" {}		//贴图 选项："white","black","gray","bump"中的一个
		_NormalMap("NormalMap", 2D)		  = "bump" {}			//定义法线贴图
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }							//声明在渲染非透明物体时调用

		LOD 200													//允许100-600之间  这个数值决定了我们能用什么样的Shader
		
		CGPROGRAM												//“Cg/HLSL”语言

		#pragma surface surf Lambert							//Lambert 表示漫反射材质(受光照影响)

		sampler2D		_MainTexture;							//再次声明变量（主纹理）
		sampler2D		_NormalMap;								//再次声明法线贴图 

		struct Input											//输入的结构体
		{
			float2 uv_MainTexture;
			float2 uv_NormalMap;
		};

		//Shader 主程序函数（表面着色）
		void surf (Input IN, inout SurfaceOutput o)				//CG规定固定了方法的“签名”
		{
			half4 c = tex2D (_MainTexture, IN.uv_MainTexture);
			o.Albedo = c.rgb;
			o.Alpha = c.a;

			//“UnpackNormal” 为法线解包函数
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));		
		}
		ENDCG
	} 

	FallBack "Diffuse"
}
