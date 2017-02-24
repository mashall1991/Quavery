
Shader "Example/Shader_15" {

	Properties{
		_MainTex("基本纹理", 2D) = "white" {}
		_Color("主颜色", Color) = (1,1,1,0)
		_SpecColor("高光颜色", Color) = (1,1,1,1)
		_Emission("自发光颜色", Color) = (0,0,0,0)
		_Shininess("光泽度", Range(0.01, 1)) = 0.7
	}
	
	//---------------------------------【子着色器】----------------------------------  
	SubShader{
		Tags{ "Queue" = "Transparent" }
		
			//----------------通道---------------  
			Pass
			{
			//-----------材质------------  
			Material
			{
				//可调节的漫反射光和环境光反射颜色  
				Diffuse[_Color]
				Ambient[_Color]
				//光泽度  
				Shininess[_Shininess]
				//高光颜色  
				Specular[_SpecColor]
				//自发光颜色  
				Emission[_Emission]
			}
			//开启光照  
			Lighting On
			//开启独立镜面反射  
			SeparateSpecular On
			//设置纹理并进行纹理混合  
			SetTexture[_MainTex]
			{
				Combine texture * primary DOUBLE, texture * primary
			}
		}

	}

	//备胎设为Unity自带的普通漫反射  
	Fallback" Diffuse "
}


/*
没有嵌套CG语言，也就是代码段中没有CGPROGARAM和ENDCG关键字的，就是固定功能着色器。
嵌套了CG语言，代码段中有surf函数的，就是表面着色器。
嵌套了CG语言，代码段中有#pragma vertex name和  #pragma fragment frag声明的，就是顶点着色器&片段着色器。



6.1 用于通道Pass中的代码写法列举

这些代码一般是写在Pass{ }中的，细节如下：

Color Color
设定对象的纯色。颜色即可以是括号中的四值（RGBA），也可以是被方框包围的颜色属性名。

Material { Material Block }
材质块被用于定义对象的材质属性。

Lighting On | Off
开启光照，也就是定义材质块中的设定是否有效。想要有效的话必须使用Lighting On命令开启光照，而颜色则通过Color命令直接给出。

SeparateSpecular On | Off
开启独立镜面反射。这个命令会添加高光光照到着色器通道的末尾，因此贴图对高光没有影响。只在光照开启时有效。

ColorMaterial AmbientAndDiffuse | Emission
使用每顶点的颜色替代材质中的颜色集。AmbientAndDiffuse 替代材质的阴影光和漫反射值;Emission 替代 材质中的光发射值。


//----------材质------------
Material
{
//可调节的漫反射光和环境光反射颜色
Diffuse[_Color]
Ambient[_Color]
//光泽度
Shininess[_Shininess]
//高光颜色
Specular[_SpecColor]
//自发光颜色
Emission[_Emission]
}

如下这些代码的使用的地方是在SubShader中的一个Pass{ }中新开一个Material{ }块，在这个Material{ }块中进行这些语句的书写。这些代码包含了包含材质如何和光线产生作用的一些设置。这些属性默认为值都被设定为黑色（也就是说不产生作用），也就是说他们一般情况下可以被忽略。当然，还是有很多时候需要使用到他们的。

Diffuse Color(R,G,B,A)
漫反射颜色构成。这是对象的基本颜色。

Ambient Color(R,G,B,A)
环境色颜色构成.这是当对象被RenderSettings.中设定的环境色所照射时对象所表现的颜色。

Specular Color(R,G,B,A)
对象反射高光的颜色。(R,G,B,A)四个分量分别代表红绿蓝和Alpha，取值为0到1之间。

Shininess Number
加亮时的光泽度，在0和1之间。0的时候你会发现更大的高亮也看起来像漫反射光照，1的时候你会获得一个细微的亮斑。

Emission Color
自发光颜色，也就是当不被任何光照所照到时，对象的颜色。(R,G,B,A)四个分量分别代表红绿蓝和Alpha，取值为0到1之间。


而打在对象上的完整光照颜色最终是：

FinalColor=
Ambient * RenderSettings ambientsetting + (Light Color * Diffuse + Light Color *Specular) + Emission


翻译过来的中文式子便是：

最终颜色=环境光反射颜色* 渲染设置环境设置 *（灯光颜色*漫反射颜色+灯光颜色*镜面反射颜色）+自发光

知道了这个式子，我们就知道了，在各种光的综合作用下，我们材质最终的颜色是怎么来的了。
需要注意的是：方程式的灯光部分（也就是带括号的部分）对所有打在对象上的光线都是重复使用的。而我们在写Shader的时候常常会将漫反射和环境光光保持一致（所有内置Unity着色器都是如此）。

*/