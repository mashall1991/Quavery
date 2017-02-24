using UnityEngine;
using System.Collections;
using Framework.Unity;

/// <summary>
/// 表示下载的资源
/// </summary>
public class LoadData
{
    /// <summary>
    /// 资源ID
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// 资源名称
    /// </summary>
    public string resName { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public int version { get; set; }

    /// <summary>
    /// 对应下载的URL
    /// </summary>
    public string URL { get; set; }

    /// <summary>
    /// 下载方式
    /// 延迟下载或是立即下载
    /// </summary>
    public AssetDownloadType downloadType { get; set; }

    /// <summary>
    /// 下载资源优先级
    /// 优先级数值越大越先下载
    /// </summary>
    public uint priority { get; set; }

    /// <summary>
    /// 资源类型
    /// </summary>
    public AssetBundleType assetType { get; set; }
}
