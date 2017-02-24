using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

namespace Framework
{
    /// <summary>
    /// 将unity主线程的任务推送给线程池处理，等线程池处理完以后，
    /// 将结果又推送回unity主线程
    /// </summary>
    /// <example>
    /// void ScaleMesh(Mesh mesh, float scale)
    /// {
    ///     // Get the vertices of a mesh
    ///     var vertices = mesh.vertices;
    ///     
    ///     // 推送到线程池线程去处理工作
    ///     Loom.RunAsync( () => {
    ///         // Loop through the vertices
    ///         for(var i = 0; i < vertices.Length; i++)
    ///         {
    ///             // Scale the vertex
    ///             vertices[i] = vertices[i] * scale;
    ///         }
    ///         
    ///         // 等处理完以后又推送给unity主线程处理
    ///         Loom.QueueOnMainThread( () => {
    ///             // Set the vertices
    ///             mesh.vertices = vertices;
    ///             // Recalculate the bounds
    ///             mesh.RecalculateBounds();
    ///         }); 
    ///     });
    /// }
    /// </example>
    public class UtilTreadLoom : MonoBehaviour
    {
        // 最大线程的数量
        public static int maxThreads = 8;
        // 线程数量
        static int numThreads;

        // 需要线程池线程立即执行的回调函数列表
        private List<Action> _actions = new List<Action>();

        // 单例模式
        private static UtilTreadLoom mCurrentLoom;

        /// <summary>
        /// 单例模式
        /// </summary>
        public static UtilTreadLoom CurrentLoom
        {
            get
            {
                Initialize();

                return mCurrentLoom;
            }
        }

        void Awake()
        {
            mCurrentLoom = this;
            initialized = true;
        }

        static bool initialized;

        static void Initialize()
        {
            if (false == initialized)
            {
                initialized = true;

                // monobehavior单例模式的写法
                var loomgameobject = new GameObject("Loom");
                mCurrentLoom = loomgameobject.AddComponent<UtilTreadLoom>();
            }
        }

        /// <summary>
        /// 要线程池进行的工作任务
        /// </summary>
        public struct DelayedQueueItem
        {
            /// <summary>
            /// 处理action回调的延时时间
            /// </summary>
            public float time;

            /// <summary>
            /// 无参无返回值的回调函数
            /// </summary>
            public Action action;
        }

        // 工作任务列表
        private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

        List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

        public static void QueueOnMainThread(Action action)
        {
            QueueOnMainThread(action, 0f);
        }

        /// <summary>
        /// 推入unity主线程中处理
        /// </summary>
        /// <param name="action">主线程的回到函数</param>
        /// <param name="time">延时时间</param>
        public static void QueueOnMainThread(Action action, float time)
        {
            // time不等于0f的情况，表示要延时处理回调函数
            if (false == Mathf.Approximately(time, 0f))
            {
                lock (CurrentLoom._delayed)
                {
                    // Time.time表示: 从游戏开始到到现在所用的时间
                    CurrentLoom._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                }
            }
            else
            {
                // 加入无延时的工作队列
                lock (CurrentLoom._actions)
                {
                    CurrentLoom._actions.Add(action);
                }
            }
        }

        /// <summary>
        /// 异步的去执行一个回调函数
        /// </summary>
        /// <param name="a">回调函数</param>
        /// <returns></returns>
        public static void RunAsync(Action action)
        {
            Initialize();

            // 检查是否大于最大线程数量
            while (numThreads >= maxThreads)
            {
                Thread.Sleep(1);
            }

            Interlocked.Increment(ref numThreads);
            // 推入线程池去执行
            ThreadPool.QueueUserWorkItem(RunAction, action);
        }

        /// <summary>
        /// 立即去执行一个回调函数
        /// </summary>
        /// <param name="action">回调函数</param>
        private static void RunAction(object action)
        {
            try
            {
                // 由线程池回调工作函数
                ((Action)action)();
            }
            catch
            {
            }
            finally
            {
                Interlocked.Decrement(ref numThreads);
            }
        }

        /// <summary>
        /// 禁用脚本的时候触发
        /// </summary>
        void OnDisable()
        {
            if (mCurrentLoom == this)
            {
                mCurrentLoom = null;
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        List<Action> _currentActions = new List<Action>();

        /// <summary>
        /// 使用unity主线程调用回调方法
        /// </summary>
        void Update()
        {
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }

            foreach (var action in _currentActions)
            {
                action();
            }

            lock (_delayed)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));

                foreach (var item in _currentDelayed)
                {
                    _delayed.Remove(item);
                }
            }

            foreach (var delayed in _currentDelayed)
            {
                delayed.action();
            }
        }
    }
}
