using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Company : MonoBehaviour
{
    Transform target;
    bool isactive = false;
    Vector3 dir;
    Queue<Vector3> pos = new Queue<Vector3>();
    Queue<Vector3> sca = new Queue<Vector3>();

    public bool useFB;
    public MMFeedbacks company;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && !isactive)
        {
            isactive = true;
            target = ToyMgr.instance.lastToy.transform;
            ToyMgr.instance.lastToy = this.gameObject;
            if (useFB)
            {
                company.PauseFeedbacks();
            }
            StartCoroutine(CompanyPlayer());
        }
    }
    public IEnumerator CompanyPlayer()
    {
        StartCoroutine(AddQueue());
        StartCoroutine(DelQueue());
        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            if (target.CompareTag("Player"))
            {
                if ((dir - transform.position).magnitude >= 3f)
                {
                    transform.position = Vector3.Lerp(transform.position, dir, 0.5f);
                }
            }
            else
            {
                if ((dir - transform.position).magnitude >= 1.5f)
                {
                    transform.position = Vector3.Lerp(transform.position, dir, 0.5f);
                }
            }
        }
    }
    public IEnumerator AddQueue()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            pos.Enqueue(target.position);
            sca.Enqueue(target.localScale);
        }
    }
    public IEnumerator DelQueue()
    {
        yield return new WaitForSeconds(0.1f);
      
        while (true)
        {
            yield return new WaitForFixedUpdate();
            dir = pos.Dequeue();
           transform.localScale = sca.Dequeue();
        }
    }

}
