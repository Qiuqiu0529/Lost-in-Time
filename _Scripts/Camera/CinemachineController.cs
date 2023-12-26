using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//dd
/// <summary>
/// 相机抖动
/// 一定要放在cinemachine brain 控件里面
/// </summary>
public class CinemachineController : MonoBehaviour
{
    public static CinemachineController Instance { get; private set; }
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    CinemachineBrain cinemachineBrain;//cm的核心
    CinemachineVirtualCameraBase cinemachineVirtualCameraBase;//virtualcamera 的基类 
    CinemachineBasicMultiChannelPerlin[] cinemachineBasicMultiChannelPerlins;//这里是获取所有的rig来设置noise

    float shakeTimer;//抖动时间
    float shakeTimerTotal;//抖动总时间
    float startingIntensity;//抖动强度
    private void Awake()
    {
        Instance = this;
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }

   
    /// <param name="intensity">强度 </param>
    /// <param name="time">持续时间</param>
    public void ShakeCamera(float intensity, float time)
    {
        if (cinemachineBrain.ActiveVirtualCamera != null)
        {
            //if (debug) cineName = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.name + "[CinemachineShake.cs]";//deubug 相机名称 
            cinemachineVirtualCameraBase = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCameraBase>();//获取当前虚拟相机的基类
            cinemachineBasicMultiChannelPerlins = cinemachineVirtualCameraBase.GetComponentsInChildren<CinemachineBasicMultiChannelPerlin>();//获取该虚拟相机下的noise 设置
        }
        if (cinemachineBasicMultiChannelPerlins.Length > 0)
        {
            foreach (var item in cinemachineBasicMultiChannelPerlins)
            {
                item.m_AmplitudeGain = intensity;
            }
            startingIntensity = intensity;
            shakeTimerTotal = time;
            shakeTimer = time;
        }
    }
    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            float lerpValue = Mathf.Lerp(startingIntensity, 0f, shakeTimer / shakeTimerTotal); //线性方式慢慢减小
            if (shakeTimer < 0) lerpValue = 0f;//这里会出现负数的情况，正常来说应该是为0，暂时用判断解决 20210802

            foreach (var item in cinemachineBasicMultiChannelPerlins)
            {
                item.m_AmplitudeGain = lerpValue;
            }
        }
    }
}
