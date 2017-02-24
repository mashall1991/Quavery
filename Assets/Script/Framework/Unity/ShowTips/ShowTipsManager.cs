using UnityEngine;
using System.Collections.Generic;

namespace Framework.Unity
{
    /// <summary>
    /// 渐隐提示管理器
    /// </summary>
    public class ShowTipsManager : Framework.Singleton<ShowTipsManager>
    {
        public const int MAXCOUNT = 10;

        /// <summary>
        /// 飘窗管理容器
        /// </summary>
        private List<GameObject> ShowTipsList;

        /// <summary>
        /// 飘窗生成的起始点
        /// </summary>
        public Vector3 InitialPostion = new Vector3(0f, 0f, 0f);

        private ShowTipsManager()
        {
            if (ShowTipsList == null)
            {
                ShowTipsList = new List<GameObject>();
            }
        }

        /// <summary>
        /// 初始化飘窗，游戏运行后必须调用一次
        /// </summary>
        /// <param name="parent">飘窗元素的父对象</param>
        /// <param name="showtipPrefab">飘窗预置</param>
        public void InitShowTips(GameObject parent, GameObject showtipPrefab)
        {
            int count = ShowTipsList.Count;
            if (count < MAXCOUNT)
            {
                for (int i = count; i < MAXCOUNT; i++)
                {
                    GameObject showtips = UGUITools.AddChild(parent, showtipPrefab);
                    showtips.transform.localPosition = InitialPostion;
                    showtips.SetActive(false);
                    ShowTipsList.Add(showtips);
                }
            }
        }

        /// <summary>
        /// 测试用例
        /// </summary>
        public void ShowTest()
        {
            string message = "Gaming Time is " + Time.time;
            SetShowTips(message);
        }

        /// <summary>
        /// 显示文字信息
        /// </summary>
        /// <param name="message"></param>
        public void SetShowTips(string message)
        {
            GameObject obj = GetNewShowTips();
            if (null == obj)
            {
                return;
            }

            obj.SetActive(true);

            ShowTips showtip = obj.GetComponent<ShowTips>();
            if (showtip)
            {
                showtip.SetTipInfo(message);
            }
        }

        /// <summary>
        /// 在屏幕的顶部显示下拉框，然后在预先设定的时间消失
        /// </summary>
        /// <param name="msg">要显示的信息</param>
        public void ShowTopTipsMsgBox(string msg)
        {
            GlobalTopTipsMsgBox.Instance.AddGlobalTopTipsMsgBox(msg);
        }

        /// <summary>
        /// 横幅全局消息框添加新消息
        /// </summary>
        /// <param name="msg">要显示的信息</param>
        public void ShowScrollMsgBox(string msg)
        {
            GlobalScollMsgBox.Instance.AddNewGlobalScollMsg(msg);
        }

        /// <summary>
        /// 中间的弹出提示框
        /// </summary>
        /// <param name="msg">要显示的信息</param>
        /// <param name="activeTime">持续的时间</param>
        public void ShowContinousMsgBox(string msg, float activeTime = 2f)
        {
            GlobalContinuousMsgBox.Instance.AddGlobalContinuousMsgBox(msg, activeTime);
        }

        /// <summary>
        /// 显示飘窗功能
        /// </summary>
        /// <param name="msg">要显示的信息</param>
        public void ShowFlutterMsgbox(string msg)
        {
            SetShowTips(msg);
        }

        /// <summary>
        /// 根据错误号ID显示信息
        /// </summary>
        /// <param name="ErrorId"></param>
        public void SetShowTips(int ErrorId)
        {

        }

        /// <summary>
        /// 获取一个可用的飘窗实例
        /// </summary>
        /// <returns></returns>
        private GameObject GetNewShowTips()
        {
            if (ShowTipsList.Count <= 0)
            {
                return null;
            }

            for (int i = 0; i < ShowTipsList.Count; i++)
            {
                if (false == ShowTipsList[i].activeSelf)
                {
                    return ShowTipsList[i];
                }
            }

            return null;
        }
    }
}
