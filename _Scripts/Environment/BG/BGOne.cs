using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGOne : MonoBehaviour
{
    Material material;
    Vector2 movement;
    Transform target;
    public Vector2 speed;
    Vector3 initDis;
    float posZ;
    private void Start()
    {
        target = Camera.main.transform;
        initDis = target.position - transform.position;
        posZ = transform.position.z;
        material = GetComponent<Renderer>().material;
        StartCoroutine(BGMove());
    }

    public IEnumerator BGMove()
    {
        while (true)
        {
            movement += speed*Time.deltaTime;
            movement.x = movement.x % 10;
            movement.y = movement.y % 10;
            material.mainTextureOffset = movement;
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, 
               target.position.y-initDis.y, posZ),0.2f);
            yield return new WaitForFixedUpdate();
        }
     }
}
