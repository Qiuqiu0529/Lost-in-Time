using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class ChangeGravity : MonoBehaviour
{
    bool isActive = true;
    public float coolTime = 3f;
    public MMFeedbacks changeGr;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")&&isActive)
        {
            Debug.Log("ChangeGravity");
            isActive = false;
            changeGr.PlayFeedbacks();
            Player.instance.DefyingGravity();
            StartCoroutine(CoolDown());
        }
    }

    public IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolTime);
        isActive = true;
    }
}
