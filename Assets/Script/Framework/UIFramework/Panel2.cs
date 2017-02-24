using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class Panel2 : BasePanel
{
    public Button mBtn;
    public Image mImage;
    private Tweener mTweener1;
    private Tweener mTweener2;
    public override void OnEnter()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        EventTriggerListener.Get(mBtn.gameObject).onClick = BackBtnOnClick;
        mImageDoScale1();
    }

    private void BackBtnOnClick(GameObject go)
    {
        UIManager.Instance.PopPanel();
    }

    public override void OnPause()
    {

    }
    public override void OnResume()
    {

    }
    private void mImageDoScale1()
    {
        mTweener1 = mImage.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f).SetEase(Ease.InOutSine).OnComplete(mImageDoScale2).SetUpdate(true);
    }
    private void mImageDoScale2()
    {
        mTweener2 = mImage.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1f).SetEase(Ease.InOutSine).OnComplete(mImageDoScale1).SetUpdate(true);
    }
    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
}
