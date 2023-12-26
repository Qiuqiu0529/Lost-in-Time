using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using MoreMountains.Feedbacks;
public class Chasing : MonoBehaviour
{
    Transform playerVi,player;

    Queue<Vector3> pos=new Queue<Vector3>();
   // Queue<Quaternion> rot = new Queue<Quaternion>();
    Queue<Vector3> sca= new Queue<Vector3>();
    Queue<Sprite> pic = new Queue<Sprite>();
    Vector3 dir;
    SpriteRenderer mysprite;
    SpriteRenderer playerVisual;

    CapsuleCollider2D collider2D;
    public ParticleSystem chasePa;
    public float waitTime = 3f;

    public bool useFB;
    public MMFeedbacks stopFB;
    private void Start()
    {
        player = Player.instance.transform;
        playerVi = PlayerVisual.instance.playerPic.transform;
        mysprite = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<CapsuleCollider2D>();
        collider2D.enabled = false;
        playerVisual = PlayerVisual.instance.playerNow;
    }
    public IEnumerator AddQueue()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            pos.Enqueue(playerVi.position);
            pic.Enqueue(PlayerVisual.instance.playerNow.sprite);
           // rot.Enqueue(player.rotation);
            sca.Enqueue(player.localScale);
        }
    }

    public IEnumerator ChasePlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            transform.localPosition=Vector3.Slerp(transform.localPosition, dir,0.3f);
        }
    }

    public IEnumerator DelQueue()
    {
        mysprite.sprite = PlayerVisual.instance.playerNow.sprite;
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(ChasePlayer());
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            dir = pos.Dequeue();
            mysprite.sprite = pic.Dequeue();
            chasePa.textureSheetAnimation.SetSprite(0, mysprite.sprite);
            transform.localScale = sca.Dequeue();
            chasePa.gameObject.transform.localScale = transform.localScale;
            //transform.localRotation = rot.Dequeue();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(AddQueue());
        StartCoroutine(DelQueue());
        StartCoroutine(Blink());
    }
    
    private void OnDisable()
    {
        StopAllCoroutines();
        collider2D.enabled = false;
        pos.Clear();
        pic.Clear();
    }

    public void Interrupt()
    {
        StopAllCoroutines();
        pos.Clear();
        pic.Clear();
        if (useFB)
           stopFB.PlayFeedbacks();
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 10; ++i)
        {
            Color color = mysprite.color;
            color.a = 0.2f;
            mysprite.color = color;
            yield return new WaitForSeconds(0.1f);
            Color color1 = mysprite.color;
            color1.a = 1;
            mysprite.color = color1;
            yield return new WaitForSeconds(0.1f);
        }
        collider2D.enabled = true;
    }
}
