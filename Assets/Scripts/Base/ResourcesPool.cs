using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 资源加载类型
/// </summary>
public enum ResourceType
{
    /// <summary>
    /// 地格资源
    /// </summary>
    Tile = 0,
    /// <summary>
    /// 人物模型
    /// </summary>
    Role,
    /// <summary>
    /// 道具模型
    /// </summary>
    GridItem,


    Max,
}
/// <summary>
/// 用来管理游戏内需要预加载的东西，主要是战斗内的模型、地格等物体
/// </summary>
public class ResourcesPool : MonoBehaviour
{

    static ResourcesPool _Instance = null;
    public static ResourcesPool Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = (new GameObject("ResourcesPool")).AddComponent<ResourcesPool>();
                DontDestroyOnLoad(_Instance.gameObject);
                _Instance.Init();
            }
            return _Instance;
        }
    }

    // 此处用于存储完全相同的对象
    Dictionary<ResourceType, SinglePool> pools;
    // 此处用于存储根据路径存储的对象，如人物模型-----未使用
    Dictionary<string, List<GameObject>> pathPoolsUnUse;
    // 此处用于存储根据路径存储的对象，如人物模型-----已使用
    Dictionary<string, List<GameObject>> pathPoolsUsed;
    // 无需清理的
    List<ResourceType> DontClearTypes;

    public void Init()
    {
        pools = new Dictionary<ResourceType, SinglePool>();
        for (int i = 0; i < (int)ResourceType.Max; i++)
        {
            pools.Add((ResourceType)i, new SinglePool());
        }
        pathPoolsUnUse = new Dictionary<string, List<GameObject>>();
        pathPoolsUsed = new Dictionary<string, List<GameObject>>();
        DontClearTypes = new List<ResourceType>()
        {
            ResourceType.Max,
        };
    }

    /// <summary>
    /// 加载特殊模型
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private async Task LoadByPath(string path)
    {
        CheckPathDics(path);
        GameObject resourceGo = await ResourcesCustom.Me.LoadAsync<GameObject>(path);
        GameObject obj = GameObject.Instantiate(resourceGo);
        obj.transform.SetParent(this.transform, false);
        pathPoolsUnUse[path].Add(obj);
        obj.SetActive(false);
    }

    /// <summary>
    /// 使用特殊模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T UseByPath<T>(string path, Transform parent = null) where T : Component
    {
        GameObject obj;
        CheckPathDics(path);

        if (pathPoolsUnUse[path].Count > 0)
        {
            obj = pathPoolsUnUse[path][0];
            pathPoolsUnUse[path].RemoveAt(0);
        }
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>(path));
        }
        pathPoolsUsed[path].Add(obj);
        if (parent != null)
        {
            obj.transform.SetParent(parent, false);
            PanelTools.ResetTrans(obj.transform);
        }
        obj.SetActive(true);
        return obj.GetComponent<T>();
    }

    /// <summary>
    /// 不再使用，返回池里
    /// </summary>
    /// <param name="path"></param>
    /// <param name="go"></param>
    public void ResetByPath(string path, GameObject go)
    {
        ClearITileContainable(go);
        CheckPathDics(path);
        if (pathPoolsUsed[path].Contains(go))
        {
            pathPoolsUsed[path].Remove(go);
        }
        pathPoolsUnUse[path].Add(go);
        go.transform.SetParent(ResourcesPool.Instance.transform, false);
        go.SetActive(false);
    }

    /// <summary>
    /// 清除根据路径加载的资源
    /// </summary>
    public void ClearPathResources()
    {
        foreach (var pair in pathPoolsUnUse)
        {
            for (int i = pair.Value.Count - 1; i >= 0; i--)
            {
                GameObject go = pair.Value[i];
                pair.Value.RemoveAt(i);
                GameObject.Destroy(go);
            }
        }
        pathPoolsUnUse.Clear();
        foreach (var pair in pathPoolsUsed)
        {
            for (int i = pair.Value.Count - 1; i >= 0; i--)
            {
                GameObject go = pair.Value[i];
                pair.Value.RemoveAt(i);
                GameObject.Destroy(go);
            }
        }
        pathPoolsUsed.Clear();
    }

    /// <summary>
    /// 重置根据路径加载的资源
    /// </summary>
    public void ResetPathResources()
    {
        foreach (var pair in pathPoolsUsed)
        {
            List<GameObject> used = pair.Value;
            for (int i = used.Count - 1; i >= 0; i--)
            {
                ResetByPath(pair.Key, used[i]);
            }
        }
    }

    private void CheckPathDics(string path)
    {
        if (!pathPoolsUnUse.ContainsKey(path))
        {
            pathPoolsUnUse.Add(path, new List<GameObject>());
        }
        if (!pathPoolsUsed.ContainsKey(path))
        {
            pathPoolsUsed.Add(path, new List<GameObject>());
        }
    }

    /// <summary>
    /// 加载到对象池
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private async Task Load(ResourceType type, string path)
    {
        SinglePool pool = pools[type];
        await pool.Load(path);
    }

    /// <summary>
    /// 加载到对象池
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private void Load(ResourceType type, GameObject go)
    {
        SinglePool pool = pools[type];
        pool.Load(go);
    }

    /// <summary>
    /// 使用，获取对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public T Use<T>(ResourceType type, string path, Transform parent = null) where T : Component
    {
        SinglePool pool = pools[type];
        return pool.Use<T>(path, parent);
    }

    /// <summary>
    /// 使用，获取对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public T Use<T>(ResourceType type, GameObject go, Transform parent = null) where T : Component
    {
        SinglePool pool = pools[type];
        return pool.Use<T>(go, parent);
    }

    /// <summary>
    /// 重置，存回池里
    /// </summary>
    /// <param name="type"></param>
    /// <param name="go"></param>
    /// <returns></returns>
    public void Reset(ResourceType type, GameObject go)
    {
        SinglePool pool = pools[type];
        pool.Reset(go);
    }

    /// <summary>
    /// 清除所有，战斗结束后使用
    /// </summary>
    public void ClearAll()
    {
        ResetAll();
        for (int i = 0; i < (int)ResourceType.Max; i++)
        {
            ResourceType type = (ResourceType)i;
            if (!DontClearTypes.Contains(type))
            {
                pools[type].Clear();
            }
        }
        ClearPathResources();
    }

    /// <summary>
    /// 重置所有，重新开始战斗使用
    /// </summary>
    public void ResetAll()
    {
        for (int i = 0; i < (int)ResourceType.Max; i++)
        {
            ResourceType type = (ResourceType)i;
            if (!DontClearTypes.Contains(type))
            {
                pools[type].ResetAll();
            }
        }
        ResetPathResources();
    }

    /// <summary>
    /// 回收前的清理
    /// </summary>
    /// <param name="containable"></param>
    public void ClearITileContainable(GameObject go)
    {

    }
}
/// <summary>
/// 单个对象池
/// </summary>
public class SinglePool
{
    public List<GameObject> UnUse = new List<GameObject>();
    public List<GameObject> Used = new List<GameObject>();
    /// <summary>
    /// 主要用于loading
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task Load(string path)
    {
        GameObject resourceGo = await ResourcesCustom.Me.LoadAsync<GameObject>(path);
        Load(resourceGo);
    }
    /// <summary>
    /// 主要用于loading
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public void Load(GameObject go)
    {
        GameObject obj = GameObject.Instantiate(go);
        obj.transform.SetParent(ResourcesPool.Instance.transform, false);
        UnUse.Add(obj);
        obj.SetActive(false);
    }
    /// <summary>
    /// 使用，从对象池里拿出来
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public T Use<T>(string path, Transform parent = null) where T : Component
    {
        GameObject obj;
        if (UnUse.Count > 0)
        {
            obj = UnUse[0];
            UnUse.RemoveAt(0);
        }
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>(path));
        }
        Used.Add(obj);
        if (parent != null)
        {
            obj.transform.SetParent(parent, false);
            PanelTools.ResetTrans(obj.transform);
        }
        obj.SetActive(true);
        return obj.GetComponent<T>();
    }
    /// <summary>
    /// 使用，从对象池里拿出来
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public T Use<T>(GameObject go, Transform parent = null) where T : Component
    {
        GameObject obj;
        if (UnUse.Count > 0)
        {
            obj = UnUse[0];
            UnUse.RemoveAt(0);
        }
        else
        {
            obj = GameObject.Instantiate(go);
        }
        Used.Add(obj);
        if (parent != null)
        {
            obj.transform.SetParent(parent, false);
            PanelTools.ResetTrans(obj.transform);
        }
        obj.SetActive(true);
        return obj.GetComponent<T>();
    }
    /// <summary>
    /// 重置，使用完后，不销毁，存回池里
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public void Reset(GameObject go)
    {
        ResourcesPool.Instance.ClearITileContainable(go);
        if (Used.Contains(go))
        {
            Used.Remove(go);
        }
        UnUse.Add(go);
        go.transform.SetParent(ResourcesPool.Instance.transform, false);
        go.SetActive(false);
    }
    /// <summary>
    /// 销毁，战斗结束后调用
    /// </summary>
    public void Clear()
    {
        for (int i = UnUse.Count - 1; i >= 0; i--)
        {
            GameObject go = UnUse[i];
            UnUse.RemoveAt(i);
            GameObject.Destroy(go);
        }
        for (int i = Used.Count - 1; i >= 0; i--)
        {
            GameObject go = Used[i];
            Used.RemoveAt(i);
            GameObject.Destroy(go);
        }
    }
    /// <summary>
    /// 重置所有
    /// </summary>
    public void ResetAll()
    {
        for (int i = Used.Count - 1; i >= 0; i--)
        {
            Reset(Used[i]);
        }
    }
}