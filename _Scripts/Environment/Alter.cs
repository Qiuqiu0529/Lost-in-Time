using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.ParticleSystemJobs;
public class Alter : MonoBehaviour
{
    public Player.PlayerLifeStage lifeStage;
    public int damage;
    public MMFeedbacks appearFB;
    public MMFeedbacks useFB;
    Player player;
    public float coolTime = 5f;
    public bool isActive = true;
    public int useCountMax = 5;
    public int count = 0;
    public AudioSource fireAudio;

    public ParticleSystem fire;
    bool playerInin;
    private void Start()
    {
        SoundManager.instance.PlayFire(fireAudio);
        fireAudio.Stop();
        StartCoroutine(ChangeFireSound());

    }

    public void ChangelifeTime()
    {
        PLayerInteract.instance.Sacrifice(-damage);
        switch (lifeStage)
        {
            case Player.PlayerLifeStage.CHILD:
                ChangeLifeStage.instance.ChangeToChild();
                break;
            case Player.PlayerLifeStage.TEEN:
                ChangeLifeStage.instance.ChangeToTeen();
                break;
            case Player.PlayerLifeStage.ADULT:
                ChangeLifeStage.instance.ChangeToAdult();
                break;
        }
        PLayerInteract.instance.SavePlayerState();
        player.canMove = true;
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && isActive)
        {
            Debug.Log("OnTriggerStay2D");
            if (player.interact)
            {
                if (count < useCountMax
                    && PLayerInteract.instance.life > damage
                    && player.lifeStage != lifeStage)
                {
                    isActive = false;
                    useFB.PlayFeedbacks();
                    player.canMove = false;
                    count++;
                    StartCoroutine(FireDown());
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && isActive)
        {
            player = Player.instance;
            playerInin = true;
            isActive = false;
            useFB.PlayFeedbacks();
            StartCoroutine(FireUp());
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //player.interact = false;
            //StopCoroutine(FireUp());
            //playerInin = false;
            //StartCoroutine(FireDown());
        }
    }

    IEnumerator ChangeFireSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            fireAudio.pitch = 1 + Random.Range(-0.1f, 0.1f);
        }
    }

    IEnumerator Dis()
    {
        yield return new WaitForSeconds(coolTime);

        if (count < useCountMax)
        {
            StartCoroutine(FireUp());
        }
    }

    IEnumerator FireUp()
    {
        fireAudio.Play();
        float i = 0;
        fire.Play();
        appearFB.PlayFeedbacks();
        while (i < 0.45f)
        {
            i += Random.Range(0.03f, 0.05f);
            if (fireAudio.volume < SoundManager.instance.soundValue)
                fireAudio.volume += Random.Range(0.1f, 0.2f);
            fire.startSize = i;
            yield return new WaitForFixedUpdate();
        }
        fireAudio.volume = SoundManager.instance.soundValue;
        GameManager.instance.AddFire();
    }

    IEnumerator FireDown()
    {
        yield return new WaitForSeconds(2f);
        if (!playerInin)
        {
            float i = 0.45f;
            fireAudio.volume = SoundManager.instance.soundValue;
            while (i < 0.8)
            {
                i += Random.Range(0.03f, 0.05f);
                fire.startSize = i;
                yield return new WaitForFixedUpdate();
            }
            while (i > 0)
            {
                i -= Random.Range(0.03f, 0.05f);
                if (fireAudio.volume > 0)
                    fireAudio.volume -= Random.Range(0.1f, 0.2f);
                fire.startSize = i;
                yield return new WaitForFixedUpdate();
            }
            fireAudio.volume = 0f;
            fire.Stop();
        }
        if (!playerInin)
            GameManager.instance.MinusFire();
        // ChangelifeTime();
        // StartCoroutine(Dis());
    }
}
