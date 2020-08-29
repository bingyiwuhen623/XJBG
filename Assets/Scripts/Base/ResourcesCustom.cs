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
                DontDestroyOnLoad(me);
            }
            return me;
        }
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
        ResourceRequest request = Resources.LoadAsync<T>(filePath);
        await Extend.WaitUntil(() => request.isDone);
        asset = request.asset;
        return asset as T;
    }
}