using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PushItem : MonoBehaviour
{
    public AudioSource pushAudio;
    Player player;
    public Rigidbody2D rigidbody;

    private void Start()
    {
        player = Player.instance;
        SoundManager.instance.PlayPush(pushAudio);
        pushAudio.Stop();
    }
    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (player.interact )
            {
                if (rigidbody.bodyType != RigidbodyType2D.Dynamic)
                {
                    rigidbody.bodyType = RigidbodyType2D.Dynamic; 
                }
                if (Mathf.Abs(rigidbody.velocity.x)>0)
                {
                    if (!pushAudio.isPlaying)
                    {
                        pushAudio.volume = SoundManager.instance.soundValue;
                        pushAudio.Play();
                    }
                }
                else
                {
                    if (pushAudio.isPlaying)
                    {
                        pushAudio.Stop();
                    }
                }
            }
            else
            {
                if (rigidbody.bodyType != RigidbodyType2D.Static)
                {
                    rigidbody.bodyType = RigidbodyType2D.Static; 
                }
                if (pushAudio.isPlaying)
                {
                    pushAudio.Stop();
                }
            }
        }
    }
    
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (rigidbody.bodyType != RigidbodyType2D.Static)
                rigidbody.bodyType = RigidbodyType2D.Static;
            if (pushAudio.isPlaying)
            {
                pushAudio.Stop();
            }
        }
    }
}
