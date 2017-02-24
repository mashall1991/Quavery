using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework.Unity
{
    class AdaptationCamera3DPos : MonoBehaviour
    {
        /// <summary>
        /// 最小屏幕宽高比情况下的相机位置(16 : 9)
        /// </summary>
        public Vector3 MinimunWidthHeightRatioCameraPos = Vector3.zero;

        /// <summary>
        /// 最大屏幕宽高比情况下的相机位置(4 : 3)
        /// </summary>
        public Vector3 MaxmunWidthHeightRatioCameraPos = Vector3.zero;

        // 屏幕屏幕宽高比在最大和最小之间的差距
        private const float ResolutionDistance = (16f / 9) - (4f / 3);

        // 最大屏幕宽高比
        private const float MaxWidthHeightRatio = 16f / 9;

        // 最大屏幕宽高比和最小屏幕宽高比中相机位置的差距
        private Vector3 mCameraPosGap;

        void Awake()
        {
            mCameraPosGap = MaxmunWidthHeightRatioCameraPos - MinimunWidthHeightRatioCameraPos;
        }

        void Start()
        {
            // 在实际设备上运行的宽高比
            float currentWidthHeightRatio = Screen.width * 1.0f / Screen.height;

            // 设计宽高比 减去 实际宽高比以后的值
            float scalingReference = MaxWidthHeightRatio - currentWidthHeightRatio;

            // 最后计算出的需要乘以的一个系数值
            float multiplyingCoefficient = scalingReference / ResolutionDistance;

            // 计算出摄像机新的x y z 轴的坐标值
            float x = transform.localPosition.x - mCameraPosGap.x * multiplyingCoefficient;
            float y = transform.localPosition.y - mCameraPosGap.y * multiplyingCoefficient;
            float z = transform.localPosition.z - mCameraPosGap.z * multiplyingCoefficient;

            // 设置摄像机的位置
            this.transform.localPosition = new Vector3(x, y, z);
        }
    }
}
