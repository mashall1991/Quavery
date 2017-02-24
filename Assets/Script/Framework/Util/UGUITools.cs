using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class UGUITools
{
    /// <summary>
    /// 获得min与max中间的随机值（包括max）
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <returns></returns>
    static public int RandomRange(int min, int max)
    {
        if (min == max) return min;
        return UnityEngine.Random.Range(min, max + 1);
    }

    /// <summary>
    /// 获取游戏物体的层级结构
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    static public string GetHierarchy(GameObject obj)
    {
        if (obj == null) return "";
        string path = obj.name;

        while (obj.GetComponent<RectTransform>().parent != null)
        {
            obj = obj.GetComponent<RectTransform>().parent.gameObject;
            path = obj.name + "\\" + path;
        }
        return path;
    }

    /// <summary>
    /// 添加一个空游戏对象
    /// </summary>

    static public GameObject AddChild(GameObject parent)
    {
        // 创建一个新对象
        GameObject go = new GameObject();

        if (parent != null)
        {
            RectTransform t = go.GetComponent<RectTransform>();
            //找不到rectTrans,直接寻找Transform
            if (t == null)
            {
                Transform trans = go.GetComponent<Transform>();
                trans.parent = parent.GetComponent<Transform>();
                trans.localPosition = Vector3.zero;
                trans.localRotation = Quaternion.identity;
                trans.localScale = Vector3.one;
                go.layer = parent.layer;
                return go;
            }
            t.parent = parent.GetComponent<RectTransform>();
            t.SetAsLastSibling();
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }

        return go;
    }

    /// <summary>
    /// 实例化预制并添加到固定父物体下
    /// 默认将新添加的放在变换列表末尾
    /// </summary>
    /// <param name="parent">预制父对象</param>
    /// <param name="prefab">预制游戏物体</param>
    /// <param name="isResetLayer">是否重设层级</param>
    /// <returns></returns>
    static public GameObject AddChild(GameObject parent, GameObject prefab, bool isResetLayer = true)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
#if UNITY_EDITOR
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
        if (go != null && parent != null)
        {
            RectTransform t = go.GetComponent<RectTransform>();
            //找不到rectTrans,直接寻找Transform
            if (t == null)
            {
                Transform trans = go.GetComponent<Transform>();
                trans.parent = parent.GetComponent<Transform>();
                trans.localPosition = Vector3.zero;
                trans.localRotation = Quaternion.identity;
                trans.localScale = Vector3.one;
                if (isResetLayer)
                    SetLayer(trans, parent.layer);
                return go;
            }
            t.SetAsLastSibling();
            t.SetParent(parent.GetComponent<RectTransform>());
            if (t.parent == null)
                t.SetParent(parent.GetComponent<Transform>());
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            if (isResetLayer)
                SetLayer(t, parent.layer);
        }
        return go;
    }


    /// <summary>
    /// 实例化预制并添加到固定父物体下
    /// 需要传入交换位置的游戏物体
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="prefab"></param>
    /// <param name="SiblingGameObject">需要交换变换索引的游戏物体</param>
    /// <returns></returns>
    static public GameObject AddChild(GameObject parent, GameObject prefab, GameObject SiblingGameObject)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
#if UNITY_EDITOR
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
        if (go != null && parent != null)
        {
            RectTransform t = go.GetComponent<RectTransform>();
            t.SetSiblingIndex(SiblingGameObject.GetComponent<RectTransform>().GetSiblingIndex());
            t.parent = parent.GetComponent<RectTransform>();
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            SetLayer(t, parent.layer);
        }
        return go;
    }

    /// <summary>
    /// 设置子物体的层级
    /// </summary>
    /// <param name="t"></param>
    /// <param name="layer"></param>
    static public void SetLayer(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            SetLayer(child, layer);
        }
    }

    /// <summary>
    /// 给滑动面板添加子物体
    /// (只还原缩放，不还原位置和旋转)
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="prefab"></param>
    /// <returns></returns>
    static public GameObject AddChildToLayout(GameObject parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
#if UNITY_EDITOR
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
        if (go != null && parent != null)
        {
            RectTransform t = go.GetComponent<RectTransform>();
            //找不到rectTrans,直接寻找Transform
            if (t == null)
            {
                Transform trans = go.GetComponent<Transform>();
                trans.parent = parent.GetComponent<Transform>();
                trans.localScale = Vector3.one;
                SetLayer(trans, parent.layer);
                return go;
            }
            t.SetAsLastSibling();
            t.SetParent(parent.GetComponent<RectTransform>());
            if (t.parent == null)
                t.SetParent(parent.GetComponent<Transform>());
            t.localScale = Vector3.one;
            SetLayer(t, parent.layer);
        }
        return go;
    }

    /// <summary>
    /// Finds the specified component on the game object or one of its parents.
    /// 在此对象或者其父控件中找一个组件
    /// </summary>

    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null)
        {
            return null;
        }

        // Commented out because apparently it causes Unity 4.5.3 to lag horribly:
        // http://www.tasharen.com/forum/index.php?topic=10882.0
        //#if UNITY_4_3
#if UNITY_FLASH
		object comp = go.GetComponent<T>();
#else
        T comp = go.GetComponent<T>();
