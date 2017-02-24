
using UnityEngine;
using System.Collections;

namespace Framework.UI
{
    [ExecuteInEditMode]
    public class DrawUIUtility : MonoBehaviour
    {
        /// <summary>
        /// 2D纹理
        /// </summary>
        public Texture2D MyTexture;

        /// <summary>
        /// 屏幕矩形框
        /// </summary>
        private Rect mRect;

        /// <summary>
        /// 纹理所使用的材质
        /// </summary>
        private Material mMaterial;

        void Awake()
        {
            Shader sd = Shader.Find("Unlit/Transparent");
            mMaterial = new Material(sd);
            mMaterial.color = new Color(255f, 255f, 255f, 70f);
        }

        [ContextMenu("Load")]
        public void Load()
        {

        }

        void Update()
        {
            mRect = Rect.MinMaxRect(0f, 0f, Screen.width, Screen.height);
        }

        [ContextMenu("DrawTexture")]
        public void OnGUI()
        {
            if (MyTexture == null)
                return;
            // 在屏幕坐标下绘制一个纹理
            Graphics.DrawTexture(mRect, MyTexture, mMaterial);
        }
    }
}
