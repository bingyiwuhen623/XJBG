using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Linear:匀速
/// Spring:减速，最后弹跳一次
/// In:先慢后快
/// Out:先快后慢
/// InOut:慢-快-慢
/// Sine:弧度微小
/// Cubic:弧度小
/// Quint:弧度中
/// Circ:弧度大
/// Elastic:震动
/// Quad:弧度微小
/// Quart:弧度小
/// Expo:弧度中
/// Back:弹出
/// Bounce:弹跳
/// </summary>
public enum EaseType
{
    EaseInQuad,
    EaseOutQuad,
    EaseInOutQuad,
    EaseInCubic,
    EaseOutCubic,
    EaseInOutCubic,
    EaseInQuart,
    EaseOutQuart,
    EaseInOutQuart,
    EaseInQuint,
    EaseOutQuint,
    EaseInOutQuint,
    EaseInSine,
    EaseOutSine,
    EaseInOutSine,
    EaseInExpo,
    EaseOutExpo,
    EaseInOutExpo,
    EaseInCirc,
    EaseOutCirc,
    EaseInOutCirc,
    Linear,
    Spring,
    /* GFX47 MOD START */
    //bounce,
    EaseInBounce,
    EaseOutBounce,
    EaseInOutBounce,
    /* GFX47 MOD END */
    EaseInBack,
    EaseOutBack,
    EaseInOutBack,
    /* GFX47 MOD START */
    //elastic,
    EaseInElastic,
    EaseOutElastic,
    EaseInOutElastic,
    /* GFX47 MOD END */
}
/// <summary>
/// 循环类型
/// </summary>
public enum LoopType
{
    /// <summary>
    /// 不循环
    /// </summary>
    None,
    /// <summary>
    /// 循环
    /// </summary>
    Loop,
    /// <summary>
    /// 返回一次
    /// </summary>
    PingPong
}
public class TweenMgr : SingletonMonoBehaviour<TweenMgr>
{
    Dictionary<Type, List<TweenBase>> mAllTweenDic;

    protected override void Init()
    {
        mAllTweenDic = new Dictionary<Type, List<TweenBase>>();
    }

    private void AddTween(Type type, TweenBase tween)
    {
        List<TweenBase> list;
        if (mAllTweenDic.ContainsKey(type))
        {
            list = mAllTweenDic[type];
        }
        else
        {
            list = new List<TweenBase>();
            mAllTweenDic.Add(type, list);
        }
        list.Add(tween);
    }

    /// <summary>
    /// 暂停所有动画
    /// </summary>
    public void PauseAllTween()
    {
        foreach(KeyValuePair<Type, List<TweenBase>> pair in mAllTweenDic)
        {
            foreach (TweenBase tween in pair.Value)
            {
                tween.Pause();
            }
        }
    }

    /// <summary>
    /// 继续播放所有动画
    /// </summary>
    public void ContinueAllTween()
    {
        foreach (KeyValuePair<Type, List<TweenBase>> pair in mAllTweenDic)
        {
            foreach (TweenBase tween in pair.Value)
            {
                tween.Continue();
            }
        }
    }

    /// <summary>
    /// 停止所有动画
    /// </summary>
    public void StopAllTween()
    {
        foreach (KeyValuePair<Type, List<TweenBase>> pair in mAllTweenDic)
        {
            foreach (TweenBase tween in pair.Value)
            {
                tween.Stop();
            }
        }
    }

    /// <summary>
    /// 暂停对应类型所有动画
    /// </summary>
    public void PauseByType(Type type)
    {
        foreach (TweenBase tween in mAllTweenDic[type])
        {
            tween.Pause();
        }
    }

    /// <summary>
    /// 继续播放对应类型所有动画
    /// </summary>
    public void ContinueByType(Type type)
    {
        foreach (TweenBase tween in mAllTweenDic[type])
        {
            tween.Continue();
        }
    }

    /// <summary>
    /// 停止对应类型所有动画
    /// </summary>
    public void StopByType(Type type)
    {
        foreach (TweenBase tween in mAllTweenDic[type])
        {
            tween.Stop();
        }
    }

    public TweenBase LocalMove(GameObject go, Vector3 start, Vector3 end, float time,
        EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
        Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        return LocalMove(go.transform, start, end, time, easeType, loopType, loopTimes, startAction, endAction, updateAction, killAction);
    }
    public TweenBase LocalMove(Transform trans, Vector3 start, Vector3 end, float time,
        EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
        Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        TweenLocalPosition tween = trans.gameObject.AddComponent<TweenLocalPosition>();
        tween.Init(trans, start, end, easeType, loopType, loopTimes, time);
        tween.SetCallBack(startAction, endAction, updateAction, killAction);
        tween.Play();
        AddTween(typeof(TweenLocalPosition), tween);
        return tween;
    }

