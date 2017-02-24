using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class Panel1 : BasePanel
{
    public Button[] mBtns;
    public Image mImage;
    private Tweener mTweener1;
    private Tweener mTweener2;
    public override void OnEnter()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        EventTriggerListener.Get(mBtns[0].gameObject).onClick = NextBtnOnClick;
        EventTriggerListener.Get(mBtns[1].gameObject).onClick = BackBtnOnClick;
        mImageDoScale1();
    }

    private void NextBtnOnClick(GameObject go)
    {
        UIManager.Instance.PushPanel(UIPanelInfo.Panel2);
    }

    private void BackBtnOnClick(GameObject go)
    {
        UIManager.Instance.PopPanel();
    }

    public override void OnPause()
    {
        mTweener1.SetUpdate(false);
        mTweener2.SetUpdate(false);
        for (int i = 0; i < mBtns.Length; i++)
        {
            mBtns[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
    public override void OnResume()
    {
        mTweener1.SetUpdate(true);
        mTweener2.SetUpdate(true);
        for (int i = 0; i < mBtns.Length; i++)
        {
            mBtns[i].GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
    private void mImageDoScale1()
    {
        mTweener1 = mImage.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f).SetEase(Ease.InOutSine).OnComplete(mImageDoScale2).SetUpdate(true);
    }
    private void mImageDoScale2()
    {
        mTweener2 = mImage.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1f).SetEase(Ease.InOutSine).OnComplete(mImageDoScale1).SetUpdate(true);
    }
    private void OnClickBtn(GameObject go)
    {
        UIManager.Instance.PushPanel(UIPanelInfo.Panel1);
    }
}
