using UnityEngine;
using System.Collections;

/// <summary>
/// 穿戴位置.
/// </summary>
public enum EnumWearPosition
{
    /// <summary>
    /// 身体
    /// </summary>
    WEAR_POS_BODY = 10,

    /// <summary>
    /// 头发
    /// </summary>
    WEAR_POS_HAIR = 11,

    /// <summary>
    /// 上衣
    /// </summary>
    WEAR_POS_CLOTH_UP = 12,

    /// <summary>
    /// 下衣
    /// </summary>
    WEAR_POS_CLOTH_DOWN = 13,

    /// <summary>
    /// 脚
    /// </summary>
    WEAR_POS_FOOT = 14,

    /// <summary>
    /// 脸
    /// </summary>
    WAER_POS_FACE = 15,

    /// <summary>
    /// 眼睛
    /// </summary>
    WEAR_POS_EYE = 16,

    /// <summary>
    /// 大腿
    /// </summary>
    WEAR_POS_THIGH = 17,

    /// <summary>
    /// 小腿
    /// </summary>
    WEAR_POS_CRUS = 18,

    /// <summary>
    /// 袜子
    /// </summary>
    WEAR_POS_SOCKS = 19,

    /// <summary>
    /// 头部
    /// </summary>
    WEAR_POS_HEAD = 20,

    /// <summary>
    /// 面部或眼睛
    /// </summary>
    WEAR_POS_MASK = 21,

    /// <summary>
    /// 后背(翅膀、乐器、背包)
    /// </summary>
    WEAR_POS_BACK = 22,

    /// <summary>
    /// 手指（戒指）
    /// </summary>
    WEAR_POS_FINGER = 25,

    /// <summary>
    /// 耳环
    /// </summary>
    WEAR_POS_EARRING = 24,

    /// <summary>
    /// 手（手镯，护腕）
    /// </summary>
    WEAR_POS_HAND = 26,

    /// <summary>
    /// 胳膊（臂环）
    /// </summary>
    WEAR_POS_ARM = 27,

    /// <summary>
    /// 脖子（项链）
    /// </summary>
    WEAR_POS_NECK = 28,

    /// <summary>
    /// 尾部
    /// </summary>
    WEAR_POS_TAIL = 29,

    /// <summary>
    /// 套装
    /// </summary>
    WEAR_POS_SUIT = 30,

    /// <summary>
    /// 宠物的位置
    /// </summary>
    PET = 31
}

/// <summary>
/// 穿戴方向.
/// </summary>
public enum EnumWearPosDirection
{
    /// <summary>
    /// 无方向
    /// </summary>
    WEAR_POS_DIRECTION_NONE = 0,

    /// <summary>
    /// 左边
    /// </summary>
    WEAR_POS_DIRECTION_LEFT = 1,

    /// <summary>
    /// 右边
    /// </summary>
    WEAR_POS_DIRECTION_RIGHT = 2,

    /// <summary>
    /// 两边
    /// </summary>
    WEAR_POS_DIRECTION_BOTH = 3,
}

/// <summary>
/// 人物性别.
/// </summary>
public enum EnumRoleSex
{
    /// <summary>
    /// 男孩
    /// </summary>
    Boy = 1,

    /// <summary>
    /// 女孩
    /// </summary>
    Girl = 2,
}
