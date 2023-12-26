using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class LockSkillPoint : MonoBehaviour
{
    Player player;
    bool isUse = false;
    public bool LockJump, LockAirJump, LockDash, LockWallJump, LockClimb;
    
    public float recoverTime = 0f;

    public bool useFB;
    public MMFeedbacks lockSkill;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")&&!isUse)
        {
            player = Player.instance;
            isUse = true;
            if (useFB)
                lockSkill.PlayFeedbacks();
            Lock();
            
        }
    }

    public void Lock()
    {
        if (LockJump)
        {
            player.LockJumpSkill();
        }
        if (LockAirJump)
        {
            player.LockAirJumpSkill();
        }
        if (LockClimb)
        {
            player.LockClimbSkill();
        }
        if (LockDash)
        {
            player.LockDashSkill();
        }
        if (LockWallJump)
        {
            player.LockWallJumpSkill();
        }
        if (recoverTime > 0)
        {
            StartCoroutine(Reseting());
        }
    }

    public IEnumerator Reseting()
    {
        yield return new WaitForSeconds(recoverTime);
        if (LockJump)
        {
            player.UnlockJumpSkill();
        }
        if (LockAirJump)
        {
            player.UnlockAirJumpSkill();
        }
        if (LockClimb)
        {
            player.UnlockClimbSkill();
        }
        if (LockDash)
        {
            player.UnlockDashSkill();
        }
        if (LockWallJump)
        {
            player.UnlockWallJumpSkill();
        }
        isUse = false;
    }
}