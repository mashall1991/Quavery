using UnityEngine;
using System.Collections;
using System;
using Framework.Unity;

/// <summary>
/// 该类针对单个热更新程序
/// </summary>
public class DownLoadOne : MonoBehaviour
{
    /// <summary>
    /// 对应的热更新资源ID
    /// </summary>
    public int ResourceID { get; private set; }

    /// <summary>
    /// 任务的下载状态
    /// </summary>
    public DownloadState state { get; private set; }

    /// <summary>
    /// 下载中的提示
    /// </summary>
    public GameObject DownloadingTips;

    /// <summary>
    /// 开始下载的委托
    /// </summary>
    public Action DownLoadBegin;

    /// <summary>
    /// 下载中途的委托
    /// </summary>
    /// <param name="rate"></param>
    public delegate void DownloadingRate(float rate);
    public DownloadingRate OnDownloadingRate;

    /// <summary>
    /// 下载完成使用游戏物体的委托
    /// </summary>
    public DownLoadManager.DownloadGameobjCallback OnDownLoadComplete;

    /// <summary>
    /// 下载完成后告知管理器的委托
    /// 目前暂定只能由管理器添加
    /// </summary>
    /// <param name="component"></param>
    public delegate void DownLoadToManager(int resID,DownLoadOne component);
    public DownLoadToManager OnDownLoadToManager;

    /// <summary>
    /// 下载www
    /// </summary>
    private WWW www;


    void Awake()
    {
        state = DownloadState.NotDownload;
    }

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(state == DownloadState.Downloading && www !=null && OnDownloadingRate !=null)
        {
            OnDownloadingRate(www.progress);
            Debug.Log(this.name + "   下载了  " + www.progress);
        }
	}

    /// <summary>
    /// 初始化下载任务
    /// </summary>
    /// <param name="resID"></param>
    /// <param name="beginCallback"></param>
    /// <param name="downloadingCallback"></param>
    /// <param name="completeCallback"></param>
    public void InitDownLoadTask(int resID,Action beginCallback = null, DownloadingRate downloadingCallback = null, DownLoadManager.DownloadGameobjCallback completeCallback = null)
    {
        ResourceID = resID;

        //添加事件
        DownLoadBegin = beginCallback;
        OnDownloadingRate = downloadingCallback;
        OnDownLoadComplete = completeCallback;

        if(DownLoadBegin != null)
        {
            DownLoadBegin();
        }
    }

    public void StartDownLoad()
    {
        state = DownloadState.Downloading;
        StartCoroutine(DownLoading());
    }

    private IEnumerator DownLoading()
    {
        Debug.Log("开始下载   name is "+gameObject.name);
        GameObject obj = null;
        LoadData data = DownLoadManager.Instance.GetDelayLoadDataByID(ResourceID);

        if (data == null)
            yield break;

        // 下载资源
        WWW www = WWW.LoadFromCacheOrDownload(data.URL, data.version);
        yield return www;

        if (www.isDone && string.IsNullOrEmpty(www.error))
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

        //关闭状态
        state = DownloadState.DownLoadComplete;

        //无论对错抛出外层事件
        if (OnDownLoadComplete != null)
        {
            OnDownLoadComplete(obj);
        }

        //告知管理器下载完成
        if (OnDownLoadToManager !=null)
        {
            OnDownLoadToManager(ResourceID, this);
        }
    }
}
