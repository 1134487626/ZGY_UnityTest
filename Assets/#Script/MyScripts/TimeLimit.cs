using Anole;
using UnityEngine;

/// <summary>
/// 时间倒计时
/// </summary>
public class TimeLimit
{
    private float maxTime;
    private float minTime = 0;
    private bool loop = false;//倒计时完成是否直接重新开始

    /// <summary>
    /// 是否开始计时
    /// </summary>
    public bool Run { get; private set; }

    private TimeLimit() { }

    public TimeLimit(float r_MaxTime)
    {
        IsGreaterZero(r_MaxTime);
    }

    public TimeLimit(float r_MaxTime, bool r_Loop)
    {
        if (IsGreaterZero(r_MaxTime))
        {
            loop = r_Loop;
        }
    }

    /// <summary>
    /// 检测传入的限制时间是否大于0
    /// </summary>
    /// <param name="r_MaxTime"></param>
    /// <returns></returns>
    private bool IsGreaterZero(float r_MaxTime)
    {
        if (r_MaxTime > 0)
        {
            maxTime = r_MaxTime;
            return true;
        }
        else
        {
            DeBug.LogError("注意：传入的上限时间低于或等于0，无法进行倒计时！");
            return false;
        }
    }

    /// <summary>
    /// 重置初始参数
    /// </summary>
    private void Init()
    {
        Run = false;
        minTime = 0;
    }

    /// <summary>
    /// 放在Update每帧监测
    /// </summary>
    public void Start()
    {
        if (Run)
        {
            minTime += Time.deltaTime;
            if (minTime >= maxTime)
            {
                if (loop)
                {
                    Reset();
                }
                else
                {
                    Init();
                }
            }
        }
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    public void Reset()
    {
        minTime = 0;
        Run = true;
    }
}

