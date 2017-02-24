using UnityEngine;
using System.Collections;

namespace Framework
{
    public abstract class State
    {
        /// <summary>
        /// 状态枚举
        /// </summary>
        /// <returns></returns>
        public EnumState PlayerState = EnumState.NoneState;

        /// <summary>
        /// 状态回调
        /// </summary>
        /// <param name="state"></param>
        public delegate void StateCallback(EnumState state);
        public StateCallback onStateEnter;

        /// <summary>
        /// 进入状态回调此方法
        /// </summary>
        /// <param name="stateMachine">控制此状态的状态机</param>
        /// <param name="prevState">进入此状态的前状态</param>
        public virtual void OnEnter(StateMachine stateMachine, params object[] objs)
        {

        }

        /// <summary>
        /// 离开状态时回调此方法
        /// </summary>
        /// <param name="nextState">离开此状态后的下一状态</param>
        public virtual void OnLeave(params object[] objs)
        {

        }

        /// <summary>
        /// 每帧的OnUpdate方法回调
        /// </summary>
        public virtual void OnUpdate()
        {

        }

        /// <summary>
        /// 每帧的FixedUpdate回调
        /// </summary>
        public virtual void OnFixedUpdate()
        {

        }

        /// <summary>
        /// 每帧的LateUpdate回调
        /// </summary>
        public virtual void OnLateUpdate()
        {

        }

        /// <summary>
        /// 是否允许相同的状态重入
        /// </summary>
        /// <returns></returns>
        public virtual bool AllowSameTransition()
        {
            return false;
        }     
        
        /// <summary>
        /// 可以转移到那些状态
        /// </summary>
        /// <param name="targetState"></param>
        /// <returns></returns>
        public virtual bool CanTransitionTo( EnumState targetState )
        {
            return true;
        }

        /// <summary>
        /// 获取这个状态的NormalizedTime 仅小数点后有效
        /// </summary>
        /// <returns></returns>
        public virtual float GetNormalizedTime()
        {
            return 0;
        }
    }
}
