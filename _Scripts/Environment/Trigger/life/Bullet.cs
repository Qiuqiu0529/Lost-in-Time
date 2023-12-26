using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector2 velocity;
    public Rigidbody2D rigidbody;
    public AnimationCurve velcityX, velcityY;
    public float SurvivalTime = 5f;
    public float timer;
    public float speedLimit;
    public BulletPool parent;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        timer = 0f;
    }

    private void FixedUpdate()
    {
        if (timer >= SurvivalTime)
        {
            parent.itemInpool.Enqueue(this);
            gameObject.SetActive(false);
            timer = 0f;
        }
        else
        {
            velocity.x = speedLimit * velcityX.Evaluate(timer);
            velocity.y = speedLimit * velcityY.Evaluate(timer);
            timer += Time.fixedDeltaTime;
            rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        }
       
    }
}
