using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System;
using Framework.Unity;
using Framework;
using System.IO;
using System.Diagnostics; 

public class DownLoadManager : MonoSingleton<DownLoadManager>
{
    //资源配置文件
    public readonly string AssetBundlesXml = "aseet_bundle_config_xml.xml";

    /// <summary>
    /// 本地已有的配置表文件
    /// 用来跟服务器新表作对比
    /// </summary>
    private List<LoadData> cacheServerAssetBundleList;

    /// <summary>
    /// 立即下载的资源列表
    /// </summary>
    private List<LoadData> immediateDownLoadList;

    /// <summary>
    /// 延后下载的资源列表
    /// </summary>
    private List<LoadData> delayDownLoadList;

    /// <summary>
    /// 最近一次登录下载得热更新资源配置表
    /// </summary>
    private List<LoadData> downloadServerAssetBundleList;

    /// <summary>
    /// 下载完成回调
    /// </summary>
    public Action DownloadCompleteCallback;

    /// <summary>
    /// 获取游戏对象的回调
    /// </summary>
    /// <param name="obj"></param>
    public delegate void DownloadGameobjCallback(GameObject obj);

    protected override void Awake()
    {
        base.Awake();
        cacheServerAssetBundleList = new List<LoadData>();
        immediateDownLoadList = new List<LoadData>();
        delayDownLoadList = new List<LoadData>();
        downloadServerAssetBundleList = new List<LoadData>();

        //初始化时就加载缓存配置表数据
        InitCacheServerAssetBundleList();
    }

    // Use this for initialization
    void Start ()
    {
	    
	}

    private void InitCacheServerAssetBundleList()
    {
        // 如果在磁盘中不存在这个配置文件, 就创建一个
        if (false == File.Exists(UtilXmlSerializer.PathURL + AssetBundlesXml))
        {
            // 序列化一个空表
            UtilXmlSerializer.Instance.SerializerXmlToFile<List<LoadData>>(AssetBundlesXml, cacheServerAssetBundleList);
        }

        //填充数据
        cacheServerAssetBundleList = UtilXmlSerializer.Instance.DeserializeFromFile<List<LoadData>>(AssetBundlesXml);
    }

    /// <summary>
    /// 获取缓存数据字典，方便内部调用
    /// </summary>
    /// <returns></returns>
    private Dictionary<int,LoadData> GetCacheServerAssetBundleDic()
    {
        Dictionary<int, LoadData> dictionary = new Dictionary<int, LoadData>();
        foreach (var item in cacheServerAssetBundleList)
        {
            dictionary.Add(item.ID, item);
        }
        return dictionary;
    }

    /// <summary>
    /// 外层调用方法接口
    /// </summary>
    /// <param name="ServerAssetBundleConfigXmlUrl">热更新资源配置文件的下载地址</param>
    public void StartDownLoad(string ServerAssetBundleConfigXmlUrl)
    {
        StartCoroutine(DownLoadServerAssetBundleXml(ServerAssetBundleConfigXmlUrl + AssetBundlesXml));
    }

    /// <summary>
    /// 先要下载游戏服务器的资源配置文件,参数为xml配置文件在服务器的地址
    /// </summary>
    /// <param name="ServerAssetBundleConfigXmlUrl"></param>
    /// <returns></returns>
    private IEnumerator DownLoadServerAssetBundleXml(string ServerAssetBundleConfigXmlUrl)
    {
        // 下载热更新xml配置文件
        WWW www = new WWW(ServerAssetBundleConfigXmlUrl);
        yield return www;


        // 下载配置文件成功
        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            string assetbundleConfigStr = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(www.text));


            XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(assetbundleConfigStr);

            XmlElement ChildNode = (XmlElement)xmlDoc.DocumentElement.FirstChild;
            while (ChildNode != null)
            {
                LoadData data = new LoadData();
                // 自定义的AssetBundle的ID
                data.ID = Convert.ToInt32(ChildNode.GetAttribute("ID"));
                // 文件名
                data.resName = ChildNode.GetAttribute("ResName");
                // 版本号
                data.version = Convert.ToInt32(ChildNode.GetAttribute("Version"));
                // 资源服务器地址
                data.URL = ChildNode.GetAttribute("ServerUrl");
                // 下载类型
                data.downloadType = (AssetDownloadType)(Convert.ToInt32(ChildNode.GetAttribute("DownloadType")));                
                // 扩展数据
                data.priority = Convert.ToUInt32(ChildNode.GetAttribute("Priority"));
                // 资源类型
                data.assetType = (AssetBundleType)(Convert.ToInt32(ChildNode.GetAttribute("AssetBundleType")));


                downloadServerAssetBundleList.Add(data);

                // 判断哪些资源是新需要立即下载和延迟下载
                Dictionary<int, LoadData> cacheDic = GetCacheServerAssetBundleDic();
                //文件不存在或者当前版本低于需下载版本
                if (!cacheDic.ContainsKey(data.ID) || cacheDic[data.ID].version < data.version)
                {
                    switch (data.downloadType)
                    {
                        case AssetDownloadType.Immediate:
                            immediateDownLoadList.Add(data);
                            break;
                        case AssetDownloadType.Delay:
                            delayDownLoadList.Add(data);
                            break;
                        default:
                            break;
                    }
                }

                //对下载资源列表进行优先级排序
                SortDownlosdList(immediateDownLoadList);
                SortDownlosdList(delayDownLoadList);
                
                ChildNode = (XmlElement)ChildNode.NextSibling;
            }
            
