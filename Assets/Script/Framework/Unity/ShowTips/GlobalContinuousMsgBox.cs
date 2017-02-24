using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GlobalContinuousMsgBox : Framework.MonoSingleton<GlobalContinuousMsgBox> 
{
    /// <summary>
    /// 控制显示的sprite
    /// <summary>
    public GameObject BackgroundSprite;

    /// <summary>
    /// 需要显示的文字
    /// <summary>
    public Text MsgInfoLabel;

    /// <summary>
    /// 控制一直显示
    /// <summary>
    private bool ActiveTrue = false;


    private float ActiveTime = -1f;

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

	void Start () 
    {
	
	}

    void OnDestroy()
    {

    }
	
	void Update () 
    {
	    if(ActiveTime >= 0)
        {
            ActiveTime -= Time.deltaTime;            
        }

        if (ActiveTime < 0 && false == ActiveTrue)
        {
            BackgroundSprite.SetActive(false);
        }     
	}

    /// <summary>
    /// 提供给外层调用，添加新消息
    /// </summary>
    /// <param name="msg">要显示的信息</param>
    /// <param name="activeTime">持续的时间</param>
    public void AddGlobalContinuousMsgBox(string msgInfo, float activeTime)
    {
        MsgInfoLabel.text = msgInfo;
        ActiveTime = activeTime;
        BackgroundSprite.SetActive(true);
    }

    /// <summary>
    /// 提供给外层调用，让提示框一直显示，直到主动关闭
    /// </summary>
    public void AddGlobalContinuousMsgBox(string msgInfo, bool activeTrue)
    {
        ActiveTrue = true;
        MsgInfoLabel.text = msgInfo;
        BackgroundSprite.SetActive(true);
    }

    /// <summary>
    /// 提供给外层调用，立即停止显示持续消息框
    /// </summary>
    public void StopGlobalContinuousMsgBox()
    {
        ActiveTrue = false;
        ActiveTime = -1f;
    }

}
