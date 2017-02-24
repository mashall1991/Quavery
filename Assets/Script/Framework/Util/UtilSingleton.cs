
using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

namespace Framework
{
    /// <summary>
    /// MonoBehaviour�����汾
    /// </summary>
    /// <typeparam name="T">���Ͳ���</typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // ����
        private static T instance;

        /// <summary>
        /// ���ö����ʵ��
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
    /// ��Ϊ��Ϸ������ʾ��MonoBehaviour�����汾
    /// </summary>
    /// <typeparam name="T">���Ͳ���</typeparam>
    public class MonoSingletonAsGameObject<T> : MonoBehaviour where T : MonoBehaviour
    {
        // ����
        private static T instance;

        // ��������Ϸ����
        protected static GameObject singleton { get; set; }

        public static T Instance
        {
            get
            {
                if (null == instance)
                {
                    // ����new T��ԭ����monoBehaviour�����಻������Ĭ�Ϲ��캯��
                    // ͬʱҲ�Ǳ�֤TΪ��������ΪT�Ĺ�����unity������ɵ�
                    singleton = new GameObject();

                    // ���µĿն����й���ű�
                    instance = singleton.AddComponent<T>();

                    // Ĭ��ʹ���������
                    singleton.name = instance.GetType().ToString();

                    // ȥ�����ƿռ�ǰ׺
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
    /// ��ͨ����ģʽ�ķ�װ
    /// </summary>
    /// <typeparam name="T">ϣ����Ϊ������������</typeparam>
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
        /// �������ʽ����д��
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

                // ��ȡ�޲ι��캯��(˽�еĻ��߹��е�)
                ConstructorInfo[] constructorInfoArray = type.GetConstructors(BindingFlags.Instance
                                                                              | BindingFlags.NonPublic
                                                                              | BindingFlags.Public);

                ConstructorInfo noParameterConstructorInfo = null;

                foreach (var constructorInfo in constructorInfoArray)
                {
                    // Ѱ���޲����İ汾
                    ParameterInfo[] parameterInfoArray = constructorInfo.GetParameters();
                    if (0 == parameterInfoArray.Length)
                    {
                        noParameterConstructorInfo = constructorInfo;
                        break;
                    }
                }

                if (null == noParameterConstructorInfo)
                {
                    throw new NotSupportedException("û���ṩ�޲ι��캯��, ���ṩ!");
                }

                return (T)noParameterConstructorInfo.Invoke(null);
            }
        }
    }
}