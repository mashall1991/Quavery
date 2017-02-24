using UnityEngine;
using System.Collections;

public class UIPanelInfo
{
    public string UIPanelPath { get; private set; }//界面预设路径
    public string UIPanelName { get; private set; }//界面名字

    public UIPanelInfo(string path)
    {
        UIPanelPath = path;
        UIPanelName = path.Substring(path.LastIndexOf("/") + 1);
    }

    //需要管理的界面路径，需要再添加
    public static readonly UIPanelInfo MainMenu = new UIPanelInfo("UIFramTest/View/MainMenuView");//    主菜单
    public static readonly UIPanelInfo Panel1 = new UIPanelInfo("UIFramTest/View/Panel1");
    public static readonly UIPanelInfo Panel2 = new UIPanelInfo("UIFramTest/View/Panel2");
}
