using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPoint : MonoBehaviour
{
    public float trackingTime=5f;
    public int isInSafeArea = 0;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("CatchPointIn");
            isInSafeArea = 1;
            StartCoroutine(Catching());
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("CatchPointOut");
            isInSafeArea = 0;
            StopCoroutine(Catching());
        }
    }

    public IEnumerator Catching()
    {
        Player.instance.StayStatic();
        float temp = trackingTime;
        float tempScale = Player.instance.rigidbody2.gravityScale;
        Player.instance.rigidbody2.gravityScale = 0f;
        while (temp > 0)
        {
            Player.instance.transform.position = Vector3.Slerp(Player.instance.transform.position,
                transform.position,0.2f);
            Player.instance.CheckSlope();
            temp -= Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        Player.instance.rigidbody2.gravityScale = tempScale;
        Player.instance.canMove = true;

        yield return 0;
    }
}
