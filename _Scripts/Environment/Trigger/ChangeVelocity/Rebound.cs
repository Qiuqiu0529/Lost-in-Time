using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class Rebound : MonoBehaviour
{
    public Vector3 dir;
    public float speed = 30f;
    public float coolTime = 0.3f;

    public float reloadTime = 5f;

    public AnimationCurve speedCurve;
    Player player;

    public bool isActive = true;
    SpriteRenderer sprite;

    public bool useFB;
    public MMFeedbacks useFeedBack;
    public MMFeedbacks appFB;
    bool isIn = false;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isIn = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && !isIn&&isActive)
        {
            player = Player.instance;
            isIn = true;
            isActive = false;
            dir = player.Velocity * -1;

            if (useFB)
                useFeedBack.PlayFeedbacks();

            StopCoroutine(Reseting());
            player.AccelVelocity(dir, speed);

            player.Velocity = Vector3.zero;
            StartCoroutine(Dis());
            StartCoroutine(Reseting());
        }
    }


    public IEnumerator Dis()
    {
        //Color temp = sprite.color;
        yield return new WaitForSeconds(0.5f);
        //temp.a = 0.2f;
        //sprite.color = temp;
        sprite.enabled = false;
        yield return new WaitForSeconds(reloadTime - 0.5f);
        sprite.enabled = true;
        isActive = true;
        //temp.a = 1;
        //sprite.color = temp;
        if (useFB)
        {
            appFB.PlayFeedbacks();
        }
    }

    public IEnumerator Reseting()
    {
        float timer = 0;
        float temp;
        while (timer < coolTime)
        {
            temp = Mathf.Lerp(0, speed, speedCurve.Evaluate(timer));
            player.Velocity /= 2;
            player.AccelVelocity(dir, temp);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (!isIn)
        {
            player.AccelVelocity(dir, 0);
        }
    }
}
