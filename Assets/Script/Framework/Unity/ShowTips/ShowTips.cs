using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

namespace Framework.Unity
{
    public class ShowTips : MonoBehaviour
    {
        /*****************常量定义*****************/

        private Vector3 mTargetPostion;
        /// <summary>
        /// 移动到的目标位置
        /// </summary>
        public Vector3 TargetPostion
        {
            get
            {
                return mTargetPostion;
            }

            set
            {
                mTargetPostion = value;
            }
        }

        /// <summary>
        /// 渐隐结束的alpha值
        /// </summary>
        private const float EndAlpha = 0.3f;

        /// <summary>
        /// 动画持续时间
        /// </summary>
        private const float TweenDuraction = 1.0f;

        /*****************公共变量*****************/
        public Text showText;

        void Awake()
        {
            TargetPostion = new Vector3(0f, 0.8f, 0f);
        }

        /// <summary>
        /// 对外接口，显示飘窗内容
        /// </summary>
        /// <param name="message">飘窗中的文字信息</param>
        public void SetTipInfo(string message)
        {
            ResetShowTips();

            showText.text = message;

            // 并发动画
            transform.DOMove(TargetPostion, TweenDuraction).SetEase(Ease.OutCubic).SetRelative(true);
            DOTween.ToAlpha(() => showText.color, x => showText.color = x, EndAlpha, TweenDuraction).OnComplete(() => OnTweenOver());
        }

        /// <summary>
        /// 动画结束完成回调
        /// </summary>
        private void OnTweenOver()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 还原渐隐提示状态值
        /// </summary>
        private void ResetShowTips()
        {
            showText.text = string.Empty;
            this.gameObject.transform.localPosition = ShowTipsManager.Instance.InitialPostion;
            Color color = showText.color;
            color.a = 1f;
            showText.color = color;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
