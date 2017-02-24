using UnityEngine;
using System.Collections;

public abstract class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 进入panel
    /// </summary>
    public virtual void OnEnter()
    {

    }

    /// <summary>
    /// 暂停panel
    /// </summary>
    public virtual void OnPause()
    {

    }

    /// <summary>
    /// 恢复panel
    /// </summary>
    public virtual void OnResume()
    {

    }

    /// <summary>
    /// 退出panel
    /// </summary>
    public virtual void OnExit()
    {

    }
}
