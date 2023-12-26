using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class ChangeTime : MonoBehaviour
{
    public MMFeedbacks timeCh;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (!Player.instance.timeChange)
            {
                Player.instance.timeChange = true;
                timeCh.PlayFeedbacks();
            }
        }
    }
}
