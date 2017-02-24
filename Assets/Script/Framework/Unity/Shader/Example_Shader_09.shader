
Shader "Example/Shader_09" {
	Properties 
	{
		_MainTexture("MainTexture (RGB)", 2D)	 = "white" {}		//贴图 选项："white","black","gray","bump"中的一个
		_BumpMap("Bumpmap", 2D)					 = "bump" {}
		_RimColor("RimColor", Color) = (1.0, 1.0, 1.0, 1.0)			//边缘颜色
		_RimPower("RimPower", Range(0.5,8.0)) = 3.0					//边缘颜色强度
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }								//声明在渲染非透明物体时调用

		LOD 200														//允许100-600之间  这个数值决定了我们能用什么样的Shader
		
		CGPROGRAM													//“Cg/HLSL”语言

		#pragma surface surf Lambert								//Lambert 表示漫反射材质(受光照影响)

		sampler2D		_MainTexture;								//再次声明变量（主纹理）
		sampler2D		_BumpMap;
		float4			_RimColor;
		float			_RimPower;

		struct Input												//输入的结构体
		{
			//主纹理
			float2 uv_MainTexture;
			//凹凸纹理的uv值  
			float2 uv_BumpMap;
			//当前坐标的视角方向  
			float3 viewDir;
		};

		//Shader 主程序函数（表面着色）
		void surf (Input IN, inout SurfaceOutput o)					//CG规定固定了方法的“签名”
		{
			half4 c		= tex2D(_MainTexture, IN.uv_MainTexture);		

			o.Albedo	= c.rgb;
			o.Alpha		= c.a;

			o.Normal	= UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			//从_RimColor参数获取自发光颜色  
			half rim	= 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission  = _RimColor.rgb * pow(rim, _RimPower);
		}

		ENDCG
	} 

	FallBack "Diffuse"
}
