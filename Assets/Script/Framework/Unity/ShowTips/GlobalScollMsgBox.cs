using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class GlobalScollMsgBox : Framework.MonoSingleton<GlobalScollMsgBox> 
{
    /// <summary>
    /// 显示消息的label
    /// <summary>
    public List<Text> mLabelList;

    /// <summary>
    /// 背景
    /// <summary>
    public Image mBackGroundSprite;

    //是否可以滑动一个新的消息
    private bool mCanMoveNewMsg = true;

    //记录下一个显示文字用的label的索引
    private int CanSelectIndex = 0;
    
    //提示文字信息的列表
    private List<string> msgList;

    /// <summary>
    /// 添加滚动文字提示
    /// </summary>
    /// <param name="msg">需要显示的信息</param>
    public void AddNewGlobalScollMsg(string msg)
    {
        msgList.Add(msg);
        msgList.Add(msg);
    }

    /// <summary>
    /// 设置系统公告框的位置
    /// </summary>
    /// <param name="position">目标位置</param>
    public void SetPosition(Vector3 position)
    {
        // 世界坐标系
        transform.position = position;
    }

    void OnEnable()
    {
        msgList = new List<string>();

        mBackGroundSprite.gameObject.SetActive(false);
    }

    void OnDisable()
    {

    }

    void Start()
    {

    }

	void Update () 
    {
        if (msgList.Count > 0 && mCanMoveNewMsg)
        {
            mBackGroundSprite.gameObject.SetActive(true);
            mCanMoveNewMsg = false;

            Text mLabelMessageInfo = mLabelList[CanSelectIndex];
            mLabelMessageInfo.gameObject.SetActive(true);
            mLabelMessageInfo.text = msgList[0];
            //mLabelMessageInfo.transform.localPosition = new Vector3(mBackGroundSprite.width / 2 + mLabelMessageInfo.width / 2, mLabelMessageInfo.transform.localPosition.y, 0);           

            ////先要计算出这次移动的距离范围有多大，然后再计算出一共需要移动的时间
            //float widthX = mLabelMessageInfo.width / 2 + mBackGroundSprite.width / 2;

            //以2000移动12s为单位,x每多移动1个像素,则需要0.006s
            //float moveTimes = widthX * 0.006f;

            ////开始移动
            //mLabelMessageInfo.transform.DOLocalMoveX(-widthX, moveTimes).SetEase(Ease.Linear).OnComplete(OnCheckCloseScollMsgBox);
            
            //这里主要是为实现还未移动完就出现第二条文字信息
            //mLabelMessageInfo.transform.DOLocalMoveZ(0, moveTimes / 2).OnComplete(OnOneMsgMoveComplete);
        }
	}

    //通过回调执行显示下一条文字信息,并清空一条文字数据
    void OnOneMsgMoveComplete()
    {
        CanSelectIndex++;
        if (CanSelectIndex == 3)
        {
            CanSelectIndex = 0;
        }

        mCanMoveNewMsg = true;
        msgList.RemoveAt(0);
    }

    //检查文字移动完以后是否需要关闭scoll消息框
    void OnCheckCloseScollMsgBox()
    {
        //如果列表已经没有数据了, 就将文字框设置为不显示
        if (msgList.Count == 0 && mBackGroundSprite.gameObject.activeSelf)
        {
            StartCoroutine(SetBackGroundNoActive());
        }
    }

    IEnumerator SetBackGroundNoActive()
    {
        yield return new WaitForSeconds(1f);

        mBackGroundSprite.gameObject.SetActive(false);
    }
}
