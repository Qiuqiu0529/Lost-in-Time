using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public Vector3 dir;
    public float speed=5f;
    public float coolTime = 1f;
    public  bool isin = false;
    Player player;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") )
        {
            player = Player.instance;
            isin = true;
            player.AccelVelocity(dir, speed);
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isin = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isin = false;
        StartCoroutine(Reseting());
    }

    public IEnumerator Reseting()
    {
        yield return new WaitForSeconds(coolTime);
        if (!isin)
        {
            player.RestAddVelocity();
        }
    }
}
