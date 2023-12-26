using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using MoreMountains.Feedbacks;
using MoreMountains.Feel;
using MoreMountains.Tools;

public class PlayerVisual : MonoBehaviour
{
    public static PlayerVisual instance;

    Animator animatorNow;
    public SpriteRenderer playerNow;
    public GameObject playerPic;

    public GameObject[] lifeStagePlayer;

    [Header("particle")]
    public ParticleSystem dashingPa;
    public ParticleSystem jumpUpPa;
    public ParticleSystem landingPa;
    public ParticleSystem movePa;

    [Header("feedBack")]
    public MMFeedbacks jumpFeedBack;
    public MMFeedbacks landFeedBack;
    public MMFeedbacks dashFeedBack;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            playerPic = lifeStagePlayer[0];
            animatorNow = playerPic.GetComponent<Animator>();
            playerNow = playerPic.GetComponent<SpriteRenderer>();
        }
    }

    public void ChangeLifeStage(int i)
    {
        playerPic.SetActive(false);
        lifeStagePlayer[i].SetActive(true);
        playerPic = lifeStagePlayer[i];
        animatorNow = playerPic.GetComponent<Animator>();
        playerNow = playerPic.GetComponent<SpriteRenderer>();
    }

    public static void PlayMovePa()
    {
        if (!instance.movePa.isPlaying)
            instance.movePa.Play();
    }

    public static void StopMovePa()
    {
        if (instance.movePa.isPlaying)
            instance.movePa.Stop();
    }

    public void PlayGroundAnimation(float set)
    {
        AnimatorStateInfo animatorInfo = animatorNow.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsName("dunqi") && animatorInfo.normalizedTime < 1f)
        {
            Debug.Log(" animatorNow.Play(squat");
            return;
        }
        if (set < 0.2f)
        {
            //Debug.Log(" animatorNow.Play(idle");
            if (!animatorInfo.IsName("Idle"))
            {
                Debug.Log(" animatorNow.canplay idle");
                animatorNow.Play("Idle");
            }
            return;
        }
        else if (set < 5f)
        {
            if (!animatorNow.GetCurrentAnimatorStateInfo(0).IsName("walk"))
               animatorNow.Play("walk");
            return;
        }

        if (!animatorNow.GetCurrentAnimatorStateInfo(0).IsName("run"))
            animatorNow.Play("run");
    }

    public void PlaySquatAnimation(float set)
    {
        AnimatorStateInfo animatorInfo = animatorNow.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsName("dunzou"))    
        {
            if (set < 0.2)
                PauseAnimation();
            else
                PauseContinue();

            return;
        }
        if (animatorInfo.IsName("squat") && animatorInfo.normalizedTime < 1f)
        {
            Debug.Log(" animatorNow.Play(squat");
            return;
        }
        else
        {
            PlayDunzouAnimation();
        }
    }

    public void PlayIdleAnimation()
    {
        animatorNow.Play("Idle");
    }

    public void PauseAnimation()
    {
        animatorNow.speed = 0;
    }
    public void PauseContinue()
    {
        animatorNow.speed = 1;
    }

    public void PlayJumpAnimation()
    {
        jumpFeedBack.PlayFeedbacks();
        animatorNow.Play("jump");
    }

    public void Land()
    {
        landFeedBack.PlayFeedbacks();
    }

    public void PlayJumpPa()
    {
        jumpUpPa.Play();
    }

    public static void BeHit()
    {
        instance.StartCoroutine(instance.Blink());
    }

    public void PlayDashAnimation()
    {
       dashingPa.gameObject.transform.localScale
           = Player.instance.gameObject.transform.localScale;
        dashingPa.Play();
        dashFeedBack.PlayFeedbacks();
        animatorNow.Play("dash");
    }
    public void PlayClimbAnimation()
    {
        animatorNow.Play("climb");
    }
    public void PlayFallAnimation()
    {
        animatorNow.Play("fall");
    }
    public void PlaySquatAnimation()
    {
        animatorNow.Play("squat");//����
    }
    public void PlayDunqiAnimation()
    {
        Debug.Log(" animatorNow.Play(dunqi");
        animatorNow.Play("dunqi");//����
    }
    public void PlayDunzouAnimation()
    {
        animatorNow.Play("dunzou");//����
    }

    IEnumerator Blink()
    {
        for (int i = 0; i < 3; ++i)
        {
            Color color = playerNow.color;
            color.a = 0;
            playerNow.color = color;
            yield return new WaitForSeconds(0.1f);
            Color color1 = playerNow.color;
            color1.a = 1;
            playerNow.color = color1;
            yield return new WaitForSeconds(0.1f);
        }
    }

}
