using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class ChangeLifeStage : MonoBehaviour
{
    public MMFeedbacks changeChild, changeTeen, changeAdult;

    public static ChangeLifeStage instance;
    public Player player;
    public PlayerVisual playerVisual;
    public MMFeedbacks Change;


    public Vector2[] colliderSize=new Vector2[3];
    public Vector2[] colliderOffset = new Vector2[3];

    public Vector2[] squatColliderSize = new Vector2[3];
    public Vector2[] squatColliderOffset = new Vector2[3];


    public Vector3[] wallCheckPos=new Vector3[3];
    public Vector3[] headCheckPos = new Vector3[3];

    public Vector3[] squatHeadCheckPos = new Vector3[3];


    public bool[] blank = new bool[5];

    public bool[] childSkill = new bool[5];
    public bool[] teenSkill = new bool[5];
    public bool[] adultSkill = new bool[5];


    public float[] accel=new float[3];
    public float[] runSpeedMax=new float[3];
    public float[] jumpForce = new float[3];
    public float[] fallSpeedAccel = new float[3];
    public float[] fallSpeedMax = new float[3];

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
    }

    public void BlankSkill()
    {
        for(int i=0;i<5;++i)
            player.skill[i] = blank[i];
    }
    public void ChildSkill()
    {
        for (int i = 0; i < 5; ++i)
            player.skill[i] = childSkill[i];
    }
    public void TeenSkill()
    {
        for (int i = 0; i < 5; ++i)
            player.skill[i] = teenSkill[i];
    }
    public void AdultSkill()
    {
        for (int i = 0; i < 5; ++i)
            player.skill[i] = adultSkill[i];
    }
    public void ChangeParameter(int i)
    {
        player.collider2D.size = colliderSize[i];
        player.normalColliderSize= colliderSize[i];
        player.squatColliderSize = squatColliderSize[i];

        player.collider2D.offset = colliderOffset[i];
        player.normalColliderOffset= colliderOffset[i];
        player.squatColliderOffSet = squatColliderOffset[i];

        player.headCheck.localPosition = headCheckPos[i];
        player.normalHeadCheckPos= headCheckPos[i];
        player.squatHeadCheckPos = squatHeadCheckPos[i];

        player.wallCheck.localPosition = wallCheckPos[i];
       
        player.accel = accel[i];
        player.runSpeedMax = runSpeedMax[i];
        player.jumpForce = jumpForce[i];
        player.fallSpeedAccel = fallSpeedAccel[i];
        player.fallSpeedMax = fallSpeedMax[i];
    }

    public void ChangeChildFunc()
    {
        player.lifeStage = Player.PlayerLifeStage.CHILD;
        playerVisual.ChangeLifeStage(Globle.Child);
        ChangeParameter(Globle.Child);
        ChildSkill();
    }

    public void ChangeToChild()
    {
        if (player.lifeStage != Player.PlayerLifeStage.CHILD)
        {
            Change.PlayFeedbacks();
            changeChild.PlayFeedbacks();
            SoundManager.PlayChangeStateToChild();
            ChangeChildFunc();
        }
    }

    public void ChangeTeenFunc()
    {
        player.lifeStage = Player.PlayerLifeStage.TEEN;
        playerVisual.ChangeLifeStage(Globle.Teen);
        ChangeParameter(Globle.Teen);
        TeenSkill();
    }

    public void ChangeToTeen()
    {
        if (player.lifeStage != Player.PlayerLifeStage.TEEN)
        {
            Change.PlayFeedbacks();
            changeTeen.PlayFeedbacks();
            SoundManager.PlayChangeStateToTeen();
            ChangeTeenFunc();
        }
    }

    public void ChangeAdultFunc()
    {
        player.lifeStage = Player.PlayerLifeStage.ADULT;
        playerVisual.ChangeLifeStage(Globle.Adult);
        ChangeParameter(Globle.Adult);
        AdultSkill();
    }

    public void ChangeToAdult()
    {
        if (player.lifeStage != Player.PlayerLifeStage.ADULT)
        {
            Change.PlayFeedbacks();
            changeAdult.PlayFeedbacks();
            SoundManager.PlayChangeStateToAdult();
            ChangeAdultFunc();
        }
    }
}
