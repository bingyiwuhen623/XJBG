using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using XJBG.Base;
using System;

namespace XJBG.UI
{
    /// <summary>
    /// 需要动态加载的界面类型
    /// </summary>
    public enum UIType
    {
        None,       // 无界面
        LoginPanel, // 登录界面
        MainPanel,  // 主界面
        TopMoney,   // 货币栏
    }
    /// <summary>
    /// 仅用来动态加载UI
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        private Transform uiParent;
        private Dictionary<UIType, string> allLoadPrefabsPath;
        private Dictionary<UIType, UIBase> allLoadedUI;
        private TopMoneyPanel _TopMoneyPanel;
        private int lastSortOrder;
        private UIType lastShowPanel;
        /// <summary>
        /// 记录跳转的UI，界面返回使用
        /// </summary>
        private List<UIType> backList;
        /// <summary>
        /// 不参与到返回机制的界面
        /// </summary>
        private List<UIType> unBackList;
        private bool isBattleBack;
        public bool IsBattleBack
        {
            get
            {
                return isBattleBack;
            }
            set
            {
                isBattleBack = value;
            }
        }
        private UISpecialData specialData;
        /// <summary>
        /// 特殊数据，用来保存进入战斗前一个界面的显示内容
        /// </summary>
        public UISpecialData SpecialData
        {
            get
            {
                return specialData;
            }
            set
            {
                specialData = value;
            }
        }

        public void Init(Transform parent)
        {
            InitParent(parent);
            allLoadPrefabsPath = new Dictionary<UIType, string>();
            allLoadedUI = new Dictionary<UIType, UIBase>();
            lastSortOrder = 0;
            specialData = new UISpecialData();
            isBattleBack = false;
            backList = new List<UIType>();
            InitUnBackList();
            InitPrefabsPath();
        }

