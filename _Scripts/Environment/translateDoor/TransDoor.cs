using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class TransDoor : MonoBehaviour
{
    public TransDoor dirPoint;
    public bool active = true;
    public TransType trans;
    public float coolTime = 2f;
    Player player;

    public bool useFB;
    public MMFeedbacks transDoor;
    public enum TransType
    {
        ONEWAY,
        LOOP,
        ONCE,
    }

    private void Awake()
    {
        if (dirPoint != null)
        {
            dirPoint.dirPoint = this;
            dirPoint.trans = trans;
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player = Player.instance;
            if (active)
            {
                dirPoint.active = false;
                if (useFB)
                    transDoor.PlayFeedbacks();
                SoundManager.PlayTansDoor();
                switch (trans)
                {
                    case TransType.LOOP://双方可循环传
                        dirPoint.StartCoroutine(dirPoint.TransCoolDown());
                        break;
                    case TransType.ONCE://只能从这个点到另一个点且只能传一次
                        active = false;
                        break;
                }
                player.transform.position = dirPoint.gameObject.transform.position;
               
            }
        }
    }

    public IEnumerator TransCoolDown()
    {
       
        yield return new WaitForSeconds(coolTime);
        active = true;
    }

}
