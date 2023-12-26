using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Disappear : Shaker
{
    public float shakingtime=1.2f;
    public float disTime=3f;
    bool tri = false;
    public GameObject kid;
    public bool useFB;
    public MMFeedbacks disFB;
    public MMFeedbacks appearFB;
    SpriteRenderer sprite;
    Collider2D collider;
    private void Start()
    {
        startPos = transform.localPosition;
        sprite = kid.GetComponent<SpriteRenderer>();
        collider = kid.GetComponent<Collider2D>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Disappear");
            if (!tri)
            {
                tri = true;
                shakeTime = shakingtime;
                Shake(shakeIntensity, shakeTime);
                StartCoroutine(Dis());
            }   
        }
    }

    public IEnumerator Dis()
    {
        if (useFB)
            disFB.PlayFeedbacks();
        Color tempcolor = sprite.color;
        float timer = 0;
        while (timer < shakeTime)
        {
            if (tempcolor.a >0)
            {
                tempcolor.a -= Random.Range(0.03f, 0.05f);
                sprite.color = tempcolor;
            }
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        collider.enabled = false;
        tempcolor.a = 0;
        sprite.color = tempcolor;
      

        yield return new WaitForSeconds(disTime);
        if (useFB)
            appearFB.PlayFeedbacks();
        Shake(shakeIntensity, 0.5f);
        while (timer < 1f)
        {
            if (tempcolor.a < 1)
            {
                tempcolor.a += Random.Range(0.03f, 0.05f);
                sprite.color = tempcolor;
            }
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        tempcolor.a = 1;
        sprite.color = tempcolor;
        collider.enabled = true;
        tri = false;
    }
}