using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class KillZone : MonoBehaviour
{
    public int damage = 0;
    public float coolTime = 1f;
    bool coolDown = true;
    public bool useFB;
    public MMFeedbacks useFeedBack;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")&&coolDown)
        {
            coolDown = false;
            Debug.Log("KillZone");
            PLayerInteract.instance.ChangeLife(-damage);
            if (useFB)
                useFeedBack.PlayFeedbacks();
            StartCoroutine(KillZoneCoolDown());
        }
    }

    public IEnumerator KillZoneCoolDown()
    {
        yield return new WaitForSeconds(coolTime);
        coolDown = true;
    }
}
