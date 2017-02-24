using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public sealed class UtilObjectPool : MonoBehaviour
    {
        /// <summary>
        /// 控制对象池的创建预置的时机
        /// </summary>
        public enum StartupPoolMode
        {
            /// <summary>
            /// Awake方法中创建
            /// </summary>
            Awake,

            /// <summary>
            /// Start方法中创建
            /// </summary>
            Start,

            /// <summary>
            /// 用户手动创建
            /// </summary>
            CallManually
        };

        /// <summary>
        /// 加这句是为了保证在unity编辑器上可以看到
        /// </summary>
        [System.Serializable]
        public class StartupPool
        {
            /// <summary>
            /// 初始状态创建多少个对象
            /// </summary>
            public int size;

            /// <summary>
            /// 要创建的对象类型
            /// </summary>
            public GameObject prefab;
        }

        // 单例模式
        private static UtilObjectPool _instance;
        private static List<GameObject> tempList = new List<GameObject>();

        // 对象池
        private Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();
        // 保存分配出去的对象
        private Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();

        /// <summary>
        /// 控制创建对象池的时机
        /// </summary>
        public StartupPoolMode startupPoolMode;
        
        /// <summary>
        /// 用户希望创建的对象
        /// </summary>
        public List<StartupPool> startupPools;

        /// <summary>
        /// 对象池是否已经被创建了
        /// </summary>
        private bool mObjectPoolsCreated = false;

        void Awake()
        {
            _instance = this;

            if (startupPoolMode == StartupPoolMode.Awake)
            {
                CreateStartupPools();
            }
        }

        void Start()
        {
            if (startupPoolMode == StartupPoolMode.Start)
            {
                CreateStartupPools();
            }
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        public static void CreateStartupPools()
        {
            if (false == instance.mObjectPoolsCreated)
            {
                instance.mObjectPoolsCreated = true;

                var pools = instance.startupPools;
                if (pools != null && pools.Count > 0)
                {
                    for (int i = 0; i < pools.Count; ++i)
                    {
                        CreatePool(pools[i].prefab, pools[i].size);
                    }
                }
            }
        }

        public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
        {
            CreatePool(prefab.gameObject, initialPoolSize);
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="prefab">要创建的游戏对象</param>
        /// <param name="initialPoolSize">要创建的游戏对象的个数</param>
        public static void CreatePool(GameObject prefab, int initialPoolSize)
        {
            if (prefab != null
                && false == instance.pooledObjects.ContainsKey(prefab))
            {
                var list = new List<GameObject>();
                // 在池中添加新的对象
                instance.pooledObjects.Add(prefab, list);

                if (initialPoolSize > 0)
                {
                    // activeSelf（read only只读）:物体本身的active状态，对应于其在inspector中的checkbox是否被勾选
                    // activeInHierarchy（read only只读）:物体在层次中是否是active的。也就是说要使这个值为true，这个物体及其所有父物体(及祖先物体)的activeself状态都为true。
                    bool active = prefab.activeSelf;
                    prefab.SetActive(false);

                    // 获取poolmanager组件的Transform
                    Transform parent = instance.transform;
                    while (list.Count < initialPoolSize)
                    {
                        // 相当于new
                        var obj = (GameObject)Object.Instantiate(prefab);
                        obj.transform.parent = parent;

                        // 添加到对象池
                        list.Add(obj);
                    }

                    prefab.SetActive(active);
                }
            }
        }

        public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
        {
            return Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Vector3 position) where T : Component
        {
            return Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Transform parent) where T : Component
        {
            return Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab) where T : Component
        {
            return Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }

        /// <summary>
        /// 获取相应的游戏对象
        /// </summary>
        /// <param name="prefab">预置</param>
        /// <param name="parent">父节点</param>
        /// <param name="position">位置</param>
        /// <param name="rotation">旋转</param>
        /// <returns>返回相应的游戏对象</returns>
        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            List<GameObject> list = null;
            Transform trans = null;
            GameObject obj = null;

            // 找到共享对象链表
            if (instance.pooledObjects.TryGetValue(prefab, out list))
            {
                if (list.Count > 0)
                {
                    // 获取一个共享对象
                    while (obj == null && list.Count > 0)
                    {
                        // 分配一个游戏对象
                        obj = list[0];
                        list.RemoveAt(0);
                    }

                    // 获取对象成功
                    if (obj != null)
                    {
                        trans = obj.transform;
                        trans.parent = parent;
                        trans.localPosition = position;
                        trans.localRotation = rotation;
                        obj.SetActive(true);

                        // 保存分配出去的游戏对象
                        instance.spawnedObjects.Add(obj, prefab);

                        return obj;
                    }
                }

                // 无法分配到共享池中的对象就创建一个新的对象
                obj = (GameObject)Object.Instantiate(prefab);
                trans = obj.transform;
                trans.parent = parent;
                trans.localPosition = position;
                trans.localRotation = rotation;
                instance.spawnedObjects.Add(obj, prefab);

                return obj;
            }
            // 没有这样的共享对象，就创建一个新的对象
            else
            {
                obj = (GameObject)Object.Instantiate(prefab);
                trans = obj.GetComponent<Transform>();
                trans.parent = parent;
                trans.localPosition = position;
                trans.localRotation = rotation;

                return obj;
            }
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
        {
            return Spawn(prefab, parent, position, Quaternion.identity);
        }
        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Spawn(prefab, null, position, rotation);
        }
        public static GameObject Spawn(GameObject prefab, Transform parent)
        {
            return Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }
        public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
            return Spawn(prefab, null, position, Quaternion.identity);
        }
        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(T obj) where T : Component
        {
            Recycle(obj.gameObject);
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="obj"></param>
        public static void Recycle(GameObject obj)
        {
            GameObject prefab;
            if (instance.spawnedObjects.TryGetValue(obj, out prefab))
            {
                Recycle(obj, prefab);
            }
            else
            {
                Object.Destroy(obj);
            }
        }

        /// <summary>
        /// 回收共享资源
        /// </summary>
        /// <param name="obj">value</param>
        /// <param name="prefab">key</param>
        private static void Recycle(GameObject obj, GameObject prefab)
        {
            instance.pooledObjects[prefab].Add(obj);
            instance.spawnedObjects.Remove(obj);

            // 将共享对象挂回自己的位置
            obj.transform.parent = instance.transform;
            // 隐藏此对象
            obj.SetActive(false);
        }

        public static void RecycleAll<T>(T prefab) where T : Component
        {
            RecycleAll(prefab.gameObject);
        }

        /// <summary>
        /// 回收对象池中此种类型的所有游戏对象
        /// </summary>
        /// <param name="prefab">游戏对象</param>
        public static void RecycleAll(GameObject prefab)
        {
            foreach (var item in instance.spawnedObjects)
            {
                if (item.Value == prefab)
                {
                    tempList.Add(item.Key);
                }
            }

            // 回收所有对象
            for (int i = 0; i < tempList.Count; ++i)
            {
                Recycle(tempList[i]);
            }

            tempList.Clear();
        }

        /// <summary>
        /// 回收所有游戏对象
        /// </summary>
        public static void RecycleAll()
        {
            tempList.AddRange(instance.spawnedObjects.Keys);

            for (int i = 0; i < tempList.Count; ++i)
            {
                Recycle(tempList[i]);
            }

            tempList.Clear();
        }

        /// <summary>
        /// 此游戏对象是否被分配出去了
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsSpawned(GameObject obj)
        {
            return instance.spawnedObjects.ContainsKey(obj);
        }

        public static int CountPooled<T>(T prefab) where T : Component
        {
            return CountPooled(prefab.gameObject);
        }

        public static int CountPooled(GameObject prefab)
        {
            List<GameObject> list = null;

            if (instance.pooledObjects.TryGetValue(prefab, out list))
            {
                return list.Count;
            }

            return 0;
        }

        public static int CountSpawned<T>(T prefab) where T : Component
        {
            return CountSpawned(prefab.gameObject);
        }

        public static int CountSpawned(GameObject prefab)
        {
            int count = 0;
            foreach (var instancePrefab in instance.spawnedObjects.Values)
            {
                if (prefab == instancePrefab)
                {
                    ++count;
                }
            }

            return count;
        }

        public static int CountAllPooled()
        {
            int count = 0;

            foreach (var list in instance.pooledObjects.Values)
            {
                count += list.Count;
            }

            return count;
        }

        public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
        {
            if (list == null)
            {
                list = new List<GameObject>();
            }

            if (!appendList)
            {
                list.Clear();
            }

            List<GameObject> pooled;
            if (instance.pooledObjects.TryGetValue(prefab, out pooled))
            {
                list.AddRange(pooled);
            }

            return list;
        }

        public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
        {
            if (list == null)
            {
                list = new List<T>();
            }

            if (!appendList)
            {
                list.Clear();
            }

            List<GameObject> pooled;
            if (instance.pooledObjects.TryGetValue(prefab.gameObject, out pooled))
            {
                for (int i = 0; i < pooled.Count; ++i)
                {
                    list.Add(pooled[i].GetComponent<T>());
                }
            }

            return list;
        }

        public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
        {
            if (list == null)
            {
                list = new List<GameObject>();
            }

            if (!appendList)
            {
                list.Clear();
            }

            foreach (var item in instance.spawnedObjects)
            {
                if (item.Value == prefab)
                {
                    list.Add(item.Key);
                }
            }
            
            return list;
        }

        public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
        {
            if (list == null)
            {
                list = new List<T>();
            }

            if (!appendList)
            {
                list.Clear();
            }

            var prefabObj = prefab.gameObject;
            foreach (var item in instance.spawnedObjects)
            {
                if (item.Value == prefabObj)
                {
                    list.Add(item.Key.GetComponent<T>());
                }
            }

            return list;
        }

        public static void DestroyPooled(GameObject prefab)
        {
            List<GameObject> pooled;
            if (instance.pooledObjects.TryGetValue(prefab, out pooled))
            {
                for (int i = 0; i < pooled.Count; ++i)
                {
                    GameObject.Destroy(pooled[i]);
                }

                pooled.Clear();
            }
        }

        public static void DestroyPooled<T>(T prefab) where T : Component
        {
            DestroyPooled(prefab.gameObject);
        }

        public static void DestroyAll(GameObject prefab)
        {
            RecycleAll(prefab);
            DestroyPooled(prefab);
        }

        public static void DestroyAll<T>(T prefab) where T : Component
        {
            DestroyAll(prefab.gameObject);
        }

        public static UtilObjectPool instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = Object.FindObjectOfType<UtilObjectPool>();
                if (_instance != null)
                {
                    return _instance;
                }

                // 如果用户没有创建游戏对象的话，程序创建一个新的游戏对象
                var obj = new GameObject("UtilObjectPool");
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;

                _instance = obj.AddComponent<UtilObjectPool>();
                return _instance;
            }
        }
    }

    /// <summary>
    /// 对Component和GameObject的扩展方法
    /// </summary>
    public static class ObjectPoolExtensions
    {
        public static void CreatePool<T>(this T prefab) where T : Component
        {
            UtilObjectPool.CreatePool(prefab, 0);
        }
        public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component
        {
            UtilObjectPool.CreatePool(prefab, initialPoolSize);
        }
        public static void CreatePool(this GameObject prefab)
        {
            UtilObjectPool.CreatePool(prefab, 0);
        }
        public static void CreatePool(this GameObject prefab, int initialPoolSize)
        {
            UtilObjectPool.CreatePool(prefab, initialPoolSize);
        }

        public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return UtilObjectPool.Spawn(prefab, parent, position, rotation);
        }
        public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return UtilObjectPool.Spawn(prefab, null, position, rotation);
        }
        public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component
        {
            return UtilObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
        }
        public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
        {
            return UtilObjectPool.Spawn(prefab, null, position, Quaternion.identity);
        }
        public static T Spawn<T>(this T prefab, Transform parent) where T : Component
        {
            return UtilObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }
        public static T Spawn<T>(this T prefab) where T : Component
        {
            return UtilObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }
        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            return UtilObjectPool.Spawn(prefab, parent, position, rotation);
        }
        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return UtilObjectPool.Spawn(prefab, null, position, rotation);
        }
        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
        {
            return UtilObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
        }
        public static GameObject Spawn(this GameObject prefab, Vector3 position)
        {
            return UtilObjectPool.Spawn(prefab, null, position, Quaternion.identity);
        }
        public static GameObject Spawn(this GameObject prefab, Transform parent)
        {
            return UtilObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }
        public static GameObject Spawn(this GameObject prefab)
        {
            return UtilObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(this T obj) where T : Component
        {
            UtilObjectPool.Recycle(obj);
        }
        public static void Recycle(this GameObject obj)
        {
            UtilObjectPool.Recycle(obj);
        }

        public static void RecycleAll<T>(this T prefab) where T : Component
        {
            UtilObjectPool.RecycleAll(prefab);
        }
        public static void RecycleAll(this GameObject prefab)
        {
            UtilObjectPool.RecycleAll(prefab);
        }

        public static int CountPooled<T>(this T prefab) where T : Component
        {
            return UtilObjectPool.CountPooled(prefab);
        }
        public static int CountPooled(this GameObject prefab)
        {
            return UtilObjectPool.CountPooled(prefab);
        }

        public static int CountSpawned<T>(this T prefab) where T : Component
        {
            return UtilObjectPool.CountSpawned(prefab);
        }
        public static int CountSpawned(this GameObject prefab)
        {
            return UtilObjectPool.CountSpawned(prefab);
        }

        public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list, bool appendList)
        {
            return UtilObjectPool.GetSpawned(prefab, list, appendList);
        }
        public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list)
        {
            return UtilObjectPool.GetSpawned(prefab, list, false);
        }
        public static List<GameObject> GetSpawned(this GameObject prefab)
        {
            return UtilObjectPool.GetSpawned(prefab, null, false);
        }
        public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component
        {
            return UtilObjectPool.GetSpawned(prefab, list, appendList);
        }
        public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component
        {
            return UtilObjectPool.GetSpawned(prefab, list, false);
        }
        public static List<T> GetSpawned<T>(this T prefab) where T : Component
        {
            return UtilObjectPool.GetSpawned(prefab, null, false);
        }

        public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list, bool appendList)
        {
            return UtilObjectPool.GetPooled(prefab, list, appendList);
        }
        public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list)
        {
            return UtilObjectPool.GetPooled(prefab, list, false);
        }
        public static List<GameObject> GetPooled(this GameObject prefab)
        {
            return UtilObjectPool.GetPooled(prefab, null, false);
        }
        public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component
        {
            return UtilObjectPool.GetPooled(prefab, list, appendList);
        }
        public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component
        {
            return UtilObjectPool.GetPooled(prefab, list, false);
        }
        public static List<T> GetPooled<T>(this T prefab) where T : Component
        {
            return UtilObjectPool.GetPooled(prefab, null, false);
        }

        public static void DestroyPooled(this GameObject prefab)
        {
            UtilObjectPool.DestroyPooled(prefab);
        }
        public static void DestroyPooled<T>(this T prefab) where T : Component
        {
            UtilObjectPool.DestroyPooled(prefab.gameObject);
        }

        public static void DestroyAll(this GameObject prefab)
        {
            UtilObjectPool.DestroyAll(prefab);
        }
        public static void DestroyAll<T>(this T prefab) where T : Component
        {
            UtilObjectPool.DestroyAll(prefab.gameObject);
        }
    }
}
