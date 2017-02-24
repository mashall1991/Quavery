/*
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Framework.OSAbstract;
using Assets.Scripts.Datas.ConfigDatas;
using Assets.Scripts.Manager.Config;
using Framework.Unity;

/// <summary>
/// 装备位置点信息
/// </summary>
[System.Serializable]
public class EquipmentPoint
{
    /// <summary>
    /// 穿戴位置名字
    /// </summary>
    public string m_WearName;

    /// <summary>
    /// 自己的装备点.
    /// </summary>
    public GameObject m_OwnEquipmentPoint;

    /// <summary>
    /// 对应的骨骼位置点.
    /// </summary>
    public GameObject m_Bones;

    /// <summary>
    /// 穿戴位置.
    /// </summary>
    public EnumWearPosition m_WearPos;

    /// <summary>
    /// 穿戴方向.
    /// </summary>
    public EnumWearPosDirection m_Direction;

    /// <summary>
    /// 自己的蒙皮网格组件
    /// </summary>
    public SkinnedMeshRenderer m_SkinnedMeshRender;
}

/// <summary>
/// 人物换装管理系统
/// </summary>
public class RoleEquipment : MonoBehaviour
{
	/// <summary>
	/// 所有的装备位置点信息.
	/// </summary>
	public List<EquipmentPoint> AllEquipmentPoint = new List<EquipmentPoint>();
	public string PointName;

	[ContextMenu("AddNewPoint")]
	void AddNewPoint()
	{
		EquipmentPoint ep = new EquipmentPoint ();
		ep.m_WearName = PointName;
		AllEquipmentPoint.Add (ep);
	}

	[ContextMenu("ClearAll")]
	void ClearAll()
	{
		AllEquipmentPoint.Clear ();
	}

	/// <summary>
	/// 换装主接口
	/// </summary>
    /// <param name="SexType">角色性别类型</param>
	/// <param name="WearPos">穿戴位置</param>
	/// <param name="TargetData">要换装的数据，穿null表示脱下</param>
	public void ChangeEquipment(EnumRoleSex SexType, EnumWearPosition WearPos, EquipmentData TargetData)
	{
		EquipmentPoint point = FindEqiupmentPointFromPosition(WearPos);
        if (null == point)
        {
            CSysLog.Instance.Write(SysLogLevel.ERR, String.Format("无法找到人物的穿戴点, WearPos[{0}]", WearPos));
            return;
        }

		//如果穿戴位置是面部装饰
		if(WearPos == EnumWearPosition.WEAR_POS_MASK)
		{
			ChangeEquipmentAndBones(point, TargetData);
			return;
		}

		//如果穿戴位置是套装 脱下上衣跟下衣 在传上套装
		if (point.m_WearPos == EnumWearPosition.WEAR_POS_SUIT) 
		{
			EquipmentPoint cloth_up = FindEqiupmentPointFromPosition(EnumWearPosition.WEAR_POS_CLOTH_UP);
			EquipmentPoint cloth_down = FindEqiupmentPointFromPosition(EnumWearPosition.WEAR_POS_CLOTH_DOWN);

            // 脱下上衣和下衣
            ChangeCloth(cloth_up, null);
            ChangeCloth(cloth_down, null);

            // 换装
            ChangeCloth(point, TargetData);

            // 表示脱去了套装了，我们就应该为其穿上默认服装
            if (null == TargetData)
            {
                // 查看是否需要替换成默认装备
                if (null == cloth_up.m_SkinnedMeshRender.sharedMesh)
                {
                    EquipmentData data = ConfigEquipmentManager.Instance.GetDefaultEquipmentByWearPos(SexType, (Int32)EnumWearPosition.WEAR_POS_CLOTH_UP);
                    ChangeCloth(cloth_up, ConfigEquipmentManager.Instance.GetById(data.EquipID));
                }

                if (null == cloth_down.m_SkinnedMeshRender.sharedMesh)
                {
                    EquipmentData data = ConfigEquipmentManager.Instance.GetDefaultEquipmentByWearPos(SexType, (Int32)EnumWearPosition.WEAR_POS_CLOTH_DOWN);
                    ChangeCloth(cloth_down, ConfigEquipmentManager.Instance.GetById(data.EquipID));
                }
            }
            return;
		}
		// 如果穿戴位置是上衣或下衣，需要先脱下套装
		else if (point.m_WearPos == EnumWearPosition.WEAR_POS_CLOTH_DOWN
                 || point.m_WearPos == EnumWearPosition.WEAR_POS_CLOTH_UP)
		{
			EquipmentPoint suit = FindEqiupmentPointFromPosition(EnumWearPosition.WEAR_POS_SUIT);
			EquipmentPoint cloth_up = FindEqiupmentPointFromPosition(EnumWearPosition.WEAR_POS_CLOTH_UP);
			EquipmentPoint cloth_down = FindEqiupmentPointFromPosition(EnumWearPosition.WEAR_POS_CLOTH_DOWN);

            // 脱掉套装
            ChangeCloth(suit, null);
            // 穿新的衣服
            ChangeCloth(point, TargetData);

            // 查看是否需要替换成默认装备
			if(null == cloth_up.m_SkinnedMeshRender.sharedMesh)
			{
                EquipmentData data = ConfigEquipmentManager.Instance.GetDefaultEquipmentByWearPos(SexType, (Int32)EnumWearPosition.WEAR_POS_CLOTH_UP);
                ChangeCloth(cloth_up, ConfigEquipmentManager.Instance.GetById(data.EquipID));
			}

			if(null == cloth_down.m_SkinnedMeshRender.sharedMesh)
			{
                EquipmentData data = ConfigEquipmentManager.Instance.GetDefaultEquipmentByWearPos(SexType, (Int32)EnumWearPosition.WEAR_POS_CLOTH_DOWN);
                ChangeCloth(cloth_down, ConfigEquipmentManager.Instance.GetById(data.EquipID));
			}

			return;
		}
        // 发型的处理
        else if (point.m_WearPos == EnumWearPosition.WEAR_POS_HAIR)
        {
            EquipmentPoint hair = FindEqiupmentPointFromPosition(EnumWearPosition.WEAR_POS_HAIR);

            // 更换新的发型
            ChangeCloth(point, TargetData);

            // 查看是否需要替换成默认发型
			if(null == hair.m_SkinnedMeshRender.sharedMesh)
            {
                EquipmentData data = ConfigEquipmentManager.Instance.GetDefaultEquipmentByWearPos(SexType, (Int32)EnumWearPosition.WEAR_POS_HAIR);
                ChangeCloth(hair, ConfigEquipmentManager.Instance.GetById(data.EquipID));
            }

            return;
        }

        ChangeCloth(point, TargetData);
	}

    /// <summary>
    /// 更换服装
    /// </summary>
    /// <param name="point">换装的点</param>
    /// <param name="TargetData">要换的装备</param>
    private void ChangeCloth(EquipmentPoint point, EquipmentData TargetData)
    {
        // 替换装备
        if (null != TargetData)
        {
            ChangeSkinMesh(point.m_SkinnedMeshRender, TargetData.GetSkinndMesh());

            // 如果有特效
            if (TargetData.HasEffect)
            {
                GameObject effect = TargetData.GetEffect();
                AddGameEffects(effect, point.m_Bones.transform);
            }
        }
        // 换下装备
        else
        {
            ChangeSkinMesh(point.m_SkinnedMeshRender, null);
        }
    }

	/// <summary>
	/// 换游戏物体.
	/// </summary>
	/// <param name="WearPos">Wear position.</param>
	/// <param name="TargetData">Target data.</param>
	public void ChangeEquipmentAndBones(EquipmentPoint point, EquipmentData TargetData)
	{
//		EquipmentPoint point = FindEqiupmentPointFromPosition(WearPos);
		if(point.m_Bones.transform.childCount > 0)
		{
			DestroyImmediate(point.m_Bones.transform.GetChild(0).gameObject);
		}

		if (null != TargetData)
		{
			//GameObject obj = Instantiate(Resources.Load("Prefab/Role/"+ TargetData.ResName)) as GameObject;
            ItemListData itemData = ConfigItemListManager.Instance.GetItem(TargetData.EquipID);

            GameObject objGo = ConfigItemListManager.Instance.GetItemGameObjectById(itemData.ID);

            GameObject obj = GameObject.Instantiate(objGo) as GameObject;

			AddChild(obj, point.m_Bones.transform);
		}
	}

	/// <summary>
	/// 加入游戏特效到指定的位置.
	/// </summary>
	void AddGameEffects(GameObject Effect , Transform Target)
	{
		// 先移除原来的特效然后在加入新的特效
		while(Target.childCount > 0)
        {
            DestroyImmediate(Target.GetChild(0).gameObject);
        }

		AddChild (Effect, Target);
	}

    /// <summary>
    /// 添加子物体
    /// </summary>
    /// <param name="Child">子物体</param>
    /// <param name="Parent">父物体</param>
	void AddChild(GameObject Child, Transform Parent)
	{
		Child.transform.parent = Parent;
		Child.transform.localPosition = Vector3.zero;
		Child.transform.localEulerAngles = Vector3.zero;
		Child.transform.localScale = Vector3.one;
	}

	/// <summary>
	/// 通过穿戴位置找到穿戴信息点.
	/// </summary>
	/// <returns>The eqiupment point from position and direction.</returns>
	/// <param name="typeName">Type name.</param>
	EquipmentPoint FindEqiupmentPointFromPosition(EnumWearPosition WearPos)
	{
		EquipmentPoint equipmentPoint = null;
		equipmentPoint = AllEquipmentPoint.Find(
			delegate(EquipmentPoint obj)
			{
			    return obj.m_WearPos == WearPos;
		    });

		return equipmentPoint;
	}

	/// <summary>
	/// 通过穿戴位置和穿戴方向查找位置.
	/// </summary>
	/// <returns>The eqiupment point.</returns>
	/// <param name="WearPostion">Type name.</param>
	/// <param name="Direction">Direction.</param>
	EquipmentPoint FindEqiupmentPointFromPositionAndDirection(EnumWearPosition WearPostion, EnumWearPosDirection Direction)
	{
		EquipmentPoint equipmentPoint = null;
		equipmentPoint = AllEquipmentPoint.Find(
			delegate(EquipmentPoint obj)
			{
			    return obj.m_WearPos == WearPostion && obj.m_Direction == Direction;
		    });

		return equipmentPoint;
	}

	/// <summary>
	/// 通过穿戴名称查找穿戴位置信息.
	/// </summary>
	/// <returns>The eqiupment point.</returns>
	/// <param name="WearName">String.</param>
    EquipmentPoint FindEqiupmentPointFromWearNameString(string WearName)
	{
		EquipmentPoint OutPoint = null;
		foreach (EquipmentPoint ep in AllEquipmentPoint) 
		{
			if(ep.m_WearName == WearName)
			{
				OutPoint = ep;
                break;
			}
		}

		return OutPoint;
	}
	
	/// <summary>
	/// 根据穿戴位置 返回一个穿戴位置的组(0代表左,1代表右).
	/// </summary>
	/// <returns>The both point.</returns>
	/// <param name="typeName">Type name.</param>
	EquipmentPoint[] FindBothPoint(EnumWearPosition typeName)
	{
		EquipmentPoint[] BothPoint = new EquipmentPoint[2];

        BothPoint[0] = AllEquipmentPoint.Find(
			delegate(EquipmentPoint obj)
			{
			    return obj.m_WearPos == typeName && obj.m_Direction == EnumWearPosDirection.WEAR_POS_DIRECTION_LEFT;
		    });

        BothPoint[1] = AllEquipmentPoint.Find(
			delegate(EquipmentPoint obj)
			{
			    return obj.m_WearPos == typeName && obj.m_Direction == EnumWearPosDirection.WEAR_POS_DIRECTION_RIGHT;
		    });
		
		return BothPoint;
	}

	/// <summary>
	/// 改变网格（普通 不带蒙皮布料）.
	/// </summary>
	/// <param name="desMeshRenderer">DES mesh renderer.</param>
	/// <param name="srcMeshRenderer">Source mesh renderer. 为null表示脱下身上的服装</param>
	private void ChangeSkinMesh(SkinnedMeshRenderer desMeshRenderer, SkinnedMeshRenderer srcMeshRenderer)
	{
        // 更换服装
		if (desMeshRenderer != null && srcMeshRenderer != null)
        {
            // 更换网格
			desMeshRenderer.sharedMesh = srcMeshRenderer.sharedMesh;

            // 更换材质球
			desMeshRenderer.sharedMaterials = srcMeshRenderer.sharedMaterials;
		}
        // 脱下服装
        else if (null != desMeshRenderer && null == srcMeshRenderer) 
		{
			desMeshRenderer.sharedMesh = null;

			for(int i = 0; i < desMeshRenderer.sharedMaterials.Length; i++)
			{
				desMeshRenderer.sharedMaterials[i] = null;
			}
		}
	}
}
*/