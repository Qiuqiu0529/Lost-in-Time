using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CineMachine : MonoBehaviour
{
    CinemachineVirtualCamera cinemachineVirtual;
    private void Awake()
    {
        cinemachineVirtual = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        cinemachineVirtual.Follow = Player.instance.transform;
    }

}
