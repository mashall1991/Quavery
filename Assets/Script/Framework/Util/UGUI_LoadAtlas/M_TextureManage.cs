using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//************************************************************************纹理图集加载管理类*********************************************************************************

//统一管理加载，是一个单例类，找个不被销毁的GameObject绑定就行， 用一个Dictionary按图集的路径过key将加载过的图集缓存起来，需要时再由外部删除掉
public class M_TextureManage : MonoBehaviour
{
    private static GameObject m_pMainObject;
    private static M_TextureManage m_pContainer = null;
    public static M_TextureManage Instance
    {
        get
        {
            if (m_pContainer == null)
            {
                m_pContainer = m_pMainObject.GetComponent<M_TextureManage>();
            }
            return m_pContainer;
        }
    }
    private Dictionary<string, UnityEngine.Object[]> m_pAtlasDic;   //图集的集合
    void Awake()
    {
        InitData();
    }
    private void InitData()
    {
        m_pMainObject = gameObject;
        m_pAtlasDic = new Dictionary<string, UnityEngine.Object[]>();
    }

    /// <summary>
    /// 加载图集上的一个精灵
    /// </summary>
    /// <param name="spriteAtlasPath">精灵图集路径(图集名字)</param>
    /// <param name="spriteName">精灵名字</param>
    /// <returns>返回的精灵图片</returns>
    public Sprite LoadAtlasSprite(string spriteAtlasPath, string spriteName)
    {
        Sprite m_Sprite = FindSpriteFormBuffer(spriteAtlasPath, spriteName);
        if (m_Sprite == null)
        {
            UnityEngine.Object[] atlas = Resources.LoadAll(spriteAtlasPath);
            m_pAtlasDic.Add(spriteAtlasPath, atlas);
            m_Sprite = SpriteFormAtlas(atlas, spriteName);
        }
        return m_Sprite;
    }
    /// <summary>
    /// 删除图集
    /// </summary>
    /// <param name="spriteAtlasPath"></param>
    public void DeleteAtlas(string spriteAtlasPath)
    {
        if (m_pAtlasDic.ContainsKey(spriteAtlasPath))
        {
            m_pAtlasDic.Remove(spriteAtlasPath);
        }
    }
    /// <summary>
    /// 从缓存中查找图集，并找出sprite
    /// </summary>
    /// <param name="spriteAtlasPath">精灵图集路径(图集名字)</param>
    /// <param name="spriteName">精灵名字</param>
    /// <returns>返回的精灵图片</returns>
    private Sprite FindSpriteFormBuffer(string spriteAtlasPath, string spriteName)
    {
        if (m_pAtlasDic.ContainsKey(spriteAtlasPath))
        {
            UnityEngine.Object[] atlas = m_pAtlasDic[spriteAtlasPath];
            Sprite m_Sprite = SpriteFormAtlas(atlas, spriteName);
            return m_Sprite;
        }
        return null;
    }
    /// <summary>
    /// 从图集中，并找出sprite
    /// </summary>
    /// <param name="atlas">图集</param>
    /// <param name="spriteName">精灵名称</param>
    /// <returns>返回的精灵图片</returns>
    private Sprite SpriteFormAtlas(UnityEngine.Object[] atlas, string spriteName)
    {
        for (int i = 0; i < atlas.Length; i++)
        {
            if (atlas[i].GetType() == typeof(Sprite))
            {
                if (atlas[i].name == spriteName)
                {
                    return (Sprite)atlas[i];
                }
            }
        }
        Debug.LogError("图片名：" + spriteName + "在图集中找不到...");
        return null;
    }
}
