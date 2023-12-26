using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenStone : MonoBehaviour
{
    public Rigidbody2D[] stones;
    bool first=true;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")&& first)
        {
            Debug.Log("Playerenter");
            foreach (var stone in stones)
            {
                stone.bodyType = RigidbodyType2D.Dynamic;
            }
            first = false;
        }
    }

}