    public TweenBase Move(GameObject go, Vector3 start, Vector3 end, float time,
       EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
        Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        return Move(go.transform, start, end, time, easeType, loopType, loopTimes, startAction, endAction, updateAction, killAction);
    }
    public TweenBase Move(Transform trans, Vector3 start, Vector3 end, float time,
        EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
        Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        TweenPosition tween = trans.gameObject.AddComponent<TweenPosition>();
        tween.Init(trans, start, end, easeType, loopType, loopTimes, time);
        tween.SetCallBack(startAction, endAction, updateAction, killAction);
        tween.Play();
        AddTween(typeof(TweenPosition), tween);
        return tween;
    }

    public TweenBase Scale(GameObject go, Vector3 start, Vector3 end, float time,
       EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
        Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        return Scale(go.transform, start, end, time, easeType, loopType, loopTimes, startAction, endAction, updateAction, killAction);
    }
    public TweenBase Scale(Transform trans, Vector3 start, Vector3 end, float time,
        EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
        Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        TweenLocalScale tween = trans.gameObject.AddComponent<TweenLocalScale>();
        tween.Init(trans, start, end, easeType, loopType, loopTimes, time);
        tween.SetCallBack(startAction, endAction, updateAction, killAction);
        tween.Play();
        AddTween(typeof(TweenLocalScale), tween);
        return tween;
    }

    public TweenBase Rotation(GameObject go, Vector3 start, Vector3 end, float time,
      EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
       Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        return Rotation(go.transform, start, end, time, easeType, loopType, loopTimes, startAction, endAction, updateAction, killAction);
    }
    public TweenBase Rotation(Transform trans, Vector3 start, Vector3 end, float time,
        EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
        Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        TweenRotation tween = trans.gameObject.AddComponent<TweenRotation>();
        tween.Init(trans, start, end, easeType, loopType, loopTimes, time);
        tween.SetCallBack(startAction, endAction, updateAction, killAction);
        tween.Play();
        AddTween(typeof(TweenRotation), tween);
        return tween;
    }

    public TweenBase LocalRotation(GameObject go, Vector3 start, Vector3 end, float time,
      EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
       Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        return LocalRotation(go.transform, start, end, time, easeType, loopType, loopTimes, startAction, endAction, updateAction, killAction);
    }
    public TweenBase LocalRotation(Transform trans, Vector3 start, Vector3 end, float time,
        EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
        Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        TweenLocalRotation tween = trans.gameObject.AddComponent<TweenLocalRotation>();
        tween.Init(trans, start, end, easeType, loopType, loopTimes, time);
        tween.SetCallBack(startAction, endAction, updateAction, killAction);
        tween.Play();
        AddTween(typeof(TweenLocalRotation), tween);
        return tween;
    }

    public TweenBase SizeDelta(GameObject go, Vector2 start, Vector2 end, float time,
      EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
       Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        return SizeDelta(go.GetComponent<RectTransform>(), start, end, time, easeType, loopType, loopTimes, startAction, endAction, updateAction, killAction);
    }
    public TweenBase SizeDelta(RectTransform trans, Vector2 start, Vector2 end, float time,
        EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
        Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        TweenSizeDelta tween = trans.gameObject.AddComponent<TweenSizeDelta>();
        tween.Init(trans, start, end, easeType, loopType, loopTimes, time);
        tween.SetCallBack(startAction, endAction, updateAction, killAction);
        tween.Play();
        AddTween(typeof(TweenSizeDelta), tween);
        return tween;
    }

    public TweenBase Alpha(Graphic graphic, float start, float end, float time,
        EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
        Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        TweenAlpha tween = graphic.gameObject.AddComponent<TweenAlpha>();
        tween.Init(graphic, start, end, easeType, loopType, loopTimes, time);
        tween.SetCallBack(startAction, endAction, updateAction, killAction);
        tween.Play();
        AddTween(typeof(TweenAlpha), tween);
        return tween;
    }

    public TweenBase Color(Graphic graphic, Color start, Color end, float time,
        EaseType easeType = EaseType.Linear, LoopType loopType = LoopType.None, int loopTimes = 0,
        Action startAction = null, Action endAction = null, Action updateAction = null, Action killAction = null)
    {
        TweenColor tween = graphic.gameObject.AddComponent<TweenColor>();
        tween.Init(graphic, start, end, easeType, loopType, loopTimes, time);
        tween.SetCallBack(startAction, endAction, updateAction, killAction);
        tween.Play();
        AddTween(typeof(TweenColor), tween);
        return tween;
    }
}