            // 获取完配置信息后开始下载资源
            StartCoroutine(DownloadImmediateRes());
        }
        else
        {
           
        }
    }

    /// <summary>
    /// 对存储的链表进行排序
    /// </summary>
    /// <param name="downloadList"></param>
    private void SortDownlosdList(List<LoadData> downloadList)
    {
        if (downloadList == null || downloadList.Count == 0)
            return;

        downloadList.Sort((a, b) =>
        {
            if (a.priority > b.priority)
                return 1;
            else
                return 0;
        });
    }

    /// <summary>
    /// 开始下载资源
    /// </summary>
    /// <returns></returns>
    private IEnumerator DownloadImmediateRes()
    {
        
        if (immediateDownLoadList.Count == 0)
            yield break;

        bool isDownloadError = false;
        for (int i = 0; i < immediateDownLoadList.Count; i++)
        {
            LoadData data = immediateDownLoadList[i];
            
            // 通过http的方式下载资源
            WWW www = WWW.LoadFromCacheOrDownload(data.URL, data.version);
            yield return www;

            if(www.isDone && www.error == null)
            {
                // 这里必须要执行释放占用的内存
                if (null != www.assetBundle)
                {
                    www.assetBundle.Unload(false);
                }

                switch (data.assetType)
                {
                    //配置表直接读进内存
                    case AssetBundleType.ASSETBUNDLE_XML:

                        break;
                    case AssetBundleType.ASSETBUNDLE_SOUND:

                        break;
                    case AssetBundleType.ASSETBUNDLE_UIATLAS:

                        break;
                    default:
                        break;
                }
            }
            else
            {
                isDownloadError = true;
            }
        }

        //没有下载出错
        if(!isDownloadError)
        {
            //存储新的配置表,并且该表就是最新的配置文件,所有资源对应关系都在其中
            UtilXmlSerializer.Instance.SerializerXmlToFile(AssetBundlesXml, downloadServerAssetBundleList);
            InitCacheServerAssetBundleList();

            if(DownloadCompleteCallback != null)
            {
                DownloadCompleteCallback();
            }
        }
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resID"></param>
    /// <returns></returns>
    public T GetAssetData<T>(int resID) where T : UnityEngine.Object
    {
        LoadData data = GetLoadDataByID(resID);
        T obj = null;
        Stopwatch watch = new Stopwatch();
        watch.Start();
        // 下载资源
        WWW www = WWW.LoadFromCacheOrDownload(data.URL, data.version);
        if (www.isDone)
        {
            AssetBundle bundle = www.assetBundle;

            //去掉加载资源文件的后缀名
            string prefabeName = System.Text.RegularExpressions.Regex.Replace(data.resName, ".assetbundle", "");

            // 获取到assetbundle中的游戏对象
            obj = bundle.LoadAsset<T>(prefabeName);
            bundle.Unload(false);
        }
        else
        {
        }
        watch.Stop();
        return obj;
    }

   
    /// <summary>
    /// 外部接口，获取游戏物体
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="OnDownloadGameobjCallback"></param>

    public void GetDownloadObj(int ID, DownloadGameobjCallback OnDownloadGameobjCallback)
    {
        StartCoroutine(GetTargetObj(ID, OnDownloadGameobjCallback));
    }

    /// <summary>
    /// 从缓存或者url获取游戏对象
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator GetTargetObj(int ID, DownloadGameobjCallback callback)
    {
        GameObject obj = null;
        LoadData data = GetLoadDataByID(ID);

        if (data == null)
            yield break;

        // 下载资源
        WWW www = WWW.LoadFromCacheOrDownload(data.URL, data.version);
        yield return www;
        
        if (www.isDone)
        {
            AssetBundle bundle = www.assetBundle;

            //去掉加载资源文件的后缀名
            string prefabeName = System.Text.RegularExpressions.Regex.Replace(data.resName, ".assetbundle", "");

            // 获取到assetbundle中的游戏对象
            obj = bundle.LoadAsset(prefabeName) as GameObject;
            bundle.Unload(false);
        }
        else
        {
        }

        if(callback != null)
        {
            callback(obj);
        }
    }

    /// <summary>
    /// 通过ID获取配置数据
    /// </summary>
    /// <param name="resId"></param>
    /// <returns></returns>
    private LoadData GetLoadDataByID(int resId)
    {
        for (int i = 0; i < downloadServerAssetBundleList.Count; i++)
        {
            if (downloadServerAssetBundleList[i].ID == resId)
                return downloadServerAssetBundleList[i];
        }

        return null;
    }

    /// <summary>
    /// 通过ID获取配置数据
    /// </summary>
    /// <param name="resId"></param>
    /// <returns></returns>
    public LoadData GetDelayLoadDataByID(int resId)
    {
        for (int i = 0; i < delayDownLoadList.Count; i++)
        {
            if (delayDownLoadList[i].ID == resId)
                return delayDownLoadList[i];
        }

        return null;
    }


    #region 单个资源热更新管理器

    /// <summary>
    /// 最大下载数
    /// </summary>
    private const int MAXDOWNLOADCOUNT = 2;

    private Dictionary<int, DownLoadOne> DownLoadTaskDic = new Dictionary<int, DownLoadOne>();

    /// <summary>
    /// 添加单个下载任务
    /// </summary>
    /// <param name="resID"></param>
    /// <param name="task"></param>
    /// <param name="beginCallback"></param>
    /// <param name="downloadingCallback"></param>
    /// <param name="completeCallback"></param>
    public void AddDownLoadTask(int resID,GameObject task, Action beginCallback = null, 
                                DownLoadOne.DownloadingRate downloadingCallback = null, 
                                DownLoadManager.DownloadGameobjCallback completeCallback = null)
    {
        if (task == null)
        {
            return;
        }
        DownLoadOne component = task.GetComponent<DownLoadOne>();
        if(component == null)
        {
            component = task.AddComponent<DownLoadOne>();
        }

        if(DownLoadTaskDic.ContainsKey(resID))
        {
            return;
        }

        //添加下载完毕的回调
        component.OnDownLoadToManager = OnDownLoadToManager;
        //初始化下载数据
        component.InitDownLoadTask(resID,beginCallback,downloadingCallback,completeCallback);

        //给组件脚本添加事件完成事件
        DownLoadTaskDic.Add(resID,component);

    }

    /// <summary>
    /// 单个元素下载完毕后告知管理器
    /// </summary>
    /// <param name="resID"></param>
    /// <param name="component"></param>
    private void OnDownLoadToManager(int resID, DownLoadOne component)
    {
        UnityEngine.Debug.Log("下载完成 id = "+resID +"   name is "+component.name);
        component.OnDownLoadToManager = null;
        DownLoadTaskDic.Remove(resID);

        //补充新的下载任务
        StartDownLoadOneTask();
    }

    public void StartDownLoadOneTask()
    {
        int[] counts = GetDownLoadingtaskCount();

        //拿到最多能下载的个数
        int canDownloadCount = DownLoadManager.MAXDOWNLOADCOUNT - counts[0] ;

        //得到最小能下载的个数
        canDownloadCount = canDownloadCount <= counts[1] ? canDownloadCount : counts[1];
        if (canDownloadCount > 0)
        {
            foreach (var item in DownLoadTaskDic)
            {
                //找到未下载任务，开始下载
                if(item.Value.state == DownloadState.NotDownload)
                {
                    item.Value.StartDownLoad();
                    canDownloadCount--;
                    //下载次数满足
                    if(canDownloadCount == 0)
                    {
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 获取正在下载的个数，以及可以下载的个数
    /// 索引为0代表正在下载得个数，索引为1表示还可以下载的个数
    /// </summary>
    /// <returns></returns>
    public int[] GetDownLoadingtaskCount()
    {
        int[] counts = new int[2];
        foreach (var item in DownLoadTaskDic)
        {
            switch (item.Value.state)
            {
                //未下载添加到索引为1
                case DownloadState.NotDownload:
                    counts[1]++;
                    break;
                
                //已下载添加到索引为0
                case DownloadState.Downloading:
                    counts[0]++;
                    break;
                default:
                    break;
            }
        }

        return counts;
    }



    #endregion
}
