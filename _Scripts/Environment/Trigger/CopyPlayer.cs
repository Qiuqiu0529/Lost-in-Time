using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
public class CopyPlayer : MonoBehaviour
{
    Transform playerVi, player;
    Player target;
 
    SpriteRenderer mysprite;
    public Rigidbody2D rigidbody;
    public ParticleSystem chasePa;
    public CapsuleCollider2D collider2D;


    Queue<Vector3> scale = new Queue<Vector3>();
    Queue<Sprite> sprites = new Queue<Sprite>();
    Queue<Vector2> colliderSize= new Queue<Vector2>();
    Queue<Vector2> colliderOffset = new Queue<Vector2>();
    Queue<Vector2> rigidBodyVelocity = new Queue<Vector2>();
    Queue<float> rigidGravity = new Queue<float>();

    public float waitTime;
    public float effect;
    private void Start()
    {
        target = Player.instance;
        player = Player.instance.transform;
        playerVi = PlayerVisual.instance.playerPic.transform;
        rigidbody = GetComponent<Rigidbody2D>();
        mysprite = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<CapsuleCollider2D>();
        StartCoroutine(AddQueue());
        StartCoroutine(DelQueue());
    }


    
 
    public IEnumerator AddQueue()
    {
        while (true)
        {
            scale.Enqueue(player.localScale);
            sprites.Enqueue(PlayerVisual.instance.playerNow.sprite);
            rigidBodyVelocity.Enqueue(target.rigidbody2.velocity);
            rigidGravity.Enqueue(target.rigidbody2.gravityScale);
            colliderOffset.Enqueue(target.collider2D.offset - new Vector2(0, 0.6f));
            colliderSize.Enqueue(target.collider2D.size);
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator DelQueue()
    {
        mysprite.sprite = PlayerVisual.instance.playerNow.sprite;
        yield return new WaitForSeconds(waitTime);
        while (true)
        {
            Vector2 vector = rigidBodyVelocity.Dequeue();
            if (vector.magnitude > 0.5f)
            {
                rigidbody.velocity = vector * effect;
            }
            rigidbody.gravityScale = rigidGravity.Dequeue();
            collider2D.size = colliderSize.Dequeue();
            collider2D.offset = colliderOffset.Dequeue();

            mysprite.sprite = sprites.Dequeue();
            chasePa.textureSheetAnimation.SetSprite(0, mysprite.sprite);

            transform.localScale = scale.Dequeue();
            chasePa.gameObject.transform.localScale = transform.localScale;
            yield return new WaitForFixedUpdate();
            
        }
    }
}
