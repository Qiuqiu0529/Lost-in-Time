using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public enum ChapterType
    {
       Child,teen,adult,menu
    };

    public ChapterType chapter;
    public float soundValue;

    AudioSource bgAudioSource;
    AudioSource playerAudioSource;
    AudioSource UIAudioSource;
    AudioSource ambientAudioSource;
    AudioSource otherAudioSource;
    public List<AudioSource> otherAudio;

    [Header("�����ֺͻ�����Ч")]
    public AudioClip[] bgAudio ;
    public AudioClip[] ambientAudio;

    [Header("�����Ч")]
    public AudioClip[] clickAudio;
    public AudioClip[] clockPressAudio;
    public AudioClip[] clockLooseAudio;

    [Header("�ƶ���Ч")]
    public AudioClip[] walksound;
    public AudioClip[] runsound;
    public AudioClip[] jumpsound;
    public AudioClip[] landsound;
    public AudioClip[] dashsound;
    public AudioClip[] climbsound;

    [Header("other")]
    public AudioClip[] heartBeat;
    public AudioClip[] heal;
    public AudioClip[] hit;
    public AudioClip[] deaeh;
    public AudioClip[] transDoor;
    public AudioClip[] changeStateToChild;
    public AudioClip[] changeStateToTeen;
    public AudioClip[] changeStateToAdult;
    public AudioClip[] defyGravity;
    public AudioClip[] skillOnAir;
    public AudioClip[] buff;
    public AudioClip[] push;
    public AudioClip fire;
    public AudioClip paper;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Object.Destroy(this.gameObject);
            return;
        }    
        bgAudioSource = gameObject.AddComponent<AudioSource>();
        playerAudioSource = gameObject.AddComponent<AudioSource>();
        UIAudioSource = gameObject.AddComponent<AudioSource>();
        ambientAudioSource = gameObject.AddComponent<AudioSource>();
        otherAudioSource= gameObject.AddComponent<AudioSource>();
    }
    
    private void Start()
    {
        StartCoroutine(StartPlay());
    }

    public IEnumerator StartPlay()
    {
        yield return new WaitForSeconds(0.2f);
        switch (chapter)
        {
            case ChapterType.Child:
                PlayBGM(0);
                break;
            case ChapterType.teen:
                PlayBGM(1);
                break;
            case ChapterType.adult:
                PlayBGM(2);
                break;
        }
    }

    public static void PlayWalkStep()
    {
        instance.playerAudioSource.clip 
            = instance.walksound[Random.Range(0, instance.walksound.Length)];
        instance.playerAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.playerAudioSource.Play();
    }
    public static void PlayRunStep()
    {
        instance.playerAudioSource.clip
           = instance.runsound[Random.Range(0, instance.runsound.Length)];
        instance.playerAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.playerAudioSource.Play();
    }
    public static void PlayJump()
    {
        // instance.playerAudioSource.clip
        //     = instance.jumpsound[Random.Range(0, instance.jumpsound.Length)];
        // instance.playerAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        // instance.playerAudioSource.Play();
    }
    public static void PlayLand()
    {
        // instance.playerAudioSource.clip
        //     = instance.landsound[Random.Range(0, instance.landsound.Length)];
        // instance.playerAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        // instance.playerAudioSource.Play();
    }
    public static void PlayDash()
    {
        instance.playerAudioSource.clip
          = instance.dashsound[Random.Range(0, instance.dashsound.Length)];
        instance.playerAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.playerAudioSource.Play();
    }
    public static void PlayClimb()
    {
        instance.playerAudioSource.clip
          = instance.climbsound[Random.Range(0, instance.climbsound.Length)];
        instance.playerAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.playerAudioSource.Play();
    }

    private void OnDisable()
    {
        otherAudio.Clear();
    }
    public void PlayFire(AudioSource firesource)
    {
        firesource.clip = fire;
        firesource.loop = true;
        firesource.Play();
        otherAudio.Add(firesource);
        
    }
    public void PlayPaper(AudioSource firesource)
    {
        firesource.clip = paper;
        firesource.loop = true;
        firesource.Play();
        otherAudio.Add(firesource);

    }
    public void PlayPush(AudioSource firesource)
    {
        firesource.clip = push[Random.Range(0,push.Length)];
        firesource.loop = true;
        firesource.Play();
        otherAudio.Add(firesource);
    }

    public static void PlayHeal()//
    {
        instance.otherAudioSource.clip
          = instance.heal[Random.Range(0, instance.heal.Length)];
        instance.otherAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.otherAudioSource.Play();
    }

    public static void PlayHit()
    {
        instance.otherAudioSource.clip
          = instance.hit[Random.Range(0, instance.hit.Length)];
        instance.otherAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.otherAudioSource.Play();
    }//
    public static void PlayDeaeh()
    {
        instance.otherAudioSource.clip
          = instance.deaeh[Random.Range(0, instance.deaeh.Length)];
        instance.otherAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.otherAudioSource.Play();
    }//

    public static void PlayTansDoor()
    {
        instance.otherAudioSource.clip
          = instance.transDoor[Random.Range(0, instance.transDoor.Length)];
        instance.otherAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.otherAudioSource.Play();
    }

    public static void PlayChangeStateToChild()
    {
        instance.otherAudioSource.clip
          = instance.changeStateToChild[Random.Range(0, instance.changeStateToChild.Length)];
        instance.otherAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.otherAudioSource.Play();
    }//
    public static void PlayChangeStateToTeen()
    {
        instance.otherAudioSource.clip
          = instance.changeStateToTeen[Random.Range(0, instance.changeStateToTeen.Length)];
        instance.otherAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.otherAudioSource.Play();
    }//
    public static void PlayChangeStateToAdult()
    {
        instance.otherAudioSource.clip
          = instance.changeStateToAdult[Random.Range(0, instance.changeStateToAdult.Length)];
        instance.otherAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.otherAudioSource.Play();
    }//

    public static void PlayAward()
    {
        instance.otherAudioSource.clip
          = instance.skillOnAir[Random.Range(0, instance.skillOnAir.Length)];
        instance.otherAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.otherAudioSource.Play();
    }//

    public static void PlayBuff()
    {
        instance.otherAudioSource.clip
          = instance.buff[Random.Range(0, instance.buff.Length)];
        instance.otherAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.otherAudioSource.Play();
    }//

    public static void PlayDefyGravity()
    {
        instance.otherAudioSource.clip
          = instance.defyGravity[Random.Range(0, instance.defyGravity.Length)];
        instance.otherAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.otherAudioSource.Play();
    }//
   /* public static void PlayClock()
    {
        instance.otherAudioSource.clip
          = instance.clock[Random.Range(0, instance.clock.Length)];
        instance.otherAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.otherAudioSource.Play();
    }*/

    public static void PlayBGM(int Type)
    {
        instance.bgAudioSource.loop = true;
        instance.bgAudioSource.clip = instance.bgAudio[Random.Range(0, instance.bgAudio.Length)];
        instance.bgAudioSource.Play();

        /*ambientAudioSource.loop = true;
        ambientAudioSource.clip = ambientAudio[Random.Range(0, ambientAudio.Length)];
        ambientAudioSource.Play();*/
    }

    public static void PlayClockPressAudio()
    {
        instance.UIAudioSource.clip
          = instance.clockPressAudio[Random.Range(0, instance.clockPressAudio.Length)];
        instance.UIAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.UIAudioSource.Play();
    }
    public static void PlayClockLooseAudio()
    {
        instance.UIAudioSource.clip
          = instance.clockLooseAudio[Random.Range(0, instance.clockLooseAudio.Length)];
        instance.UIAudioSource.pitch = 1 + Random.Range(-0.3f, 0.3f);
        instance.UIAudioSource.Play();
    }
    public static void PlayUIClickAudio()
    {
        instance.UIAudioSource.clip
          = instance.clickAudio[Random.Range(0, instance.clickAudio.Length)];
        instance.UIAudioSource.pitch = 1 + Random.Range(-0.2f, 0.2f);
        instance.UIAudioSource.Play();
    }
    public static void PlayHeartBeatAudio()
    {
        instance.ambientAudioSource.clip
          = instance.heartBeat[Random.Range(0, instance.heartBeat.Length)];
        instance.ambientAudioSource.pitch = 1 + Random.Range(0, 0.5f);
        instance.ambientAudioSource.Play();
    }
    public static void SetMusic(float value)
    {
        instance.bgAudioSource.volume = value;
    }
    public static void SetSound(float value)
    {
        instance.soundValue = value;
        instance.playerAudioSource.volume = value;
        instance.UIAudioSource.volume = value;
        instance.ambientAudioSource.volume = value;
        instance.otherAudioSource.volume = value;
        if (instance.otherAudio.Count > 0)
        {
            foreach (var audio in instance.otherAudio)
            {
                if (audio.isPlaying)
                {
                    audio.volume = value;
                }
            }
        }
    }
}

