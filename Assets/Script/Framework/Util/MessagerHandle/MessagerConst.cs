using System;
using UnityEngine;

/*
*用作客户端消息定义
*/
public static class ClientMessagerConst
{
    #region 客户端自定义消息ID

    #region 客户端测试用例
    public const int CLIENT_TEST_FIRST_DEBUG = -1;

    public const int CLIENT_TEST_SECOND_DEBUG = -10;

    public const int CLIENT_SLOT_ITEM_RESULT = -21;

    public const int CLIENT_SPIN_BUTTON_VISIBLE = -22;
    #endregion

    /// <summary>
    /// 客户端网络ping值事件ID
    /// </summary>
    public const int CLIENT_PING_TIME = -31;

    #endregion

    #region 游戏内玩家操作ID
    //客户端本地操作，拳击
    public const int CLIENT_GAME_PUNCH = -101;

    //客户端本地操作，抱起
    public const int CLIENT_GAME_LIFT = -102;

    //客户端本地操作，举炸弹
    public const int CLIENT_GAME_BOMB = -103;

    //客户端本地操作，跳跃
    public const int CLIENT_GAME_JUMP = -104;

    //炸弹消失
    public const int CLIENT_GAME_BOMB_DISAPPEAR = -105;

    //玩家复活
    public const int CLIENT_GAME_REVIVE_ROLE = -106;

    //玩家死亡
    public const int CLIENT_GAME_DEATH_ROLE = -107;

    //挣扎
    public const int CLIENT_GAME_STRUGGLE = -108;

    //挣脱
    public const int CLIENT_GAME_SRUGGLE_TO_FREE = -109;

    #endregion

    #region 玩家互动

    public const int CLIENT_PLAYER_BEING_HELD = -200;
    public const int CLIENT_PLAYER_BEING_DROPPED = -201;
    public const int CLIENT_PLAYER_GLOVE_STATE = -202;

    #endregion

    #region 游戏道具相关ID

    public const int CLIENT_GAME_PROP_SPAWN = -300;
    public const int CLIENT_GAME_PROP_TRIGGER = -301;
    public const int CLIENT_GAME_PROP_SPAWN_SENDMSG = -302;
    public const int CLIENT_GAME_TNT_SPAWN = -303;
    public const int CLIENT_GAME_MEDAL_SPAWN_SENDMSG = -304;

    #endregion

    #region 炸弹爆炸
    public const int CLIENT_BOMB_EXPLODE = -400;
    public const int CLIENT_CURSEBOMB_SENDMSG = -401;
    #endregion

    #region 地图操作
    public const int CLIENT_MAP_LOADED = -500;
    #endregion

    #region 人物死亡
    //摄像机拉远
    public const int CLIENT_DEATH_CAMERA_TWEEN = -600;
    //玩家复活 重设相机位置
    public const int CLIENT_GAME_REVIVE_ROLE_RESET_CAM = -601;
    //死亡拳击相关处理
    public const int CLIENT_GAME_DEATH_ROLE_RESET_STATE = -602;
    /// <summary>
    /// 标识玩家从舞台上移除
    /// </summary>
    public const int CLIENT_DEATH_REMOVE_FROM_STAGE = -603; 
    #endregion

    #region 勋章争夺规则

    public const int CLIENT_GAME_MEMAL_TRIGGER = -700;
    public const int CLIENT_MEDAL_RANK_REFRESH = -701;
    #endregion

    //争夺开始
    public const int CLIENT_GAME_SCRAMBLE_BEGIN = -702;
    //update 
    //public const int CLIENT_GAME_SCRAMBLE_UPDATE = -603;
    //争夺结束
    public const int CLIENT_GAME_SCRAMBLE_END = -704;

    public const int CLIENT_GAME_PLAYERHEADPANEL = -800;
    public const int CLIENT_GAME_PLAYERREVIVEPANEL = -801;

    //更新游戏时间
    public const int CLIENT_GAME_TIME_SYNC = -900;
}
