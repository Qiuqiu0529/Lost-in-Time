using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class Rhyme : MonoBehaviour
{
    public float activeTime;
    public float waitTime;
    public MMFeedbacks able, enable;

    private void Start()
    {
        StartCoroutine(Rhming());
    }

    IEnumerator Rhming()
    {
        while (true)
        {
            yield return new WaitForSeconds(activeTime);
            able.PlayFeedbacks();
            yield return new WaitForSeconds(waitTime);
            enable.PlayFeedbacks();
        }
    }

}
