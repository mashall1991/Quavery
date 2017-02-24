Shader "Example/Shader_01" {
	Properties 
	{
		_MainTexture("MainTexture (RGB)", 2D) = "white" {}		//贴图 选项："white","black","gray","bump"中的一个
		_MainColor("MainColor", Color)		  = (1,0,0,1.0)		//颜色		
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }							//声明在渲染非透明物体时调用
		//Tags { "RenderType"="Transparent" }					//含有透明效果的物体时调用  
		//Tags { "IgnoreProjector"="True"}						//不被Projectors影响
		//Tags { "ForceNoShadowCasting"="True"}					//从不产生阴影
		//Tags { "Queue"="Background"}							//指定物体的渲染顺序
																//Background  （最早被调用，渲染天空盒子等，等价数值1000）
																//Geometry    (默认数值等价数值2000)
																//AlphaTest  （用来渲染经过Alpha Test的像素等价数值2450）
																//Transparent （以从后往前的顺序渲染透明物体等价数值3000）
																//Overlay    (用来渲染叠加的效果，是渲染的最后阶段等价数值4000)

		LOD 200													//允许100-600之间  这个数值决定了我们能用什么样的Shader
		
		CGPROGRAM												//“Cg/HLSL”语言
																//#pragma surface surf Lambert 声明表面着色，
																//且函数名称为“surf”,
																//使用的光照模型是：“Lambert”（也就是普通的diffuse）

		#pragma surface surf Lambert							//Lambert 表示漫反射材质(受光照影响)
		//pragma surface surf BlinnPhong alpha					//alpha混合

		sampler2D		_MainTexture;							//再次声明变量（主纹理）
		fixed4 			_MainColor;  							//色泽(Color)

		struct Input											//输入的结构体
		{
			float2 uv_MainTexture;
		};

		//Shader 主程序函数(表面着色)
		void surf (Input IN, inout SurfaceOutput o)				//CG规定固定了方法的“签名”
		{
			half4 c = tex2D (_MainTexture, IN.uv_MainTexture);
			o.Albedo = c.rgb * _MainColor;
			
			//三原色
			//o.Albedo = c.r;
			//o.Albedo = c.g;
			//o.Albedo = c.b;
			
			o.Alpha = c.a;
		}
		ENDCG

		/* SurfaceOutput 的结构如下
		struct SurfaceOutput {  
			half3 Albedo;     //像素的颜色
			half Alpha;       //像素的透明度
			half3 Normal;     //像素的法向值
			half3 Emission;   //像素的发散颜色
			half Specular;    //像素的镜面高光
			half Gloss;       //像素的发光强度
		}
		*/
		/*  Input 的结构如下
		struct Input
		{ 
		  float2 uv_MainTex;   //UV贴图
		  float3 viewDir;      //视图方向( view direction)值
		  float4 anyName:COLOR; //每个顶点(per-vertex)颜色的插值。
		  float4 screenPos;    //裁剪空间位置
		  float3 worldPos;     //世界空间位置
		  float3 worldRefl;    //世界空间中的反射向量。
		} 					
		*/

	} 

	FallBack "Diffuse"
}
