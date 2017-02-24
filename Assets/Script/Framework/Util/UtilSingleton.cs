
using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

namespace Framework
{
    /// <summary>
    /// MonoBehaviour单例版本
    /// </summary>
    /// <typeparam name="T">泛型参数</typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // 单例
        private static T instance;

        /// <summary>
        /// 设置对象的实例
        /// </summary>
        protected virtual void Awake()
        {
            instance = this as T;
        }

        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        public void OnApplicationQuit()
        {
            instance = null;
        }
    }

    /// <summary>
    /// 作为游戏对象显示的MonoBehaviour单例版本
    /// </summary>
    /// <typeparam name="T">泛型参数</typeparam>
    public class MonoSingletonAsGameObject<T> : MonoBehaviour where T : MonoBehaviour
    {
        // 单例
        private static T instance;

        // 产生的游戏对象
        protected static GameObject singleton { get; set; }

        public static T Instance
        {
            get
            {
                if (null == instance)
                {
                    // 不是new T的原因是monoBehaviour的子类不可以有默认构造函数
                    // 同时也是保证T为单例，因为T的构造是unity引擎完成的
                    singleton = new GameObject();

                    // 在新的空对象中挂入脚本
                    instance = singleton.AddComponent<T>();

                    // 默认使用类的名字
                    singleton.name = instance.GetType().ToString();

                    // 去掉名称空间前缀
                    int index = singleton.name.LastIndexOf(".");
                    if (-1 != index)
                    {
                        singleton.name = singleton.name.Substring(index + 1);
                    }
                }

                return instance;
            }
        }

        public void OnApplicationQuit()
        {
            instance = null;
            singleton = null;
        }
    }

    /// <summary>
    /// 普通单例模式的封装
    /// </summary>
    /// <typeparam name="T">希望成为单例的类类型</typeparam>
    public class Singleton<T> where T : Singleton<T>
    {
        public static T Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        public void ReleaseInstance()
        {
            Nested.ReleaseInstance();
        }

        /// <summary>
        /// 完成懒汉式单例写法
        /// </summary>
        private class Nested
        {
            internal static T instance = CreateInstance();

            internal static void ReleaseInstance()
            {
                instance = null;
            }

            private static T CreateInstance()
            {
                Type type = typeof(T);

                // 获取无参构造函数(私有的或者共有的)
                ConstructorInfo[] constructorInfoArray = type.GetConstructors(BindingFlags.Instance
                                                                              | BindingFlags.NonPublic
                                                                              | BindingFlags.Public);

                ConstructorInfo noParameterConstructorInfo = null;

                foreach (var constructorInfo in constructorInfoArray)
                {
                    // 寻找无参数的版本
                    ParameterInfo[] parameterInfoArray = constructorInfo.GetParameters();
                    if (0 == parameterInfoArray.Length)
                    {
                        noParameterConstructorInfo = constructorInfo;
                        break;
                    }
                }

                if (null == noParameterConstructorInfo)
                {
                    throw new NotSupportedException("没有提供无参构造函数, 请提供!");
                }

                return (T)noParameterConstructorInfo.Invoke(null);
            }
        }
    }
}