using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondHand : MonoBehaviour
{
    public int speed = 90;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, -speed * Time.deltaTime));
    }
   
    public void RotateStart()
    {
        speed = 90;
    }

    public void RotateStop()
    {
        speed = 0;
    }
}
