using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public Queue<Bullet> itemInpool=new Queue<Bullet>();
    public Bullet[] prefab;

    public float intervalTime = 0.5f;
    public int poolCount=10;
   
    public void OnEnable()
    {
        StartCoroutine(CreateBullet());
    }
    public void GetFromPool()
    {
        if (itemInpool.Count <= 1)
        {
            for (int i = 0; i < poolCount; ++i)
            {
                Bullet newItem = Instantiate(prefab[Random.Range(0, prefab.Length)]);
                newItem.transform.SetParent(this.transform);
                newItem.parent = this;
                newItem.gameObject.SetActive(false);
                itemInpool.Enqueue(newItem);
            }
        }
        Bullet temp = itemInpool.Dequeue();
        temp.transform.position = this.transform.position;
        temp.gameObject.SetActive(true);

    }

    public void AddItem(Bullet item)
    {
        itemInpool.Enqueue(item);
    }


    public IEnumerator CreateBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalTime);
            GetFromPool();
        }
    }
    

}
