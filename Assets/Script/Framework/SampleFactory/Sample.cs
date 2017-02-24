using System;
using System.Xml;
using UnityEngine;

namespace Framework
{
    public class Sample : object
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Sample()
        {

        }

        /// <summary>
        /// 当子类有任何需要new 操作的全部放在该方法里面执行
        /// </summary>
        /// <param name="ChildNode"></param>
        public virtual void InitSample(XmlElement ChildNode)
        {

        }

        public virtual void DoDebug()
        {
            Debug.Log(string.Format("ID = {0},Name = {1}", Id, Name));
        }
    }
}
