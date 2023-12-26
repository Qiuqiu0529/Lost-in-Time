using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAfterLeave : MonoBehaviour
{
    public GameObject[] gameObjects;
    bool first = true;

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && first)
        {
            Debug.Log("Playerenter");
            StartCoroutine(Active());
            first = false;
        }
    }
    IEnumerator Active()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var gameobj in gameObjects)
        {
            gameobj.SetActive(true);
        }
    }
   
}
