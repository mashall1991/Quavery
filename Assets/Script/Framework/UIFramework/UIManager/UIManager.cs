using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Framework;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<UIPanelInfo, BasePanel> panelDic;
    private Stack<BasePanel> panelStack;
    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }

            return canvasTransform;
        }
    }

    public void PushPanel(UIPanelInfo panelInfo)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
            Debug.Log("暂停" + topPanel.name);
        }
        BasePanel panel = GetPanel(panelInfo);
        panel.OnEnter();
        Debug.Log("进入" + panel.name);
        panelStack.Push(panel);
    }

    public void PopPanel()
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        if (panelStack.Count <= 0)
        {
            return;
        }
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();
        Debug.Log("退出界面" + topPanel.name + "..................");
        if (panelStack.Count <= 0)
        {
            return;
        }
        BasePanel topPanel2 = panelStack.Peek();
        topPanel2.OnResume();
        Debug.Log("恢复" + topPanel2.name);
    }
    private BasePanel GetPanel(UIPanelInfo panelInfo)
    {
        if (panelDic == null)
        {
            panelDic = new Dictionary<UIPanelInfo, BasePanel>();
        }
        Debug.Log("panelDic字典的个数为" + panelDic.Count);
        BasePanel panel = null;
        panelDic.TryGetValue(panelInfo, out panel);
        if (panel == null)
        {
            GameObject instPanel = AddUIWidghtToUIPanel(panelInfo.UIPanelPath, panelInfo.UIPanelName, CanvasTransform);
            BasePanel temp = instPanel.GetComponent<BasePanel>();
            panelDic.Add(panelInfo, temp);

            return temp;
        }
        else
        {
            return panel;
        }
    }
    private GameObject AddUIWidghtToUIPanel(string objPath, string objName, Transform trans_parent)
    {
        GameObject go = GameObject.Instantiate(Resources.Load(objPath) as GameObject);
        go.name = objName;
        go.transform.SetParent(trans_parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.transform.localEulerAngles = Vector3.zero;

        return go;
    }
}
