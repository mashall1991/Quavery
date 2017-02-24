using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 封装对unity基础内容的简单调用接口
    /// </summary>
    class UtilUnity
    {
        /// <summary>
        /// 获取Render中的Material信息
        /// </summary>
        /// <param name="render">渲染器</param>
        /// <returns>Material</returns>
        public static Material GetMaterial(Renderer render)
        {
            // 如果你需要修改模型材质的颜色，或者是修改材质Shader的一些属性，通常情况是用获取模型的Renderer组件，然后获取它的material属性。

            // 从效率上来说最好用sharedMaterial，它是共享材质，无论如何操作材质的属性（如更换颜色或者更换shader），内存中只会占用一份。
            // 但是如果要是用material的话，每次更换属性的时候Unity就会自动new一份新的material作用于它。
            // 它直到Application.LoadLevel() 或者Resources.UnloadUnusedAssets();的时候才会释放内存。
            // 所以material就有可能会造成内存泄漏，那么我们干脆就不要使用它。

            // 但是在代码中如果直接用render.sharedMaterial的话，你会发现在编辑器开发模式下，运行一会儿游戏本地的material文件凡是修改了的都变化了，
            // 如果这些文件都在svn管理中，那么他们都会变成红叹号，表示文件已经被修改。 
            // 这样太危险了，一不小心上传了怎么办。 为了解决这个问题，可以用一个简单的方法，每次获取material的时候根据平台而定。
        #if UNITY_EDITOR
            return render.material;
        #else
		    return render.sharedMaterial;
        #endif
        }

        /// <summary>
        /// 删除其下的所有子节点
        /// </summary>
        /// <param name="parent">父节点</param>
        public static void RemoveAllChild(Transform parent)
        {
            while (parent.childCount > 0)
            {
                GameObject.DestroyImmediate(parent.GetChild(0).gameObject);
            }
        }
    }
}
