
Shader "Example/Shader_11" {
	Properties 
	{
		_MainTexture("MainTexture (RGB)", 2D)	 = "white" {}		//贴图 选项："white","black","gray","bump"中的一个
		_FlowLightTexture("FlowLightTexture",2D) = "white"{}		//流光贴图
		_UVSpeedY("UVSpeedY", Range(-10,10)) = 0
		_UVSpeedX("UVSpeedX",Range(-10,10)) = 0.2
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" "QUEUE" = "Transparent+100" }								//声明在渲染非透明物体时调用

		LOD 200														//允许100-600之间  这个数值决定了我们能用什么样的Shader
		
		CGPROGRAM													//“Cg/HLSL”语言

		#pragma surface surf Lambert alpha							//Lambert 表示漫反射材质(受光照影响)

		sampler2D		_MainTexture;								//再次声明变量（主纹理）
		sampler2D		_FlowLightTexture;
		fixed			_UVSpeedY;
		fixed			_UVSpeedX;
		fixed4			_Color;

		struct Input												//输入的结构体
		{
			//主纹理
			float2 uv_MainTexture;
			//流光纹理
			float2 uv_FlowLightTexture;
		};

		//Shader 主程序函数（表面着色）
		void surf (Input IN, inout SurfaceOutput o)					//CG规定固定了方法的“签名”
		{
			//取得流光的uv贴图
			fixed2 ScrollUV = IN.uv_FlowLightTexture;
			fixed YScroll = _UVSpeedY *_Time.y;
			fixed XScroll = _UVSpeedX * _Time.y;
			ScrollUV += fixed2(XScroll, YScroll);

			//分别得到两个贴图的rgba值
			fixed4 c = tex2D(_MainTexture, IN.uv_MainTexture);
			fixed4 d = tex2D(_FlowLightTexture, ScrollUV);

			//o.Albedo = c.rgb * _Color + d.rgb;

			o.Emission = d.rgb  * _Color;
			o.Alpha = c.a * d.a;	
		}

		ENDCG
	} 

	FallBack "Diffuse"
}
