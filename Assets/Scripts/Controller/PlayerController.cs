using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerStateType
{
    Movable, Sprinting
}

[AddComponentMenu("Controllers/PlayerController")]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
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

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    //states
    Dictionary<ControllerStateType, ControllerState> stateDict = new Dictionary<ControllerStateType, ControllerState>();
    ControllerStateType currentState;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stateDict[ControllerStateType.Movable] = new ControllerMovableState(this);
        stateDict[ControllerStateType.Sprinting] = new ControllerSprintingState(this);

        currentState = ControllerStateType.Movable;
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
    }

    /// <summary>
    /// 左右移动
    /// </summary>
    /// <param name="horizontal">Horizontal speed scale. Range from -1 to 1.</param>
    public void OnMove(float horizontal)
    {
        rb.velocity = new Vector2(horizontal * walkSpeed, rb.velocity.y);

        //左右翻转
        if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void StateMove(float horizontal)
    {
        stateDict[currentState].OnMove(horizontal);
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    public void OnJump()
    {
        //检测是否踩在地面上
        bottomCenterGlobal = transform.position + new Vector3(bottomCenterX,bottomCenterY);
        Collider2D[] hit = Physics2D.OverlapBoxAll(bottomCenterGlobal,bottomSize,0,LayerMask.GetMask(Consts.GroundLayer));
        if (hit.Length > 0)
        {
            //Debug.Log(hit[0]);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
    public void StateJump()
    {
        stateDict[currentState].OnJump();
    }

    /// <summary>
    /// 开始冲刺
    /// </summary>
    public void OnSprint()
    {
        if (sprintCooldownTimer <= 0 && sprintDurationTimer <= 0)
        {
            sprintDurationTimer = sprintDuration;
            currentState = ControllerStateType.Sprinting;
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

        rb.velocity = new Vector2(tempSpeed, 0);

        sprintDurationTimer -= Time.deltaTime;
        if (sprintDurationTimer <= 0)
        {
            sprintCooldownTimer = sprintCooldown;

            currentState = ControllerStateType.Movable;
        }
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
