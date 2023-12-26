using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveChild : MonoBehaviour
{
    public GameObject[] gameObjects;
    bool first = true;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && first)
        {
            Debug.Log("Playerenter");
            foreach (var gameobj in gameObjects)
            {
                gameobj.SetActive(true);
            }
            first = false;
        }
    }
}
