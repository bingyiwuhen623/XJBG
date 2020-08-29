using UnityEngine;
using UnityEngine.UI;

namespace XJBG.UI
{
    public class UIBase
    {
        protected int sortingOrder;
        protected UIType thisUIType = UIType.None;
        protected GameObject gameObject;
        protected Transform transform => gameObject.transform;
        public GameObject GameObject
        {
            get
            {
                return gameObject;
            }
        }
        public Transform Transform
        {
            get
            {
                return transform;
            }
        }
        /// <summary>
        /// 初始化，管理类用
        /// </summary>
        /// <param name="sortingOrder">层级</param>
        public void Init(GameObject gameObject)
        {
            this.gameObject = gameObject;
            onInit();
        }
        /// <summary>
        /// 显示，管理类用
        /// </summary>
        /// <param name="sortingOrder">界面显示层级，小于0时，不修改层级</param>
        public void Show(int sortingOrder = -1)
        {
            if (sortingOrder >= 0)
            {
                this.sortingOrder = sortingOrder;
            }
            onShow();
        }
        /// <summary>
        /// 隐藏，管理类用
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            onHide();
        }
        /// <summary>
        /// Update，管理类用
        /// </summary>
        public void UiUpdate()
        {
            onUpdate();
        }
        /// <summary>
        /// 销毁，管理类用
        /// </summary>
        public void Destory()
        {
            onDestory();
        }
        #region 界面重写用
        /// <summary>
        /// 初始化，界面用，每次场景只调一次
        /// </summary>
        protected virtual void onInit() { }
        /// <summary>
        /// 显示，界面用，每次显示都会调用
        /// </summary>
        protected virtual void onShow() { }
        /// <summary>
        /// update，界面用
        /// </summary>
        protected virtual void onUpdate() { }
        /// <summary>
        /// 隐藏，界面用，每次隐藏都会调用
        /// </summary>
        protected virtual void onHide() { }
        /// <summary>
        /// 销毁，界面用
        /// </summary>
        protected virtual void onDestory() { }
        #endregion
    }
}