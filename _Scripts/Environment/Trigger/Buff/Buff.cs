using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Buff : MonoBehaviour
{
    public enum BuffType
    {
        JUMPFORCE,
        RUNSPEED,
        TIME
    }
    public BuffType buffType;
    bool isActive = true;

    //public bool canSuperimpos = false;
    public float add = 0f;
    public float continueTime = 0f;
    public float recoverTime = 0f;

    public bool useFB;
    public MMFeedbacks buffFB;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && isActive)
        {
            Debug.Log("Playerenter");
            isActive = false;
           
            if (useFB)
                buffFB.PlayFeedbacks();
           
            SoundManager.PlayBuff();
            StartCoroutine(Dis());

        }
    }
    public IEnumerator Dis()
    {
        yield return new WaitForSeconds(0.5f);
        Player player = Player.instance;
        switch (buffType)
        {
            case BuffType.JUMPFORCE:
                player.StartCoroutine(player.AddJumpForceTime(add, continueTime));
                break;
            case BuffType.RUNSPEED:
                player.StartCoroutine(player.AddRunSpeedTime(add, continueTime));
                break;
            case BuffType.TIME:
                PLayerInteract.instance.ChangeLife(add);
                break;
        }
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().enabled = false;
        /*if (recoverTime > 0)
            StartCoroutine(Reset());*/
    }


    public IEnumerator Reset()
    {
        yield return new WaitForSeconds(recoverTime);
        GetComponent<SpriteRenderer>().enabled = true;
        isActive = true;
    }

}
