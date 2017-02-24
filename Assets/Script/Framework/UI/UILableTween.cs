using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Framework.UI
{
    public class UILableTween : MonoBehaviour
    {
        //数字滚动时间,默认0.4S
        public float duration = 0.4f;
        //变化次数
        private int times = 10;
        public Text myLable;
        void Awake()
        {
            if (null == myLable)
            {
                myLable = GetComponent<Text>();
                if (null == myLable)
                {
                }
            }
        }

        /// <summary>
        /// 数字动画效果
        /// </summary>
        /// <param name="changeData">改变的数值</param>
        private IEnumerator LabelTween(long changeData)
        {
            //当前lable数据
            long nowNum = System.Convert.ToInt64(myLable.text);

            //完成后需要显示的总数
            long total = nowNum + changeData;

            //变化一次需要增加的数据
            long tempData = changeData  / times;

            //变化的总量小于10
            if(tempData < 1)
            {
                tempData = 1;
                times = (int)changeData;
            }

            //初始化临时变量
            int index = 0;
            while (index < times)
            {
                nowNum += tempData;
                myLable.text = nowNum.ToString();
                index++;
                yield return new WaitForSeconds(duration / times);
            }

            //最后完成后强制性还原到该显示的数字
            myLable.text = total.ToString();
            times = 10;
        }

        /// <summary>
        /// 数字动画效果(添加千位逗号)
        /// </summary>
        /// <param name="changeData">改变的数值</param>
        private IEnumerator LabelTweenAddComma(long changeData)
        {
            //当前lable数据
            long nowNum = System.Convert.ToInt64(myLable.text);

            //完成后需要显示的总数
            long total = nowNum + changeData;

            //变化一次需要增加的数据
            long tempData = changeData / times;

            //变化的总量小于10
            if (tempData < 1)
            {
                tempData = 1;
                times = (int)changeData;
            }

            //初始化临时变量
            int index = 0;
            while (index < times)
            {
                nowNum += tempData;
                myLable.text = nowNum.ToString();
                index++;
                yield return new WaitForSeconds(duration / times);
            }

            //最后完成后强制性还原到该显示的数字
            myLable.text = total.ToString();
            times = 10;
        }

        /// <summary>
        /// 调用货币总数目动画效果
        /// </summary>
        /// <param name="totalNum">货币总数目</param>
        public void TotalNum(long totalNum)
        {
            if (null == myLable.text)
            {
                return;
            }
            else
            {
                if (totalNum >= 0)
                {
                    long changeNum = totalNum - System.Convert.ToInt64(myLable.text);
                    StartCoroutine(LabelTween(changeNum));
                }
                else
                {
                }
            }
        }

        /// <summary>
        /// 调用货币总数目动画效果(添加千位逗号)
        /// </summary>
        /// <param name="totalNum">货币总数目</param>
        public void TotalNumAddComma(long totalNum)
        {
            if (null == myLable.text)
            {
                return;
            }
            else
            {
                if (totalNum >= 0)
                {
                    long changeNum = totalNum - System.Convert.ToInt64(myLable.text);
                    StartCoroutine(LabelTweenAddComma(changeNum));
                }
                else
                {
                }
            }
        }

        /// <summary>
        /// 调用货币差值动画效果
        /// </summary>
        /// <param name="changeNum">数据差值</param>
        public void ChangeNum(long changeNum)
        {
            if (null == myLable.text)
            {
                return;
            }
            else
            {
                if (changeNum != 0)
                {
                    StartCoroutine(LabelTween(changeNum));
                }
                else
                {
                }
            }
        }

        /// <summary>
        /// 调用货币差值动画效果(添加千位逗号)
        /// </summary>
        /// <param name="changeNum">数据差值</param>
        public void ChangeNumAddComma(long changeNum)
        {
            if (null == myLable.text)
            {
                return;
            }
            else
            {
                if (changeNum != 0)
                {
                    StartCoroutine(LabelTweenAddComma(changeNum));
                }
                else
                {
                }
            }
        }
    }
}

