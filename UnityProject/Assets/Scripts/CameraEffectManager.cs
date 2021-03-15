using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectManager : MonoBehaviour
{
    public static CameraEffectManager Instance;

    public CinemachineVirtualCamera dollyZoomCam;
    public CinemachineVirtualCamera leftZoomCam;
    public CinemachineVirtualCamera rightZoomCam;

    public Action OnBeginDeZoom;

    private void Awake()
    {
        Instance = this;
    }

    public void DoScreenShake()
    {

    }

    public void Zoom(Vector3 pos, Action callback)
    {
        if(pos == Vector3.left)
        {
            dollyZoomCam.enabled = false;
            rightZoomCam.enabled = false;
            leftZoomCam.enabled = true;
        }
        else
        {
            dollyZoomCam.enabled = false;
            rightZoomCam.enabled = true;
            leftZoomCam.enabled = false;
        }
        OnBeginDeZoom += callback;
        FragmentStopper.Instance.StopFragments();
        TimeEffectManager.Instance.SetTimeScale(0.2f, 1.2f);
        InvokeRealTime("DeZoom", 2f);
    }

    public void InvokeRealTime(string name, float duration)
    {
        Invoke(name, duration * Time.timeScale);
    }

    private void DeZoom()
    {
        TimeEffectManager.Instance.ResetTimeScale();
        FragmentStopper.Instance.UnStopFragments();
        dollyZoomCam.enabled = true;
        rightZoomCam.enabled = false;
        leftZoomCam.enabled = false;
        OnBeginDeZoom?.Invoke();
        OnBeginDeZoom = null;
    }
}
