using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToNextLevel : MonoBehaviour
{
    public bool isActive = true;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")&&isActive)
        {
            SceneMgr.instance.NextChapter();
            isActive = false;
            Debug.Log("´«ËÍ");
        }
    }
}
