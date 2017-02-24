using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Unity
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum AssetBundleType
    {
        ASSETBUNDLE_GAMEOBJECT      = 1,                //游戏资源gameobject

        ASSETBUNDLE_XML             = 2,                //xml文件

        ASSETBUNDLE_SOUND           = 3,                //声音资源      

        ASSETBUNDLE_UIATLAS         = 4,                //精灵图集UIAtlas
    }

    /// <summary>
    /// 标记资源是给游戏有哪个地方使用
    /// </summary>
    public enum AssetBundleUseType
    {
        USE_TYPE_NONE                       = 0,                //起始值从0开始

        USE_TYPE_TASK_XML                   = 1,                //标记这个是任务需要用到的xml资源文件

        USE_TYPE_SIGN_XML                   = 2,                //标记这个是签到需要用到的xml资源文件

        USE_TYPE_ACTIVITY_XML               = 3,                //标记这个是活动需要用到的xml资源文件

        USE_TYPE_SHOP_ADVERTISEMENT_XML     = 4,                //标记这个是商城的广告需要用到的xml资源文件

        USE_TYPE_TIP_BOX_XML                = 5,                //消息响应提示框文字要用到的xml资源文件
    
        USE_TYPE_CONFIG_SERVER_IP_XML       = 6,                //游戏服务器地址配置

        USE_TYPE_CONFIG_RECHARGE_XML        = 7,                //充值相关的xml资源文件

        USE_TYPE_ANNOUNCEMENT_XML           = 8,                //公告的xml资源文件

        USE_TYPE_TIPS_CONFIG_XML            = 9,                //loading界面提示文字的xml配置文件

        USE_TYPE_ITEMLIST_XML               = 10,               //物品表的配置文件

        USE_TYPE_SHOPLIST_XML               = 11,               //商品表的配置文件

        USE_TYPE_CREATE_ROLE_XML            = 12,               //创建角色赠送物品的配置文件

        USE_TYPE_EMOLIST_XML                = 13,               //表情的配置文件

        USE_TYPE_VIPPOWERS_XML              = 14,               //vip购买信息的配置文件

        USE_TYPE_QUICK_CHAT_XML             = 15,               //快捷聊天的配置文件
    }

    /// <summary>
    /// 资源当前的下载方式
    /// </summary>
    public enum AssetDownloadType
    {
        //直接下载
        Immediate = 0,

        //延迟下载
        Delay = 1,
    }

    /// <summary>
    /// 单个任务的下载状态
    /// </summary>
    public enum DownloadState
    {
        NotDownload = 0,

        Downloading,

        DownLoadComplete,
    }
}
