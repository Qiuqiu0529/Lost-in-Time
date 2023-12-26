using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player instance;
    //�˶��޶��ű����Ӿ�&�����ռ�Ʒ�ڱ�
    public enum PlayerLifeStage
    {
        CHILD,
        TEEN,
        ADULT,
        TEST
    }
    public PlayerLifeStage lifeStage;
    public float MaxBuffTime;
    public float LeftBuffTime;
    public bool timeChange;


    public PlayerBaseState state;
    public Vector3 Velocity;

    public Vector3 AddVelocity = Vector3.zero;

    public string statement;

    public bool[] skill = new bool[5];

    [Header("bool")]
    public bool nearRightWall = false;
    public bool nearLeftWall = false;
    public bool faceRight = true;
    public bool onGround = true;
    public bool onSlope = false;
    public bool canAirJump = true;
    public bool canFall = false;
    public bool canDash = true;
    public bool canMove = true;
    public bool goLadderTop = false;
    public bool canJumpOnAir = false;
    public bool endJumpEarly = false;
    public bool endClimb = false;
    public bool climb = false;
    public bool squat = false;
    public bool interact = false;

    [Header("posChaeck")]
    [SerializeField] public Transform groundCheck, headCheck;
    [SerializeField] public Transform wallCheck;

    [Header("walk&run")]
    public float accel, decel;
    public float runSpeedNow, runSpeedMax;
    public float addRunSpeed;
    public float squatSpeed;
    public float horizontalMove = 0f;
    public float verticleMove = 0f;
    [SerializeField] private LayerMask whatIsGround, whatIsHead;
    const float groundedRadius = .2f;

    [Header("slope")]
    public float slopeAngle, slopeAngleOld, slopeAngleMax;
    public Vector2 slopeNormal;


    [Header("jump")]
    public AnimationCurve jumpCurve, airJumpCurve;
    public float jumpForce;
    public float addJumpForce;

    [Header("fall")]
    [SerializeField] private LayerMask whatIsLadder, whatIsWall;//�����ӿ���
    public float fallSpeedWall, fallSpeedMax, fallSpeedAccel, addFallSpeed, changeFallSpeedMax;
    public float xWallSpeed;


    [Header("time")]
    public float lastJumpPressed;//��Ծ����
    public float coyoteTime;//����ʱ��
    public float jumpBuffer = 0.2f;
    public float firstJumpEnd;
    public float lastNearWallTime;
    public float airJumpBuffer = 0.3f;//��һ����Ծ�����޶�ʱ���ڰ�������������


    [Header("dash")]
    public float dashspeed;

    [Header("climb")]
    public float climbspeed;

    [Header("gravity")]
    public float gravityScale;

    [Header("general")]
    public Rigidbody2D rigidbody2;
    public CapsuleCollider2D collider2D;
    public Vector2 normalColliderSize, normalColliderOffset;
    public Vector2 squatColliderSize, squatColliderOffSet;
    public Vector3 normalHeadCheckPos, squatHeadCheckPos;
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
        faceRight = true;
        onSlope = false;
        state = new StandingState(this);
        rigidbody2 = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<CapsuleCollider2D>();
        Velocity = Vector3.zero;
        gravityScale = 1;

    }
    private void Start()
    {
        StartCoroutine(StartcoolDown());
        PlayerVisual.PlayMovePa();
        switch (lifeStage)
        {
            case PlayerLifeStage.CHILD:
                ChangeLifeStage.instance.ChangeChildFunc();
                break;
            case PlayerLifeStage.TEEN:
                ChangeLifeStage.instance.ChangeTeenFunc();
                break;
            case PlayerLifeStage.ADULT:
                ChangeLifeStage.instance.ChangeAdultFunc();
                break;
        }
    }

    public void SetPlayerState(PlayerBaseState newState)
    {
        state.LeaveState();
        state = newState;
    }

    public void Update()//����
    {
        if (!canMove)
            return;
        state.HandleInput();
        statement = state.GetType().ToString();
        if (LeftBuffTime > 0)
        {
            LeftBuffTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;
        CheckGround();
        CheckWall();
        CalcuHorizontalMove();
        state.PhysicsUpdate();
        rigidbody2.velocity = Velocity + AddVelocity;
    }

    #region Input
    public void GatherJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            lastJumpPressed = Time.time;
    }
    public void GatherDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            if (canMove && (canDash && skill[Globle.DashSkill]))//�ݶ�Ϊ��̼�
            {
                SetPlayerState(new DashingState(this));
            }
    }
    public void GatherJumpEndInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            endJumpEarly = true;
    }
    public void GatherClimbEndInput(InputAction.CallbackContext context)
    {
        climb = false;
        endClimb = true;
    }
    public void GatherClimbInput(InputAction.CallbackContext context)
    {
        climb = true;
    }
    public void GatherMoveInput(InputAction.CallbackContext context)
    {

        horizontalMove = context.ReadValue<Vector2>().x;
        verticleMove = context.ReadValue<Vector2>().y;

        if (Mathf.Abs(horizontalMove) < 0.2)
            horizontalMove = 0;
        if (Mathf.Abs(verticleMove) < 0.2)
            verticleMove = 0;
    }
    public void GatherSquatInput(InputAction.CallbackContext context)
    {
        squat = true;
    }
    public void GatherSquatEndInput(InputAction.CallbackContext context)
    {
        squat = false;
    }
    public void GatherInterActInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            interact = true;
    }

    public void GatherInteractEndInput(InputAction.CallbackContext context)
    {
        interact = false;
    }

    #endregion

    #region Check
    public void CheckGround()
    {
        onGround = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        if (colliders.Length > 0)
        {
            onGround = true;
            if (state.GetType() != typeof(StandingState)
                && state.GetType() != typeof(SquatState) && canFall)
            {
                if ((lastNearWallTime + 0.02f <= Time.time) || AddVelocity.magnitude <= 0)
                {
                    Land();
                    state = new StandingState(this);
                }
            }
        }
        else
        {
            if (state.GetType() != typeof(FallingState))
            {
                if (state.GetType() == typeof(StandingState))
                {
                    coyoteTime = Time.time;
                }
                if (canFall)
                    state = new FallingState(this);
            }
        }

    }
    public void CheckWall()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheck.position, groundedRadius, whatIsWall);
        if (colliders.Length > 0)
        {
            if (transform.localScale.x > 0)
            {
                nearRightWall = true;
                nearLeftWall = false;
            }
            else
            {
                nearRightWall = false;
                nearLeftWall = true;
            }
            lastNearWallTime = Time.time;
            return;
        }
    }
    public bool CheckLadder()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheck.position, groundedRadius, whatIsLadder);
        if (colliders.Length > 0)
        {

            return true;
        }
        return false;
    }
    public bool CheckLadderTop()
    {
        int dir = faceRight ? 1 : -1;
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position + new Vector3(0, 0.3f * gravityScale, 0),
            Vector2.right * dir, 0.5f, whatIsLadder);
        Debug.DrawRay(wallCheck.position + new Vector3(0, 0.3f * gravityScale, 0), Vector2.right * dir, Color.red);
        if (hit)
        {
            Debug.Log("CheckLadderTop()");
            return false;
        }
        Debug.Log("can'tCheckLadderTop()");
        return true;
    }
    public bool CheckHead()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(headCheck.position, 0.1f, whatIsHead);
        if (colliders.Length > 0)
        {
            if (state.GetType() == typeof(JumpingState))
                firstJumpEnd = Time.time;
            Velocity.y = 0;
            canFall = true;
            return true;
        }
        return false;
    }

    public bool CheckSquatHead()
    {
        RaycastHit2D hit = Physics2D.Raycast(headCheck.position,
            Vector2.up * gravityScale, 0.5f, whatIsHead);
        if (hit)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.red);
            return true;
        }
        return false;
    }
    public void CheckSlope()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position,
            Vector2.down * gravityScale, 0.3f, whatIsGround);

        if (hit)
        {
            if (horizontalMove != 0 || !canMove)
            {
                slopeNormal = Vector2.Perpendicular(hit.normal).normalized;
                slopeAngle = Vector2.SignedAngle(Vector2.up * gravityScale, hit.normal);
                if (slopeAngle != slopeAngleOld)
                {
                    if (slopeAngle == 0f)
                        onSlope = false;
                    else
                        onSlope = true;
                }
                slopeAngleOld = slopeAngle;

                Debug.DrawRay(hit.point, hit.normal, Color.red);
                Debug.DrawRay(hit.point, slopeNormal, Color.blue);

                /*transform.rotation = Quaternion.Lerp(transform.rotation,
                                   Quaternion.Euler(0, 0, slopeAngle/2), 0.2f);*/
            }

        }
    }



    #endregion

    #region Horizontal
    public void Horizontal()
    {
        if (horizontalMove > 0)
        {
            if (!faceRight)
            {
                faceRight = true;
                Flip();
            }

        }
        else if (horizontalMove < 0)
        {
            if (faceRight)
            {
                faceRight = false;
                Flip();
            }
        }
    }
    public void Flip()
    {
        float x;
        if (faceRight)
            x = Mathf.Abs(transform.localScale.x) * 1;
        else
            x = Mathf.Abs(transform.localScale.x) * -1;
        Vector3 scal = new Vector3(x,
            transform.localScale.y, transform.localScale.z);
        transform.localScale = scal;
    }
    public void CalcuHorizontalMove()
    {
        if (horizontalMove != 0)
        {
            runSpeedNow += horizontalMove * accel * Time.fixedDeltaTime;
            if (!squat)
                runSpeedNow = Mathf.Clamp(runSpeedNow, -(runSpeedMax + addRunSpeed), runSpeedMax + addRunSpeed);
            else
                runSpeedNow = Mathf.Clamp(runSpeedNow, -(squatSpeed + addRunSpeed), squatSpeed + addRunSpeed);
        }
        else
        {
            if (runSpeedNow != 0)
            {
                runSpeedNow = Mathf.MoveTowards(runSpeedNow, 0, decel * Time.fixedDeltaTime);
            }
        }
        Horizontal();
    }
    public void Run()//�����������ƶ�
    {
        CheckSlope();//�ڵ����ϲż��
        if (!onSlope && onGround)
        {
            Velocity.x = runSpeedNow;
            Debug.Log("onNormalGround");
        }
        else if (onSlope && Mathf.Abs(slopeAngle) < slopeAngleMax)
        {
            Velocity.x = runSpeedNow * slopeNormal.x * -1 * gravityScale;
            Velocity.y = runSpeedNow * slopeNormal.y * -1 * gravityScale;
           
            if (horizontalMove == 0)
            {
                Velocity.x = rigidbody2.velocity.x - AddVelocity.x;
                Velocity.y = rigidbody2.velocity.y - AddVelocity.y;
            }
             
            Debug.Log("onSlope");
        }
        else
        {
            Debug.Log("can't climb Slope");
            Velocity.x = rigidbody2.velocity.x - AddVelocity.x;
            Velocity.y = rigidbody2.velocity.y - AddVelocity.y;
        }
       

    }
    public void ReturnNormalRotation()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void Dashing(int dir)
    {
        Velocity.x = dir * dashspeed;
        Velocity.y = 0;
    }

    #endregion

    #region Verticle
    public void DefyingGravity()
    {
        SoundManager.PlayDefyGravity();
        gravityScale *= -1;
        rigidbody2.gravityScale *= -1;
        gameObject.transform.localScale
          = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y * -1, 1);
        PlayerVisual.instance.dashingPa.gameObject.transform.localScale
            = gameObject.transform.localScale;
    }
    public void ClimbLadder()
    {
        Velocity.y -= fallSpeedAccel * Time.deltaTime * gravityScale;
       
    }
    public void Climbing()
    {
        if (CheckLadder())
        {
            Velocity.x = 0f;
            if (verticleMove == 0)
            {
                if (rigidbody2.gravityScale != 0)
                    Velocity.y = 0.98f * gravityScale;
                else
                    Velocity.y = 0f;
                PlayerVisual.instance.PauseAnimation();
            }
            else
            {
                Velocity.y = verticleMove * climbspeed * gravityScale;
                PlayerVisual.instance.PauseContinue();
                if ((gravityScale > 0 && Velocity.y > 0) || (gravityScale < 0 && Velocity.y < 0))
                {
                    if (!goLadderTop && CheckLadderTop())
                    {
                        goLadderTop = true;
                        SetPlayerState(new ClimbLadderState(this));
                    }
                }
            }
        }
        else
        {
            PlayerVisual.instance.PauseContinue();
            canFall = true;
        }
    }
    public void Falling()
    {
        Velocity.x = runSpeedNow;
        if (lastNearWallTime + 0.1f > Time.time)
        {
            Velocity.x = 0;
            if (Mathf.Abs(Velocity.y) > fallSpeedWall)
            {
                Velocity.y = fallSpeedWall * -gravityScale;
                return;
            }
        }
        else
        {
            if (Mathf.Abs(Velocity.y) > fallSpeedMax + changeFallSpeedMax)
            {
                Velocity.y = (fallSpeedMax + changeFallSpeedMax) * -gravityScale;
                return;
            }
        }

        if (Mathf.Abs(Velocity.y) < (fallSpeedMax + changeFallSpeedMax) / 8)
            Velocity.y -= (fallSpeedAccel + addFallSpeed) / 2 * Time.deltaTime * gravityScale;
        else
            Velocity.y -= (fallSpeedAccel + addFallSpeed) * Time.deltaTime * gravityScale;

    }

    #endregion

    #region Jump
    public void Jump(float timer, float jumpbonus = 0f)
    {
        Velocity.x = runSpeedNow;
        Velocity.y = Mathf.Lerp(0, jumpForce + jumpbonus + addJumpForce, jumpCurve.Evaluate(timer)) * gravityScale;
        CheckHead();

    }
    public void AirJump(float timer)
    {
        Velocity.x = runSpeedNow;
        Velocity.y = Mathf.Lerp(0, jumpForce + addJumpForce, airJumpCurve.Evaluate(timer)) * gravityScale;
        CheckHead();
    }
    public void WallJump(float dir)
    {
        Velocity.x = xWallSpeed * dir;
        Velocity.y = 0.7f * (jumpForce + addJumpForce) * gravityScale;
        CheckHead();
    }

    #endregion

    private void OnDrawGizmos()
    {
        // Bounds
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, groundedRadius);
        Gizmos.DrawSphere(wallCheck.position, groundedRadius);
        Gizmos.DrawSphere(headCheck.position, 0.1f);
    }

    private void Land()
    {
        if (Mathf.Abs(Velocity.y) > 20f)
        {
            PlayerVisual.instance.Land();
            SoundManager.PlayLand();
        }
    }

    public void StayStatic()
    {
        canMove = false;
        Velocity = Vector2.zero;
        rigidbody2.velocity = Vector2.zero;

    }

    #region squat
    public void ChangeSquat()
    {
        collider2D.size = squatColliderSize;
        collider2D.offset = squatColliderOffSet;
        headCheck.gameObject.transform.localPosition = squatHeadCheckPos;
    }

    public void ChangeNormal()
    {
        collider2D.size = normalColliderSize;
        collider2D.offset = normalColliderOffset;
        headCheck.gameObject.transform.localPosition = normalHeadCheckPos;
    }


    #endregion

    #region Change
    public void ChangeJumpForce(float add)
    {
        addJumpForce += add;

    }

    public void ChangeRunSpeed(float add)
    {
        addRunSpeed += add;
    }

    public void ChangeFallSpeed(float add)
    {
        addFallSpeed += add;
    }

    public void ChangeFallSpeedMax(float add)
    {
        changeFallSpeedMax += add;
    }

    public void AccelVelocity(Vector3 dir, float addSpeed = 10f)
    {
        AddVelocity = dir.normalized * addSpeed;
    }

    public void RestAddVelocity()
    {
        StartCoroutine(ReduceAddVel());
        //AddVelocity = Vector3.zero;
    }


    public void SetGracity(float scale)
    {
        rigidbody2.gravityScale = scale;
    }


    #region LockAndUnlockSkill
    public void LockJumpSkill()
    {
        skill[Globle.JumpSkill] = false;
    }
    public void LockAirJumpSkill()
    {
        skill[Globle.AirJumpSkill] = false;
    }
    public void LockWallJumpSkill()
    {
        skill[Globle.WallJumpSkill] = false;
    }
    public void LockDashSkill()
    {
        skill[Globle.DashSkill] = false;
    }
    public void LockClimbSkill()
    {
        skill[Globle.ClimbSkill] = false;
    }
    public void UnlockJumpSkill()
    {
        skill[Globle.JumpSkill] = true;
    }
    public void UnlockAirJumpSkill()
    {
        skill[Globle.AirJumpSkill] = true;
    }
    public void UnlockWallJumpSkill()
    {
        skill[Globle.WallJumpSkill] = true;
    }
    public void UnlockDashSkill()
    {
        skill[Globle.DashSkill] = true;
    }
    public void UnlockClimbSkill()
    {
        skill[Globle.ClimbSkill] = true;
    }
    #endregion
    #endregion

    #region Ienum

    public IEnumerator ReduceAddVel()
    {
        while (AddVelocity.magnitude > 0.01f)
        {
            if (!goLadderTop)
                AddVelocity = Vector3.Slerp(AddVelocity, Vector3.zero, 0.5f);
            else
                break;
            yield return new WaitForFixedUpdate();
        }
        if (!goLadderTop)
            AddVelocity = Vector3.zero;
    }
    public IEnumerator StartcoolDown(float time = 1.2f)
    {
        canMove = false;
       // PlayerVisual.instance.PlayIdleAnimation();
        yield return new WaitForSeconds(time);
        canMove = true;
    }
    public IEnumerator AddJumpForceTime(float add, float time)
    {
        addJumpForce += add;
        MaxBuffTime = time;
        LeftBuffTime = MaxBuffTime;
        yield return new WaitForSeconds(time);
        MaxBuffTime = 0;
        addJumpForce -= add;
    }
    public IEnumerator AddRunSpeedTime(float add, float time)
    {
        addRunSpeed += add;
        MaxBuffTime = time;
        LeftBuffTime = MaxBuffTime;
        yield return new WaitForSeconds(time);
        MaxBuffTime = 0;
        addRunSpeed -= add;
    }
    public IEnumerator AddFallSpeedTime(float add, float time)
    {
        addFallSpeed += add;
        MaxBuffTime = time;
        LeftBuffTime = MaxBuffTime;
        yield return new WaitForSeconds(time);
        MaxBuffTime = 0;
        addFallSpeed -= add;
    }
    public IEnumerator AddFallSpeedMaxTime(float add, float time)
    {
        changeFallSpeedMax += add;
        MaxBuffTime = time;
        LeftBuffTime = MaxBuffTime;
        yield return new WaitForSeconds(time);
        MaxBuffTime = 0;
        changeFallSpeedMax -= add;
    }
    public IEnumerator CannotMove(float time)
    {
        canMove = false;
        float temp = rigidbody2.gravityScale;
        rigidbody2.gravityScale = 0;
        yield return new WaitForSeconds(time);
        rigidbody2.gravityScale = temp;
        canMove = true;

    }
    #endregion
}
