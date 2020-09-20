using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResourcesCustom : MonoBehaviour
{
    private static ResourcesCustom me;
    public static ResourcesCustom Me
    {
        get
        {
            if (me == null)
            {
                GameObject go = new GameObject("ResourcesCustom");
                me = go.AddComponent<ResourcesCustom>();
                me.Init();
                DontDestroyOnLoad(me);
            }
            return me;
        }
    }

    // 缓存，要及时清理，否则会造成卡顿或闪退
    Dictionary<(string, Type), UnityEngine.Object> customDic;
    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        customDic = new Dictionary<(string, Type), UnityEngine.Object>();
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public async Task<T> LoadAsync<T>(string filePath, bool forece_real_load = false) where T : UnityEngine.Object
    {
        UnityEngine.Object asset;
        Type type = typeof(T);
        (string, Type) key = (filePath, type);

        if (customDic.ContainsKey(key))
        {
            asset = customDic[key];
        }
        else
        {
            ResourceRequest request = Resources.LoadAsync<T>(filePath);
            await Extend.WaitUntil(() => request.isDone);
            asset = request.asset;
            customDic.Add(key, asset);
        }
        return asset as T;
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    public void ClearCustom()
    {
        customDic.Clear();
    }
}