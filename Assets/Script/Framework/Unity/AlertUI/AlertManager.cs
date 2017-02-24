using UnityEngine;
using System.Collections;
using System;

namespace Framework.Unity
{
    public static class AlertManager
    {
        /// <summary>
        /// 用于判定初始化是否完成，如果没完成就不会出现预制
        /// </summary>
        private static bool isInitAlert = false;

        /// <summary>
        /// 预制的父类对象
        /// </summary>
        private static GameObject mParent;

        /// <summary>
        /// 预制的父类对象
        /// </summary>
        private static GameObject mPrefab;

        /// <summary>
        /// 初始化函数，必须执行一次后才可以生成警告窗口
        /// </summary>
        /// <param name="parent">父对象</param>
        /// <param name="prefab">预置</param>
        public static void InitAlertManager(GameObject parent, GameObject prefab)
        {
            if (isInitAlert)
            {
                return;
            }

            mParent = parent;
            mPrefab = prefab;

            isInitAlert = true;
        }

        /// <summary>
        /// 显示警告窗口，默认使用“确定”，“取消”的按钮
        /// </summary>
        /// <param name="msg">警告消息</param>
        /// <param name="confirmCallback">确认按钮回调</param>
        /// <param name="cancelCallback">取消按钮回调</param>
        public static void ShowAlert(string msg, Action confirmCallback, Action cancelCallback = null)
        {
            Alert alert = GetNewAlert();
            if (alert != null)
            {
                alert.CreateAlert(null, msg, null, null, confirmCallback, cancelCallback);
            }
        }

        /// <summary>
        /// 显示警告窗口，对外提供可以更改确认取消按钮名字的接口
        /// </summary>
        /// <param name="msg">警告消息</param>
        /// <param name="confirmName">确认按钮新名字</param>
        /// <param name="cancelName">取消按钮新名字</param>
        /// <param name="confirmCallback">确认按钮回调</param>
        /// <param name="cancelCallback">取消按钮回调</param>
        public static void ShowAlert(string msg,
                              string confirmName,
                              string cancelName,
                              Action confirmCallback,
                              Action cancelCallback = null)
        {
            Alert alert = GetNewAlert();
            if (alert != null)
            {
                alert.CreateAlert(null, msg, confirmName, cancelName, confirmCallback, cancelCallback);
            }
        }

        /// <summary>
        /// 显示警告窗口，对外提供可以更改确认取消按钮名字的接口
        /// </summary>
        /// <param name="msg">警告消息</param>
        /// <param name="confirmName">确认按钮新名字</param>
        /// <param name="cancelName">取消按钮新名字</param>
        /// <param name="confirmCallback">确认按钮回调</param>
        /// <param name="cancelCallback">取消按钮回调</param>
        public static void ShowAlert(string titleinfo,
                              string msg,
                              string confirmName,
                              string cancelName,
                              Action confirmCallback,
                              Action cancelCallback = null)
        {
            Alert alert = GetNewAlert();
            if (alert != null)
            {
                alert.CreateAlert(titleinfo, msg, confirmName, cancelName, confirmCallback, cancelCallback);
            }
        }


        /// <summary>
        /// 显示警告窗口，只是文字信息处理
        /// </summary>
        /// <param name="msg">警告信息</param>
        public static void ShowAlert(string msg)
        {
            Alert alert = GetNewAlert();
            if (alert != null)
            {
                alert.CreateAlert(null, msg, null, null, null, null);
            }
        }

        /// <summary>
        /// 生成一个新的警告窗口
        /// </summary>
        /// <returns>警告对象</returns>
        private static Alert GetNewAlert()
        {
            if (false == isInitAlert)
            {
                return null;
            }

            GameObject obj = UGUITools.AddChild(mParent, mPrefab);

            obj.SetActive(true);
            return obj.GetComponent<Alert>();
        }
    }
}
