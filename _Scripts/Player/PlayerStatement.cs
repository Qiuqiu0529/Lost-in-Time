using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public interface PlayerBaseState
{

    void PhysicsUpdate();
    void HandleInput();
    void LeaveState();

}
public class DashingState : PlayerBaseState//
{
    private Player player;
    float timer;
    float dashtime = 0.2f;
    int dir;
    public DashingState(Player Player)
    {
        player = Player;
        Debug.Log("------------------------Player in DashingState~!（进入冲刺状态！）");
        player.canFall = false;
        player.canDash = false;
        timer = 0f;
        player.ReturnNormalRotation();
        PlayerVisual.instance.PlayDashAnimation();
        if (player.faceRight)
            dir = 1;
        else
            dir = -1;
        player.StartCoroutine(player.AddJumpForceTime(10, 0.6f));
        player.StartCoroutine(player.AddRunSpeedTime(10, 0.6f));
    }

    public void PhysicsUpdate()
    {
        timer += Time.deltaTime;
        Debug.Log("------------------------测试");
        if (timer < dashtime)
        {
            player.Dashing(dir);

        }
        else
        {
            player.canFall = true;
            player.runSpeedNow = 0;
            player.Velocity = Vector2.zero;
        }
    }

    public void HandleInput()
    {
    }

    public void LeaveState()
    { }
}
public class StandingState : PlayerBaseState//地面时
{
    protected Player player;
    public StandingState(Player Player)
    {
        player = Player;
        Debug.Log("------------------------Player in StandingState~!（进入站立状态！）");
        player.Velocity.y = 0;
        player.canDash = true;
        player.canAirJump = true;
        player.canFall = true;
        player.canJumpOnAir = false;

    }
    public void PhysicsUpdate()
    {
        player.Run();
        PlayerVisual.instance.PlayGroundAnimation
            (Mathf.Abs(player.runSpeedNow));
    }

    public void HandleInput()
    {
        if (player.lastJumpPressed + player.jumpBuffer > Time.time && player.skill[Globle.JumpSkill])
        {
            PlayerVisual.instance.PlayJumpPa();
            player.SetPlayerState(new JumpingState(player));
            return;
        }

        if (player.squat)
        {
            player.SetPlayerState(new SquatState(player));
        }

        if (player.climb &&
            player.skill[Globle.ClimbSkill])//暂定x为hold键
        {
            if (player.CheckLadder())
                player.SetPlayerState(new ClimbingState(player));

            Debug.Log("get KeyCode.X!");
            return;
        }
    }

    public void LeaveState()
    {

    }
}
public class SquatState : PlayerBaseState//地面时
{
    private Player player;
    public SquatState(Player Player)
    {
        player = Player;
        Debug.Log("------------------------Player in SquatState!（进入站立状态！）");
        /* player.canDash = true;
         player.canAirJump = true;
         player.canFall = true;
         player.canJumpOnAir = false;*/
        player.ChangeSquat();
        PlayerVisual.instance.PlaySquatAnimation();
    }
    public void PhysicsUpdate()
    {
        player.Run();
        PlayerVisual.instance.PlaySquatAnimation
            (Mathf.Abs(player.runSpeedNow));
        if (!player.squat)
        {
            if (!player.CheckSquatHead())
                player.SetPlayerState(new StandingState(player));
        }
    }

    public void HandleInput()
    {
        if (player.lastJumpPressed + player.jumpBuffer > Time.time && player.skill[Globle.JumpSkill])
        {
            PlayerVisual.instance.PlayJumpPa();
            player.SetPlayerState(new JumpingState(player));
            return;
        }
        if (player.climb &&
            player.skill[Globle.ClimbSkill])//暂定x为hold键
        {
            if (player.CheckLadder())
                player.SetPlayerState(new ClimbingState(player));

            Debug.Log("get KeyCode.X!");
            return;
        }

    }
    public void LeaveState()
    {
        PlayerVisual.instance.PauseContinue();
        PlayerVisual.instance.PlayDunqiAnimation();
        player.ChangeNormal();
    }
}
public class ClimbingState : PlayerBaseState
{
    private Player player;
    public ClimbingState(Player Player)
    {
        player = Player;
        player.endClimb = false;
        player.canFall = false;
        PlayerVisual.instance.PlayClimbAnimation();
        Debug.Log("------------------------Player in ClimbingState~!");
    }

    public void PhysicsUpdate()
    {
        if (!player.endClimb)
        {
            player.Climbing();
        }
        else
        {
            PlayerVisual.instance.PauseContinue();
            player.canFall = true;
        }

    }
    public void HandleInput()
    {
        if (player.lastJumpPressed + 0.02f > Time.time)
        {
            if (player.skill[Globle.WallJumpSkill])
                player.SetPlayerState(new WallJumpingState(player));
            Debug.Log("get KeyCode.Space!");
            return;
        }
    }

    public void LeaveState()
    { }
}
public class FallingState : PlayerBaseState//
{
    private Player player;
    public FallingState(Player Player)
    {
        player = Player;
        player.ReturnNormalRotation();
        PlayerVisual.instance.PlayFallAnimation();
        Debug.Log("------------------------Player in FallingState~!（进入状态！）");
    }

    public void PhysicsUpdate()
    {
        player.Falling();
    }

