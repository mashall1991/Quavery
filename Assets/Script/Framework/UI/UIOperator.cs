using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework.UI
{
    /// <summary>
    /// 包装对界面的操作，windows上的鼠标操作或者手机上的触屏操作
    /// </summary>
    public class UIOperator
    {
        // 鼠标左键点击的位置
        private Vector3 mMousePosition;

        /// <summary>
        /// 移动的方向
        /// </summary>
        public enum UIOperatorMoveDirection
        {
            DONT_MOVE,
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        /// <summary>
        /// 检查用户是否有按下动作
        /// </summary>
        /// <returns></returns>
        public bool CheckFingerDown()
        {
            // 鼠标左键按下的第一帧返回true
            if (Input.GetMouseButtonDown(0))
            {
                // 记录鼠标指针的屏幕位置
                mMousePosition = Input.mousePosition;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 设置是否可以多点触控
        /// </summary>
        /// <param name="multiTouchEnabled">是否可以多点触控</param>
        public void MultiTouchEnabled(bool multiTouchEnabled)
        {
            Input.multiTouchEnabled = multiTouchEnabled;
        }

        /// <summary>
        /// 检查用户是否有按下动作
        /// </summary>
        /// <returns></returns>
        public bool CheckFingerUp()
        {
            // 鼠标左键松开的第一帧返回true
            if (Input.GetMouseButtonUp(0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检查用户是否在移动手指
        /// </summary>
        /// <returns></returns>
        public bool CheckFingerMove()
        {
            // 鼠标左键按下期间一直返回true
            if (Input.GetMouseButton(0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取手指移动的距离
        /// </summary>
        /// <returns></returns>
        public UIOperatorMoveDirection GetFingerMovePosition()
        {
            // 获取手指抬起点的屏幕位置
            Vector3 pos = Input.mousePosition;

            if (Mathf.Approximately(mMousePosition.x, pos.x)
                && Mathf.Approximately(mMousePosition.y, pos.y))
            {
                return UIOperatorMoveDirection.DONT_MOVE;
            }

            // 判断手指是否水平在移动
            if (Mathf.Abs(mMousePosition.x - pos.x) > Mathf.Abs(mMousePosition.y - pos.y))
            {
                // 向左移动
                if (mMousePosition.x > pos.x)
                {
                    return UIOperatorMoveDirection.LEFT;
                }
                // 向右移动
                else
                {
                    return UIOperatorMoveDirection.RIGHT;
                }
            }
            // 手指垂直移动
            else
            {
                // 向下移动
                if (mMousePosition.y > pos.y)
                {
                    return UIOperatorMoveDirection.DOWN;
                }
                // 向上移动
                else
                {
                    return UIOperatorMoveDirection.UP;
                }
            }
        }

        /// <summary>
        /// 获取一帧时间内手指的水平移动增量
        /// </summary>
        /// <returns></returns>
        public float GetHorizontalMoveDeltaOneFrame()
        {
            return Input.GetAxis("Mouse X");
        }

        /// <summary>
        /// 获取一帧时间内手指的水平移动增量
        /// </summary>
        /// <returns></returns>
        public float GetVerticalMoveDeltaOneFrame()
        {
            return Input.GetAxis("Mouse Y");
        }

        /// <summary>
        /// 矩形包含
        /// </summary>
        /// <returns></returns>
        public bool RectangleContain(Rect rect)
        {
            return rect.Contains(Input.mousePosition);
        }
    }
}
