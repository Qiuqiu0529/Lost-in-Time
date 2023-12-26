using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int fireCount;
    public int limitCount;

    public GameObject nextLevel;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Object.Destroy(this.gameObject);
        }
    }

    public void AddFire()
    {
        fireCount++;
        if (fireCount >= limitCount)
        {
            nextLevel.SetActive(true);
        }

    }
    public void MinusFire()
    {
        fireCount--;
        if (fireCount < limitCount)
        {
            nextLevel.SetActive(false);
        }
    }
}