    public void HandleInput()
    {
        if (player.lastJumpPressed + 0.02f > Time.time)
        {
            if (player.coyoteTime + player.jumpBuffer > Time.time && player.skill[Globle.JumpSkill])//土狼跳
            {
                player.SetPlayerState(new JumpingState(player));
                return;
            }
            Debug.Log("get KeyCode.Space!");
        }

        if (player.lastJumpPressed + player.jumpBuffer > Time.time)
        {
            if (player.canJumpOnAir && player.skill[Globle.JumpSkill])
            {
                player.SetPlayerState(new JumpingState(player));
                return;
            }
            if (player.horizontalMove == 0)
            {
                if (player.canAirJump && player.firstJumpEnd + player.airJumpBuffer > Time.time
                 && player.skill[Globle.AirJumpSkill])//二段跳
                {
                    player.SetPlayerState(new AirJumpingState(player));
                    return;
                }
            }
            else
            {
                if (player.lastNearWallTime + 0.1f > Time.time && player.skill[Globle.WallJumpSkill])
                {
                    player.SetPlayerState(new WallJumpingState(player));
                    return;
                }

                if (player.canAirJump && player.firstJumpEnd + player.airJumpBuffer > Time.time
                 && player.skill[Globle.AirJumpSkill])//二段跳
                {
                    player.SetPlayerState(new AirJumpingState(player));
                    return;
                }
            }
        }


        if (player.climb
             && player.skill[Globle.ClimbSkill])//暂定x为hold键
        {
            if (player.CheckLadder())
                player.SetPlayerState(new ClimbingState(player));
            Debug.Log("get KeyCode.X!");
            return;
        }
    }

    public void LeaveState()
    { }
}
public class JumpingState : PlayerBaseState
{
    private Player player;
    float jumpTime = 0.35f;
    float timer = 0f;
    float jumpBonus = 0f;

    public JumpingState(Player Player)
    {
        timer = 0f;
        player = Player;
        player.endJumpEarly = false;
        player.canFall = false;
        player.canJumpOnAir = false;
        player.ReturnNormalRotation();
        jumpBonus = Mathf.Abs(player.runSpeedNow) / player.runSpeedMax * 2;
        PlayerVisual.instance.PlayJumpAnimation();
        player.lastJumpPressed = 0;
        Debug.Log("------------------------Player in JumpingState~!(进入跳跃状态！)");
    }

    public void PhysicsUpdate()
    {
        timer += Time.deltaTime;
        if (timer < jumpTime)
        {
            if (timer > 0.1f)
            {
                if (player.endJumpEarly)
                {
                    Debug.Log("endJumpEarly");
                    player.Velocity.y = 0;
                    player.firstJumpEnd = Time.time;
                    player.canFall = true;
                    return;
                }
            }
            player.Jump(timer, jumpBonus);
        }
        else
        {
            player.canFall = true;
            player.firstJumpEnd = Time.time;
        }
    }

    public void HandleInput()
    {
        if (player.lastJumpPressed + 0.02 >= Time.time && timer > 0.15f)
        {
            if (player.canJumpOnAir && player.skill[Globle.JumpSkill])
            {
                player.SetPlayerState(new JumpingState(player));
                return;
            }
            if (player.skill[Globle.AirJumpSkill] && player.canAirJump)
                player.SetPlayerState(new AirJumpingState(player));
            return;
        }

    }

    public void LeaveState()
    { }
}
public class AirJumpingState : PlayerBaseState
{
    private Player player;
    float jumpTime = 0.25f;
    float timer = 0f;
    public AirJumpingState(Player Player)
    {
        player = Player;
        player.canFall = false;
        player.canAirJump = false;
        player.ReturnNormalRotation();
        PlayerVisual.instance.PlayJumpAnimation();
        Debug.Log("------------------------Player in AirJumpingState~!");
    }

    public void PhysicsUpdate()
    {
        timer += Time.deltaTime;
        if (timer < jumpTime)
        {
            player.AirJump(timer);//二段跳距离更短
        }
        else
        {
            player.canFall = true;
        }
    }
    public void HandleInput()
    {

    }
    public void LeaveState()
    { }
}
public class WallJumpingState : PlayerBaseState
{
    private Player player;
    float jumpTime = 0.25f;
    float timer = 0f;
    float dir;
    public WallJumpingState(Player Player)
    {
        player = Player;
        player.endJumpEarly = false;
        player.canFall = false;
        player.runSpeedNow = 0;
        if (player.nearLeftWall)
            dir = 1;
        else
            dir = -1;
        player.ReturnNormalRotation();
        PlayerVisual.instance.PlayJumpAnimation();
        PlayerVisual.instance.PlayJumpPa();
        Debug.Log("------------------------Player in WallJumpingState~!");
    }
    public void PhysicsUpdate()
    {
        timer += Time.deltaTime;
        if (timer < jumpTime)
        {
            if (timer > 0.1f)
            {
                if (player.endJumpEarly)
                {
                    Debug.Log("endJumpEarly");
                    player.Velocity.y = 0;
                    timer = 0.25f;
                    return;
                }
            }
            player.WallJump(dir);
        }
        else if (timer < 0.3f)//一小段反应时间
        {
            player.Velocity.y = 0;
        }
        else
        {
            player.canFall = true;
        }
    }
    public void HandleInput()
    {
    }
    public void LeaveState()
    { }
}
public class ClimbLadderState : PlayerBaseState
{
    private Player player;
    float climbtime = 0.3f;
    float timer = 0f;
    Vector3 add;
    public ClimbLadderState(Player Player)
    {
        player = Player;
        add = (player.faceRight) ? new Vector3(3, 17 * player.gravityScale, 0) :
            new Vector3(-3, 17 * player.gravityScale, 0);
        player.Velocity = add;
        PlayerVisual.instance.PlayFallAnimation();
    }
    public void PhysicsUpdate()
    {
        timer += Time.deltaTime;
        if (timer < climbtime)
        {
            player.ClimbLadder();
        }
        else
        {
            player.goLadderTop = false;
            player.canFall = true;
        }
    }
    public void HandleInput()
    {
    }
    public void LeaveState()
    { }
}

