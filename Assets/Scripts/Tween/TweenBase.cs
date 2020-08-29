using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画基类，SetForwardTween、SetValue可以重写，其他不可以
/// 可以通过调用Pause、Continue实现动画的暂停和继续播放
/// </summary>
public class TweenBase : MonoBehaviour
{
    private EaseType mEaseType;
    private LoopType mLoopType;
    private int mLoopTimes;     // 动画默认播放一次，mLoopTimes是多播放几次，只在循环类型为Loop或PingPong时生效
    private float mTime;
    private bool isStarted;
    protected float mDeltaTime;
    protected float mPauseDeltaTime;    // 记录暂停时的mDeltaTime，用来暂停后继续播放
    protected Action StartCallBack;
    protected Action EndCallBack;
    protected Action UpdateCallBack;
    protected Action KillCallBack;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="ease">缓动曲线类型</param>
    /// <param name="loop">循环类型</param>
    /// <param name="loopTimes">循环次数：1则播放两次</param>
    /// <param name="time">播放时间</param>
    public void Init(EaseType ease, LoopType loop, int loopTimes, float time)
    {
        isStarted = false;
        mEaseType = ease;
        mLoopType = loop;
        mLoopTimes = loopTimes;
        mTime = time;
        mDeltaTime = mTime + 1;
        mPauseDeltaTime = -1;
        StartCallBack = null;
        EndCallBack = null;
        UpdateCallBack = null;
        KillCallBack = null;
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="start">开始时调用</param>
    /// <param name="end">结束时调用</param>
    /// <param name="update">更新时调用</param>
    /// <param name="kill">杀死时调用</param>
    public void SetCallBack(Action start, Action end, Action update, Action kill)
    {
        StartCallBack = start;
        EndCallBack = end;
        UpdateCallBack = update;
        KillCallBack = kill;
    }

    /// <summary>
    /// 开始播放
    /// </summary>
    public void Play(bool needCallBack = true)
    {
        isStarted = true;
        mDeltaTime = 0;
        if (needCallBack)
        {
            StartCallBack?.Invoke();
        }
    }

    /// <summary>
    /// 暂停播放
    /// </summary>
    public void Pause()
    {
        mPauseDeltaTime = mDeltaTime;
        mDeltaTime = mTime + 1;
    }

    /// <summary>
    /// 暂停后继续播放
    /// </summary>
    public void Continue()
    {
        if (mPauseDeltaTime >= 0)
        {
            mDeltaTime = mPauseDeltaTime;
            mPauseDeltaTime = -1;
        }
    }

    /// <summary>
    /// 停止播放
    /// </summary>
    public void Stop()
    {
        mDeltaTime = mTime + 1;
        mPauseDeltaTime = -1;
        mLoopTimes = 0;
    }

    /// <summary>
    /// 回收
    /// </summary>
    private void End()
    {
        SetValue();
        EndCallBack?.Invoke();
    }

    /// <summary>
    /// 清理
    /// </summary>
    private void Clear()
    {
        End();
        KillCallBack?.Invoke();
        DestroyImmediate(this);
    }

    private void Update()
    {
        if (isStarted)
        {
            // 时间未到
            if (mDeltaTime <= mTime)
            {
                mDeltaTime += Time.deltaTime;
                SetValue();
                // 时间到了，动画结束
                if (mDeltaTime > mTime)
                {
                    switch (mLoopType)
                    {
                        case LoopType.Loop:
                            // 循环次数小于0时，无限循环
                            // 大于0时，循环次数未用完
                            // 均需要继续播放动画
                            if (mLoopTimes != 0)
                            {
                                Play(false);
                                mLoopTimes--;
                                if (mLoopTimes < 0)
                                {
                                    mLoopTimes = -1;
                                }
                                End();
                            }
                            else
                            {
                                // 次数为0时，结束
                                Clear();
                            }
                            break;
                        case LoopType.PingPong:
                            // 循环次数小于0时，无限循环
                            // 大于0时，循环次数未用完
                            // 均需要继续播放动画
                            if (mLoopTimes != 0)
                            {
                                SetForwardTween();
                                Play(false);
                                mLoopTimes--;
                                if (mLoopTimes < 0)
                                {
                                    mLoopTimes = -1;
                                }
                                End();
                            }
                            else
                            {
                                // 次数为0时，结束
                                Clear();
                            }
                            break;
                        default:
                            mDeltaTime = mTime + 1;
                            Clear();
                            break;
                    }
                }
            }
            else
            {
                // 非暂停的情况下，此情况为动画结束，Clear
                if (mPauseDeltaTime < 0)
                {
                    Clear();
                }
            }
        }
    }

    /// <summary>
    /// 设置反向动画，PingPong专用
    /// </summary>
    protected virtual void SetForwardTween() { }

    /// <summary>
    /// 设置数值
    /// </summary>
    protected virtual void SetValue()
    {
        UpdateCallBack?.Invoke();
    }

    /// <summary>
    /// 根据时间获取插值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected float GetValue(float start, float end, float deltaTime)
    {
        float value = deltaTime / mTime;
        value = Mathf.Clamp01(value);
        switch (mEaseType)
        {
            case EaseType.EaseInQuad:
                return EaseInQuad(start, end, value);
            case EaseType.EaseOutQuad:
                return EaseOutQuad(start, end, value);
            case EaseType.EaseInOutQuad:
                return EaseInOutQuad(start, end, value);
            case EaseType.EaseInCubic:
                return EaseInCubic(start, end, value);
            case EaseType.EaseOutCubic:
                return EaseOutCubic(start, end, value);
            case EaseType.EaseInOutCubic:
                return EaseInOutCubic(start, end, value);
            case EaseType.EaseInQuart:
                return EaseInQuart(start, end, value);
            case EaseType.EaseOutQuart:
                return EaseOutQuart(start, end, value);
            case EaseType.EaseInOutQuart:
                return EaseInOutQuart(start, end, value);
            case EaseType.EaseInQuint:
                return EaseInQuint(start, end, value);
            case EaseType.EaseOutQuint:
                return EaseOutQuint(start, end, value);
            case EaseType.EaseInOutQuint:
                return EaseInOutQuint(start, end, value);
            case EaseType.EaseInSine:
                return EaseInSine(start, end, value);
            case EaseType.EaseOutSine:
                return EaseOutSine(start, end, value);
            case EaseType.EaseInOutSine:
                return EaseInOutSine(start, end, value);
            case EaseType.EaseInExpo:
                return EaseInExpo(start, end, value);
            case EaseType.EaseOutExpo:
                return EaseOutExpo(start, end, value);
            case EaseType.EaseInOutExpo:
                return EaseInOutExpo(start, end, value);
            case EaseType.EaseInCirc:
                return EaseInCirc(start, end, value);
            case EaseType.EaseOutCirc:
                return EaseOutCirc(start, end, value);
            case EaseType.EaseInOutCirc:
                return EaseInOutCirc(start, end, value);
            case EaseType.EaseInBounce:
                return EaseInBounce(start, end, value);
            case EaseType.EaseOutBounce:
                return EaseOutBounce(start, end, value);
            case EaseType.EaseInOutBounce:
                return EaseInOutBounce(start, end, value);
            case EaseType.EaseInBack:
                return EaseInBack(start, end, value);
            case EaseType.EaseOutBack:
                return EaseOutBack(start, end, value);
            case EaseType.EaseInOutBack:
                return EaseInOutBack(start, end, value);
            case EaseType.EaseInElastic:
                return EaseInElastic(start, end, value);
            case EaseType.EaseOutElastic:
                return EaseOutElastic(start, end, value);
            case EaseType.EaseInOutElastic:
                return EaseInOutElastic(start, end, value);
            case EaseType.Spring:
                return Spring(start, end, value);
            default:
                return Linear(start, end, value);
        }
    }

    #region Easing Curves

    private float Linear(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, value);
    }

