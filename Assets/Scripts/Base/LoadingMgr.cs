using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XJBG.Base;

public class LoadingMgr
{
    private static LoadingMgr instance;
    public static LoadingMgr Me
    {
        get
        {
            if (instance == null)
            {
                GameObject o = Resources.Load<GameObject>("Prefabs/Common/Loading");
                instance = new LoadingMgr();
                GameObject gameObject = UnityEngine.Object.Instantiate(o);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                instance.Init(gameObject);
            }
            return instance;
        }
    }

    GameObject loadObj;

    //-----------右下角四个白点及旋转时间-------------
    private float tsChangeDelayTime;
    private Transform t1;
    private Transform t2;
    private Transform t3;
    private Transform t4;
    //------------------------------------------------
    private Image bg;       // 用来显示关卡loading图的父级
    private GameObject bgChild;  // 关卡loading图

    public void Init(GameObject gameObject)
    {
        loadObj = gameObject;
        tsChangeDelayTime = 0.3f;
        GameObject tParent = PanelTools.Find(loadObj, "TParent");
        t1 = PanelTools.Find<Transform>(tParent, "T1");
        t2 = PanelTools.Find<Transform>(tParent, "T2");
        t3 = PanelTools.Find<Transform>(tParent, "T3");
        t4 = PanelTools.Find<Transform>(tParent, "T4");
        bg = PanelTools.Find<Image>(loadObj, "Bg");
        bgChild = null;
    }

    public void Show()
    {
        loadObj.SetActive(true);
        Time.timeScale = 1;
        bg.gameObject.SetActive(true);
        //BgChild = GameObject.Instantiate<GameObject>(s, Bg.transform);
        StartGame.Me.StartCoroutine(Loading());
    }

    public void Hide()
    {
        loadObj.SetActive(false);
        if (bgChild)
        {
            UnityEngine.Object.Destroy(bgChild);
            bgChild = null;
        }
    }

    private IEnumerator Loading()
    {
        do
        {
            Vector3 temp = t4.position;
            t4.position = t3.position;
            t3.position = t2.position;
            t2.position = t1.position;
            t1.position = temp;
            yield return new WaitForSecondsRealtime(tsChangeDelayTime);
        } while (loadObj.activeSelf);
    }
}
