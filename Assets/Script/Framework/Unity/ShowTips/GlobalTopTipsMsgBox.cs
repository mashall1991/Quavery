using UnityEngine;
using System.Collections;
using DG.Tweening;
using Framework;
using System.Collections.Generic;
using UnityEngine.UI;

public class GlobalTopTipsMsgBox : MonoSingleton<GlobalTopTipsMsgBox>
{
    /// <summary>
    /// label消息
    /// </summary>
    public Text LabelMessageInfo;

    /// <summary>
    /// toptips消息框的背景
    /// </summary>
    public GameObject SpriteBackGround;

    // 是否可以做动画
    private bool DoAnimation = false;

    //显示的文字信息列表
    private List<string> msgList = new List<string>();

    //记录当前是否正在显示一条文字
    private bool mIsActiveOneMsg = false;

    // 信息框原始位置
    private Vector3 mOriginalPosition;

    // 新显示一次提示文字的持续时间
    private float mActiveTime = 2f;

	void Start ()
    {
	    mOriginalPosition = transform.localPosition;
	}

    public void AddGlobalTopTipsMsgBox(string msg)
    {
        msgList.Add(msg);
    }
	
	void Update ()
    {
        if (msgList.Count > 0)
        {
            if (false == mIsActiveOneMsg)
            {
                DoAnimation = true;
                LabelMessageInfo.text = msgList[0];
                mActiveTime = 2f;
            }

            //开始显示文字提示
            if (DoAnimation && mActiveTime > 1f)
            {
                mIsActiveOneMsg = true;
                DoAnimation = false;
                SpriteBackGround.transform.DOLocalMove(new Vector3(0, 0, 0), 0.4f);
            }
            
            if (false == DoAnimation)
            {
                // 时间过期后, 还原位置
                mActiveTime -= Time.deltaTime;

                if (mActiveTime < 0)
                {
                    DoAnimation = true;
                    SpriteBackGround.transform.DOLocalMove(mOriginalPosition, 0.4f).OnComplete(OnTweenOver);
                }
            }
        }
	}

    /// <summary>
    /// 动画结束完成回调
    /// </summary>
    private void OnTweenOver()
    {
        if (msgList.Count > 0)
        {
            msgList.RemoveAt(0);
        }

        mIsActiveOneMsg = false;
    }
}
