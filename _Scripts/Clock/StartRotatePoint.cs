using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRotatePoint : MonoBehaviour
{
    public HandController handController;
    private int enterTime = 0;//防止再次进入错误激活转动

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")&&enterTime<=0)
        {
            handController.canRotate = true;
            enterTime++;
        }
    }
}
