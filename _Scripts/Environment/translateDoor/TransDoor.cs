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
                    case TransType.LOOP://˫����ѭ����
                        dirPoint.StartCoroutine(dirPoint.TransCoolDown());
                        break;
                    case TransType.ONCE://ֻ�ܴ�����㵽��һ������ֻ�ܴ�һ��
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
