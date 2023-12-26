using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marsh : MonoBehaviour
{
    public float minusJumpForce = 7f;
    public float minusRunSpeed = 10f;
    public float minusFallSpeed =40f;
    public float minusFallSpeedMax = 20f;
    bool isIn = false;
    Player player;

    public AudioSource Audio;
    private void Start()
    {
        SoundManager.instance.PlayPaper(Audio);
        StartCoroutine(ChangeAudioSound());
    }
    IEnumerator ChangeAudioSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            Audio.pitch = 1 + Random.Range(-0.1f, 0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")&&!isIn)
        {
            isIn = true;
            player = Player.instance;
            player.ChangeJumpForce(-minusJumpForce);
            player.ChangeRunSpeed(-minusRunSpeed);
            player.ChangeFallSpeed(-minusFallSpeed);
            player.ChangeFallSpeedMax(-minusFallSpeedMax);
            player.Velocity.y = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isIn = false;
            StartCoroutine(Reseting());
        }
    }
    public IEnumerator Reseting()
    {
        yield return new WaitForSeconds(0.1f);
        if (!isIn)
        {
            player.ChangeJumpForce(minusJumpForce);
            player.ChangeRunSpeed(minusRunSpeed);
            player.ChangeFallSpeed(minusFallSpeed);
            player.ChangeFallSpeedMax(minusFallSpeedMax);
        }
    }
}
