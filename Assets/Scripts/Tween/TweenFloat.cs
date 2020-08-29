using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenFloat : TweenBase
{
    protected float mCurrValue;
    private float mStart;
    private float mEnd;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="start">初始值</param>
    /// <param name="end">结束值</param>
    /// <param name="ease">缓动曲线</param>
    /// <param name="loop">循环类型</param>
    /// <param name="loopTimes">循环次数</param>
    /// <param name="time">动画时间</param>
    public void Init(float start, float end, EaseType ease, LoopType loop, int loopTimes, float time)
    {
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
        float temp = mStart;
        mStart = mEnd;
        mEnd = temp;
    }

    /// <summary>
    /// 设置数值
    /// </summary>
    protected override void SetValue()
    {
        base.SetValue();
        mCurrValue = GetValue(mStart, mEnd, mDeltaTime);
    }
}