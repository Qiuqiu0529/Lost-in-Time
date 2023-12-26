using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopChasing : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Enter");
        if (col.gameObject.CompareTag("Chasing"))
        {
            Debug.Log("Interrupt");
            col.GetComponent<Chasing>().Interrupt();
        }
    }
}
