using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ControllerStateType
{
    Movable, Sprinting, Pause
}

[AddComponentMenu("Controllers/PlayerController")]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    //ÒÆ¶¯
    public float walkSpeed;
    //ÌøÔ¾
    public float jumpSpeed;
    //³å´Ì
    public float sprintSpeed;
    public float sprintDuration;
    float sprintDurationTimer = 0;
    public float sprintCooldown;
    float sprintCooldownTimer = 0;

    //ÂäµØÅÐ¶¨
    public float bottomCenterX, bottomCenterY;
    Vector2 bottomCenterGlobal;
    public Vector2 bottomSize;

    //¹¥»÷
    public UnityEvent attackEvent;
    public float attackCooldown;
    float attackCooldownTimer = 0;

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
        stateDict[ControllerStateType.Pause] = new ControllerPauseState(this);

        currentState = ControllerStateType.Movable;
    }

    // Update is called once per frame
    void Update()
    {
        //³å´Ì
        if (sprintCooldownTimer > 0)
        {
            sprintCooldownTimer -= Time.deltaTime;
        }
        if (sprintDurationTimer > 0)
        {
            SprintUpdate();
        }

        //¹¥»÷
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// ×óÓÒÒÆ¶¯
    /// </summary>
    /// <param name="horizontal">Horizontal speed scale. Range from -1 to 1.</param>
    public void OnMove(float horizontal)
    {
        rb.velocity = new Vector2(horizontal * walkSpeed, rb.velocity.y);

        //×óÓÒ·­×ª
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
    /// ÌøÔ¾
    /// </summary>
    public void OnJump()
    {
        //¼ì²âÊÇ·ñ²ÈÔÚµØÃæÉÏ
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
    /// ¿ªÊ¼³å´Ì
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
    /// ³å´Ì¸üÐÂ
    /// </summary>
    void SprintUpdate()
    {
        //ÅÐ¶Ï·½Ïò
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
    /// ¹¥»÷
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

    public void SetPause(bool isPause)
    {
        if (isPause)
        {
            currentState = ControllerStateType.Pause;
        }
        else
        {
            currentState = ControllerStateType.Movable;
        }
    }

    /// <summary>
    /// Debug
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        //ÂäµØÅÐ¶¨ÇøÓò
        Gizmos.color = Color.yellow;
        bottomCenterGlobal = transform.position + new Vector3(bottomCenterX, bottomCenterY);
        Gizmos.DrawWireCube(bottomCenterGlobal,bottomSize);
    }
}
