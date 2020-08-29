using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenColor : TweenBase
{
    protected Graphic mGraphic;
    protected Color mCurrValue;
    private Color mStart;
    private Color mEnd;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="start">初始值</param>
    /// <param name="end">结束值</param>
    /// <param name="ease">缓动曲线</param>
    /// <param name="loop">循环类型</param>
    /// <param name="loopTimes">循环次数</param>
    /// <param name="time">动画时间</param>
    public void Init(Graphic graphic, Color start, Color end, EaseType ease, LoopType loop, int loopTimes, float time)
    {
        mGraphic = graphic;
        mStart = start;
        mEnd = end;
        mCurrValue = mStart;
        Init(ease, loop, loopTimes, time);
    }

    /// <summary>
    /// 设置反向动画，PingPong专用
    /// </summary>
    protected override void SetForwardTween()
    {
        base.SetForwardTween();
        Color temp = mStart;
        mStart = mEnd;
        mEnd = temp;
    }

    /// <summary>
    /// 设置数值
    /// </summary>
    protected override void SetValue()
    {
        base.SetValue();
        float rValue = GetValue(mStart.r, mEnd.r, mDeltaTime);
        float gValue = GetValue(mStart.g, mEnd.g, mDeltaTime);
        float bValue = GetValue(mStart.b, mEnd.b, mDeltaTime);
        float aValue = GetValue(mStart.a, mEnd.a, mDeltaTime);
        mCurrValue = new Color(rValue, gValue, bValue, aValue);
        mGraphic.color = mCurrValue;
    }
}