using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralPointSystem : MonoBehaviour
{
    public List<GameObject> gears = new List<GameObject>();
    public GameObject centralPoint;


    private void Awake()
    {
        CreateCentralPoint();
    }
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    void CreateCentralPoint()
    {
        for(int i=0;i<gears.Capacity;i++)
        {
            GameObject temp = GameObject.Instantiate(centralPoint, gears[i].transform.position, Quaternion.identity);
            temp.AddComponent<CircleCollider2D>();
            temp.GetComponent<CircleCollider2D>().isTrigger = true;
        }
    }
}
