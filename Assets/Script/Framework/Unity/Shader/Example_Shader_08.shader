/*
注意勾选流光贴图的 alpha from grayscal 
*/

Shader "Example/Shader_08" {
	Properties 
	{
		_MainTexture("MainTexture (RGB)", 2D)	 = "white" {}		//贴图 选项："white","black","gray","bump"中的一个
		_FlowLightTexture("FlowLightTexture",2D) = "black"{}		//流光贴图
		_UvFlowLightSpeed("FlowLightSpeed",float) = 2				//流光UV速度
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }								//声明在渲染非透明物体时调用

		LOD 200														//允许100-600之间  这个数值决定了我们能用什么样的Shader
		
		CGPROGRAM													//“Cg/HLSL”语言

		#pragma surface surf Lambert								//Lambert 表示漫反射材质(受光照影响)

		sampler2D		_MainTexture;								//再次声明变量（主纹理）
		sampler2D		_FlowLightTexture;							//流光的贴图texture
		float			_UvFlowLightSpeed;							//流光的速度

		struct Input												//输入的结构体
		{
			float2 uv_MainTexture;
		};

		//Shader 主程序函数（表面着色）
		void surf (Input IN, inout SurfaceOutput o)					//CG规定固定了方法的“签名”
		{
			half4 c		= tex2D(_MainTexture, IN.uv_MainTexture);		
			float2 uv	= IN.uv_MainTexture;
												
			uv.x += _Time.y * _UvFlowLightSpeed;					//流光UV计算

			float floLight = tex2D(_FlowLightTexture, uv).a;		//取流光亮度
			o.Albedo = c.rgb + float3(floLight, floLight, floLight);
			o.Alpha = c.a;
		}

		ENDCG
	} 

	FallBack "Diffuse"
}
