using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelTools
{
    public static void CopyRectTranform(RectTransform copy, RectTransform target)
    {
        target.SetPositionAndRotation(copy.position, copy.rotation);
        target.localScale = copy.localScale;
        target.anchorMin = copy.anchorMin;
        target.anchorMax = copy.anchorMax;
        target.offsetMin = copy.offsetMin;
        target.offsetMax = copy.offsetMax;
    }

    public static void ResetRectTransform(RectTransform transform)
    {
        transform.anchorMin = Vector2.zero;
        transform.anchorMax = Vector2.one;
        transform.offsetMax = Vector2.zero;
        transform.offsetMin = Vector2.zero;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public static void ResetTrans(Transform trans)
    {
        RectTransform rect = trans.GetComponent<RectTransform>();
        if (rect == null)
        {
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = Vector3.one;
        }
        else
        {
            ResetRectTransform(rect);
        }
    }

    /// <summary>
    /// 排序，将物体放在最下方
    /// </summary>
    public static void SortGoToLast(GameObject go)
    {
        SortTransToLast(go.transform);
    }

    /// <summary>
    /// 排序，将物体放在最下方
    /// </summary>
    public static void SortTransToLast(Transform trans)
    {
        trans.SetAsLastSibling();
    }


    /// <summary>
    /// 排序，将物体放在最上方
    /// </summary>
    public static void SortGoToFirst(GameObject go)
    {
        SortTransToFirst(go.transform);
    }

    /// <summary>
    /// 排序，将物体放在最上方
    /// </summary>
    public static void SortTransToFirst(Transform trans)
    {
        trans.SetAsFirstSibling();
    }

    public static GameObject AddChild(GameObject parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
#if UNITY_EDITOR
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }

    // 查找子窗口
    public static GameObject FindChild(GameObject parent, string name)
    {
        Transform tmp;
        for (int i = 0; i < parent.transform.childCount; ++i)
        {
            if ((tmp = parent.transform.GetChild(i)).name == name)
                return tmp.gameObject;

            //if ((obj = FindChild(tmp.gameObject, name)) != null) 
            //    return obj;
        }
        Debuger.Log("name:" + name + " not find! ");
        return null;
    }

    // 查找子窗口,通过分隔符'/'来确定父子窗口
    public static GameObject Find(GameObject parent, string name)
    {
        string[] childs = name.Split('/');
        Transform p = parent.transform;
        foreach (string child in childs)
        {
            p = p.transform.Find(child);
            if (p == null)
            {
                Debuger.Log("name:" + name + " not find! ");
                return null;
            }
        }

        return p.gameObject;
    }

    // 查找子窗口,通过分隔符'/'来确定父子窗口
    public static T Find<T>(GameObject parent, string name) where T : Component
    {
        string[] childs = name.Split('/');
        Transform p = parent.transform;
        foreach (string child in childs)
        {
            p = p.transform.Find(child);
            if (p == null)
            {
                Debuger.LogError(name + " name:" + typeof(T).Name + " not find! ");
                return null;
            }
        }

        return p.gameObject.GetComponent<T>();
    }

    public static T FindChild<T>(GameObject parent, string name) where T : Component
    {
        Transform tmp;
        //Debuger.Log(name);
        for (int i = 0; i < parent.transform.childCount; ++i)
        {
            if ((tmp = parent.transform.GetChild(i)).name == name)

                return tmp.gameObject.GetComponent<T>();
        }
        return null;
    }

    public static void SetSpriteIcon(Image image, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            image.sprite = null;
        }
        else
        {
            image.sprite = Resources.Load<Sprite>(path);
        }
    }

    /*
    通过分隔符'/'来确定父子窗口设置多语言字符串查找子窗口中的UILabel,
    然后设置标签
    */
    public static void setLabelText(GameObject parent, string name, int id)
    {
        //UILabel label = PanelTools.Find<UILabel>(parent,name);
        //if(label!=null)
        //    label.text = DataMgr.DataManager.getLanguageMgr().getString(id);
    }

    public static void setLabelText(GameObject parent, string name, string id)
    {
        //UILabel label = PanelTools.Find<UILabel>(parent,name);
        //if(label!=null)
        //    label.text = DataMgr.DataManager.getLanguageMgr().getString(id);
    }

    //销毁创建item数组
    public static void DestroyGameObject(List<GameObject> list)
    {
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null /*&& list[i].transform.parent != null*/)
                {
                    GameObject.Destroy(list[i]);
                }

            }
        }
    }

    //销毁创建item数组
    public static void DestroyGameObject(Dictionary<int, GameObject> list)
    {
        if (list.Count > 0)
        {
            foreach (KeyValuePair<int, GameObject> keyvalue in list)
            {
                if (keyvalue.Value != null && keyvalue.Value.transform.parent != null)
                {
                    GameObject.Destroy(keyvalue.Value);
                }
            }
        }
    }

    public static void DestroyGameObject(GameObject go)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            GameObject.Destroy(go.transform.GetChild(i).gameObject);
        }
    }

    //销毁创建item
    public static void DestroyGameObjectItem(GameObject go)
    {
        if (go != null)
        {
            GameObject.Destroy(go);
        }
    }

    public static void AddChildGameObject(GameObject parent, GameObject go)
    {
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
    }
}