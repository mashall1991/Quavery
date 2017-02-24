using UnityEngine;
using System.Collections;
using System;

public enum EnumState
{

    NoneState = -1,

    AnyState,

    IdleState,

   
    #region 动作
    //抱的动作
    HugAction,

    //攻击动作
    Puntch,

    //被打到，产生位移
    BeingPuntched,

    HoldBomb,

    HoldPlayer,

    BeingHeld,

    Droping,

    Curse,

    Victory,

    GetUpBack,

    GetUpFront,

    #endregion
    #region 全局状态    

    /// <summary>
    ///旋转 idle-》raotate-》walk
    /// </summary>
    Rotate,

    Walk,

    Run,
    

    //jump的4个子状态
    JumpingStatePrepare, //准备
    JumpingStateFlow, //起飞 
    JumpingStateDrop,//下落
    JumpingStateLanded, //落地缓冲    

    Ragdoll,
    //停止移动
    StopMoving,
    //[Obsolete("弃用")]
    Jump = -2,
    Fail,
    #endregion
}
