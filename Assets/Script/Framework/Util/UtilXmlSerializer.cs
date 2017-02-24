using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using System.Xml;

namespace Framework
{
    public class UtilXmlSerializer : Singleton<UtilXmlSerializer>
    {
        //不同平台下读写的路径是不同的，这里需要注意一下。
        public static readonly string PathURL =
        #if UNITY_ANDROID
		    Application.persistentDataPath + "/";
        #elif UNITY_IPHONE
		    Application.persistentDataPath + "/";
        #elif UNITY_STANDALONE_WIN || UNITY_EDITOR
            Application.dataPath + "/StreamingAssets/xmlDB/";
        #else
            string.Empty;
        #endif

        /// <summary>
        /// 把对象序列化成xml文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        public void SerializerXmlToFile<T>(string fileName, T t)
        {
            string fileFullName = PathURL + fileName;
            Stream fStream = null;
            try
            {
                // 如果不存在这样的目录的话，就帮用户创建一个目录
                if (false == Directory.Exists(PathURL))
                {
                    Directory.CreateDirectory(PathURL);
                }

                // 写文件
                using (fStream = new FileStream(fileFullName, FileMode.Create, FileAccess.ReadWrite))
                {
                    // 编码方式使用UTF-8
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Encoding = Encoding.UTF8;

                    using (XmlWriter writer = XmlWriter.Create(fStream, settings))
                    {
                        XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                        xmlFormat.Serialize(writer, t);
                    }
                }

            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// 从文件中反序列化出对象
        /// </summary>
        /// <param name="fileName">文件路径</param>
        public T DeserializeFromFile<T>(string fileName)
        {
            string fileFullName = PathURL + fileName;
            Stream fStream = null;
            try
            {
                using (fStream = new FileStream(fileFullName, FileMode.Open, FileAccess.ReadWrite))
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(T));

                    T ret = (T)xmlFormat.Deserialize(fStream);

                    return ret;
                }
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
    }
}