        /// <summary>
        /// 预制件路径注册在此,游戏初始化时调用,暂不需异步，若以后需要提前加载所有UI，则使用异步
        /// </summary>
        private void InitPrefabsPath()
        {
            AddPath(UIType.LoginPanel, "Prefabs/UI/LoginPanel");
            AddPath(UIType.MainPanel, "Prefabs/UI/MainPanel");
            AddPath(UIType.TopMoney, "Prefabs/UI/TopMoney");
        }
        private void AddPath(UIType type, string path)
        {
            allLoadPrefabsPath.Add(type, path);
        }
        /// <summary>
        /// 不参与返回机制的界面，需要添加到unBackList中
        /// </summary>
        private void InitUnBackList()
        {
            unBackList = new List<UIType>
            {
                UIType.TopMoney,
            };
        }
        public void InitParent(Transform parent)
        {
            if (uiParent != null)
            {
                UnityEngine.Object.DestroyImmediate(uiParent.gameObject);
            }
            GameObject go = parent.gameObject;
            uiParent = go.transform;
            int childCount = uiParent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                UnityEngine.Object.DestroyImmediate(uiParent.GetChild(0).gameObject);
            }
        }
        /// <summary>
        /// 进入界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public void ShowUIByPath(UIType type)
        {
            bool needAddBack = !unBackList.Contains(type);
            ShowUIByPath(type, needAddBack);
        }
        /// <summary>
        /// 进入界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="needAddBack">是否需要加入返回列表，前进为true，后退为false</param>
        /// <returns></returns>
        private void ShowUIByPath(UIType type, bool needAddBack = true)
        {
            //Type classType = Type.GetType(type.ToString());
            //UIBase canvasUI = (UIBase)Activator.CreateInstance(classType);
            UIBase canvasUI;
            if (allLoadedUI.ContainsKey(type))
            {
                canvasUI = allLoadedUI[type];
            }
            else
            {
                canvasUI = LoadUIByPath(type);
            }
            //备用日志，勿删，出bug时使用
            //Debuger.LogWarning("needAddBack(" + needAddBack + "), lastShowPanel(" + lastShowPanel + ")");
            // 先在返回列表里加入上一个进入的界面
            if (needAddBack)
            {
                AddBackList(lastShowPanel);
            }
            SetLastPanel(type);
            GameObject canvasGo = canvasUI.GameObject;
            PanelTools.SortGoToLast(canvasGo);
            canvasGo.SetActive(true);
            canvasUI.Show(lastSortOrder++);
            PanelTools.ResetRectTransform(canvasGo.GetComponent<RectTransform>());
        }
        /// <summary>
        /// 动态加载UI，预制件带Canvas
        /// </summary>
        /// <param name="type">要加载的ui类型</param>
        /// <param name="parent">父级</param>
        /// <param name="sortingOrder">层级</param>
        private UIBase LoadUIByPath(UIType type)
        {
            string path = allLoadPrefabsPath[type];
            GameObject prefab = Resources.Load<GameObject>(path);
            GameObject canvasGo = GameObject.Instantiate(prefab, uiParent);
            string className = Enum.GetName(typeof(UIType), type);
            string classNameSpace = typeof(UIBase).Namespace;
            UIBase canvasUI = Extend.Create_Object_By_Class_Name<UIBase>(classNameSpace + "." + className);
            canvasUI.Init(canvasGo);
            allLoadedUI.Add(type, canvasUI);
            return canvasUI;
        }
        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <param name="type"></param>
        public void HideLoadedUI(UIType type)
        {
            if (allLoadedUI.ContainsKey(type))
            {
                UIBase canvasUI = allLoadedUI[type];
                canvasUI.Hide();
            }
            else
            {
                Debuger.LogWarning("该界面从未显示过：type(" + type + ")");
            }
        }
        /// <summary>
        /// 返回到主界面，以后添加返回到主界面按钮可直接调用
        /// </summary>
        public void ReturnMainPanel()
        {
            ClearBackList();
            ShowUIByPath(UIType.LoginPanel);
        }
        /// <summary>
        /// 清理返回列表
        /// </summary>
        public void ClearBackList()
        {
            backList.Clear();
        }
        public void BeforeBattle()
        {
            AddBackList(lastShowPanel);
        }
        private void SetLastPanel(UIType type)
        {
            if (!unBackList.Contains(type))
            {
                lastShowPanel = type;
            }
        }
        private void AddBackList(UIType type)
        {
            if (backList.Count <= 0 || backList[backList.Count - 1] != type)
            {
                //备用日志，勿删，出bug时使用
                //Debuger.LogWarning("backList.Add(" + type + ")");
                backList.Add(type);
            }
        }
        /// <summary>
        /// 销毁已加载的UI, 解除绑定，不实际销毁场景内预制件
        /// </summary>
        /// <param name="type">UI类型</param>
        private void DestroyLoadedUI(UIType type)
        {
            UIBase canvasUI = allLoadedUI[type];
            canvasUI.Destory();
            allLoadedUI.Remove(type);
        }
        /// <summary>
        /// 获取UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public T GetCanvasUI<T>(UIType type) where T : UIBase, new()
        {
            if (allLoadedUI.ContainsKey(type))
            {
                return allLoadedUI[type] as T;
            }
            else
            {
                UIBase uiBase = LoadUIByPath(type);
                uiBase.GameObject.SetActive(false);
                return uiBase as T;
            }
        }
        /// <summary>
        /// 是否正在显示，层级不一定最高
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsShowing(UIType type)
        {
            return GetCanvasUI<UIBase>(type).GameObject.activeSelf;
        }
        /// <summary>
        /// 返回到上一界面
        /// </summary>
        public void BackToLast()
        {
            UIType type = backList[backList.Count - 1];
            if (allLoadedUI.ContainsKey(type))
            {
                UIBase canvasUI = allLoadedUI[type];
                canvasUI.GameObject.SetActive(true);
                canvasUI.Show();
                SetLastPanel(type);
            }
            else
            {
                ShowUIByPath(type, false);
            }
            //备用日志，勿删，出bug时使用
            //Debuger.LogWarning("BackToLast:UIType(" + type + ")");
            OnlyBackList(1);
        }
        /// <summary>
        /// 只后退List，不切换界面
        /// </summary>
        /// <param name="count"></param>
        public void OnlyBackList(int count)
        {
            for (int i = 0; i < count; i++)
            {
                //备用日志，勿删，出bug时使用
                //Debuger.LogWarning("OnlyBackList:index(" + i + "), type(" + backList[backList.Count - 1] + ")");
                backList.RemoveAt(backList.Count - 1);
            }
        }
        public void Update()
        {
            if (allLoadedUI != null)
            {
                foreach (var pair in allLoadedUI)
                {
                    if (pair.Value != null)
                    {
                        if (pair.Value.GameObject.activeSelf)
                        {
                            pair.Value.UiUpdate();
                        }
                    }
                }
            }
        }

        #region 特殊界面：货币栏
        public void LoadTopMoneyPanel(Transform parent)
        {
            if (_TopMoneyPanel == null)
            {
                string path = allLoadPrefabsPath[UIType.TopMoney];
                GameObject prefab = Resources.Load<GameObject>(path);
                GameObject canvasGo = GameObject.Instantiate(prefab, parent);
                _TopMoneyPanel = new TopMoneyPanel();
                _TopMoneyPanel.Init(canvasGo);
            }
            _TopMoneyPanel.Transform.SetParent(parent);
            PanelTools.ResetRectTransform(_TopMoneyPanel.GameObject.GetComponent<RectTransform>());
            _TopMoneyPanel.Show(lastSortOrder + 1);
            _TopMoneyPanel.GameObject.SetActive(true);
        }
        public void HideTopMoneyPanel()
        {
            _TopMoneyPanel.Hide();
        }
        public TopMoneyPanel GetTopMoney()
        {
            return _TopMoneyPanel;
        }
        #endregion

        public void DestoryAll()
        {
            List<UIType> uiTypes = new List<UIType>(allLoadedUI.Keys);
            foreach (UIType type in uiTypes)
            {
                DestroyLoadedUI(type);
            }
        }
    }
    /// <summary>
    /// 此类用来存储特殊数据
    /// </summary>
    public class UISpecialData
    {
        public Dictionary<UIType, object> dataDic = new Dictionary<UIType, object>();
        public void AddSpecialData(UIType type, object data)
        {
            if (!dataDic.ContainsKey(type))
            {
                dataDic.Add(type, data);
            }
            else
            {
                dataDic[type] = data;
            }
        }
    }
}