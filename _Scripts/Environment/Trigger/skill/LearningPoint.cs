using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class LearningPoint : MonoBehaviour
{
    Player player;
    bool isUse = false;
    public bool UnLockJump, UnLockAirJump, UnLockDash, UnLockWallJump, UnLockClimb;
    public float recoverTime = 0f;
    public bool useFB;
    public MMFeedbacks learnSkill;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")&&!isUse)
        {
            player = Player.instance;
            isUse = true;
            UnLock();
            if (useFB)
                learnSkill.PlayFeedbacks();
        }
    }

    public void UnLock()
    {
        if (UnLockJump)
        {
            player.UnlockJumpSkill();
        }
        if (UnLockAirJump)
        {
            player.UnlockAirJumpSkill();
        }
        if (UnLockClimb)
        {
            player.UnlockClimbSkill();
        }
        if (UnLockDash)
        {
            player.UnlockDashSkill();
        }
        if (UnLockWallJump)
        {
            player.UnlockWallJumpSkill();
        }
       
        if (recoverTime > 0)
        {
            StartCoroutine(Reseting());
        }
    }

    public IEnumerator Reseting()
    {
        yield return new WaitForSeconds(recoverTime);
        if (UnLockJump)
        {
            player.LockJumpSkill();
        }
        if (UnLockAirJump)
        {
            player.LockAirJumpSkill();
        }
        if (UnLockClimb)
        {
            player.LockClimbSkill();
        }
        if (UnLockDash)
        {
            player.LockDashSkill();
        }
        if (UnLockWallJump)
        {
            player.LockWallJumpSkill();
        }
        isUse = false;
    }
}