#endif
        if (comp == null)
        {
            Transform t = go.transform.parent;

            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
        }
#if UNITY_FLASH
		return (T)comp;
#else
        return comp;
#endif
        //#else
        //		return go.GetComponentInParent<T>();
        //#endif
    }

    /// <summary>
    /// 交换两个游戏物体在变换列表中的位置
    /// </summary>
    /// <param name="targetObj">需要变化的索引的物体</param>
    /// <param name="sourceObj">参照物</param>
    static public void ChangeSiblingIndex(GameObject targetObj, GameObject sourceObj)
    {
        if (targetObj == null || sourceObj == null)
            return;

        RectTransform targetTrans = targetObj.GetComponent<RectTransform>();
        RectTransform sourceTrans = sourceObj.GetComponent<RectTransform>();
        //父物体不同不予交换
        if (targetTrans.parent != sourceTrans)
            return;

        targetTrans.SetSiblingIndex(sourceTrans.GetSiblingIndex());
    }

    /// <summary>
    /// 计算content相对父级的bounds
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static Bounds CalculateRelativeBounds(RectTransform content)
    {
        return CalculateRelativeBounds(content.parent, content);
    }


    /// <summary>
    /// 计算相对于relativeTo的bounds
    /// </summary>
    /// <param name="relativeTo"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static Bounds CalculateRelativeBounds(Transform relativeTo, RectTransform content)
    {
        Bounds bounds = CalculateWorldBounds(content);

        if (relativeTo != null)
        {
            Vector3 temp;

            temp = relativeTo.InverseTransformPoint(bounds.size);
            temp.z = 0;
            bounds.size = temp;
            temp = relativeTo.InverseTransformPoint(bounds.center);
            temp.z = 0;
            bounds.center = temp;

            bounds.extents = bounds.size / 2f;
            bounds.min = bounds.center - bounds.extents;
            bounds.max = bounds.center + bounds.extents;

        }

        return bounds;
    }

    /// <summary>
    /// 计算content相对世界坐标的bounds
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static Bounds CalculateWorldBounds(RectTransform content)
    {
        Bounds bounds = new Bounds();
        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);


        GetRect(content, ref min, ref max);

        bounds.size = max - min;
        bounds.center = (max + min) / 2f;
        bounds.extents = bounds.size / 2f;
        bounds.min = bounds.center - bounds.extents;
        bounds.max = bounds.center + bounds.extents;

        return bounds;
    }

    /// <summary>
    /// 计算content的最大边框
    /// </summary>
    /// <param name="content"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public static void GetRect(RectTransform content, ref Vector2 min, ref Vector2 max)
    {
        Vector3[] corners = new Vector3[4];
        content.GetWorldCorners(corners);
        min.x = Mathf.Min(new float[] { min.x, corners[0].x, corners[2].x });
        min.y = Mathf.Min(new float[] { min.y, corners[0].y, corners[2].y });
        max.x = Mathf.Max(new float[] { max.x, corners[0].x, corners[2].x });
        max.y = Mathf.Max(new float[] { max.y, corners[0].y, corners[2].y });

        for (int i = 0, imax = content.childCount; i < imax; i++)
        {
            GetRect(content.GetChild(i) as RectTransform, ref min, ref max);
        }

    }

    /// <summary>
    /// 获取子物体
    /// </summary>
    /// <param name="parent">父对象</param>
    /// <param name="isContainInactive">是否包含隐藏物体(默认不将隐藏物体一起返回)</param>
    /// <returns></returns>
    public static List<Transform> GetChildList(Transform parent, bool isContainInactive = false)
    {
        List<Transform> childList = new List<Transform>();

        if (parent == null)
            return childList;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform t = parent.GetChild(i);

            if (t.gameObject.activeSelf || isContainInactive)
                childList.Add(t);
        }

        return childList;
    }

    /// <summary>
    /// 获取隐藏子物体
    /// </summary>
    /// <param name="parent">parent</param>
    /// <returns></returns>
    public static List<Transform> GetHideChildList(Transform parent)
    {
        List<Transform> childList = new List<Transform>();

        if (parent == null)
            return childList;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform t = parent.GetChild(i);

            if (!t.gameObject.activeSelf)
                childList.Add(t);
        }
        return childList;
    }

    /// <summary>
    /// 查看两个点是否近似
    /// </summary>
    /// <param name="original"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static bool ApproximatelyVector3(Vector3 original, Vector3 destination)
    {
        return Mathf.Approximately(original.x, destination.x) & Mathf.Approximately(original.y, destination.y) & Mathf.Approximately(original.z, destination.z);
    }

    /// <summary>
    /// 从游戏物体上获得脚本，若没获得，直接添加该脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T GetComponent<T>(GameObject obj) where T : Component
    {
        T t = obj.GetComponent<T>();
        if (t == null)
            t = obj.AddComponent<T>();
        return t;
    }

    /// <summary>
    /// 获取第一个隐藏的物体trans
    /// </summary>
    /// <param name="parentTrans">父物体</param>
    /// <returns></returns>
    public static Transform GetFirstHideTransInParent(Transform parentTrans)
    {
        List<Transform> childTransList = GetHideChildList(parentTrans);
        if (childTransList.Count > 0)
        {
            return childTransList[0];
        }
        return null;
    }
}
