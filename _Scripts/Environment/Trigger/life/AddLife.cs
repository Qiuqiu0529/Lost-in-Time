using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class AddLife : MonoBehaviour
{
    public float recoverTime = 2f;
    public int addLife = 5;
    public bool isactive = true;
    public SpriteRenderer sprite;
    public MMFeedbacks Feedbacks;
    public MMFeedbacks recoverFB;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") &&isactive)
        {
            isactive = false;
            Feedbacks.PlayFeedbacks();
            col.gameObject.GetComponent<PLayerInteract>().ChangeLife(addLife);
            StartCoroutine(Dis());
        }
    }
    public IEnumerator Dis()
    {
        yield return new WaitForSeconds(1.9f);
        sprite.enabled = false;
        yield return new WaitForSeconds(recoverTime - 3.8f);
        if (recoverTime > 0f)
        {
            sprite.enabled = true;
            recoverFB.PlayFeedbacks();
            yield return new WaitForSeconds(1.8f);
            isactive = true;
        }
        
    }
}
