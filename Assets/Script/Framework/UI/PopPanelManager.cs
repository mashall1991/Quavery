using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework.UI
{
    /// <summary>
    /// 二级弹出框的管理器类
    /// </summary>
    public class PopPanelManager
    {
        /// <summary>
        /// 添加二级弹出框到相应的位置
        /// </summary>
        /// <param name="addWidgetName">要动态生成的弹出框预置</param>
        /// <param name="PopPanel">二级弹出框管理节点</param>
        /// <returns>动态产生的二级弹出框</returns>
        public static GameObject AddPopWidgetToPopPanel(string prefabpath, string addWidgetName, GameObject PopPanel)
        {
            UnityEngine.Object o = Resources.Load(prefabpath);
            GameObject go = UnityEngine.Object.Instantiate(o) as GameObject;

            go.name = addWidgetName;
            go.transform.parent = PopPanel.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;

            return go;
        }
    }
}
