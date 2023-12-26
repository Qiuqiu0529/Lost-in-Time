using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float addJumpForce = 7f;
    public bool canSuperimpos = true;
    bool isactive = true;
    public float reactTime = 1f;
    Player player;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")&&isactive)
        {
            player = Player.instance;
            player.StartCoroutine(player.AddJumpForceTime(addJumpForce, reactTime));
            if (!canSuperimpos)
            {
                isactive = false;
                StartCoroutine(Reseting());
            }
        }
    }

    public IEnumerator Reseting()
    {
        yield return new WaitForSeconds(reactTime);
        isactive = true;
    }


}
