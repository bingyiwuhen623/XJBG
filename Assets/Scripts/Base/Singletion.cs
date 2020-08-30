using UnityEngine;
using System;

[Serializable]
public class Singleton<T> where T : class, new()
{
    static T this_obj;

    public static T Me
    {
        get
        {
            if (this_obj == null)
                CreateInstance();
            return this_obj;
        }
        set
        {
            this_obj = value;
        }
    }
    public static void CreateInstance()
    {
        if (this_obj != null)
            return;

        this_obj = new T();
    }

    public static void ReleaseInstance()
    {
        this_obj = default;
        this_obj = null;
    }
}

/// <summary>
/// 此类可以挂载
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonMonoBehaviourNoCreate<T> : MonoBehaviour where T : SingletonMonoBehaviourNoCreate<T>
{
    private static T this_obj = null;
    public static T Me
    {
        get
        {
            return this_obj;
        }
    }

    void Awake()
    {
        if (this_obj == null)
        {
            DontDestroyOnLoad(this.gameObject);
            this_obj = this as T;
            this_obj.Init();
        }
    }

    protected virtual void Init()
    {

    }

    private void OnApplicationQuit()
    {

    }

    public static void ReleaseInstance()
    {
        if (this_obj != null)
        {
            Destroy(this_obj.gameObject);
            this_obj = null;
        }
    }
}

/// <summary>
/// 此类不可挂载
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T this_obj = null;

    public static T Me
    {
        get
        {
            CreateInstance();
            return this_obj;
        }
    }

    private void Awake()
    {
        if (this_obj == null)
        {
            this_obj = this as T;
        }
    }

    protected virtual void Init()
    {

    }

    virtual public void Show()
    {
        if (this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }
    }

    virtual public void Hide()
    {
        if (this.gameObject.activeSelf == true)
        {
            this.gameObject.SetActive(false);
        }

    }
    private void OnApplicationQuit()
    {
        this_obj = null;
    }

    public static void CreateInstance()
    {
        if (this_obj != null)
            return;

        T[] managers = GameObject.FindObjectsOfType(typeof(T)) as T[];
        if (managers.Length != 0)
        {
            if (managers.Length == 1)
            {
                this_obj = managers[0];
                this_obj.gameObject.name = typeof(T).Name;
                DontDestroyOnLoad(this_obj.gameObject);
                return;
            }
            else
            {
                Debuger.Log("You have more than one " + typeof(T).Name + " in the scene. You only need 1, it's a singletion!");
                foreach (T manager in managers)
                {
                    Destroy(manager.gameObject);
                }
            }
        }

        GameObject gO = new GameObject(typeof(T).Name, typeof(T));
        this_obj = gO.GetComponent<T>();
        this_obj.Init();
        DontDestroyOnLoad(gO);
    }

    public static void ReleaseInstance()
    {
        if (this_obj != null)
        {
            Destroy(this_obj.gameObject);
            this_obj = null;
        }
    }
}