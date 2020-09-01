using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XJBG.UI;

namespace XJBG.Base
{
    public class StartGame : SingletonMonoBehaviourNoCreate<StartGame>
    {
        [SerializeField] public Camera UICamera;
        [SerializeField] public Light MainLight;
        [SerializeField] public EventSystem EventSystem;
        [SerializeField] public StandaloneInputModule StandaloneInputModule;
        [SerializeField] public RectTransform PanelRoot;
        [HideInInspector] public UIAdaptation UIAdaptation;
        [HideInInspector] public int mNumThreads;

        protected override void Init()
        {
            UIAdaptation.Me.Init();

            PanelRoot.sizeDelta = UIAdaptation.Me.ScreenSize;
            UIManager.Me.Init(PanelRoot);
            UIManager.Me.DestoryAll();
            StartCoroutine(LoadResources());
        }

        private IEnumerator LoadResources()
        {
            LoadingMgr.Me.Show();
            // 此处为提前加载各种模型
            yield return new WaitForSecondsRealtime(1);
            LoadingMgr.Me.Hide();
            ShowFirstPanel();
        }

        private void ShowFirstPanel()
        {
            UIManager.Me.ReturnMainPanel();
        }
    }
}