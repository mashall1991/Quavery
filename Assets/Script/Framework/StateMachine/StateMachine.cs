using UnityEngine;
using System.Collections;
using Framework;

public class StateMachine
{
    /// <summary>
    /// 当前状态 动作层级 TODO：后期重构为多层状态机
    /// </summary>
    public State CurrActionLayerState { get; private set; }

    /// <summary>
    /// 前一个状态
    /// </summary>
    public State PreviousActionLayerState { get; private set; }

    /// <summary>
    /// 全局状态 移动层级 TODO：后期重构为多层状态机
    /// </summary>
    public State CurrMotionLayerState { get; private set; }

    public StateMachine()
    {
        CurrActionLayerState = null;
        PreviousActionLayerState = null;
        CurrMotionLayerState = null;
    }

    public StateMachine(State current,params object[] objs)
    {
        //currentState = current;
        PreviousActionLayerState = null;
        CurrMotionLayerState = null;
        ChangeState(current, objs);
    }

    /// <summary>
    /// 跳转全局状态
    /// </summary>
    /// <param name="globalState"></param>
    /// <param name="objs"></param>
    public void ChangeGlobalState(State globalState,params object[] objs)
    {
        //判断状态合法性
        if (!CheckGlobalStateAvaliable(globalState))
        {
            return;
        }        

        if (null != this.CurrMotionLayerState)
        {
            if (!CurrMotionLayerState.CanTransitionTo(globalState.PlayerState))
                return;
            CurrMotionLayerState.OnLeave();
        }

        this.CurrMotionLayerState = globalState;
        this.CurrMotionLayerState.OnEnter(this, objs);
    }

    /// <summary>
    /// 跳转状态
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="objs"></param>
    public void ChangeState(State newState, params object[] objs)
    {
        //判断状态合法性
        if (!CheckNewStateAvaliable(newState))
        {
            return;
        }

        //缓存上一个状态
        if (null != CurrActionLayerState)
        {
            if (!CurrActionLayerState.CanTransitionTo(newState.PlayerState))
                return;

            CurrActionLayerState.OnLeave();
            PreviousActionLayerState = CurrActionLayerState;
        }

        CurrActionLayerState = newState;
        CurrActionLayerState.OnEnter(this,objs);
    }

    /// <summary>
    /// 跳转到上一个状态
    /// </summary>
    /// <param name="objs"></param>
    public void ChangePriviousState(params object[] objs)
    {
        ChangeState(PreviousActionLayerState,objs);
    }

    /// <summary>
    /// 检索状态合法性
    /// </summary>
    /// <param name="newState"></param>
    /// <returns></returns>
    private bool CheckNewStateAvaliable(State newState)
    {
        if (null == newState || EnumState.NoneState == newState.PlayerState || (null != CurrActionLayerState && CurrActionLayerState.PlayerState == newState.PlayerState && !CurrActionLayerState.AllowSameTransition() ))
        {
            string warningMsg = "尝试切换到空状态,错误的使用 to state:" + newState == null ? "null" : newState.ToString();

            //待后期需要关掉此层打印
            Debug.LogWarning(warningMsg);
            return false;
        }
        return true;
    }

    /// <summary>
    /// 检索状态合法性
    /// </summary>
    /// <param name="newState"></param>
    /// <returns></returns>
    private bool CheckGlobalStateAvaliable(State globalState)
    {
        if (null == globalState || EnumState.NoneState == globalState.PlayerState || (null != this.CurrMotionLayerState && this.CurrMotionLayerState.PlayerState == globalState.PlayerState && !this.CurrMotionLayerState.AllowSameTransition() )) //增加配置改状态是否允许重入
        {
            string warningMsg = "尝试将全局状态切换到空状态或者相同状态,新状态是,若有必要请谨慎使用";

            //待后期需要关掉此层打印
            Debug.LogWarning(warningMsg);
            return false;
        }
        return true;
    }

    /// <summary>
    /// 每帧的Update方法回调
    /// 在外层的updata方法中调用
    /// </summary>
    public virtual void Update()
    {
        if(null != CurrMotionLayerState)
        {
            CurrMotionLayerState.OnUpdate();
        }

        if(null != CurrActionLayerState)
        {
            CurrActionLayerState.OnUpdate();
        }
    }

    /// <summary>
    /// 每帧的FixedUpdate回调
    /// </summary>
    public virtual void FixedUpdate()
    {
        if (null != CurrMotionLayerState)
        {
            CurrMotionLayerState.OnFixedUpdate();
        }

        if (null != CurrActionLayerState)
        {
            CurrActionLayerState.OnFixedUpdate();
        }
    }

    /// <summary>
    /// 每帧的LateUpdate回调
    /// </summary>
    public virtual void LateUpdate()
    {
        if (null != CurrMotionLayerState)
        {
            CurrMotionLayerState.OnLateUpdate();
        }

        if (null != CurrActionLayerState)
        {
            CurrActionLayerState.OnLateUpdate();
        }
    }

    /// <summary>
    /// 强制进入某个状态
    /// </summary>
    /// <param name="state"></param>
    /// <param name="objs"></param>
    public virtual void SetActionState( State state, params object[] objs)
    {
        if(CurrActionLayerState != null)
        {
            CurrActionLayerState.OnLeave();
        }
        CurrActionLayerState = state;
        CurrActionLayerState.OnEnter(this, objs);
    }
}
