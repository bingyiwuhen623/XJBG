using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenSizeDelta : TweenVector2
{
    private RectTransform mTrans;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="trans">要变化的trans</param>
    /// <param name="start">初始值</param>
    /// <param name="end">结束值</param>
    /// <param name="ease">缓动曲线</param>
    /// <param name="loop">循环类型</param>
    /// <param name="loopTimes">循环次数</param>
    /// <param name="time">动画时间</param>
    public void Init(RectTransform trans, Vector2 start, Vector2 end, EaseType ease, LoopType loop, int loopTimes, float time)
    {
        mTrans = trans;
        Init(start, end, ease, loop, loopTimes, time);
    }

    /// <summary>
    /// 设置反向动画，PingPong专用
    /// </summary>
    protected override void SetForwardTween()
    {
        base.SetForwardTween();
    }

    /// <summary>
    /// 设置数值
    /// </summary>
    protected override void SetValue()
    {
        base.SetValue();
        mTrans.sizeDelta = mCurrValue;
    }
}