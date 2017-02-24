using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace Framework
{
    public class SampleFactory
    {
        private static SampleFactory mFactoty;
        public static SampleFactory Instance
        {
            get
            {
                if (mFactoty == null)
                {
                    mFactoty = new SampleFactory();
                }
                return mFactoty;
            }
        }

        private SampleFactory()
        {
            mSampleList = new List<Sample>();
        }

        private List<Sample> mSampleList;

        /// <summary>
        /// 配置标根目录
        /// </summary>
        private string XMLPath = "Config/";

        public void InitSample<T>(string name) where T : Sample
        {
            try
            {
                //获取一次type
                Type type = typeof(T);

                //读取配置表
                TextAsset xmlText = Resources.Load(XMLPath + name) as TextAsset;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlText.text);

                //检索结点信息
                XmlElement ChildNode = (XmlElement)xmlDoc.DocumentElement.FirstChild;
                while (null != ChildNode)
                {
                    //生成对应实例
                    T sampleT = (T)type.Assembly.CreateInstance(type.FullName);
                    //初始化
                    sampleT.InitSample(ChildNode);

                    SaveSample(sampleT);
                    ChildNode = (XmlElement)ChildNode.NextSibling;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("加载xml文件{0}出错! ID={1}", name, e.ToString()));
            }
        }

        private void SaveSample(Sample sample)
        {
            int index = IsContainSample(sample);

            //不存在的元素
            if (index == -1)
            {
                mSampleList.Add(sample);
            }
            else
            {
                Debug.LogError(string.Format("id = {0}的元素重复，新元素名称是 {1}，类型是{2}，将会覆盖旧名称{3}类型是{4}的数据", 
                                            sample.Id,sample.Name,sample.GetType(), mSampleList[index].Name, mSampleList[index].GetType()));
                //覆盖为新元素
                mSampleList[index] = sample;
            }
        }

        /// <summary>
        /// 判断是否已经包含，并返回存在的索引
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        private int IsContainSample(Sample sample)
        {
            for (int i = 0; i < mSampleList.Count; i++)
            {
                if (sample.Id == mSampleList[i].Id)
                {
                    Debug.Log("出现重复元素,新元素是");
                    sample.DoDebug();
                    Debug.Log("旧元素是");
                    mSampleList[i].DoDebug();
                    return i;
                }
            }
            return -1;
        }

        public T GetSample<T>(int id) where T : Sample
        {
            foreach (var item in mSampleList)
            {
                if (item.Id == id)
                {
                    return (T)item;
                }
            }

            return null;
        }

        public T GetSample<T>(string name) where T : Sample
        {
            foreach (var item in mSampleList)
            {
                if (item.Name == name)
                {
                    return (T)item;
                }
            }
            return null;
        }

        public List<T> GetSamples<T>() where T : Sample
        {
            List<T> samples = new List<T>();
            foreach (var item in mSampleList)
            {
                if (item is T)
                {
                    samples.Add((T)item);
                }
            }
            return samples;
        }

        /// <summary>
        /// 筛选符合条件的列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">字段</param>
        /// <returns></returns>
        public List<T> GetSamples<T>(string propertyName,object value) where T : Sample
        {
            List<T> samples = new List<T>();
            foreach (var item in mSampleList)
            {
                if (item is T)
                {
                    Type type = item.GetType();
                    //先获取指定名称的成员变量 
                    FieldInfo filedInfo = type.GetField(propertyName); 
                    if(filedInfo != null)
                    {
                        object selfValue = filedInfo.GetValue(item); //获取属性值
                        if (selfValue != null && selfValue.Equals(value))
                        {
                            samples.Add((T)item);
                        }
                    }
                    else//没有对应的成员变量，则获取指定名称的属性
                    {
                        PropertyInfo propertyInfo = type.GetProperty(propertyName);
                        if (propertyInfo != null)
                        {
                            object selfValue = propertyInfo.GetValue(item,null); //获取属性值
                            if (selfValue != null && selfValue.Equals(value))
                            {
                                samples.Add((T)item);
                            }
                        }
                    }
                }
            }
            return samples;
        }

        public void DebugAllSample()
        {
            foreach (var item in mSampleList)
            {
                item.DoDebug();
            }
        }

        public void ClearSamples()
        {
            mSampleList.Clear();
        }

    }
}
