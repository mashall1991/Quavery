using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class UGUIAnimation {

    /// <summary>
    /// 渐隐渐现动画
    /// </summary>
    /// <param name="rect">UGUI Transform</param>
    /// <param name="duration">持续时间</param>
    /// <param name="isIn">true出现 false消失</param>
	public static void FadeAnimation(RectTransform rect, float duration, bool isIn)
    {
        int value = isIn ? 1 : 0;
        Graphic[] list = rect.GetComponentsInChildren<Graphic>();
        for (int index = 0; index < list.Length; index++)
        {
            list[index].CrossFadeAlpha(value, duration,true);
        }
    }
    /// <summary>
    /// 伤害数值弹出，上移并放大
    /// </summary>
    public static void FadeScaleAnimation(RectTransform rect,float fadeDuration,Vector3 endPosition,float moveDuration,float scaleFactor,float scaleDuration,Ease ease = Ease.OutCubic)
    {
        MoveAndFadeAnimation(rect, endPosition, moveDuration, fadeDuration, true,ease);
        rect.DOScale(Vector3.one * scaleFactor, scaleDuration);
    }
    /// <summary>
    /// 上移并消失
    /// </summary>
    /// <param name="rect">RectTransform</param>
    /// <param name="endPosition">终点</param>
    /// <param name="moveDuration">移动时间</param>
    /// <param name="fadeDuration">渐隐/渐现时间</param>
    /// <param name="isIn">是否出现 true出现</param>
    public static void MoveAndFadeAnimation(RectTransform rect,Vector3 endPosition, float moveDuration,float fadeDuration,bool isIn,Ease ease = Ease.OutCubic)
    {
        FadeAnimation(rect, fadeDuration, isIn);
        Tween tween = rect.DOAnchorPos3D(endPosition, moveDuration);
        tween.SetEase(ease);
    }
}
