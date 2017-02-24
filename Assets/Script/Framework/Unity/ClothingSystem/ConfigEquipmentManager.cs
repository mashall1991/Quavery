/*
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;
using Framework.OSAbstract;
using Assets.Scripts.Datas.ConfigDatas;
using Framework.Unity;
using Assets.Scripts.Manager.Config;

/// <summary>
/// 装备数据信息.
/// </summary>
public class EquipmentData
{
    /// <summary>
    /// 装备ID.
    /// </summary>
    public Int32 EquipID { get; set; }

    /// <summary>
    /// 资源名.
    /// </summary>
    public string ResName { get; set; }

    /// <summary>
    /// 资源路径.
    /// </summary>
    public string ResPath { get; set; }

    /// <summary>
    /// 是否含有特效
    /// </summary>
    public bool HasEffect
    {
        get { return !string.IsNullOrEmpty(EffectsPath); }
    }

    /// <summary>
    /// 特效文件所在路径.
    /// </summary>
    public string EffectsPath { get; set; }

    /// <summary>
    /// 装备ICON名.
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 装备穿戴位置.
    /// </summary>
    public Int32 WearPos { get; set; }

    /// <summary>
    /// 装备的穿戴方向.
    /// </summary>
    public EnumWearPosDirection WearPosDirection { get; set; }

    /// <summary>
    /// 装备的描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 穿戴性别.
    /// </summary>
    public EnumRoleSex EquipSex { get; set; }

    /// <summary>
    /// 获得当前的Skinnedmeshrender组件.
    /// </summary>
    public SkinnedMeshRenderer GetSkinndMesh()
    {
        //GameObject obj = UnityEngine.Resources.Load("Prefab/Role/" + ResName) as GameObject;

        //统一换成从资源服务器热更新加载方式
        GameObject obj = ConfigItemListManager.Instance.GetItemGameObjectById(EquipID);

        SkinnedMeshRenderer smr = obj.GetComponent<SkinnedMeshRenderer>();
        if (null != smr)
        {
            return smr;
        }

        return null;
    }
	
    public GameObject GetEffect()
    {
        return GameObject.Instantiate(UnityEngine.Resources.Load("Perfabs/Effect/" + EffectsPath)) as GameObject;
    }
}

/// <summary>
/// 装备管理器
/// </summary>
public class ConfigEquipmentManager : Framework.Singleton<ConfigEquipmentManager>
{
    // 要读取的文件
    private const string fileAllName = "Configs/ItemList";
    
    /// <summary>
    /// 所有的装备数据列表.
    /// </summary>
    private List<EquipmentData> mAllEquipmentDataList;

    /// <summary>
    /// 女性角色默认装备数据
    /// KEY: 穿戴位置
    /// VALUE: 装备数据
    /// </summary>
    private Dictionary<Int32, EquipmentData> mFemaleRoleDefaultEquipmentData;

    /// <summary>
    /// 男性角色默认装备数据
    /// KEY: 穿戴位置
    /// VALUE: 装备数据
    /// </summary>
    private Dictionary<Int32, EquipmentData> mMaleRoleDefaultEquipmentData;

    /// <summary>
    /// 保存所有的默认服装
    /// </summary>
    private HashSet<Int32> mDefaultCloth;

    /// <summary>
    /// 男性服装的起始序列号，小于这个序号就表示为女性
    /// </summary>
    private const Int32 cFemaleClothStartIndex = 130001;

	/// <summary>
	/// 构造函数
	/// </summary>
    public ConfigEquipmentManager()
    {
        mAllEquipmentDataList = new List<EquipmentData>();

        mFemaleRoleDefaultEquipmentData = new Dictionary<Int32, EquipmentData>();
        mMaleRoleDefaultEquipmentData = new Dictionary<Int32, EquipmentData>();
        mDefaultCloth = new HashSet<Int32>();
	}

	/// <summary>
	/// 通过ID找到装备信息.
	/// </summary>
	/// <param name="EquipID">装备id</param>
    /// <returns>装备数据</returns>
    public EquipmentData GetById(Int32 EquipID)
	{
        foreach (EquipmentData data in mAllEquipmentDataList) 
		{
			if(data.EquipID == EquipID)
			{
				return data;
			}
		}

		return null;
	}

    /// <summary>
    /// 根据穿戴位置获取角色的默认装备
    /// </summary>
    /// <param name="wearpos">穿戴位置</param>
    /// <returns>装备数据</returns>
    public EquipmentData GetDefaultEquipmentByWearPos(EnumRoleSex sextype, Int32 wearpos)
    {
        if (EnumRoleSex.Girl == sextype)
        {
            if (false == mFemaleRoleDefaultEquipmentData.ContainsKey(wearpos))
            {
                CSysLog.Instance.Write(SysLogLevel.ERR, String.Format("获取女性角色默认装备失败"));
                return null;
            }

            return mFemaleRoleDefaultEquipmentData[wearpos];
        }
        else
        {
            if (false == mMaleRoleDefaultEquipmentData.ContainsKey(wearpos))
            {
                CSysLog.Instance.Write(SysLogLevel.ERR, String.Format("获取男性角色默认装备失败"));
                return null;
            }

            return mMaleRoleDefaultEquipmentData[wearpos];
        }
    }

	/// <summary>
	/// 通过资源名找到装备信息
	/// </summary>
	/// <param name="ResName">资源名</param>
    /// <returns>装备数据/returns>
	public EquipmentData GetByResName(string ResName)
	{
		foreach (EquipmentData data in mAllEquipmentDataList)
		{
			if(data.ResName == ResName)
			{
				return data;
			}
		}

		return null;
	}

    /// <summary>
    /// 读取装备配置
    /// </summary>
    public void LoadXml(List<ItemListData> itemList)
    {
        mAllEquipmentDataList.Clear();
        for(int i=0; i<itemList.Count; i++)
        {
            EquipmentData newData = new EquipmentData();
            newData.EquipID = itemList[i].ID;
            //newData.ResName = itemList[i].ResName;
            newData.Icon = itemList[i].Icon;
            newData.EquipSex = (EnumRoleSex)(itemList[i].Flag);

            newData.WearPos = (Int32)(itemList[i].WearPos);
            newData.WearPosDirection = (EnumWearPosDirection)(itemList[i].WearPosDirection);
            newData.Description = itemList[i].Description;
            // 判断性别
            int Sexflag = (int)(itemList[i].Flag);

            // 这个字段为-1，表示它为默认装备
            if ("-1" == itemList[i].Validity)
            {
                // 保存所有角色的默认服装
                mDefaultCloth.Add(newData.EquipID);

                // 保存女性默认装备
                if (Sexflag == 2)
                {
                    if (!mFemaleRoleDefaultEquipmentData.ContainsKey(newData.WearPos))
                    {
                        mFemaleRoleDefaultEquipmentData.Add(newData.WearPos, newData);
                    }
                }
                else
                {
                    if (!mMaleRoleDefaultEquipmentData.ContainsKey(newData.WearPos))
                    {
                        mMaleRoleDefaultEquipmentData.Add(newData.WearPos, newData);
                    }
                }
            }

            mAllEquipmentDataList.Add(newData);
        }
    }

    #region 不再使用的方法
    /// <summary>
    /// 读取配置文件
    /// </summary>
	public void LoadXml()
	{
		try
        {
			mAllEquipmentDataList.Clear();

            TextAsset xmlText = Resources.Load(fileAllName) as TextAsset;
			XmlDocument xmlDoc = new System.Xml.XmlDocument();
			xmlDoc.LoadXml(xmlText.text);
			
			XmlElement Node = (XmlElement)xmlDoc.DocumentElement.FirstChild;
			while(Node != null)
			{
                // 检查是否具有ItemId字段
                if (false == Node.HasAttribute("ID"))
				{
					Node = (XmlElement)Node.NextSibling;
					continue;
				}
                
				EquipmentData newData = new EquipmentData();
                newData.EquipID = Int32.Parse(Node.GetAttribute("ID"));
                newData.ResName = Node.GetAttribute("ResName");
				newData.Icon = Node.GetAttribute("Icon");
                newData.EquipSex = (EnumRoleSex)(int.Parse(Node.GetAttribute("Flag")));

				newData.WearPos = Int32.Parse(Node.GetAttribute("WearPos"));
				newData.WearPosDirection = (EnumWearPosDirection)Int32.Parse(Node.GetAttribute("WearPosDirection"));
				newData.Description = Node.GetAttribute("Description");
                // 判断性别
                int Sexflag = int.Parse(Node.GetAttribute("Flag"));

                // 这个字段为-1，表示它为默认装备
                if ("-1" == Node.GetAttribute("Validity"))
                {
                    // 保存所有角色的默认服装
                    mDefaultCloth.Add(newData.EquipID);

                    // 保存女性默认装备
                    if (Sexflag == 2)
                    {
                        if (!mFemaleRoleDefaultEquipmentData.ContainsKey(newData.WearPos))
                        {
                            mFemaleRoleDefaultEquipmentData.Add(newData.WearPos, newData);
                        }
                    }
                    else
                    {
                        if (!mMaleRoleDefaultEquipmentData.ContainsKey(newData.WearPos))
                        {
                            mMaleRoleDefaultEquipmentData.Add(newData.WearPos, newData);
                        }
                    }
                }

				mAllEquipmentDataList.Add(newData);

				Node = (XmlElement)Node.NextSibling;

                //CSysLog.Instance.Write(SysLogLevel.DEBUG, String.Format("装备管理器获取装备配置 ID={0}, Description={1}", newData.EquipID, newData.Description));
			}
		}
		catch(System.Exception ex)
		{
            CSysLog.Instance.Write(SysLogLevel.ERR, String.Format("加载xml[{0}]文件出错! {1}", fileAllName, ex.ToString()));
		}
	}
    #endregion

    /// <summary>
    /// 是否是角色的默认服装
    /// </summary>
    /// <param name="itemid"></param>
    /// <returns></returns>
    public bool IsDefaultCloth(Int32 itemid)
    {
        if (mDefaultCloth.Contains(itemid))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 是否为男性装备
    /// </summary>
    /// <param name="itemid">物品id</param>
    /// <returns></returns>
    public EnumRoleSex GetClothSexType(Int32 itemid)
    {
        EnumRoleSex sex = EnumRoleSex.Girl;

        
        for (int i = 0; i < mAllEquipmentDataList.Count; i++)
        {
            if(itemid == mAllEquipmentDataList[i].EquipID)
            {
                if(mAllEquipmentDataList[i].EquipSex == EnumRoleSex.Boy)
                {
                    sex = EnumRoleSex.Boy;
                }
                break;
            }
        }

        return sex;
    }
}
*/
