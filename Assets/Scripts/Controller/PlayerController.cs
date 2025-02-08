using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public enum ControllerStateType
{
    Movable, Sprinting, Pause, Dead
}

[AddComponentMenu("Controllers/PlayerController")]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //移动
    public float walkSpeed;
    //跳跃
    public float jumpSpeed;
    //冲刺
    public float sprintSpeed;
    public float sprintDuration;
    float sprintDurationTimer = 0;
    public float sprintCooldown;
    float sprintCooldownTimer = 0;

    //落地判定
    public float bottomCenterX, bottomCenterY;
    Vector2 bottomCenterGlobal;
    public Vector2 bottomSize;
    bool isOnSlope = false;
    bool isJumping = false;

    //攻击
    public UnityEvent attackEvent;
    public float attackCooldown;
    float attackCooldownTimer = 0;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Player player;
    Collider2D[] colliders;
    Animator animator;

    //states
    Dictionary<ControllerStateType, ControllerState> stateDict = new Dictionary<ControllerStateType, ControllerState>();
    ControllerStateType currentState;
    ControllerStateType UnpausedState;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = GetComponent<Player>();
        colliders = GetComponents<Collider2D>();
        animator = GetComponent<Animator>();

        bottomCenterGlobal = transform.position + new Vector3(bottomCenterX, bottomCenterY);

        stateDict[ControllerStateType.Movable] = new ControllerMovableState(this);
        stateDict[ControllerStateType.Sprinting] = new ControllerSprintingState(this);
        stateDict[ControllerStateType.Pause] = new ControllerPauseState(this);
        stateDict[ControllerStateType.Dead] = new ControllerDeadState(this);

        currentState = ControllerStateType.Movable;
        UnpausedState = currentState;
    }

    // Update is called once per frame
    void Update()
    {
        //冲刺
        if (sprintCooldownTimer > 0)
        {
            sprintCooldownTimer -= Time.deltaTime;
        }
        if (sprintDurationTimer > 0)
        {
            SprintUpdate();
        }

        //攻击
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        //脚下判定
        bottomCenterGlobal = transform.position + new Vector3(bottomCenterX, bottomCenterY);
        Collider2D[] hit = Physics2D.OverlapBoxAll(bottomCenterGlobal, bottomSize, 0, LayerMask.GetMask(Consts.WallLayer));
        //Debug.Log("isJumping:" + isJumping.ToString());
        //Debug.Log("isOnSlope:"+isOnSlope.ToString());
        //不在斜坡上时
        if (!isOnSlope)
        {
            //不在跳跃过程中时，脚下是斜坡则判定在斜坡上
            if (hit.Length > 0)
            {
                //Debug.Log(hit[0]);
                if (!isJumping)
                {
                    foreach (Collider2D col in hit)
                    {
                        //Debug.Log(col.gameObject.name);
                        if (col.tag == Consts.SlopeTag)
                        {
                            isOnSlope = true;
                        }
                    }
                }
            }
            //脚下没有地面时跳跃过程结束
            else
            {
                //Debug.Log(hit.Length);
                isJumping = false;
            }
        }
        //在斜坡上时
        else
        {
            //若脚下不是斜坡，则判定离开斜坡
            bool isLeaveSlope = true;
            if (hit.Length > 0)
            {
                foreach (Collider2D col in hit)
                {
                    //Debug.Log(col.gameObject.name);
                    if (col.tag == Consts.SlopeTag)
                    {
                        isLeaveSlope = false;
                    }
                }
            }

            //若离开斜坡,则纵向速度归零
            if (isLeaveSlope)
            {
                isOnSlope = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }

        //动画
        animator.SetBool(Consts.IsJumpingAnimatorArgument, isJumping);
        if (hit.Length <= 0)
        {
            //Debug.Log(hit.Length);
            animator.SetBool(Consts.IsAirAnimatorArgument, true);
        }
        else
        {
            //Debug.Log(hit[0]);
            animator.SetBool(Consts.IsAirAnimatorArgument, false);
        }
        animator.SetFloat(Consts.YSpeedAnimatorArgument, rb.velocity.y);
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="state"></param>
    void StateTransition(ControllerStateType state)
    {
        stateDict[currentState].OnExit();
        currentState = state;
        stateDict[currentState].OnEnter();
    }


    /// <summary>
    /// 左右移动
    /// </summary>
    /// <param name="horizontal">Horizontal speed scale. Range from -1 to 1.</param>
    public void OnMove(float horizontal)
    {
        rb.velocity = new Vector2(HorizontalToSpeed(horizontal), rb.velocity.y);

        //左右翻转
        if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }

        player.animator.SetFloat(Consts.SpeedAnimatorArgument,Mathf.Abs(horizontal));
    }

    public void StateMove(float horizontal)
    {
        stateDict[currentState].OnMove(horizontal);
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    public void OnJump(bool isNoGroundJump = false)
    {
        //Debug.Log("isNoGroundJump:" + isNoGroundJump);

        //检测是否踩在地面上
        bottomCenterGlobal = transform.position + new Vector3(bottomCenterX,bottomCenterY);
        Collider2D[] hit = Physics2D.OverlapBoxAll(bottomCenterGlobal,bottomSize,0,LayerMask.GetMask(Consts.WallLayer));
        //Debug.Log(hit);
        if (hit.Length > 0 || isNoGroundJump)
        {
            //Debug.Log(hit[0]);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);

            isJumping = true;
            isOnSlope = false;
        }
    }
    public void StateJump(bool isNoGroundJump = false)
    {
        //Debug.Log("isNoGroundJump:" + isNoGroundJump);
        stateDict[currentState].OnJump(isNoGroundJump);
    }

    /// <summary>
    /// 开始冲刺
    /// </summary>
    public void OnSprint()
    {
        if (sprintCooldownTimer <= 0 && sprintDurationTimer <= 0)
        {
            sprintDurationTimer = sprintDuration;
            StateTransition(ControllerStateType.Sprinting);

            player.isSprinting = true;
        }
    }
    public void StateSprint()
    {
        stateDict[currentState].OnSprint();
    }

    /// <summary>
    /// 冲刺更新
    /// </summary>
    void SprintUpdate()
    {
        //判断方向
        float tempSpeed = sprintSpeed;
        if (spriteRenderer.flipX)
        {
            tempSpeed = -tempSpeed;
        }

        rb.velocity = new Vector2(tempSpeed, rb.velocity.y);

        sprintDurationTimer -= Time.deltaTime;
        if (sprintDurationTimer <= 0)
        {
            sprintCooldownTimer = sprintCooldown;
            
            StateTransition(ControllerStateType.Movable);

            player.isSprinting = false;
        }
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void OnAttack()
    {
        if (attackCooldownTimer <= 0)
        {
            attackCooldownTimer = attackCooldown;
            attackEvent?.Invoke();
        }
    }

    public void StateAttack()
    {
        stateDict[currentState].OnAttack();
    }

    /// <summary>
    /// 设置暂停状态
    /// </summary>
    /// <param name="isPause"></param>
    public void SetPause(bool isPause)
    {
        if (isPause)
        {
            UnpausedState = currentState;
            StateTransition(ControllerStateType.Pause);
        }
        else
        {
            StateTransition(UnpausedState);
        }
    }

    /// <summary>
    /// 切换暂停状态
    /// </summary>
    public void SwitchPause()
    {
        if (currentState == ControllerStateType.Pause)
        {
            StateTransition(UnpausedState);
        }
        else if (currentState == ControllerStateType.Movable)
        {
            UnpausedState = currentState;
            StateTransition(ControllerStateType.Pause);
        }
    }

    /// <summary>
    /// 死亡状态
    /// </summary>
    public void OnDie()
    {
        StateTransition(ControllerStateType.Dead);
    }

    /// <summary>
    /// 计算操作速度
    /// </summary>
    /// <param name="horizontal"></param>
    /// <returns></returns>
    public float HorizontalToSpeed(float horizontal)
    {
        return horizontal * walkSpeed;
    }

    /// <summary>
    /// 楼梯判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {/*
        if (collision.CompareTag(Consts.SlopeTag))
        {
            Debug.LogWarning(isOnSlope);
            Collider2D[] collisions = collision.GetComponents<Collider2D>();

            //Debug.LogWarning(rb.velocity.y + "  " + walkSpeed);
            if (!isOnSlope)
            {
                foreach (Collider2D collision2 in collisions)
                {
                    if (!collision2.isTrigger)
                    {
                        foreach (Collider2D collider in colliders)
                        {
                            Physics2D.IgnoreCollision(collision2, collider, true);
                        }
                    }
                }
                //Debug.LogWarning("ignored");
            }
            else if (rb.velocity.y < walkSpeed) //移动方向小于45度
            {
                foreach (Collider2D collision2 in collisions)
                {
                    if (!collision2.isTrigger)
                    {
                        foreach (Collider2D collider in colliders)
                        {
                            Physics2D.IgnoreCollision(collision2, collider, false);
                        }
                    }
                }
                //Debug.LogWarning("not ignored");
            }
        }*/
    }

    /// <summary>
    /// Debug
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        //落地判定区域
        Gizmos.color = Color.yellow;
        bottomCenterGlobal = transform.position + new Vector3(bottomCenterX, bottomCenterY);
        Gizmos.DrawWireCube(bottomCenterGlobal,bottomSize);
    }
}
