using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public GameObject kid;
    bool isactive = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isactive = true;
            StartCoroutine(Motivate());
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isactive = false;
            StartCoroutine(Reseting());
        }
    }

    public IEnumerator Motivate()
    {
        yield return new WaitForSeconds(0.05f);
        kid.SetActive(true);
    }
     
    public IEnumerator Reseting()
    {
        yield return new WaitForSeconds(0.1f);
        if(!isactive)
           kid.SetActive(false);
    }
}
