using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeEffectManager : MonoBehaviour
{
    public static TimeEffectManager Instance;
    bool isTimeScaleReady = true;

    float targetTime;

    private void Awake()
    {
        Instance = this;
    }

    public void SetTimeScale(float target, float duration)
    {
        isTimeScaleReady = false;
        Time.timeScale = target;
        targetTime = Time.realtimeSinceStartup + duration;
    }

    public void ResetTimeScale()
    {
        isTimeScaleReady = true;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if(!isTimeScaleReady && Time.realtimeSinceStartup > targetTime )
        {
            ResetTimeScale();
        }
    }
}