    private float Clerp(float start, float end, float value)
    {
        float min = 0.0f;
        float max = 360.0f;
        float half = Mathf.Abs((max - min) * 0.5f);
        float retval = 0.0f;
        float diff = 0.0f;
        if ((end - start) < -half)
        {
            diff = ((max - start) + end) * value;
            retval = start + diff;
        }
        else if ((end - start) > half)
        {
            diff = -((max - end) + start) * value;
            retval = start + diff;
        }
        else retval = start + (end - start) * value;
        return retval;
    }

    private float Spring(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
        return start + (end - start) * value;
    }

    private float EaseInQuad(float start, float end, float value)
    {
        end -= start;
        return end * value * value + start;
    }

    private float EaseOutQuad(float start, float end, float value)
    {
        end -= start;
        return -end * value * (value - 2) + start;
    }

    private float EaseInOutQuad(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value + start;
        value--;
        return -end * 0.5f * (value * (value - 2) - 1) + start;
    }

    private float EaseInCubic(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value + start;
    }

    private float EaseOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }

    private float EaseInOutCubic(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value * value + start;
        value -= 2;
        return end * 0.5f * (value * value * value + 2) + start;
    }

    private float EaseInQuart(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value + start;
    }

    private float EaseOutQuart(float start, float end, float value)
    {
        value--;
        end -= start;
        return -end * (value * value * value * value - 1) + start;
    }

    private float EaseInOutQuart(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value * value * value + start;
        value -= 2;
        return -end * 0.5f * (value * value * value * value - 2) + start;
    }

    private float EaseInQuint(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value * value + start;
    }

    private float EaseOutQuint(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value * value * value + 1) + start;
    }

    private float EaseInOutQuint(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value * value * value * value + start;
        value -= 2;
        return end * 0.5f * (value * value * value * value * value + 2) + start;
    }

    private float EaseInSine(float start, float end, float value)
    {
        end -= start;
        return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
    }

    private float EaseOutSine(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
    }

    private float EaseInOutSine(float start, float end, float value)
    {
        end -= start;
        return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
    }

    private float EaseInExpo(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Pow(2, 10 * (value - 1)) + start;
    }

    private float EaseOutExpo(float start, float end, float value)
    {
        end -= start;
        return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
    }

    private float EaseInOutExpo(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
        value--;
        return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
    }

    private float EaseInCirc(float start, float end, float value)
    {
        end -= start;
        return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
    }

    private float EaseOutCirc(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * Mathf.Sqrt(1 - value * value) + start;
    }

    private float EaseInOutCirc(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
        value -= 2;
        return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
    }

    /* GFX47 MOD START */
    private float EaseInBounce(float start, float end, float value)
    {
        end -= start;
        float d = 1f;
        return end - EaseOutBounce(0, end, d - value) + start;
    }
    /* GFX47 MOD END */

    /* GFX47 MOD START */
    //private float bounce(float start, float end, float value){
    private float EaseOutBounce(float start, float end, float value)
    {
        value /= 1f;
        end -= start;
        if (value < (1 / 2.75f))
        {
            return end * (7.5625f * value * value) + start;
        }
        else if (value < (2 / 2.75f))
        {
            value -= (1.5f / 2.75f);
            return end * (7.5625f * (value) * value + .75f) + start;
        }
        else if (value < (2.5 / 2.75))
        {
            value -= (2.25f / 2.75f);
            return end * (7.5625f * (value) * value + .9375f) + start;
        }
        else
        {
            value -= (2.625f / 2.75f);
            return end * (7.5625f * (value) * value + .984375f) + start;
        }
    }
    /* GFX47 MOD END */

    /* GFX47 MOD START */
    private float EaseInOutBounce(float start, float end, float value)
    {
        end -= start;
        float d = 1f;
        if (value < d * 0.5f) return EaseInBounce(0, end, value * 2) * 0.5f + start;
        else return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
    }
    /* GFX47 MOD END */

    private float EaseInBack(float start, float end, float value)
    {
        end -= start;
        value /= 1;
        float s = 1.70158f;
        return end * (value) * value * ((s + 1) * value - s) + start;
    }

    private float EaseOutBack(float start, float end, float value)
    {
        float s = 1.70158f;
        end -= start;
        value = (value) - 1;
        return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
    }

    private float EaseInOutBack(float start, float end, float value)
    {
        float s = 1.70158f;
        end -= start;
        value /= .5f;
        if ((value) < 1)
        {
            s *= (1.525f);
            return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
        }
        value -= 2;
        s *= (1.525f);
        return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
    }

    /* GFX47 MOD START */
    private float EaseInElastic(float start, float end, float value)
    {
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d) == 1) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
    }
    /* GFX47 MOD END */

    /* GFX47 MOD START */
    //private float elastic(float start, float end, float value){
    private float EaseOutElastic(float start, float end, float value)
    {
        /* GFX47 MOD END */
        //Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d) == 1) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p * 0.25f;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
    }

    /* GFX47 MOD START */
    private float EaseInOutElastic(float start, float end, float value)
    {
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d * 0.5f) == 2) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
        return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
    }
    /* GFX47 MOD END */

    #endregion
}