
Shader "Example/Shader_17" {

	Properties{

	}

	SubShader{
		//在所有不透明几何体之后绘制  
		Tags{ "Queue" = "Transparent" }

		//捕获对象后的屏幕到_GrabTexture中  
		GrabPass{}

		//用前面捕获的纹理渲染对象
		Pass
		{
			SetTexture[_GrabTexture]
		}
	}
}
