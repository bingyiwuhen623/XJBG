using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XJBG.Base;

namespace XJBG.UI
{
    public class LoginPanel : UIBase
    {
        private Image background;
        private Image img;
        private TweenBase tween;

        protected override void onInit()
        {
            background = PanelTools.Find<Image>(gameObject, "Background");
            img = PanelTools.Find<Image>(gameObject, "Image");
        }

        protected override void onShow()
        {
            tween = TweenMgr.Me.Move(img.transform, Vector3.zero, new Vector3(500, 300, 0), 3f, EaseType.Spring, LoopType.Loop, -1);
            StartGame.Me.StartCoroutine(StopTween());
        }

        private IEnumerator StopTween()
        {
            yield return new WaitForSeconds(10f);
            TweenMgr.Me.PauseAllTween();
            yield return new WaitForSeconds(3f);
            TweenMgr.Me.ContinueAllTween();
            yield return new WaitForSeconds(8f);
            TweenMgr.Me.PauseByType(typeof(TweenPosition));
            yield return new WaitForSeconds(3f);
            TweenMgr.Me.ContinueByType(typeof(TweenPosition));
            yield return new WaitForSeconds(9f);
            TweenMgr.Me.StopByType(typeof(TweenPosition));
        }

        protected override void onUpdate()
        {
            base.onUpdate();
        }

        protected override void onHide()
        {
            base.onHide();
        }

        protected override void onDestory()
        {
            base.onDestory();
        }
    }
}