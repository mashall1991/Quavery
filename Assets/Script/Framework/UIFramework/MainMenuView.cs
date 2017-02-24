using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class MainMenuView : BasePanel
{
    public Image mImage;
    public Button mBtn;
    public override void OnEnter()
    {
        EventTriggerListener.Get(mBtn.gameObject).onClick = OnClickBtn;
        mImageDoScale1();
    }
    public override void OnPause()
    {
        mBtn.GetComponent<CanvasGroup>().blocksRaycasts = false;
        Time.timeScale = 0f;
    }
    public override void OnResume()
    {
        mBtn.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Time.timeScale = 1f;
    }
    public override void OnExit()
    {

    }
    private void mImageDoScale1()
    {
        mImage.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f).SetEase(Ease.InOutSine).OnComplete(mImageDoScale2);
    }
    private void mImageDoScale2()
    {
        mImage.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1f).SetEase(Ease.InOutSine).OnComplete(mImageDoScale1);
    }
    private void OnClickBtn(GameObject go)
    {
        UIManager.Instance.PushPanel(UIPanelInfo.Panel1);
    }
}
