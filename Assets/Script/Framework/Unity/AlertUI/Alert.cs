using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace Framework.Unity
{
    public class Alert : MonoBehaviour
    {
        /// <summary>
        /// 取消按钮回调
        /// </summary>
        private Action cancelCallback;

        /// <summary>
        /// 确定按钮回调
        /// </summary>
        private Action confirmCallback;

        /// <summary>
        /// 取消按钮
        /// </summary>
        public GameObject cancelBtn;

        /// <summary>
        /// 确认按钮
        /// </summary>
        public GameObject confirmBtn;

        /// <summary>
        /// 确认按钮的Label
        /// </summary>
        public Text confirmNameLabel;

        /// <summary>
        /// 取消按钮的Label
        /// </summary>
        public Text cancelNameLabel;

        /// <summary>
        /// 弹出框的标题的Label
        /// </summary>
        public Text TitleLabel;

        /// <summary>
        /// 显示消息的Label
        /// </summary>
        public Text messageLabel;

        void Awake()
        {
            AddButtonEvent();
        }

        void Start()
        {
            
        }

        void Update()
        {

        }

        void OnDisable()
        {

            cancelCallback = null;
            confirmCallback = null;
        }

        void AddButtonEvent()
        {
            EventTriggerListener.Get(confirmBtn).onClick = OnConfirmClick;
            EventTriggerListener.Get(cancelBtn).onClick = OnCancelClick;
        }

        /// <summary>
        /// 确认按钮点击
        /// </summary>
        /// <param name="sender"></param>
        void OnConfirmClick(GameObject sender)
        {
            if (confirmCallback != null)
            {
                confirmCallback();
            }

            DestoryAlert();
        }

        /// <summary>
        /// 取消按钮点击
        /// </summary>
        /// <param name="sender"></param>
        void OnCancelClick(GameObject sender)
        {
            if (cancelCallback != null)
            {
                cancelCallback();
            }

            DestoryAlert();
        }

        /// <summary>
        /// 需要外层调用检查删除此对象的保存
        /// </summary>
        public void DestoryAlert()
        {
            DestroyImmediate(this.gameObject);
        }

        /// <summary>
        /// 对外接口，刷新警告窗口
        /// </summary>
        /// <param name="alertMsg">需要显示的信息</param>
        /// <param name="confrimName">确认按钮的按钮名称</param>
        /// <param name="cancelName">取消按钮的按钮名称</param>
        /// <param name="confirmCallback">确认按钮的回调</param>
        /// <param name="cancelCallback">取消按钮的回调</param>
        public void CreateAlert(string titleInfo,
                                string alertMsg,
                                string confrimName,
                                string cancelName,
                                Action confirmCallback,
                                Action cancelCallback)
        {
            if (string.IsNullOrEmpty(titleInfo))
            {
                titleInfo = "消息提示";
            }
            if(TitleLabel != null)
            {
                TitleLabel.text = titleInfo;
            }
            if (messageLabel!= null)
            {
                messageLabel.text = alertMsg;
            }
            if (cancelNameLabel != null)
            {
                cancelNameLabel.text = !string.IsNullOrEmpty(cancelName) ? cancelName : "取 消";
            }

            if (confirmNameLabel != null)
            {
                confirmNameLabel.text = !string.IsNullOrEmpty(confrimName) ? confrimName : "确 定";
            }

            this.cancelCallback = cancelCallback;

            this.confirmCallback = confirmCallback;

            // 没有cancel按钮的情况
            if (null == cancelCallback)
            {
                cancelBtn.SetActive(false);

                // 如果没有确认按钮的话，就把取消按钮居中显示
                Vector3 postion = confirmBtn.transform.localPosition;
                postion.x = 0f;
                confirmBtn.transform.localPosition = postion;
            }

            //放于最后，便于显示在最上层 
            this.GetComponent<RectTransform>().SetAsLastSibling();
        }
    }
}
