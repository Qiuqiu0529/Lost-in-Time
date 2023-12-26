using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;
using MoreMountains.Feel;
using MoreMountains.Tools;

public class PLayerInteract : MonoBehaviour
{
    bool debug = true;
    public static PLayerInteract instance;
    public float life;
    public float invisibleTime = 0.5f;
    public float deleteRate = 0;
    bool canbeDamage = true;
    bool lowLife = false;
    bool death = false;
    Player player;
    public MMFeedbacks lowLifeFeedBack;
    public MMFeedbacks hitFeedBack;
    public MMFeedbacks addLifeFeedBack;
    public MMFeedbacks sacrificeFB;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Object.Destroy(this.gameObject);
        }
        life = 100;
    }
    private void Start()
    {
        player = Player.instance;
        SceneMgr.instance.LoadSaveData();
        if (debug)
            return;
        if (!SceneMgr.instance.save.onlyStart)
            LoadPlayerState();
    }


    private void FixedUpdate()
    {
       // Sacrifice(-deleteRate*Time.fixedDeltaTime);
    }
    public void SavePlayerState()//�洢һЩ����
    {
        SceneMgr.instance.save.onlyStart = false;
        SceneMgr.instance.save.chapterdata = SceneManager.GetActiveScene().buildIndex;
        SceneMgr.instance.save.lifeStage = player.lifeStage;
        SceneMgr.instance.save.PlayerLife = life;
        SceneMgr.instance.save.playerLastPos = gameObject.transform.localPosition;
        SceneMgr.instance.save.playerLastSca = gameObject.transform.localScale;
        SceneMgr.instance.save.gravityScale = player.gravityScale;
        SceneMgr.instance.save.Rigi2GravityScale = player.rigidbody2.gravityScale;
        SceneMgr.instance.save.FaceRight = player.faceRight;
        SceneMgr.instance.SaveGameData();
    }
    public void LoadPlayerState()//��ȡ����
    {
        player.gravityScale = SceneMgr.instance.save.gravityScale;
        player.SetGracity(SceneMgr.instance.save.Rigi2GravityScale);
        Debug.Log(SceneMgr.instance.save.gravityScale);
        life = SceneMgr.instance.save.PlayerLife;
        gameObject.transform.localPosition = SceneMgr.instance.save.playerLastPos;
        gameObject.transform.localScale = SceneMgr.instance.save.playerLastSca;
        player.faceRight = SceneMgr.instance.save.FaceRight;
        if (player.lifeStage != SceneMgr.instance.save.lifeStage)
        {
            player.lifeStage = SceneMgr.instance.save.lifeStage;
            switch (player.lifeStage)
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
        }

    }
    public void Death()
    {
        SoundManager.PlayDeaeh();
        player.canMove = false;
        player.rigidbody2.gravityScale = 0;
        PlayerVisual.instance.PauseAnimation();
        player.StartCoroutine(player.StartcoolDown(2.5f));//���Ҵ�����������
        SceneMgr.instance.ReloadScene();
    }
    public void ChangeLife(float add)
    {
        if (add > 0)
        {
            life += add;
            addLifeFeedBack.PlayFeedbacks();
            SoundManager.PlayHeal();
            if (life > 100)
                life = 100;
            if (lowLife && life >= 30)
            {
                lowLifeFeedBack.StopFeedbacks();
                StopCoroutine(HeartBeat());
                lowLife = false;
            }
        }
        if (add <= 0)
        {
            if (canbeDamage)
            {
                life += add;
                if (life <= 30 && !lowLife)
                {
                    lowLife = true;
                    StartCoroutine(HeartBeat());
                }
                else
                    hitFeedBack.PlayFeedbacks();
                if (life <= 0)
                {
                    life = 0;
                    canbeDamage = false;
                    Death();
                }
                else
                {
                    StartCoroutine(Invisible());
                    SoundManager.PlayHit(); 
                }
            }
        }
    }

    public void Sacrifice(float sac)
    {
        if (!death)
        {
            life += sac;
            if (life <= 0)
            {
                death = true;
                Death();
            }
            else if (life <= 30 && !lowLife)
            {
                lowLife = true;
                StartCoroutine(HeartBeat());
            }
        }
    }

    public IEnumerator Invisible()
    {
        canbeDamage = false;
        PlayerVisual.BeHit();
        yield return new WaitForSeconds(invisibleTime);
        canbeDamage = true;
    }

    public IEnumerator HeartBeat()
    {
        lowLifeFeedBack.PlayFeedbacks();
        while (lowLife)
        {
            yield return new WaitForSeconds(1.6f + life / 100);
            SoundManager.PlayHeartBeatAudio();
        }
    }

}
